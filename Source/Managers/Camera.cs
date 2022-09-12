using System;
using Microsoft.Xna.Framework;
using TapsasEngine.Utilities;

namespace ZA6
{
    public class Camera
    {
        public bool Locked;
        public int Width = Static.NativeWidth;
        public int Height = Static.NativeHeight;
        public int SceneWidth;
        public int SceneHeight;
        public bool ScrollX;
        public bool ScrollY;
        public Vector2 ShakeVelocity = Vector2.Zero;
        public Vector2 ShakeOffset { get; private set; } = Vector2.Zero;
        private Vector2 _target;
        private Vector2 _offset;
        
        public Vector2 Target {
            get => _target;
            set
            {
                if (!Locked && _target != value)
                {
                    _target = value;
                    Update();
                }
            }
        }
        public Vector2 Offset
        {
            get => _offset;
            set
            {
                _offset = value;

                Screen = new Rectangle(
                    -(int)_offset.X,
                    -(int)_offset.Y,
                    Width,
                    Height
                );
            }
        }
        public Rectangle Screen { get; private set; }

        private void Update()
        {
            int x = (int)_target.X - Width / 2;
            int y = (int)_target.Y - Height / 2;
            
            if (!ScrollX)
                x = Math.Clamp(x, 0, SceneWidth - Width);
            if (!ScrollY)
                y = Math.Clamp(y, 0, SceneHeight - Height);

            if (ShakeVelocity != Vector2.Zero)
            {
                ShakeVelocity.X = (float)Math.Max(ShakeVelocity.X - 0.1, 0);
                ShakeVelocity.Y = (float)Math.Max(ShakeVelocity.Y - 0.1, 0);

                ShakeOffset = new Vector2(
                    Utility.RandomBetween((int)-ShakeVelocity.X, (int)ShakeVelocity.X),
                    Utility.RandomBetween((int)-ShakeVelocity.Y, (int)ShakeVelocity.Y)
                );

                x += (int)ShakeOffset.X;
                y += (int)ShakeOffset.Y;
            }

            Offset = new Vector2(-x, -y);
        }

    }
}