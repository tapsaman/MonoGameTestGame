using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine;
using ZA6.Managers;

namespace ZA6.Animations
{
    public class JapanLevel : Animation
    {
        public static DialogManager DialogManager;
        public Taalasmaa Character;

        public JapanLevel()
        {
            DialogManager = new DialogManager();
            Character = new Taalasmaa() { Position = Static.NativeSize };
            Static.Scene.Add(Character);

            DialogManager.DialogBox = new JapanDialogBox();
            BitmapFontRenderer.Font = new BitmapFont.MSGothic();

            Stages = new AnimationStage[]
            {
                new TalkStage(new Dialog("あてうせい!")),
                new Jump.JumpToStage(Character, new Vector2(-50, -70)),
                new WaitStage(0.4f),
                new Jump.JumpToStage(Character, new Vector2(-50, -65)),
                new TalkStage(new Dialog("たま おん てちlあうせ!\nつlえ むかあん\nせいかいlううん!!!")),
                new TalkStage(Dialog.Ask("りき つlえ?", "lうおkせに たあs", "lいさあ てちあ ぽじ")),
                
                new TalkStage(new Dialog("たあs lあうせ!!")),
                new TalkStage(new Dialog("かいけんまあいlまん\nきkけlいみえひ. たあllあぴたあ\nしえたあ!"), true),
                new TalkStage(new Dialog("あがいん ひい あのへ\nてぴえcえ あぢだ!")),
                new Jump.JumpToStage(Character, new Vector2(146, 135)),
                /*
                 new TextEvent(new Dialog("たあs lあうせ!!")),
                new RunEvent(() => _taalasmaa.AnimatedSprite.SetAnimation("AngryTalk")),
                new TextEvent(new Dialog("かいけんまあいlまん\nきkけlいみえひ. たあllあぴたあ\nしえたあ!")),
                new RunEvent(() => _taalasmaa.AnimatedSprite.SetAnimation("Talk")),
                new TextEvent(new Dialog("あがいん ひい あのへ\nてぴえcえ あぢだ!")),
                new RunEvent(() => _taalasmaa.AnimatedSprite.SetAnimation("Idle")),
                new AnimateEvent(new Animations.Jump.To(_taalasmaa, Static.NativeSize)),
                new RunEvent(() =>
                {
                                Static.GameData.Save("scenario", "arrows");
                                Static.Game.StateMachine.TransitionTo("StartOver");
                            })
                            
                
                    },
                    new RunEvent(() => {
                        BitmapFontRenderer.Font = new BitmapFont.LinkToThePast();
                        Static.DialogManager.DialogBox = new LinkToThePastSectionedDialogBox();
                    })
                */ 
            };
        }

        public override void Exit()
        {
            Static.Game.StateMachine.TransitionTo("Default");
        }

        private class TalkStage : AnimationStage
        {
            private Dialog _dialog;
            private bool _angry;

            public TalkStage(Dialog dialog, bool angry = false)
            {
                _dialog = dialog;
                _angry = angry;
            }

            public override void Enter()
            {
                Static.Game.StateMachine.TransitionTo("Cutscene");
                DialogManager.DialogEnd += OnDialogEnd;
                DialogManager.Load(_dialog, true);
            }

            public override void Update(float elapsedTime)
            {
                DialogManager.Update(Tengine.GameTime);

                string spriteKey;

                if (DialogManager.CurrentState == DialogManager.State.Typing)
                {
                    spriteKey = _angry ? "AngryTalk" : "Talk";
                }
                else
                {
                    spriteKey = _angry ? "AngryIdle" : "Idle";
                }

                ((JapanLevel)Animation).Character.AnimatedSprite.SetAnimation(spriteKey);
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                DialogManager.Draw(spriteBatch);
            }

            private void OnDialogEnd()
            {
                DialogManager.DialogEnd -= OnDialogEnd;
                IsDone = true;
            }
        }
    }
}