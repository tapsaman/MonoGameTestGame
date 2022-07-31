using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTestGame
{
    public class Rendering
    {
        public static Color BackgroundColor = new Color(207, 55, 55);
        private static Rendering _rendering;
        private static RenderTarget2D _nativeRenderTarget; 
        private static Rectangle _actualScreenRectangle;
        private static Effect _postEffect = null;
        private static int _damageEffectIter = 0;
        private static event Action<GameTime> _doAfterPostEffect;
        private static float _noiseSeed = 0f;
        
        public static void Init(GraphicsDevice graphicsDevice)
        {
            _rendering = new Rendering();

            _nativeRenderTarget = new RenderTarget2D(graphicsDevice, StaticData.NativeWidth, StaticData.NativeHeight);
            _actualScreenRectangle = new Rectangle(0, 0, StaticData.BackBufferWidth, StaticData.BackBufferHeight);
            
            StaticData.Graphics.PreferredBackBufferWidth = StaticData.BackBufferWidth;
            StaticData.Graphics.PreferredBackBufferHeight = StaticData.BackBufferHeight;
            StaticData.Graphics.ApplyChanges();
        }

        public static void Start(GraphicsDevice graphicsDevice)
        {
            graphicsDevice.SetRenderTarget(_nativeRenderTarget);
            graphicsDevice.Clear(BackgroundColor);
            // Need to use wrapping sampler state for repeating textures (shaded wood overlay)
            StaticData.SpriteBatch.Begin(samplerState: SamplerState.PointWrap);
        }

        public static void Start(GraphicsDevice graphicsDevice, Effect effect)
        {
            graphicsDevice.SetRenderTarget(_nativeRenderTarget);
            graphicsDevice.Clear(BackgroundColor);
            // Need to use wrapping sampler state for repeating textures (shaded wood overlay)
            StaticData.SpriteBatch.Begin(samplerState: SamplerState.PointWrap, effect: effect);
        }

        public static void End(GraphicsDevice graphicsDevice, GameTime gameTime)
        {
            StaticData.SpriteBatch.End();
            // after drawing the game at native resolution we can render _nativeRenderTarget to the backbuffer!
            // First set the GraphicsDevice target back to the backbuffer
            graphicsDevice.SetRenderTarget(null);
            // RenderTarget2D inherits from Texture2D so we can render it just like a texture
            StaticData.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, effect: _postEffect);
            StaticData.SpriteBatch.Draw(_nativeRenderTarget, _actualScreenRectangle, Color.White/*, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f*/);
            StaticData.SpriteBatch.End();

            _doAfterPostEffect?.Invoke(gameTime);
        }

        public static void ChangeToDefault()
        {
            StaticData.SpriteBatch.End();
            StaticData.SpriteBatch.Begin(samplerState: SamplerState.PointWrap);
        }

        public static void ChangeToEffect(Effect effect)
        {
            StaticData.SpriteBatch.End();
            StaticData.SpriteBatch.Begin(samplerState: SamplerState.PointWrap, effect: effect);
        }

        public static void ChangeToDamageEffect()
        {
            StaticData.SpriteBatch.End();

            Shaders.EnemyDamage.Parameters["paramColorIter"].SetValue(_damageEffectIter++);
            if (_damageEffectIter == 3)
                _damageEffectIter = 0;

            StaticData.SpriteBatch.Begin(samplerState: SamplerState.PointWrap, effect: Shaders.EnemyDamage);
        }

        public static void ApplyPostEffect(Effect effect)
        {
            _postEffect = effect;
            _doAfterPostEffect -= _doAfterPostEffect;

            if (effect == Shaders.MildNoise || effect == Shaders.Noise)
            {
                _doAfterPostEffect += AfterNoiseEffect;
            }
        }

        private static void AfterNoiseEffect(GameTime gameTime)
        {
            //_noiseSeed += (float)gameTime.ElapsedGameTime.TotalSeconds * 2;
            _noiseSeed += 0.001f;
            _postEffect.Parameters["seed"].SetValue(_noiseSeed);
        }
    }
}