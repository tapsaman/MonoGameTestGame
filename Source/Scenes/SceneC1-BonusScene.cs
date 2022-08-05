using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGameTestGame.Managers;
using MonoGameTestGame.Models;

namespace MonoGameTestGame
{
    public class SceneC1 : Scene
    {
        private Texture2D _overlay;

        public SceneC1()
        {
            Theme = Static.Content.Load<Song>("linktothepast/forest");
            TileMap = new MapC1();
        }

        protected override void Load()
        {
            _overlay = Static.Content.Load<Texture2D>("linktothepast/shadedwoodtransparency");
        }

        public override void DrawOverlay(SpriteBatch spriteBatch)
        {
            // Reduce native size for panning
            var overlayPosition = OverlayOffset + DrawOffset * 0.5f - Static.NativeSize;
            
            spriteBatch.Draw(
                _overlay,
                overlayPosition,
                new Rectangle(0, 0, Width * 3, Height * 2),
                new Color(255, 255, 255, 0.5f)
            );
        }
    }
}