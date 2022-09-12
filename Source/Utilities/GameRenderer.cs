using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Utilities;

namespace ZA6.Utilities
{
    public class GameRenderer : PointStretchRenderer
    {
        private RenderTarget2D _startMenuSceneRenderTarget;

        public GameRenderer(GraphicsDevice device, GraphicsDeviceManager deviceManager)
            : base(device, deviceManager) {}

        public override void Init(int nativeWidth, int nativeHeight, RenderResolution resolution)
        {
            base.Init(nativeWidth, nativeHeight, resolution);
            _startMenuSceneRenderTarget = new RenderTarget2D(Device, NativeWidth * 2, NativeHeight * 2);
            OnPostDraw += PostDraw;
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

            Device.SetRenderTarget(_nativeTarget);
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
            SpriteBatch.Draw(_nativeTarget, ScreenRectangle, Color.White);
            Static.DevUtils.Draw(Static.SpriteBatch);
            SpriteBatch.End();
        }

        private void PostDraw()
        {
            Static.DevUtils.Draw(Static.SpriteBatch);
            //_doAfterPostEffect.Invoke();

            if (Static.Game.StateMachine.CurrentStateKey == "MainMenu")
            {
                DrawPlayTimeText();   
            }
        }

        private void DrawPlayTimeText()
        {
            string playTimeText = "Play time: " + Static.PlayTimeTimer.HourMinuteSecondString;
            Vector2 size = Static.Font.MeasureString(playTimeText);

            Static.SpriteBatch.DrawString(
                Static.Font,
                playTimeText,
                Resolution.Size - size,
                Color.White
            );
        }
    }
}