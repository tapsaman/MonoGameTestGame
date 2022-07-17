using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Models;
using MonoGameTestGame.Sprites;

namespace MonoGameTestGame
{
    public class Bat : Character
    {
        public Bat()
        {
            Interactable = false;
            Hittable = true;
            Colliding = false;
            
            var texture = StaticData.Content.Load<Texture2D>("linktothepast-enemysprites");
            Animation.DefaultFrameWidth = Animation.DefaultFrameHeight = 34;

            Dictionary<string, Animation> animations = new Dictionary<string, Animation>()
            {
                { "Default", new Animation(texture, 3) }
            };

            Sprite.SetAnimations(animations, "Default");
            Hitbox.Load(14, 14);
            Hitbox.Color = Color.Red;
            SpriteOffset = new Vector2(-2, -2);
        }

        public override void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
        }
    }
}
