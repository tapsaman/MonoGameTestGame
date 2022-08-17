using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TapsasEngine;

namespace ZA6
{
    public class SceneB2 : Scene
    {
        private Texture2D _overlay;
        public Seppo Seppo;
        private bool _seppoActivated;

        public SceneB2()
        {
            Theme = Static.Content.Load<Song>("linktothepast/forest");
        }

        protected override void Load()
        {
            _overlay = Static.Content.Load<Texture2D>("linktothepast/shadedwoodtransparency");
            Seppo = new Seppo();
            Seppo.Position = TileMap.ConvertTileXY(35, 8);

            var seppoActivateEventTrigger = new TouchEventTrigger(TileMap.ConvertTileXY(35, 16), 16, 8);
            seppoActivateEventTrigger.Trigger += SeppoActivateEvent;
            
            var seppoDectivateEventTrigger = new TouchEventTrigger(TileMap.ConvertTileXY(35, 20), 16, 8);
            seppoDectivateEventTrigger.Trigger += SeppoDeactivateEvent;

            Add(Seppo);
            Add(seppoActivateEventTrigger);
            Add(seppoDectivateEventTrigger);
        }

        private void SeppoActivateEvent(Character toucher)
        {
            if (!_seppoActivated && toucher == Player)
            {
                Sys.Log("activate seppo");
                _seppoActivated = true;
                //Seppo.Sprite.SetAnimation("IdleDown");
                Seppo.Direction = Direction.Down;
                MediaPlayer.Pause();
            }
        }

        private void SeppoDeactivateEvent(Character toucher)
        {
            if (_seppoActivated && toucher == Player)
            {
                Sys.Log("deactivate seppo");
                _seppoActivated = false;
                //Seppo.Sprite.SetAnimation("IdleLeft");
                Seppo.Direction = Direction.Left;
                MediaPlayer.Resume();
            }
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