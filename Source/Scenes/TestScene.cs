using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using ZA6.Models;
using TapsasEngine.Enums;
using TapsasEngine;

namespace ZA6
{
    public class TestScene : Scene
    {
        private Event[] _erkkiEvents;
        private Event _signEvent;
        private float _noiseSeed = 0f;

        public TestScene()
        {
            // Load basics in constructor
        }

        protected override void Load()
        {
            // Init events and map objects in Load
            var erkki = new Erkki();
            erkki.Position = TileMap.ConvertTileXY(22, 22);
            erkki.Trigger += StartErkkiDialog;

            var signEventTrigger = new EventTrigger(TileMap.ConvertTileXY(9, 4), 14, 14);
            _signEvent = new TextEvent(new Dialog("ZELDA'S TENT HOME"), signEventTrigger);
            signEventTrigger.Trigger += ReadSign;

            Add(erkki);
            Add(signEventTrigger);

            var msg1 = "Hi Zelda. Good thing\nyour awake. Zelda has\nbeen capture again!\nLooks Like Ganondorf is at it\nagain!\nplz hurry and save the world!!!";

            var erkkiDialog2 = new Dialog(
                "Sample text"
            );

            //Static.EventSystem.Load(new AnimateEvent(new Animations.Evaporate(erkki.AnimatedSprite, true)));
            _erkkiEvents = new Event[]
            {
                new ConditionEvent(DataStoreType.Scene, "spoken to erkki")
                {
                    IfFalse = new Event[]
                    {
                        new FaceEvent(erkki, Player),
                        new TextEvent(new Dialog(msg1), erkki),
                        new FaceEvent(erkki, Direction.Down),
                        new ConditionEvent(() => Static.GameData.GetString("scenario") == null)
                        {
                            IfTrue = new Event[]
                            {
                                new SaveValueEvent(DataStoreType.Scene, "spoken to erkki", true)
                            },
                            IfFalse = new Event[]
                            {
                                new AnimateEvent(new Animations.Evaporate(erkki.AnimatedSprite, true)),
                                new RemoveEvent(erkki)
                            }
                        }
                    },
                    IfTrue = new Event[]
                    {
                        new FaceEvent(erkki, Player),
                        new TextEvent(erkkiDialog2, erkki),
                        new FaceEvent(erkki, Direction.Down)
                    }
                }
            };
        }

        public override void Start()
        {
            base.Start();
            
            if (Static.GameData.GetString("scenario") == "noise")
            {
                Static.Renderer.ApplyPostEffect(Shaders.Noise);
            }
        }
        
        private void StartErkkiDialog(Character _)
        {
            Static.EventSystem.Load(_erkkiEvents);
        }

        private void ReadSign(Character _)
        {
            Static.EventSystem.Load(_signEvent);
        }
    }
}