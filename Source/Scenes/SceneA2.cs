using Microsoft.Xna.Framework.Media;
using MonoGameTestGame.Managers;
using MonoGameTestGame.Models;

namespace MonoGameTestGame
{
    public class SceneA2 : Scene
    {
        private Event _signEvent;
        
        protected override void Load()
        {
            TileMap = new MapA2();

            var signEventTrigger = new EventTrigger(TileMap.GetPosition(9, 4), 14, 14);
            _signEvent = new TextEvent(new Dialog("ZELDA'S TENT HOME"), signEventTrigger);
            signEventTrigger.Trigger += ReadSign;

            Add(signEventTrigger);
        }

        private void ReadSign(Character _)
        {
            EventManager.Load(_signEvent);
        }
    }
}