using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace ZA6
{
    public class SceneB1 : Scene
    {
        private Texture2D _overlay;
        public Seppo Seppo;

        public SceneB1()
        {
            Theme = Static.Content.Load<Song>("linktothepast/forest");
            ExitTransitions[Direction.Left] = TransitionType.FadeToBlack;
        }

        protected override void Load()
        {
            _overlay = Static.Content.Load<Texture2D>("linktothepast/shadedwoodtransparency");
        }

        public override void Start()
        {
            base.Start();
            Static.Game.TitleText.Enter();
        }

        // NOTE drawing tree shadow overlay on top of dialog looks cool 
        public override void DrawOverlay(SpriteBatch spriteBatch)
        {
            // Reduce native size for panning
            var overlayPosition = OverlayOffset + DrawOffset * 0.5f - Static.NativeSize;
            
            spriteBatch.Draw(
                _overlay,
                overlayPosition,
                new Rectangle(0, 0, Width * 3, Height * 2),
                new Color(255, 255, 255, 0.5f)
            );
        }
    }
}