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
        private Dialog _enemyDialog = new Dialog("Sample text", "Simo ruumishuoneelta moi.\nOletko ajatellut kuolla?\nNyt se nimittäin kannattaa.");
        private Event[] _enemyEvents;

        public override void Load()
        {
            Song song = StaticData.Content.Load<Song>("linktothepast-darkworld");
            //MediaPlayer.Play(song);
            MediaPlayer.Volume = 0.2f;
            MediaPlayer.IsRepeating = true;
            TileMap = new TestMap();

            Player = new Player(new Vector2(100, 100));
            Enemy enemy = new Enemy(new Vector2(TileMap.ConvertTileX(22), TileMap.ConvertTileY(22)));
            Bat bat = new Bat(new Vector2(250, 200));

            enemy.Trigger += StartEnemyDialog;

            DialogManager = new DialogManager();
            DialogManager.DialogEnd += QuitDialog;

            Add(enemy);
            Add(Player);
            //Add(bat);

            Dictionary<string, State> states = new Dictionary<string, State>()
            {
                { "Default", new GameStateDefault(this) },
                { "Dialog", new GameStateDialog(this) }
            };

            StateMachine = new StateMachine(states, "Default");
            EventManager = new EventManager();
            _enemyEvents = new Event[]
            {
                new FaceEvent(enemy, Player),
                new TextEvent(_enemyDialog, enemy),
                new FaceEvent(enemy, Direction.Down)
            };
        }
        public override void Update(GameTime gameTime)
        {
            EventManager.Update();
            StateMachine.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            TileMap.Draw(spriteBatch);
            CollidingEntities.Sort();

            foreach (var mapEntity in CollidingEntities)
            {
                mapEntity.Draw(spriteBatch);
            }
            foreach (var mapEntity in UncollidingEntities)
            {
                mapEntity.Draw(spriteBatch);
            }

            Player.SwordHitbox.Draw(spriteBatch);
            DialogManager.Draw(spriteBatch);
            spriteBatch.DrawString(StaticData.Font, Player.Position.ToString(), new Vector2(500, 300), Color.Black);
        }
        private void StartEnemyDialog()
        {
            //DialogManager.Load(_enemyDialog, true);
            //StateMachine.TransitionTo("Dialog");
            EventManager.Load(_enemyEvents);
        }
        private void QuitDialog()
        {
            StateMachine.TransitionTo("Default");
        }
    }
}