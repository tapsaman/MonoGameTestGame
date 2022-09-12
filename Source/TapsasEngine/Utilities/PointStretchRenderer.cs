using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine.Models;

namespace TapsasEngine.Utilities
{
    public class PointStretchRenderer : Renderer
    {
        public Color BackgroundColor = new Color(207, 55, 55);
        public int NativeWidth { get; private set; }
        public int NativeHeight { get; private set; }
        public int DrawWidth { get; private set; }
        public int DrawHeight { get; private set; }
        public Vector2 NativeSizeMultiplier { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }
        
        public RenderResolution Resolution
        {
            get => _resolution;
            set
            {
                _resolution = value;

                if (_resolution.Stretched)
                    ApplyStretchedFullscreenResolution();
                else if (_resolution.IsFullscreen)
                    ApplyFullscreenResolution();
                else
                    ApplyResolution(_resolution.Width, _resolution.Height);
            
                Sys.Debug(
                    "New resoultion " + Resolution +
                    " set " + ScreenRectangle +
                    " screen " + Device.Adapter.CurrentDisplayMode
                );
            }
        }

        public Rectangle ScreenRectangle
        {
            get => _screenRectangle;
            private set
            {
                _screenRectangle = value;
                ScreenPosition = new Vector2(value.X, value.Y);
            }
        }

        public Vector2 ScreenPosition { get; private set; }
        public Action OnPostDraw;
        protected RenderTarget2D _nativeTarget;
        protected RenderTarget2D _postEffectTarget;
        protected RenderTarget2D _uiTarget;
        protected Effect _currentEffect;
        protected Effect _postEffect;
        public Effect FinalEffect;
        private RenderResolution _resolution;
        private Rectangle _screenRectangle;
        private DepthStencilState _depthStencil1;
        private DepthStencilState _depthStencil2;
        private List<IDraw> _uiLayer;
        private bool _drawingUI;

        public PointStretchRenderer(GraphicsDevice device, GraphicsDeviceManager deviceManager)
            : base(device, deviceManager)
        {
            SpriteBatch = new SpriteBatch(Device);

            _uiLayer = new List<IDraw>();

            _depthStencil1 = new DepthStencilState
            {
                StencilEnable = true,
                StencilFunction = CompareFunction.Always,
                StencilPass = StencilOperation.Replace,
                ReferenceStencil = 1,
                DepthBufferEnable = false,
            };

            _depthStencil2 = new DepthStencilState
            {
                StencilEnable = true,
                StencilFunction = CompareFunction.LessEqual,
                StencilPass = StencilOperation.Keep,
                ReferenceStencil = 1,
                DepthBufferEnable = false,
            };
        }

        public virtual void Init(int nativeWidth, int nativeHeight, RenderResolution resolution)
        {
            NativeWidth = nativeWidth;
            NativeHeight = nativeHeight;
            Resolution = resolution;
            _nativeTarget = new RenderTarget2D(Device, NativeWidth, NativeHeight);
            _postEffectTarget = new RenderTarget2D(Device, NativeWidth, NativeHeight);
            _uiTarget = new RenderTarget2D(Device, NativeWidth, NativeHeight, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
        }

        public void Start()
        {
            Device.SetRenderTarget(_uiTarget);
            Device.Clear(Color.Transparent);

            Device.SetRenderTarget(_nativeTarget);
            Device.Clear(BackgroundColor);

            // Need to use wrapping sampler state for repeating textures (shaded wood overlay)
            SpriteBatch.Begin(samplerState: SamplerState.PointWrap);
            _currentEffect = null;
        }

        public void ChangeToUITarget()
        {
            SpriteBatch.End();
            Device.SetRenderTarget(_uiTarget);
            Device.Clear(Color.Transparent);
            //Device.Clear(Color.Transparent);
            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointWrap);
            //SpriteBatch.Begin(samplerState: SamplerState.PointWrap, blendState: BlendState.AlphaBlend);
            _drawingUI = true;
        }

        public void DrawOnUILayer(IDraw drawable)
        {
            _uiLayer.Add(drawable);
        }

