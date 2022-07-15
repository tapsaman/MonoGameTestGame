using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTestGame.Models
{
    public class Animation
    {
        public static int DefaultFrameWidth = 40;
        public static int DefaultFrameHeight = 50;
        public int CurrentFrame;
        public int FrameCount;
        public int FrameHeight; //{ get { return Texture.Height; }}
        public int FrameWidth; //{ get { return Texture.Width / FrameCount; }}
        public float FrameDuration;

        public bool IsLooping;

        public Texture2D Texture { get; private set; }

        public int TextureYPos;
        public int TextureXPos;

        public Animation(Texture2D texture, int frameCount, int textureYPos = 0, int textureXPos = 0, bool isLooping = true, float frameDuration = 0.1f)
        {
            Texture = texture;
            FrameCount = frameCount;
            TextureYPos = textureYPos;
            TextureXPos = textureXPos;
            IsLooping = isLooping;
            FrameDuration = frameDuration;
            FrameWidth = DefaultFrameWidth;
            FrameHeight = DefaultFrameHeight;
        }
    }
}