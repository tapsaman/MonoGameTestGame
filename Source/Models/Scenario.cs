using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZA6.Managers;

namespace ZA6.Models
{
    public abstract class Scenario : State
    {
        public override bool CanReEnter { get; protected set; } = false;
        public override void Enter(StateArgs args) {}
        public override void Update(GameTime gameTime) {}
        public override void Exit() {}
        public virtual void Apply(Scene scene) {}
    }

    public class NoneScenario : Scenario {}

    public class NoiseScenario : Scenario
    {
        public override void Enter(StateArgs args)
        {
            Static.Game.StateMachine.TransitionTo("Cartoon");
        }

        public override void Apply(Scene scene)
        {
            Static.Renderer.ApplyPostEffect(Shaders.Noise);

            if (scene is SceneAB1)
                scene.TileMap.LoadAlternativeTiles("AlternativeNoise");
        }
    }

    public class TapeScenario : Scenario
    {
        public override void Apply(Scene scene)
        {
            Static.Renderer.ApplyPostEffect(Shaders.Noise);

            if (scene is SceneAB1)
            {
                var frog1 = new FrogGuy() { Position = scene.TileMap.ConvertTileXY(24, 12) };
                var frog2 = new FrogGuy() { Position = scene.TileMap.ConvertTileXY(36, 18) };
                scene.Add(frog1);
                scene.Add(frog2);
                Static.EventSystem.Load(
                    new AnimateEvent(new Animations.RunAround(frog1)),
                    Managers.EventSystem.Settings.Parallel
                );
                Static.EventSystem.Load(
                    new AnimateEvent(new Animations.RunAround(frog2)),
                    Managers.EventSystem.Settings.Parallel
                );
                var rattler1 = new Rattler() { Position = scene.TileMap.ConvertTileXY(23, 4) };
                var rattler2 = new Rattler() { Position = scene.TileMap.ConvertTileXY(39, 4) };
                scene.Add(rattler1);
                scene.Add(rattler2);
            }
        }
    }

    public class ArrowsScenario : Scenario
    {
        public override void Apply(Scene scene)
        {
            Static.Renderer.ApplyPostEffect(Shaders.Noise);

            if (scene is SceneB1)
            {
                scene.TileMap.LoadAlternativeTiles("AlternativeArrows");
            }
        }
    }
}