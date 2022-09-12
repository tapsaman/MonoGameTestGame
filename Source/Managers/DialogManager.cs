using System;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine;
using TapsasEngine.Utilities;

namespace ZA6.Managers
{
    public class DialogManager
    {
        public enum State
        {
            Typing,
            ShiftingLine,
            PageDone,
            MessageDone,
            QuestionDone,
            QuestionAnswered
        }

        public bool IsDone { get; set; } = true;
        public bool Borderless { get; set; }
        public event Action DialogEnd;
        public DialogBox DialogBox = new LinkToThePastSectionedDialogBox();
        public string Answer { get; private set; } = null;
        public int? AnswerIndex { get; private set; } = null;
        public Vector2 DrawOffset = Vector2.Zero;
        public State CurrentState => _state;
        private State _state;
        const float _LETTER_TIME = 0.018f;
        const float _INPUT_WAIT_TIME = 0.2f;
        const float _LINE_SHIFT_TIME = 0.2f;
        const float _ANSWER_FLASH_TIME = 1f;
        private float _elapsedTime = 0f;
        private int _drawLetterCount = 0;
        private bool _dialogBoxIsFull;
        private int _currentStartOfMessage;
        private int _pageStart;
        private int _pageLength;
        private string _currentMessage;
        private string _currentString;
        private int _pageLineIndex;
        private int _dialogBoxTextHeight;
        private int _dialogContentIndex;
        private float _cropY;
        private Dialog _dialog;
        private bool _topDialogBox;
        private bool _asking;
        private int? _currentOptionIndex = null;


        public void Load(Dialog dialog, bool topDialogBox = false)
        {
            IsDone = false;
            _dialog = dialog;
            _topDialogBox = topDialogBox;
            _dialogContentIndex = 0;
 
            StartNewContent();
        }

        private void StartNewContent()
        {
            AnswerIndex = null;
            Answer = null;

            _asking = false;
            _currentOptionIndex = null;
            var currentContent = _dialog.Content[_dialogContentIndex];

            if (currentContent is DialogText textContent)
            {
                StartNewMessage(textContent.Message);
            }
            else if (currentContent is DialogAsk askContent)
            {
                _asking = true;
                StartNewMessage(DialogAskToText(askContent, null));
            }
        }

        private static string DialogAskToText(DialogAsk ask, int? optionIndex)
        {
            string text = ask.Question;
            
            for (int i = 0; i < ask.Options.Length; i++)
            {
                if (i == optionIndex)
                    text += "\n     > " + ask.Options[i];
                else
                    text += "\n        " + ask.Options[i];
            }

            return text;
        }

        private static string DialogAskToAnsweredText(DialogAsk ask, int answerIndex)
        {
            string text = ask.Question;
            
            for (int i = 0; i < ask.Options.Length; i++)
            {
                if (i == answerIndex)
                    text += "\n     \f01> " + ask.Options[i] + "\f00";
                else
                    text += "\n        " + ask.Options[i];
            }

            return text;
        }

        private void StartNewMessage(string message)
        {
            _currentMessage = message;
            _dialogBoxIsFull = false;
            _drawLetterCount = 0;
            _currentStartOfMessage = 0;
            _elapsedTime = 0;
            _currentString = "";
            _pageLineIndex = 0;
            _state = State.Typing;
            _pageStart = 0;
            _pageLength = _currentMessage.IndexOfNth('\n', 3);

            if (_pageLength == -1)
                _pageLength = _currentMessage.Length;
            
            int currentLineCount = Regex.Matches(_currentMessage, "\n").Count + 1;
            _dialogBoxTextHeight = Math.Min(currentLineCount, 3) * BitmapFontRenderer.Font.LineHeight;
        }

        private void NextContent()
        {
            _dialogContentIndex++;

            if (_dialogContentIndex < _dialog.Content.Length)
            {
                StartNewContent();
            }
            else
            {
                EndDialog();
            }
        }

        private void EndDialog()
        {
            IsDone = true;
            _dialogContentIndex = 0;
            SFX.MessageFinish.Play();
            DialogEnd?.Invoke();
        }

        public void Update(GameTime gameTime)
        {
            if (IsDone)
                return;

            switch(_state)
            {
                case State.Typing:
                    if (GotNextInput())
                        SkipAhead();
                    else
                        UpdateTyping(gameTime);
                    break;
                case State.ShiftingLine:
                    if (GotNextInput())
                        SkipAhead();
                    else
                        UpdateLineShifting(gameTime);
                    break;
                case State.PageDone:
                    if (GotNextInput())
                        NextPage();
                break;
                case State.MessageDone:
                    if (GotNextInput())
                        NextContent();
                break;
                case State.QuestionDone:
                    UpdateAsking(gameTime);
                break;
                case State.QuestionAnswered:
                    UpdateAfterAnswer(gameTime);
                break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_currentString == null)
                return;
            
            DialogBox.Draw(
                spriteBatch,
                _topDialogBox,
                _currentString,
                _cropY,
                _dialogBoxTextHeight, 
                DrawOffset,
                Borderless
            );
        }

