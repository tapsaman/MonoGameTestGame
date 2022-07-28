using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGameTestGame.Managers;
using MonoGameTestGame.Models;

namespace MonoGameTestGame
{
    public class SceneB1 : Scene
    {
        private Event _signEvent;
        private Song _song;
        private Texture2D _overlay;

        protected override void Load()
        {
            _overlay = StaticData.Content.Load<Texture2D>("linktothepast/shadedwoodtransparency");
            _song = StaticData.Content.Load<Song>("linktothepast/forest");
            TileMap = new MapB1();

            var signEventTrigger = new EventTrigger(TileMap.GetPosition(9, 4), 14, 14);
            _signEvent = new TextEvent(new Dialog("ZELDA'S TENT HOME"), signEventTrigger);
            signEventTrigger.Trigger += ReadSign;

            Add(signEventTrigger);
        }

        public override void Start()
        {
            MediaPlayer.Play(_song);
            MediaPlayer.Volume = 0.2f;
            MediaPlayer.IsRepeating = true;
            base.Start();
        }

        private void ReadSign(Character _)
        {
            EventManager.Load(_signEvent);
        }

        protected override void DrawOverlay(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_overlay, Vector2.Zero + DrawOffset * 0.5f, new Rectangle(0, 0, StaticData.NativeWidth * 2, StaticData.NativeHeight * 3), new Color(255, 255, 255, 0.5f));
        }
    }
}