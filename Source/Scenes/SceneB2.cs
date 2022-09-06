using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TapsasEngine;
using TapsasEngine.Enums;

namespace ZA6
{
    public class SceneB2 : Scene
    {
        public Seppo Seppo;
        private bool _seppoActivated;

        public SceneB2()
        {
            Theme = Songs.DarkWorld;
            ExitTransitions[Direction.Up] = TransitionType.FadeToBlack;
        }

        protected override void Load()
        {
            if (Static.GameData.GetString("scenario") == null)
            {
                MediaPlayer.Stop();
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
            else if (Static.GameData.GetString("scenario") == "hole")
            {
                for (int i = Characters.Count - 1; i >= 0 ; i--)
                {
                    if (Characters[i] is Guard guard)
                    {
                        Remove(guard);
                    }
                }

                Add(new GrowingHole(TileMap.ConvertTileXY(22, 40)));
                //Locked = true;
            }
        }

        private void SeppoActivateEvent(Character toucher)
        {
            if (!_seppoActivated && toucher == Player)
            {
                Sys.Log("ACTIVATE SEPPO");
                _seppoActivated = true;
                Seppo.Facing = Direction.Down;
                Music.Pause(this);

                Static.GameData.Save("scenario", "hole");
            }
        }

        private void SeppoDeactivateEvent(Character toucher)
        {
            if (_seppoActivated && toucher == Player)
            {
                Sys.Log("DEACTIVATE SEPPO");
                _seppoActivated = false;
                Seppo.Facing = Direction.Left;
                Music.Resume(this);

                for (int i = Characters.Count - 1; i >= 0 ; i--)
                {
                    if (Characters[i] is Guard guard)
                    {
                        Remove(guard);
                    }
                }

                foreach (TileMap.Object obj in TileMap.Objects)
                {
                    if (obj.TypeName == "Guard")
                    {
                        Add(new SeppoStatue() { Position = obj.Position });
                    }
                }
            }
        }
    }
}