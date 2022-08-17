using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using ZA6.Models;

namespace ZA6
{
    public class TestScene : Scene
    {
        private Event[] _erkkiEvents;
        private Event _signEvent;
        private float _elapsedSongTime = 0;

        public TestScene()
        {
            // Load basics in constructor
            Theme = Static.Content.Load<Song>("linktothepast/darkworld");
            ExitTransitions[Direction.Right] = TransitionType.FadeToBlack;
        }

        protected override void Load()
        {
            // Init events and map objects in Load
            var erkki = new Erkki();
            erkki.Position = TileMap.ConvertTileXY(22, 22);
            erkki.Trigger += StartErkkiDialog;

            var bat = new Bat();
            bat.Position = new Vector2(180, 200);

            var guard = new Guard();
            guard.Position = TileMap.ConvertTileXY(4, 22);

            var signEventTrigger = new EventTrigger(TileMap.ConvertTileXY(9, 4), 14, 14);
            _signEvent = new TextEvent(new Dialog("ZELDA'S TENT HOME"), signEventTrigger);
            signEventTrigger.Trigger += ReadSign;

            Add(erkki);
            //Add(bat);
            //Add(guard);
            Add(signEventTrigger);

            var msg1 = "Hi Zelda. Good thing\nyour awake. Zelda has\nbeen capture again!\nLooks Like Ganondorf is at it\nagain!\nplz hurry and save the world!!!\n123456790909\nvitu juu";
            var msg2 = "Simo ruumishuoneelta\nmoi. Oletko ajatellut kuolla?\nNyt se nimittäin kannattaa.";

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
                        new FaceEvent(erkki, Direction.Right),
                        new WaitEvent(1),
                        new TextEvent(new Dialog(msg2), erkki),
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

        /*
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _elapsedSongTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (_elapsedSongTime > 0.1f)
            {
                _elapsedSongTime = 0f;
                MediaPlayer.Play(_song, new System.TimeSpan(0,0,Utility.RandomBetween(0,30)));
            }
        }
        */
        
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