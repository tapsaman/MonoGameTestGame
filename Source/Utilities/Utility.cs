using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTestGame
{
    public static class Utility
    {
        // Must be disposed!
        public static Texture2D CreateColorTexture(int width, int height, Color color)
        {
            var texture = new Texture2D(StaticData.Graphics.GraphicsDevice, width, height);
            var data = new Color[width * height];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Color.White;
            }

            texture.SetData(data);

            return texture;
        }
    }
}