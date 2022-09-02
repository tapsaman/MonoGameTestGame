using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TapsasEngine;
using TapsasEngine.Enums;

namespace ZA6
{
    public class SceneB2 : Scene
    {
        private Texture2D _overlay;
        public Seppo Seppo;
        private bool _seppoActivated;

        public SceneB2()
        {
            Theme = Static.Content.Load<Song>("linktothepast/darkworld");
            ExitTransitions[Direction.Up] = TransitionType.FadeToBlack;
        }

        protected override void Load()
        {
            if (Static.GameData.GetInt("progress") == 0)
            {
                Seppo = new Seppo();
                Seppo.Position = TileMap.ConvertTileXY(35, 8);

                var seppoActivateEventTrigger = new TouchEventTrigger(TileMap.ConvertTileXY(35, 16), 16, 8);
                seppoActivateEventTrigger.Trigger += SeppoActivateEvent;
                
                var seppoDectivateEventTrigger = new TouchEventTrigger(TileMap.ConvertTileXY(32, 26), 16, 8);
                seppoDectivateEventTrigger.Trigger += SeppoDeactivateEvent;

                Add(Seppo);
                Add(seppoActivateEventTrigger);
                Add(seppoDectivateEventTrigger);
            }
        }

        private void SeppoActivateEvent(Character toucher)
        {
            if (!_seppoActivated && toucher == Player)
            {
                Sys.Log("activate seppo");
                _seppoActivated = true;
                Seppo.Facing = Direction.Down;
                Music.Pause(this);

                if (Static.GameData.GetInt("progress") == 0)
                    Static.GameData.Save("progress", 1);
            }
        }

        private void SeppoDeactivateEvent(Character toucher)
        {
            if (_seppoActivated && toucher == Player)
            {
                Sys.Log("deactivate seppo");
                _seppoActivated = false;
                Seppo.Facing = Direction.Left;
                Music.Resume(this);
            }
        }
    }
}