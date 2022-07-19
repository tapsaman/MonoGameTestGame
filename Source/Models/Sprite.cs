using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using MonoGameTestGame.Managers;
using MonoGameTestGame.Models;

namespace MonoGameTestGame.Sprites
{
  public class Sprite
  {
    #region Fields

    protected AnimationManager _animationManager;

    protected Dictionary<string, Animation> _animations;

    protected Vector2 _position;

    protected Texture2D _texture;
    protected Rectangle _sourceRectangle; 

    #endregion

    #region Properties

    public Vector2 Position
    {
      get { return _position; }
      set
      {
        _position = value;

        if (_animationManager != null)
          _animationManager.Position = _position;
      }
    }

    public Rectangle Rectangle
    {
      get
      {
        return new Rectangle((int)Position.X, (int)Position.Y, 10, 10);
      }
    }

    #endregion

    #region Methods

    public Sprite() {}

    public virtual void Update(GameTime gameTime)
    {
      if (_animationManager != null)
        _animationManager.Update(gameTime);
    }

    public virtual void Draw(SpriteBatch spriteBatch, Vector2 offset)
    {
      if (_texture != null)
        spriteBatch.Draw (_texture, Position + offset, _sourceRectangle, Color.White);
      else if (_animationManager != null)
        _animationManager.Draw(spriteBatch, offset);
      else throw new Exception("No texture or animations defined for sprite");
    }

    public void SetAnimation(string animationName = null)
    {
      if (animationName == null)
        _animationManager.Stop();
      else
        _animationManager.Play(_animations[animationName]);
    }

    public void SetTexture(Texture2D texture)
    {
      _texture = texture;
      _sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
    }

    public void SetTexture(Texture2D texture, Rectangle sourceRectangle)
    {
      _texture = texture;
      _sourceRectangle = sourceRectangle;
    }

    public void SetAnimations(Dictionary<string, Animation> animations, string initialAnimationName = "")
    {
      _animations = animations;
      _animationManager = new AnimationManager(initialAnimationName == "" ? _animations.First().Value : _animations[initialAnimationName]);
    }

    #endregion
  }
}