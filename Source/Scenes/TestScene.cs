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

        public override void Load()
        {
            var song = StaticData.Content.Load<Song>("linktothepast-darkworld");
            MediaPlayer.Play(song);
            MediaPlayer.Volume = 0.2f;
            MediaPlayer.IsRepeating = true;
            TileMap = new TestMap();

            Player = new Player();
            Player.Position = new Vector2(100, 100);

            var enemy = new Enemy();
            enemy.Position = new Vector2(TileMap.ConvertTileX(22), TileMap.ConvertTileY(22));
            enemy.Trigger += StartEnemyDialog;

            var bat = new Bat();
            bat.Position = new Vector2(180, 200);

            var signEventTrigger = new EventTrigger(TileMap.GetPosition(9, 4), 14, 14);
            _signEvent = new TextEvent(new Dialog("ZELDA'S TENT HOME"), signEventTrigger);
            signEventTrigger.Trigger += ReadSign;

            Add(enemy);
            Add(Player);
            Add(bat);
            Add(signEventTrigger);

            var enemyDialog = new Dialog(
                "Sample text",
                "Simo ruumishuoneelta\nmoi. Oletko ajatellut kuolla?\nNyt se nimittäin kannattaa."
            )
            {
                Title = "Friend"
            };

            _enemyEvents = new Event[]
            {
                new FaceEvent(enemy, Player),
                new TextEvent(enemyDialog, enemy),
                new FaceEvent(enemy, Direction.Down)
            };
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