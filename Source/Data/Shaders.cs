using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine;
using TapsasEngine.Models;
using TapsasEngine.Utilities;

namespace ZA6
{
    public static class Shaders
    {
        public static Effect HighContrast;
        public static Effect Rainbow;
        public static Effect InvertedColor;
        public static NoiseShader Noise;
        public static Effect SpriteNoise;
        public static MildNoiseShader MildNoise;
        public static Effect Spotlight;
        public static Effect Crispy;
        public static Effect TestEffect;
        public static EnemyDamageShader EnemyDamage;
        public static WavyShader Wavy;
        public static HighlightShader Highlight;
        public static ColorRoundShader ColorRound;
        public static VHSShader VHS;
        public static EvaporateShader Evaporate;

        public static void Load()
        {
            HighContrast = Static.Content.Load<Effect>("Shaders/HighContrastEffect");
            Rainbow = Static.Content.Load<Effect>("Shaders/RainbowEffect");
            InvertedColor = Static.Content.Load<Effect>("Shaders/InvertedColorEffect");
            SpriteNoise = Static.Content.Load<Effect>("Shaders/SpriteNoiseEffect");
            Spotlight = Static.Content.Load<Effect>("Shaders/SpotlightEffect");
            Crispy = Static.Content.Load<Effect>("Shaders/CrispyEffect");
            TestEffect = Static.Content.Load<Effect>("Shaders/ShaderTestSpriteEffect");
            Evaporate = new EvaporateShader();
            EnemyDamage = new EnemyDamageShader();
            Wavy = new WavyShader();
            Highlight = new HighlightShader();
            Noise = new NoiseShader();
            MildNoise = new MildNoiseShader();
            VHS = new VHSShader();
        }

        public class EnemyDamageShader : Shader
        {
            private int _iter;
            private float _lastDrawCount;

            public EnemyDamageShader() : base("Shaders/EnemyDamageEffect") {}

            public override void Update()
            {
                if (_lastDrawCount != Tengine.DrawCount)
                {
                    _iter = _iter == 3 ? 0 : _iter + 1;
                    _lastDrawCount = Tengine.DrawCount;
                }

                Parameters["paramColorIter"].SetValue(_iter);
            }

            public override string ToString()
            {
                return base.ToString();
            }
        }

        public class HighlightShader : Shader
        {
            private int _iter;

            public HighlightShader() : base("Shaders/HighlightEffect") {}

            public override void Update()
            {
                Parameters["paramIter"].SetValue(_iter);

                if (++_iter == 2)
                {
                    _iter = 0;
                }
            }
        }

        public class WavyShader : Shader
        {
            private int _iter;
            private float _elapsedTime;
            private float _speed;

            public WavyShader() : base("Shaders/WavyEffect") {}

            public override void Update()
            {
                _elapsedTime += Tengine.Delta;
                Parameters["yOffset"].SetValue(_elapsedTime * _speed);
            }

            public void SetParameters(float waveWidth, float waveHeight, float speed)
            {
                Parameters["waveWidth"].SetValue(waveWidth);
                Parameters["waveHeight"].SetValue(waveHeight);
                _speed = speed;
            }
        }

        public class NoiseShader : Shader
        {
            private float _elapsedTime;

            public NoiseShader() : base("Shaders/NoiseEffect") {}

            public override void Update()
            {
                _elapsedTime += Tengine.Delta;
                Parameters["seed"].SetValue(_elapsedTime * 2);
            }
        }

        public class MildNoiseShader : Shader
        {
            private float _elapsedTime;

            public MildNoiseShader() : base("Shaders/MildNoiseEffect") {}

            public override void Update()
            {
                _elapsedTime += Tengine.Delta;
                //Parameters["seed"].SetValue(_elapsedTime * 2);
            }
        }

        public class ColorRoundShader : Shader
        {
            private float _elapsedTime;

