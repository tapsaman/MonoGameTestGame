
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine;
using TapsasEngine.Utilities;
using ZA6.Managers;

namespace ZA6.Animations
{
    public class Cartoon : Animation
    {
        private const int _BORDER_HEIGHT = 57;
        public DialogManager DialogManager;

        public Cartoon(DialogManager dialogManager)
        {
            DialogManager = dialogManager;
            var framePos = new Vector2(0, 57);

            Stages = new AnimationStage[]
            {
                new FadeStage(3f),
                new ImageStage("Images/Cartoon/01_a", 2f, framePos),
                new ImageStage("Images/Cartoon/01_b", .5f, framePos),
                new ImageStage("Images/Cartoon/01_c", .2f, framePos),
                new ImageStage("Images/Cartoon/01_b", .2f, framePos),
                new ImageStage("Images/Cartoon/01_c", 1f, framePos),
                new ImageStage("Images/Cartoon/01_b", .2f, framePos),
                new ImageStage("Images/Cartoon/01_d", .2f, framePos),
                new ImageStage("Images/Cartoon/01_e", .2f, framePos),
                new ImageStage("Images/Cartoon/01_f", 2f, framePos),
                new ImageStage("Images/Cartoon/01_g", .15f, framePos),
                new ImageStage("Images/Cartoon/01_h", 1f, framePos),
                new ImageStage("Images/Cartoon/01_i", 1f, framePos),
                new ImageStage("Images/Cartoon/02_a", .5f, framePos),
                new Stage02(framePos),
                new Stage03A(framePos),
                new Stage03B(framePos),
                new Stage04A(framePos),
                new Stage04B(framePos),
                new Stage05(framePos),
            };
        }

        private class FadeStage : AnimationStage
        {
            private Texture2D _img;
            private RenderTarget2D _firstRenderTarget;
            private float _change;

            public FadeStage(float time)
            {
                StageTime = time;
                _firstRenderTarget = new RenderTarget2D(Static.Renderer.Device, Static.NativeWidth, Static.NativeHeight);
                _img = Static.Content.Load<Texture2D>("Images/Cartoon/01_a");
            }
            
            public override void Update(float elapsedTime)
            {
                _change = elapsedTime / (float)StageTime;
            }
            
            public override void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.End();
                Static.Renderer.Device.SetRenderTarget(_firstRenderTarget);
                spriteBatch.Begin(samplerState: SamplerState.AnisotropicWrap);

                var firstRenderScale = new Vector2(ConcaveArc(_change));
                var imgSize = new Vector2(_img.Width, _img.Height);
                var renderedImgSize = imgSize * firstRenderScale;

                spriteBatch.Draw(_img, Vector2.Zero, null, Color.White, 0, Vector2.Zero, firstRenderScale, SpriteEffects.None, 0);
                spriteBatch.End();
                Static.Renderer.Start();
                spriteBatch.Draw(_firstRenderTarget, new Vector2(0, 57), null, Color.White, 0, Vector2.Zero, imgSize / renderedImgSize, SpriteEffects.None, 0);
                //spriteBatch.Draw(_firstRenderTarget, new Vector2(0, 57), null, Color.White);

                
                Utility.DrawOverlay(spriteBatch, new Color(1f - _change, 1f - _change, 1f - _change, 1f - _change));
            }

            private float ConcaveArc(float x)
            {
                return (float)Math.Tan(x * 1.45f) / 8f;
            }
        }

        public class ImageStage : AnimationStage
        {
            private Texture2D Image;
            private Vector2 Position;

            public ImageStage(string imagePath, float time, Vector2 position)
            {
                Image = Static.Content.Load<Texture2D>(imagePath);
                StageTime = time;
                Position = position;
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(Image, Position, Color.White);
            }
        }

        public class Stage02 : AnimationStage
        {
            private Texture2D _imgBg;
            private Texture2D _imgHead;
            private float _headY;
            private Vector2 Position;

            public Stage02(Vector2 position)
            {
                _imgBg = Static.Content.Load<Texture2D>("Images/Cartoon/02_a");
                _imgHead = Static.Content.Load<Texture2D>("Images/Cartoon/02_b");
                Position = position;
            }

            public override void Enter()
            {
                _headY = 110;
                ((Cartoon)Animation).DialogManager.Load(new Dialog(
                    "\"Mom!!! Are you OK?\"",
                    "\"I went outside to the forest\nand there was an evil elf and...\nI got pulled into a giant hole!\""
                ));
            }

            public override void Update(float elapsedTime)
            {
                _headY = (float)Math.Max(0, 110 - elapsedTime * 110);
                IsDone = IsDone || ((Cartoon)Animation).DialogManager.IsDone;

            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(_imgBg, Position, Color.White);
                spriteBatch.Draw(_imgHead, Position.AddY(_headY), Color.White);
            }
        }

