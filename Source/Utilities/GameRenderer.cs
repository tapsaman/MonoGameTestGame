using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Utilities;

namespace ZA6.Utilities
{
    public class GameRenderer : PointStretchRenderer
    {
        private RenderTarget2D _startMenuSceneRenderTarget;
        private int _damageEffectIter = 0;
        private int _highlightEffectIter = 0;
        private event Action<GameTime> _doAfterPostEffect;
        private float _noiseSeed = 0f;
        private float _spotlightSize = 0f;

        public GameRenderer(GraphicsDevice device, GraphicsDeviceManager deviceManager)
            : base(device, deviceManager) {}

        public override void Init(int nativeWidth, int nativeHeight, RenderResolution resolution)
        {
            base.Init(nativeWidth, nativeHeight, resolution);
            _startMenuSceneRenderTarget = new RenderTarget2D(Device, NativeWidth * 2, NativeHeight * 2);
        }

        public override void PostDraw()
        {
            Static.DevUtils.Draw(Static.SpriteBatch);
            //_doAfterPostEffect.Invoke();
        }

        public void StartMenuStart()
        {
            Device.SetRenderTarget(_startMenuSceneRenderTarget);
            Device.Clear(BackgroundColor);
            SpriteBatch.Begin(samplerState: SamplerState.LinearClamp);
        }

        public void StartMenuStartUI()
        {
            SpriteBatch.End();

            Device.SetRenderTarget(_nativeRenderTarget);
            SpriteBatch.Begin(samplerState: SamplerState.LinearClamp);
            SpriteBatch.Draw(_startMenuSceneRenderTarget, new Rectangle(0, 0, Static.NativeWidth, Static.NativeHeight), Color.White);
            SpriteBatch.End();
            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        }


        public void StartMenuEnd()
        {
            SpriteBatch.End();

            Device.SetRenderTarget(null);
            /*SpriteBatch.Begin(samplerState: SamplerState.LinearClamp);
            SpriteBatch.Draw(_startMenuSceneRenderTarget, new Rectangle(0, 0, Static.NativeWidth, Static.NativeHeight), Color.White);
            SpriteBatch.End();*/
            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            SpriteBatch.Draw(_nativeRenderTarget, ScreenRectangle, Color.White);
            Static.DevUtils.Draw(Static.SpriteBatch);
            SpriteBatch.End();
        }

        public void ChangeToDamageEffect()
        {
            ChangeToEffect(Shaders.EnemyDamage);

            Shaders.EnemyDamage.Parameters["paramColorIter"].SetValue(_damageEffectIter++);
            if (_damageEffectIter == 3)
                _damageEffectIter = 0;
        }

        public void ChangeToHighlightEffect()
        {
            ChangeToEffect(Shaders.Highlight);

            Shaders.Highlight.Parameters["paramIter"].SetValue(_highlightEffectIter++);
            if (_highlightEffectIter == 2)
                _highlightEffectIter = 0;
        }

        public override void ApplyPostEffect(Effect effect)
        {
            base.ApplyPostEffect(effect);

            _doAfterPostEffect -= _doAfterPostEffect;

            if (effect == Shaders.MildNoise || effect == Shaders.Noise)
            {
                _doAfterPostEffect += AfterNoiseEffect;
            }
            /*else if (effect == Shaders.Spotlight)
            {
                _spotlightSize = 0f;
                _postEffect.Parameters["size"].SetValue(_spotlightSize);
                _postEffect.Parameters["target"].SetValue(Static.Player.Hitbox.Rectangle.Center / Static.NativeSize);
                _doAfterPostEffect += AfterSpotlightEffect;
            }*/
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
            _postEffect.Parameters["target"].SetValue((Static.Player.Hitbox.Rectangle.Center + Static.Scene.DrawOffset) / Static.NativeSize);
        }
    }
}