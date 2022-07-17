using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTestGame.Managers
{
    public class DialogManager
    {
        const float LetterTime = 0.05f;
        const float ArrowTime = 0.4f;
        private float _elapsedTime = 0f;
        private int _drawLetterCount = 0;
        private string _currentString;
        private int _dialogMessageIndex = 0;
        private Dialog _dialog;
        private bool _typing = false;
        private bool _running = false;
        private bool _displayArrow = false;
        private bool _topDialogBox = false;
        public event Action DialogEnd;
        public DialogBox DialogBox = new LinkToThePastDialogBox();

        public void Load(Dialog dialog, bool topDialogBox = false)
        {
            _dialog = dialog;
            _dialogMessageIndex = 0;
            _drawLetterCount = 0;
            _elapsedTime = 0;
            _currentString = "";
            _running = true;
            _typing = true;
            _displayArrow = false;
            _topDialogBox = topDialogBox;
        }

        private void Next()
        {
            _dialogMessageIndex++;

            if (_dialogMessageIndex < _dialog.Messages.Length)
            {
                _drawLetterCount = 0;
                _elapsedTime = 0;
                _currentString = "";
                _typing = true;
                _displayArrow = false;
            }
            else
            {
                _running = false;
                _dialogMessageIndex = 0;
                SFX.MessageFinish.Play();
                DialogEnd?.Invoke();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!_running) return;
            if (_typing)
            {
                if (Input.JustPressed(Input.A) || Input.JustPressed(Input.B) || Input.JustPressedMouseLeft())
                {
                    _drawLetterCount = _dialog.Messages[_dialogMessageIndex].Length;
                    _currentString = _dialog.Messages[_dialogMessageIndex].Substring(0, _drawLetterCount);
                    _elapsedTime = 0f;
                    _typing = false;
                    _displayArrow = true;
                }
                else
                {
                    _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (_elapsedTime > LetterTime)
                    {
                        _drawLetterCount++;
                        _currentString = _dialog.Messages[_dialogMessageIndex].Substring(0, _drawLetterCount);
                        _elapsedTime = 0f;

                        if (_dialog.Messages[_dialogMessageIndex][_drawLetterCount - 1] != ' ')
                            SFX.Message.Play();

                        if (_drawLetterCount == _dialog.Messages[_dialogMessageIndex].Length)
                        {
                            _typing = false;
                            _displayArrow = true;
                        }
                    }
                }
            }
            else
            {
                if (Input.JustPressed(Input.A) || Input.JustPressed(Input.B) || Input.JustPressedMouseLeft())
                {
                    Next();
                }
                else
                {
                    _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    
                    if (_elapsedTime > ArrowTime)
                    {
                        _displayArrow = !_displayArrow;
                        _elapsedTime = 0f;
                    }
                } 
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!_running) return;

            DialogBox.Draw(spriteBatch, _dialog.Title, _currentString, _displayArrow, _topDialogBox);
        }
    }
}