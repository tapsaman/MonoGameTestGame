using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTestGame
{
    public class Renderer
    {
        public Resolution Resolution
        {
            get { return _resolution; }
            set
            {
                switch(value)
                {
                    case Resolution.FullScreen:
                        ApplyFullScreenResolution();
                        break;
                    case Resolution._256x224:
                        ApplyResolution(256, 224);
                        break;
                    case Resolution._512x448:
                        ApplyResolution(512, 448);
                        break;
                    case Resolution._768x672:
                        ApplyResolution(768, 672);
                        break;
                    case Resolution._1024x896:
                        ApplyResolution(1024, 896);
                        break;
                    case Resolution._1234X1080:
                        ApplyResolution(1234, 1080);
                        break;
                    default:
                        throw new Exception("Unexpected resolution value " + value);
                }

                _resolution = value;

                Sys.Debug(
                    "New resoultion " + Resolution +
                    " set " + _actualScreenRectangle +
                    " screen " + _graphicsDevice.Adapter.CurrentDisplayMode
                );
            }
        }
        public float NativeSizeMultiplier { get; private set; }
        public int ScreenX { get; private set; }
        public int ScreenY { get; private set; }
        public Color BackgroundColor = new Color(207, 55, 55);
        private Resolution _resolution;
        private GraphicsDevice _graphicsDevice;
        private RenderTarget2D _nativeRenderTarget; 
        private Rectangle _actualScreenRectangle;
        private Effect _currentEffect = null;
        private Effect _postEffect = null;
        private int _damageEffectIter = 0;
        private event Action<GameTime> _doAfterPostEffect;
        private float _noiseSeed = 0f;
        private float _spotlightSize = 0f;
        
        public void Init(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;            
            _nativeRenderTarget = new RenderTarget2D(graphicsDevice, Static.NativeWidth, Static.NativeHeight);

            if (true)
            {
                Resolution = Resolution._768x672;
            }
            else
            {
                int screenW = _graphicsDevice.Adapter.CurrentDisplayMode.Width;
                int screenH = _graphicsDevice.Adapter.CurrentDisplayMode.Height;
                _actualScreenRectangle = new Rectangle(
                    0,
                    0,
                    screenW,
                    screenH
                );
                Static.Graphics.PreferredBackBufferWidth = _actualScreenRectangle.Width;
                Static.Graphics.PreferredBackBufferHeight = _actualScreenRectangle.Height;
                Static.Graphics.IsFullScreen = true;
                Sys.Debug("screenW " + screenW + "screenH " + screenH);
                Sys.Debug("New screen reso " + Resolution + " " + _actualScreenRectangle);
                Static.Graphics.ApplyChanges();
            }
            
            //Static.Graphics.PreferredBackBufferWidth = Static.BackBufferWidth;
            //Static.Graphics.PreferredBackBufferHeight = Static.BackBufferHeight;

            // Change the resolution to match your current desktop
            
        }

        public void Start()
        {
            _graphicsDevice.SetRenderTarget(_nativeRenderTarget);
            _graphicsDevice.Clear(BackgroundColor);
            // Need to use wrapping sampler state for repeating textures (shaded wood overlay)
            Static.SpriteBatch.Begin(samplerState: SamplerState.PointWrap);
            _currentEffect = null;
        }

        public void Start(Effect effect)
        {
            _graphicsDevice.SetRenderTarget(_nativeRenderTarget);
            _graphicsDevice.Clear(BackgroundColor);
            // Need to use wrapping sampler state for repeating textures (shaded wood overlay)
            Static.SpriteBatch.Begin(samplerState: SamplerState.PointWrap, effect: effect);
            _currentEffect = effect;
        }

        public void End(GameTime gameTime)
        {
            Static.SpriteBatch.End();
            // after drawing the game at native resolution we can render _nativeRenderTarget to the backbuffer!
            // First set the GraphicsDevice target back to the backbuffer
            _graphicsDevice.SetRenderTarget(null);
            // RenderTarget2D inherits from Texture2D so we can render it just like a texture
            Static.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, effect: _postEffect);
            Static.SpriteBatch.Draw(_nativeRenderTarget, _actualScreenRectangle, Color.White/*, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f*/);
            Static.SpriteBatch.End();

            _doAfterPostEffect?.Invoke(gameTime);
        }

        public void ChangeToDefault()
        {
            if (_currentEffect == null)
                return;
            
            Static.SpriteBatch.End();
            Static.SpriteBatch.Begin(samplerState: SamplerState.PointWrap);
            _currentEffect = null;
        }

        public void ChangeToEffect(Effect effect)
        {
            Static.SpriteBatch.End();
            Static.SpriteBatch.Begin(samplerState: SamplerState.PointWrap, effect: effect);
            _currentEffect = effect;
        }

        public void ChangeToDamageEffect()
        {
            Static.SpriteBatch.End();

            Shaders.EnemyDamage.Parameters["paramColorIter"].SetValue(_damageEffectIter++);
            if (_damageEffectIter == 3)
                _damageEffectIter = 0;

            Static.SpriteBatch.Begin(samplerState: SamplerState.PointWrap, effect: Shaders.EnemyDamage);
            _currentEffect = Shaders.EnemyDamage;
        }

        public void ApplyPostEffect(Effect effect)
        {
            _postEffect = effect;
            _doAfterPostEffect -= _doAfterPostEffect;

            if (effect == Shaders.MildNoise || effect == Shaders.Noise)
            {
                _doAfterPostEffect += AfterNoiseEffect;
            }
            else if (effect == Shaders.Spotlight)
            {
                _spotlightSize = 0f;
                _postEffect.Parameters["size"].SetValue(_spotlightSize);
                _postEffect.Parameters["target"].SetValue(Static.Player.Hitbox.Rectangle.Center / Static.NativeSize);
                _doAfterPostEffect += AfterSpotlightEffect;
            }
        }

        private void AfterNoiseEffect(GameTime gameTime)
        {
            //_noiseSeed += (float)gameTime.ElapsedGameTime.TotalSeconds * 2;
            _noiseSeed += 0.001f;
            _postEffect.Parameters["seed"].SetValue(_noiseSeed);
        }

        private void AfterSpotlightEffect(GameTime gameTime)
        {
            _spotlightSize += (float)gameTime.ElapsedGameTime.TotalSeconds * 0.8f;
            _postEffect.Parameters["size"].SetValue(_spotlightSize);
            _postEffect.Parameters["target"].SetValue(Static.Player.Hitbox.Rectangle.Center / Static.NativeSize);
        }

         private void ApplyFullScreenResolution()
        {
            int screenW = _graphicsDevice.Adapter.CurrentDisplayMode.Width;
            int screenH = _graphicsDevice.Adapter.CurrentDisplayMode.Height;
            int width, height;

            if (screenW > screenH)
            {
                NativeSizeMultiplier = (float)screenH / Static.NativeHeight;
                width = (int)(Static.NativeWidth * NativeSizeMultiplier);
                height = screenH;
                ScreenX = (int)((screenW - width) / 2);
                ScreenY = 0;
            }
            else
            {
                NativeSizeMultiplier = (float)screenW / Static.NativeWidth;
                width = screenW;
                height = (int)(Static.NativeHeight * NativeSizeMultiplier);
                ScreenX = 0;
                ScreenY = (int)((screenH - height) / 2);
            }

            _actualScreenRectangle = new Rectangle(ScreenX, ScreenY, width, height);
            Static.Graphics.IsFullScreen = true;
            Static.Graphics.PreferredBackBufferWidth = screenW;
            Static.Graphics.PreferredBackBufferHeight = screenH;
            Static.Graphics.ApplyChanges();
        }

        private void ApplyResolution(int width, int height)
        {
            NativeSizeMultiplier = (float)width / Static.NativeWidth;
            ScreenX = 0;
            ScreenY = 0;
            _actualScreenRectangle = new Rectangle(0, 0, width, height);
            
            Static.Graphics.IsFullScreen = false;
            Static.Graphics.PreferredBackBufferWidth = width;
            Static.Graphics.PreferredBackBufferHeight = height;
            Static.Graphics.ApplyChanges();
        }
    }
}