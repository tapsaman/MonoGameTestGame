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

        protected override void Load()
        {
            _overlay = StaticData.Content.Load<Texture2D>("linktothepast/shadedwoodtransparency");
            Theme = StaticData.Content.Load<Song>("linktothepast/forest");
            TileMap = new MapC1();
        }

        public override void DrawOverlay(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_overlay, Vector2.Zero + DrawOffset * 0.5f, new Rectangle(0, 0, StaticData.NativeWidth * 2, StaticData.NativeHeight * 3), new Color(255, 255, 255, 0.5f));
        }
    }
}