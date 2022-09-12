using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using TapsasEngine;
using TapsasEngine.Enums;
using TapsasEngine.Utilities;
using ZA6.Managers;
using ZA6.Models;

namespace ZA6
{
    public class SceneVoid : Scene
    {
        private Vector2 _previousPlayerPosition;
        private float _playerWalkDistance;
        private VoidSeppo _seppo;
        
        public override void Start()
        {
            base.Start();
            Music.Stop();

            Static.EventSystem.Load(
                new Event[]
                {
                    new WaitEvent(5f),
                    new DoEvent(() => new Event[] {
                        new RunEvent(() => GetFYouSound().Play()),
                        new TextEvent(new Dialog(GetFYouText()))
                    }),
                },
                EventSystem.Settings.Parallel | EventSystem.Settings.Looping
            );
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Vector2 diff = Player.Position - _previousPlayerPosition;
            _previousPlayerPosition = Player.Position;

            _playerWalkDistance += diff.ToAbsoluteFloat();

            Sys.Log("walk distance " + _playerWalkDistance);

            if (_playerWalkDistance > 300)
            {
                _playerWalkDistance = 0;

                if (_seppo == null)
                {
                    _seppo = new VoidSeppo();
                    _seppo.Trigger += TalkToSeppo;
                    Add(_seppo);
                }

                _seppo.Position = Player.Position 
                    + (diff.ToDirection().ToVector()
                    * Static.NativeSize / 2);
            }

        }

        private void TalkToSeppo(Character _)
        {
            Static.EventSystem.Load(new Event[]
            {
                new TextEvent(new Dialog("I just love it here. It's so\npeacuful, don't you agree?"), _seppo),
                new AskEvent(Dialog.Ask("Tell me, why did you come here?", "I followed the arrows", "Nothing else to do"), _seppo)
                {
                    IfOption1 = new Event[]
                    {
                        new TextEvent(new Dialog("I see. Akin to take directions?\nDo you ever think for\nyourself?"), _seppo),
                    }
                }
            });
        }

        private static SoundEffectInstance GetFYouSound()
        {
            var sfx = SFX.FYou.CreateInstance();
            sfx.Pitch = (float)Utility.RandomDouble() * 2 - 1;
            return sfx;
        }

        private static string GetFYouText()
        {
            int random = Utility.RandomBetween(0, 255); // 255 = 0b_0000_0000_1111_1111
            string textSource = "fuck you";
            string text = "";

            for (int i = 0; i < 8; i++)
            {
                text += (random & 1 << i) == 0
                    ? textSource[i].ToString()
                    : textSource[i].ToString().ToUpper();
            }

            int fourthAndFifthBits = random >> 4 & 2;

            switch (fourthAndFifthBits)
            {
                case 1:
                    text += "!";
                    break;
                case 2:
                    text += " !";
                    break;
                case 3:
                    text += " !!";
                    break;
            }

            return text;
        }
    }
}