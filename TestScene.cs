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
        private StateMachine _stateMachine;
        private Dialog _enemyDialog = new Dialog("asdasd", "fuu \"fuu\" ä'ä'-.,!#¤%&/()=?");

        public override void Load()
        {
            Song song = StaticData.Content.Load<Song>("linktothepast-darkworld");
            //MediaPlayer.Play(song);
            MediaPlayer.Volume = 0.2f;
            MediaPlayer.IsRepeating = true;
            TileMap = new ZeldaMap();

            Player = new Player();
            Enemy enemy = new Enemy();
            Bat bat = new Bat(new Vector2(250, 200));

            enemy.MapEntity.Trigger += StartEnemyDialog;

            DialogManager = new DialogManager();
            DialogManager.DialogEnd += QuitDialog;

            MapEntities = new MapEntity[]
            {
                enemy.MapEntity,
                bat,
                Player.MapEntity
            };

            Dictionary<string, State> states = new Dictionary<string, State>()
            {
                { "Default", new GameStateDefault(this) },
                { "Dialog", new GameStateDialog(this) }
            };

            _stateMachine = new StateMachine(states, "Default");
        }
        public override void Update(GameTime gameTime)
        {
            _stateMachine.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            TileMap.Draw(spriteBatch);

            foreach (var mapEntity in MapEntities)
            {
                mapEntity.Draw(spriteBatch);
            }

            Player.SwordHitbox.Draw(spriteBatch);
            DialogManager.Draw(spriteBatch);
            spriteBatch.DrawString(StaticData.Font, Player.MapEntity.Position.ToString(), new Vector2(500, 300), Color.Black);
        }
        private void StartEnemyDialog()
        {
            DialogManager.Load(_enemyDialog);
            _stateMachine.TransitionTo("Dialog");
        }
        private void QuitDialog()
        {
            _stateMachine.TransitionTo("Default");
        }
    }
}