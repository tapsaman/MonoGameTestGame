using Microsoft.Xna.Framework;
namespace MonoGameTestGame
{
    public class SceneA2 : Scene
    {
        TouchEventTrigger _holeEventTrigger;

        public SceneA2()
        {
            TileMap = new MapA2();
        }
        
        protected override void Load()
        {
            _holeEventTrigger = new TouchEventTrigger(TileMap.ConvertTileXY(10, 22), 16, 16);
            _holeEventTrigger.Trigger += PullToHole;
            var innerHoleEventTrigger = new TouchEventTrigger(TileMap.ConvertTileXY(10, 22) + new Vector2(6, 0), 4, 16);
            innerHoleEventTrigger.Trigger += PullToHole;
            
            Add(_holeEventTrigger);
            Add(innerHoleEventTrigger);
        }

        private void PullToHole(Character character)
        {
            character.ElementalVelocity = _holeEventTrigger.Hitbox.Rectangle.Center - character.Hitbox.Rectangle.Center;
            character.ElementalVelocity.Normalize();
            character.ElementalVelocity *= 30f;

            if (_holeEventTrigger.Hitbox.Rectangle.Contains(character.Hitbox.Rectangle))
            {
                character.MakeFall();
            }
        }
    }
}