using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using MonoGameTestGame.Managers;
using MonoGameTestGame.Models;

namespace MonoGameTestGame
{
    public class TestScene : Scene
    {
        private Event[] _erkkiEvents;
        private Event _signEvent;
        private Song _song;
        private float _elapsedSongTime = 0; 

        protected override void Load()
        {
            _song = StaticData.Content.Load<Song>("linktothepast/darkworld");
            TileMap = new TestMap();

            var erkki = new Erkki();
            erkki.Position = new Vector2(TileMap.ConvertTileX(22), TileMap.ConvertTileY(22));
            erkki.Trigger += StartErkkiDialog;

            var bat = new Bat();
            bat.Position = new Vector2(180, 200);

            var guard = new Guard();
            guard.Position = new Vector2(TileMap.ConvertTileX(4), TileMap.ConvertTileY(22));

            var signEventTrigger = new EventTrigger(TileMap.GetPosition(9, 4), 14, 14);
            _signEvent = new TextEvent(new Dialog("ZELDA'S TENT HOME"), signEventTrigger);
            signEventTrigger.Trigger += ReadSign;

            Add(erkki);
            Add(bat);
            Add(guard);
            Add(signEventTrigger);

            var erkkiDialog1 = new Dialog(
                "Hi Zelda. Good thing\nyour awake. Zelda has\nbeen capture again!\nLooks Like Ganondorf is at it\nagain!\nplz hurry and save the world!!!\n123456790909\nvitu juu",
                "Simo ruumishuoneelta\nmoi. Oletko ajatellut kuolla?\nNyt se nimittäin kannattaa."
            );

            var erkkiDialog2 = new Dialog(
                "Sample text"
            );

            _erkkiEvents = new Event[]
            {
                new ConditionEvent(EventStore.Scene, "spoken to erkki")
                {
                    IfFalse = new Event[]
                    {
                        new FaceEvent(erkki, Player),
                        new TextEvent(erkkiDialog1, erkki),
                        new FaceEvent(erkki, Direction.Down),
                        new SaveValueEvent(EventStore.Scene, "spoken to erkki", true)
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
            MediaPlayer.Play(_song);
            MediaPlayer.Volume = 0.2f;
            MediaPlayer.IsRepeating = true;
            base.Start();
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
            //DialogManager.Load(_erkkiDialog, true);
            //StateMachine.TransitionTo("Dialog");
            EventManager.Load(_erkkiEvents);
        }

        private void ReadSign(Character _)
        {
            EventManager.Load(_signEvent);
        }
    }
}