        private void SkipAhead()
        {
            _cropY = 0;
            _elapsedTime = 0;

            if (_asking)
            {
                _currentStartOfMessage = _pageStart;
                _currentString = _currentMessage.Substring(_currentStartOfMessage, _pageLength);
                _pageLineIndex = 3;
                _dialogBoxIsFull = true;
                _state = State.QuestionDone;
            }
            else if (_pageStart + _pageLength == _currentMessage.Length)
            {
                _currentString = _currentMessage.Substring(_pageStart);
                _state = State.MessageDone;
            }
            else
            {
                _currentStartOfMessage = _pageStart;
                _currentString = _currentMessage.Substring(_currentStartOfMessage, _pageLength);
                _pageLineIndex = 3;
                _dialogBoxIsFull = true;
                _state = State.PageDone;
            }
            
            _drawLetterCount = _currentStartOfMessage + _currentString.Length + 1;
        }

        private void UpdateTyping(GameTime gameTime)
        {
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsedTime > _LETTER_TIME)
            {
                _drawLetterCount++;
                _elapsedTime = 0;

                char currentChar = _currentMessage[_drawLetterCount - 1];

                if (currentChar == '\n')
                {
                    _pageLineIndex++;

                    if (_pageLineIndex != 3 && _dialogBoxIsFull)
                    {
                        _state = State.ShiftingLine;
                    }
                }
                else if (currentChar != ' ')
                {
                    //_currentString = _currentMessage.Substring(_currentStartOfMessage, _drawLetterCount - _currentStartOfMessage);
                    SFX.Message.Play();
                }

                _currentString += currentChar;

                if (_drawLetterCount == _currentMessage.Length)
                {
                    _state = _asking ? State.QuestionDone : State.MessageDone;
                }
                else if (_pageLineIndex == 3)
                {
                    _dialogBoxIsFull = true;
                    _state = State.PageDone;
                }
            }
        }

        private void UpdateLineShifting(GameTime gameTime)
        {
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsedTime < _LINE_SHIFT_TIME)
            {
                _cropY = BitmapFontRenderer.Font.LineHeight * _elapsedTime / _LINE_SHIFT_TIME;
            }
            else
            {
                int nextNewLineIndex = _currentString.IndexOf('\n');
                _cropY = 0;
                _currentStartOfMessage += nextNewLineIndex + 1;
                //_currentString = _currentString.Substring(nextNewLineIndex + 1, _currentString.Length - nextNewLineIndex);
                _currentString = _currentMessage.Substring(_currentStartOfMessage, _drawLetterCount - _currentStartOfMessage);
                _elapsedTime = 0;
                _state = State.Typing;
            }
        }

        private void NextPage()
        {
            _pageLineIndex = 0;
            _pageStart = _drawLetterCount;
            _pageLength = _currentMessage.IndexOfNth('\n', 3, _pageStart) - _pageStart;
            if (_pageLength < 0)
                _pageLength = _currentMessage.Length - _pageStart;
            _state = State.ShiftingLine;
        }

        private void UpdateAsking(GameTime gameTime)
        {
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsedTime > _INPUT_WAIT_TIME && _dialog.Content[_dialogContentIndex] is DialogAsk ask)
            {
                if (_currentOptionIndex == null)
                {
                    _currentOptionIndex = 0;
                    _currentString = DialogAskToText(ask, _currentOptionIndex);
                    SFX.Cursor.Play();
                }
                else if (Input.P1.JustPressed(Input.P1.A))
                {
                    AnswerIndex = _currentOptionIndex;
                    Answer = ask.Options[(int)_currentOptionIndex];
                    _elapsedTime = 0;
                    _state = State.QuestionAnswered;
                    _currentString = DialogAskToAnsweredText(ask, (int)AnswerIndex);
                    //NextContent();
                    SFX.WalkGrass.Play();
                }
                else
                {
                    var dir = Input.P1.GetDirectionVector();
                    
                    if (dir.Y < 0)
                    {
                        _currentOptionIndex = (_currentOptionIndex == 0 ? ask.Options.Length : _currentOptionIndex) - 1;
                        _currentString = DialogAskToText(ask, _currentOptionIndex);
                        SFX.Cursor.Play();
                        _elapsedTime = 0;
                    }
                    else if (dir.Y > 0)
                    {
                        _currentOptionIndex = _currentOptionIndex + 1 < ask.Options.Length ? _currentOptionIndex + 1 : 0;
                        _currentString = DialogAskToText(ask, _currentOptionIndex);
                        SFX.Cursor.Play();
                        _elapsedTime = 0;
                    }
                }
            }
        }

        private void UpdateAfterAnswer(GameTime gameTime)
        {
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsedTime > _ANSWER_FLASH_TIME)
            {
                NextContent();
            }
        }

        private bool GotNextInput()
        {
            return Input.P1.JustPressed(Input.P1.A) || Input.P1.JustPressed(Input.P1.B) || Input.P1.JustPressedMouseLeft();
        }
    }
}