        public void End()
        {
            /*if (false && !_mainIsDrawn)
            {
                SpriteBatch.End();

                Device.SetRenderTarget(_secondTarget);
                SpriteBatch.Begin(samplerState: SamplerState.PointWrap, effect: _postEffect);
                SpriteBatch.Draw(_nativeTarget, Vector2.Zero, Color.White);
                _mainIsDrawn = true;
            }*/

            if (_uiLayer.Count != 0)
            {
                if (!_drawingUI)
                {
                    ChangeToUITarget();
                }

                foreach (var item in _uiLayer)
                {
                    item.Draw(SpriteBatch);
                }

                _uiLayer.Clear();
            }

            _drawingUI = false;

            // Apply post effect
            SpriteBatch.End();
            Device.SetRenderTarget(_postEffectTarget);
            Device.Clear(BackgroundColor);
            SpriteBatch.Begin(samplerState: SamplerState.PointWrap, effect: _postEffect);
            SpriteBatch.Draw(_nativeTarget, Vector2.Zero, Color.White);

            SpriteBatch.End();
            // after drawing the game at native resolution we can render _nativeRenderTarget to the backbuffer!
            // First set the GraphicsDevice target back to the backbuffer
            Device.SetRenderTarget(null);
            // RenderTarget2D inherits from Texture2D so we can render it just like a texture
            
            //SpriteBatch.Begin(samplerState: SamplerState.PointClamp, effect: FinalEffect, blendState: BlendState.AlphaBlend);
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, _depthStencil1, null, null);
           
            SpriteBatch.Draw(_postEffectTarget, ScreenRectangle, Color.White);
            SpriteBatch.End();
            //SpriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, _depthStencil2, null, FinalEffect);
            SpriteBatch.Draw(_uiTarget, ScreenRectangle, Color.White);
            
            OnPostDraw?.Invoke();
            
            SpriteBatch.End();

            if (_postEffect is Shader shader)
            {
                shader.Update();
            }
        }

        public void ChangeToEffect(Effect effect)
        {
            SpriteBatch.End();
            SpriteBatch.Begin(samplerState: SamplerState.PointWrap, effect: effect);
            _currentEffect = effect;

            if (effect is Shader shader)
            {
                shader.Update();
            }
        }

        public void ChangeToDefault()
        {
            if (_currentEffect == null)
                return;
            
           SpriteBatch.End();
           SpriteBatch.Begin(samplerState: SamplerState.PointWrap);
            _currentEffect = null;
        }

        public virtual void ApplyPostEffect(Effect effect)
        {
            _postEffect = effect;
        }

        private void ApplyFullscreenResolution()
        {
            int screenW = Device.Adapter.CurrentDisplayMode.Width;
            int screenH = Device.Adapter.CurrentDisplayMode.Height;
            int width, height, screenX, screenY;

            if (screenW > screenH)
            {
                NativeSizeMultiplier = new Vector2((float)screenH / NativeHeight);
                width = (int)(NativeWidth * NativeSizeMultiplier.X);
                height = screenH;
                screenX = (int)((screenW - width) / 2);
                screenY = 0;
            }
            else
            {
                NativeSizeMultiplier = new Vector2((float)screenW / NativeWidth);
                width = screenW;
                height = (int)(NativeHeight * NativeSizeMultiplier.Y);
                screenX = 0;
                screenY = (int)((screenH - height) / 2);
            }

            ScreenRectangle = new Rectangle(screenX, screenY, width, height);
            DeviceManager.IsFullScreen = true;
            DeviceManager.PreferredBackBufferWidth = screenW;
            DeviceManager.PreferredBackBufferHeight = screenH;
            DeviceManager.ApplyChanges();
        }

        private void ApplyResolution(int width, int height)
        {
            NativeSizeMultiplier = new Vector2(
                (float)width / NativeWidth,
                (float)height / NativeHeight
            );
            ScreenRectangle = new Rectangle(0, 0, width, height);
            
            DeviceManager.IsFullScreen = false;
            DeviceManager.PreferredBackBufferWidth = width;
            DeviceManager.PreferredBackBufferHeight = height;
            DeviceManager.ApplyChanges();
        }

        private void ApplyStretchedFullscreenResolution()
        {
            int screenW = Device.Adapter.CurrentDisplayMode.Width;
            int screenH = Device.Adapter.CurrentDisplayMode.Height;
            
            NativeSizeMultiplier = new Vector2(
                (float)screenW / NativeWidth,
                (float)screenH / NativeHeight
            );
            ScreenRectangle = new Rectangle(0, 0, screenW, screenH);

            DeviceManager.IsFullScreen = true;
            DeviceManager.PreferredBackBufferWidth = screenW;
            DeviceManager.PreferredBackBufferHeight = screenH;
            DeviceManager.ApplyChanges();
        }
    }

    public class RenderResolution
    {
        public int Width;
        public int Height;
        public bool IsFullscreen;
        public bool Stretched;
        public string Description;

        public Vector2 Size
        {
            get => new Vector2(Width, Height);
        }

        public RenderResolution(int width, int height, string description = null)
        {
            Width = width;
            Height = height;
            Description = description;
        }

        public override string ToString()
        {
            if (Description != null)
                return Description;
            if (Stretched)
                return "Stretched fullscreen";
            if (IsFullscreen)
                return "Fullscreen";
            
            return Width + "x" + Height;
        }
    }
}