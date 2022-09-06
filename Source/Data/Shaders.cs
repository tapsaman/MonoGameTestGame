using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine;
using TapsasEngine.Models;

namespace ZA6
{
    public static class Shaders
    {
        public static Effect HighContrast;
        public static Effect Rainbow;
        public static Effect InvertedColor;
        public static Effect Noise;
        public static Effect SpriteNoise;
        public static Effect MildNoise;
        public static Effect Spotlight;
        public static Effect Crispy;
        public static Effect TestEffect;
        public static EnemyDamageShader EnemyDamage;
        public static WavyShader Wavy;
        public static HighlightShader Highlight;

        public static void Load()
        {
            HighContrast = Static.Content.Load<Effect>("Shaders/HighContrastEffect");
            Rainbow = Static.Content.Load<Effect>("Shaders/RainbowEffect");
            InvertedColor = Static.Content.Load<Effect>("Shaders/InvertedColorEffect");
            Noise = Static.Content.Load<Effect>("Shaders/NoiseEffect");
            SpriteNoise = Static.Content.Load<Effect>("Shaders/SpriteNoiseEffect");
            MildNoise = Static.Content.Load<Effect>("Shaders/MildNoiseEffect");
            Spotlight = Static.Content.Load<Effect>("Shaders/SpotlightEffect");
            Crispy = Static.Content.Load<Effect>("Shaders/CrispyEffect");
            TestEffect = Static.Content.Load<Effect>("Shaders/ShaderTestSpriteEffect");
            EnemyDamage = new EnemyDamageShader();
            Wavy = new WavyShader();
            Highlight = new HighlightShader();
        }

        public class EnemyDamageShader : Shader
        {
            private int _iter;
            private GameTime _lastGameTime;

            public EnemyDamageShader() : base("Shaders/EnemyDamageEffect") {}

            public override void Update()
            {
                if (_lastGameTime != Tengine.GameTime)
                {
                    _iter = _iter == 3 ? 0 : _iter + 1;
                    _lastGameTime = Tengine.GameTime;
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
                Shaders.Wavy.Parameters["yOffset"].SetValue(_elapsedTime * _speed);
            }

            public void SetParameters(float waveWidth, float waveHeight, float speed)
            {
                Shaders.Wavy.Parameters["waveWidth"].SetValue(waveWidth);
                Shaders.Wavy.Parameters["waveHeight"].SetValue(waveHeight);
                _speed = speed;
            }
        }
    }
}