using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGameTestGame.Managers;
using MonoGameTestGame.Models;

namespace MonoGameTestGame
{
    public class SceneB1 : Scene
    {
        private Texture2D _overlay;
        public Seppo Seppo;
        private bool _seppoActivated;

        protected override void Load()
        {
            _overlay = StaticData.Content.Load<Texture2D>("linktothepast/shadedwoodtransparency");
            Theme = StaticData.Content.Load<Song>("linktothepast/forest");
            TileMap = new MapB1();

            Seppo = new Seppo();
            Seppo.Position = new Vector2(TileMap.ConvertTileX(35), TileMap.ConvertTileY(6));

            var seppoActivateEventTrigger = new TouchEventTrigger(TileMap.GetPosition(35, 16), 16, 8);
            seppoActivateEventTrigger.Trigger += SeppoActivateEvent;
            
            var seppoDectivateEventTrigger = new TouchEventTrigger(TileMap.GetPosition(35, 20), 16, 8);
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
                Music.Stop();
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
                Music.Play(Theme);
            }
        }

        // NOTE drawing tree shadow overlay on top of dialog looks cool 
        public override void DrawOverlay(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_overlay, Vector2.Zero + DrawOffset * 0.5f, new Rectangle(0, 0, StaticData.NativeWidth * 2, StaticData.NativeHeight * 3), new Color(255, 255, 255, 0.5f));
        }
    }
}