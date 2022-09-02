using Microsoft.Xna.Framework.Graphics;

namespace ZA6
{
    public static class Shaders
    {
        public static Effect EnemyDamage;
        public static Effect HighContrast;
        public static Effect Rainbow;
        public static Effect InvertedColor;
        public static Effect Wavy;
        public static Effect Noise;
        public static Effect MildNoise;
        public static Effect Spotlight;
        public static Effect Highlight;
        public static Effect Crispy;
        public static Effect TestEffect;

        public static void Load()
        {
            EnemyDamage = Static.Content.Load<Effect>("Shaders/EnemyDamageEffect");
            HighContrast = Static.Content.Load<Effect>("Shaders/HighContrastEffect");
            Rainbow = Static.Content.Load<Effect>("Shaders/RainbowEffect");
            InvertedColor = Static.Content.Load<Effect>("Shaders/InvertedColorEffect");
            Wavy = Static.Content.Load<Effect>("Shaders/WavyEffect");
            Noise = Static.Content.Load<Effect>("Shaders/NoiseEffect");
            MildNoise = Static.Content.Load<Effect>("Shaders/MildNoiseEffect");
            Spotlight = Static.Content.Load<Effect>("Shaders/SpotlightEffect");
            Highlight = Static.Content.Load<Effect>("Shaders/HighlightEffect");
            Crispy = Static.Content.Load<Effect>("Shaders/CrispyEffect");
            TestEffect = Static.Content.Load<Effect>("Shaders/ShaderTestSpriteEffect");
        }
    }
}