        public class Stage03A : AnimationStage
        {
            private Texture2D _img;
            private Vector2 Position;

            public Stage03A(Vector2 position)
            {
                _img = Static.Content.Load<Texture2D>("Images/Cartoon/03_a");
                Position = position;
            }

            public override void Enter()
            {
                ((Cartoon)Animation).DialogManager.Load(new Dialog(
                    "\"Elf? And a hole? There's not\neven a forest nearby!\""
                ));
            }

            public override void Update(float elapsedTime)
            {
                IsDone = IsDone || ((Cartoon)Animation).DialogManager.IsDone;

            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(_img, Position, Color.White);
            }
        }

        public class Stage03B : AnimationStage
        {
            private Texture2D _img;
            private Vector2 Position;

            public Stage03B(Vector2 position)
            {
                _img = Static.Content.Load<Texture2D>("Images/Cartoon/03_b");
                Position = position;
            }

            public override void Enter()
            {
                ((Cartoon)Animation).DialogManager.Load(new Dialog(
                    "\"I think that's just another\none of your silly dreams!\"",
                    "\"You've slept for so long,\nyou must be really hungy!\""
                ));
            }

            public override void Update(float elapsedTime)
            {
                IsDone = IsDone || ((Cartoon)Animation).DialogManager.IsDone;
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(_img, Position, Color.White);
            }
        }

        public class Stage04A : AnimationStage
        {
            private Texture2D _img;
            private Texture2D _imgBg;
            private Color _imgColor;
            private float _imgY;
            private Vector2 Position;

            public Stage04A(Vector2 position)
            {
                _imgBg = Static.Content.Load<Texture2D>("Images/Cartoon/04_a");
                _img = Static.Content.Load<Texture2D>("Images/Cartoon/04_b");
                Position = position;
            }

            public override void Enter()
            {
                _imgColor = new Color(0f, 0f, 0f, 0f);
                _imgY = -3f;

                ((Cartoon)Animation).DialogManager.Load(new Dialog(
                    "\"Come on, let's eat!\nI made pizza!\""
                ));
            }

            public override void Update(float elapsedTime)
            {
                _imgY = (float)Math.Min(57, (elapsedTime * 45f) - 3);
                var c = (float)Math.Min(1f, elapsedTime);
                _imgColor = new Color(c, c, c, c);
                IsDone = IsDone || ((Cartoon)Animation).DialogManager.IsDone;
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(_imgBg, Position, Color.White);
                spriteBatch.Draw(_img, new Vector2(0, _imgY), _imgColor);
            }
        }

         public class Stage04B : AnimationStage
        {
            private Texture2D _img;
            private Texture2D _imgBg;
            private Color _imgColor;
            private float _imgY;
            private Vector2 Position;

            public Stage04B(Vector2 position)
            {
                _imgBg = Static.Content.Load<Texture2D>("Images/Cartoon/04_a");
                _img = Static.Content.Load<Texture2D>("Images/Cartoon/04_c");
                Position = position;
            }

            public override void Enter()
            {
                _imgColor = new Color(0f, 0f, 0f, 0f);
                _imgY = 167f;

                ((Cartoon)Animation).DialogManager.Load(new Dialog(
                    "\"Yippiee, pizza!\" Link exclaimed.\n\"My favorite!\""
                ));
            }

            public override void Update(float elapsedTime)
            {
                _imgY = (float)Math.Max(57, 114 - (elapsedTime * 60f));
                var c = (float)Math.Min(1f, elapsedTime);
                _imgColor = new Color(c, c, c, c);
                IsDone = IsDone || ((Cartoon)Animation).DialogManager.IsDone;
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(_imgBg, Position, Color.White);
                spriteBatch.Draw(_img, new Vector2(0, _imgY), _imgColor);
            }
        }

        public class Stage05 : AnimationStage
        {
            private Texture2D _img;
            private Vector2 Position;

            public Stage05(Vector2 position)
            {
                _img = Static.Content.Load<Texture2D>("Images/Cartoon/05");
                Position = position;
            }

            public override void Enter()
            {
                ((Cartoon)Animation).DialogManager.Load(new Dialog(
                    "So Link and his Mom ate\nand laughed, they watched\nStar Trek their tummies full\nof pepperoni, cheese and basil\n(except for Link who picked\nhis basil off) and they drank\ncoke and watched The Next\nGeneration season 3\nuntil night-time when Link\nremembered he lives alone\nin a fucking tent."
                ));
            }

            public override void Update(float elapsedTime)
            {
                IsDone = IsDone || ((Cartoon)Animation).DialogManager.IsDone;
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(_img, Position, Color.White);
            }
        }
    }
}