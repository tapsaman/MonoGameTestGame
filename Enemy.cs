using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameTestGame.Controls;
using MonoGameTestGame.Managers;
using MonoGameTestGame.Models;
using MonoGameTestGame.Sprites;

namespace MonoGameTestGame
{
    public class Enemy
    {

        public MapEntity MapEntity;
            
        public Enemy(Texture2D playerTexture, GraphicsDeviceManager graphics)
        {
            Dictionary<string, Animation> animations = new Dictionary<string, Animation>()
            {
                { "IdleUp", new Animation(playerTexture, 1, 4, 3) },
                { "IdleRight", new Animation(playerTexture, 1, 4, 12) },
                { "IdleDown", new Animation(playerTexture, 1, 0, 1) },
                { "IdleLeft", new Animation(playerTexture, 1, 1, 12) },
                { "WalkUp", new Animation(playerTexture, 8, 4) },
                { "WalkRight", new Animation(playerTexture, 6, 4, 8) },
                { "WalkDown", new Animation(playerTexture, 8, 1) },
                { "WalkLeft", new Animation(playerTexture, 6, 1, 8) },
                { "SwordHitUp", new Animation(playerTexture, 5, 6, 0, false, 0.04f) },
                { "SwordHitRight", new Animation(playerTexture, 5, 6, 8, false, 0.04f) },
                { "SwordHitDown", new Animation(playerTexture, 6, 3, 0, false, 0.04f) },
                { "SwordHitLeft", new Animation(playerTexture, 5, 3, 8, false, 0.04f) },
            };

            Sprite sprite = new Sprite(animations, "SwordHitDown");
            Hitbox hitbox = new Hitbox(graphics, 18, 22);
            hitbox.Color = Color.Red;

            MapEntity = new MapEntity(sprite, hitbox)
            {
                Position = new Vector2(300, 100)
            };
        }
    }
}
