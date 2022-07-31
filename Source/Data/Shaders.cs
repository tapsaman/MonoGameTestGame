using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTestGame
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
        public static Effect TestEffect;

        public static void Load()
        {
            EnemyDamage = StaticData.Content.Load<Effect>("Shaders/EnemyDamageEffect");
            HighContrast = StaticData.Content.Load<Effect>("Shaders/HighContrastEffect");
            Rainbow = StaticData.Content.Load<Effect>("Shaders/RainbowEffect");
            InvertedColor = StaticData.Content.Load<Effect>("Shaders/InvertedColorEffect");
            Wavy = StaticData.Content.Load<Effect>("Shaders/WavyEffect");
            Noise = StaticData.Content.Load<Effect>("Shaders/NoiseEffect");
            MildNoise = StaticData.Content.Load<Effect>("Shaders/MildNoiseEffect");
            TestEffect = StaticData.Content.Load<Effect>("Shaders/ShaderTestSpriteEffect");
        }
    }
}