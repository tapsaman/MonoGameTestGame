using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGameTestGame.Managers;
using MonoGameTestGame.Models;

namespace MonoGameTestGame
{
    public class TestScene : Scene
    {
        private Event[] _enemyEvents;
        private Event _signEvent;
        private Song _song;

        public TestScene(Player player): base(player) {}

        protected override void Load()
        {
            _song = StaticData.Content.Load<Song>("linktothepast-darkworld");
            TileMap = new TestMap();

            var enemy = new Enemy();
            enemy.Position = new Vector2(TileMap.ConvertTileX(22), TileMap.ConvertTileY(22));
            enemy.Trigger += StartEnemyDialog;

            var bat = new Bat();
            bat.Position = new Vector2(180, 200);

            var signEventTrigger = new EventTrigger(TileMap.GetPosition(9, 4), 14, 14);
            _signEvent = new TextEvent(new Dialog("ZELDA'S TENT HOME"), signEventTrigger);
            signEventTrigger.Trigger += ReadSign;

            Add(enemy);
            Add(bat);
            Add(signEventTrigger);

            var enemyDialog1 = new Dialog(
                "Hi Zelda. Good thing your awake.\nZelda has been capture again!\nLooks Like Ganondorf is at it again!",
                "Simo ruumishuoneelta\nmoi. Oletko ajatellut kuolla?\nNyt se nimittäin kannattaa."
            )
            {
                Title = "Erkki"
            };

            var enemyDialog2 = new Dialog(
                "Sample text"
            )
            {
                Title = "Erkki"
            };

            _enemyEvents = new Event[]
            {
                new ConditionEvent(EventStore.Scene, "spoken to erkki")
                {
                    IfFalse = new Event[]
                    {
                        new FaceEvent(enemy, Player),
                        new TextEvent(enemyDialog1, enemy),
                        new FaceEvent(enemy, Direction.Down),
                        new SaveValueEvent(EventStore.Scene, "spoken to erkki", true)
                    },
                    IfTrue = new Event[]
                    {
                        new FaceEvent(enemy, Player),
                        new TextEvent(enemyDialog2, enemy),
                        new FaceEvent(enemy, Direction.Down)
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
        
        private void StartEnemyDialog()
        {
            //DialogManager.Load(_enemyDialog, true);
            //StateMachine.TransitionTo("Dialog");
            EventManager.Load(_enemyEvents);
        }

        private void ReadSign()
        {
            EventManager.Load(_signEvent);
        }
    }
}