            public ColorRoundShader() : base("Shaders/ColorRoundEffect") {}

            public override void Update()
            {
                _elapsedTime += Tengine.Delta;

                if (_elapsedTime > 4)
                    _elapsedTime = 0;
                
                Parameters["divider"].SetValue(_elapsedTime / 2);
            }
        }

        public class VHSShader : Shader
        {
            public float _scanlineSpeed = 1f;
            private float _elapsedTime;
            private float _changeSpeedTime = 1f;
            private bool _paused;
            private float _scanlineY;
            public float ScanlineHeight;
            public float _yOffset;
            private float _drawingScanlineHeight;

            public VHSShader() : base("Shaders/VHSEffect")
            {
                ScanlineHeight = _drawingScanlineHeight = 0f;
            }

            public override void Update()
            {
                _elapsedTime += Tengine.Delta;

                if (_elapsedTime > _changeSpeedTime)
                {
                    _changeSpeedTime = _elapsedTime + (float)Utility.RandomDouble() * 1.8f + 0.2f;
                    _scanlineSpeed = _paused ? 0f : (float)Utility.RandomDouble() * 1.6f + 0.4f;
                    ScanlineHeight = (float)Utility.RandomDouble() * 0.15f - 0.075f;
                }

                _scanlineY += Tengine.Delta * _scanlineSpeed;

                if (ScanlineHeight != _drawingScanlineHeight)
                {
                    _drawingScanlineHeight = ScanlineHeight > _drawingScanlineHeight
                        ? (float)Math.Min(ScanlineHeight, _drawingScanlineHeight + Tengine.Delta * 0.05f)
                        : (float)Math.Max(ScanlineHeight, _drawingScanlineHeight - Tengine.Delta * 0.05f);
                }
                if (_yOffset != 0f)
                {
                    _yOffset = (float)Math.Max(0, _yOffset - Tengine.Delta * 0.5f);
                }

                Parameters["seed"].SetValue(_elapsedTime);
                Parameters["scanlineY"].SetValue(_scanlineY);
                Parameters["scanlineHeight"].SetValue(_drawingScanlineHeight);
                Parameters["yOffset"].SetValue(_yOffset);
            }

            public void OnPause()
            {
                //Speed = 10f;
                //_drawingScanlineHeight = 0.15f;
                //_yOffset = 0.3f;
                _paused = true;
                _scanlineSpeed = 0f;
            }

            public void OnPlay()
            {
                _paused = false;
                _yOffset = 0.1f;
                _scanlineSpeed = 10f;
                _drawingScanlineHeight = 0.15f;
            }
        }

        public class EvaporateShader : Shader
        {
            public float Time = 4f;
            private float _elapsedTime;
            private bool _disappearing;

            public EvaporateShader() : base("Shaders/EvaporateEffect") {}

            public void Reset(bool disappear)
            {
                _elapsedTime = 0f;
                _disappearing = disappear;
            }

            public void SetParameters(int textureHeight, int spriteY, int spriteHeight)
            {
                Parameters["spriteTop"].SetValue((float)spriteY / (float)textureHeight);
                Parameters["spriteBottom"].SetValue((float)spriteY + spriteHeight / (float)textureHeight);
            }

            public override void Update()
            {
                if (_elapsedTime == Time)
                    return;

                _elapsedTime = Math.Min(_elapsedTime + Tengine.Delta, Time);

                float distance = _disappearing ? _elapsedTime : Time - _elapsedTime;
                
                Matrix view = Matrix.Identity;
                int width = Static.NativeWidth; // Static.Renderer.Device.Viewport.Width;
                int height = Static.NativeHeight; //Static.Renderer.Device.Viewport.Height;
                Matrix projection = Matrix.CreateOrthographicOffCenter(0, width, height, distance * 50, 0, 1);

                Parameters["viewProjection"].SetValue(view * projection);
                Parameters["distance"].SetValue(distance);
                
            }
        }
    }
}