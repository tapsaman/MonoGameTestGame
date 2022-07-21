using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;
using MonoGameTestGame.Models;
using MonoGameTestGame.Sprites;

namespace MonoGameTestGame
{
    public class Bat : Character
    {
        public Bat()
        {
            Interactable = false;
            Hittable = false;
            Colliding = false;
            
            var texture = StaticData.Content.Load<Texture2D>("linktothepast/enemy-sprites");
            Animation.DefaultFrameWidth = Animation.DefaultFrameHeight = 34;

            Dictionary<string, Animation> animations = new Dictionary<string, Animation>()
            {
                { "Default", new Animation(texture, 3, 0.1f, true) },
                { "TakenDamage", new Animation(texture, 1, 0.1f, false) }
            };

            Sprite.SetAnimations(animations, "Default");
            Hitbox.Load(24, 10);
            Hitbox.Color = Color.Red;
            SpriteOffset = new Vector2(-5, -14);

            Dictionary<string, State> states = new Dictionary<string, State>()
            {
                { "Default", new BatStateDefault(this) }
            };

            StateMachine = new StateMachine(states, "Default");
        }

        public override void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
            StateMachine.Update(gameTime);
        }
    }
}
