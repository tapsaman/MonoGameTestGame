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
            // Theme = Static.Content.Load<Song>("linktothepast/darkworld");
            //ExitTransitions[Direction.Right] = TransitionType.FadeToBlack;
        }

        protected override void Load()
        {
            if (Static.GameData.GetString("scenario") == "end")
            {
                Static.DevUtils.RenderDevInfo = true;
            }

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

            _erkkiEvents = new Event[]
            {
                new ConditionEvent(DataStoreType.Scene, "spoken to erkki")
                {
                    IfFalse = new Event[]
                    {
                        new FaceEvent(erkki, Player),
                        new TextEvent(new Dialog(msg1), erkki),
                        new FaceEvent(erkki, Direction.Down),
                        new SaveValueEvent(DataStoreType.Scene, "spoken to erkki", true)
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
                Static.Renderer.OnPostDraw += IterateNoiseEffect;
            }
        }

        private void IterateNoiseEffect()
        {
            _noiseSeed += 0.001f;
            Shaders.Noise.Parameters["seed"].SetValue(_noiseSeed);
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