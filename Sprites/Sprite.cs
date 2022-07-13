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

    public virtual void Draw(SpriteBatch spriteBatch)
    {
      if (_texture != null)
        spriteBatch.Draw(_texture, Position, Color.White);
      else if (_animationManager != null)
        _animationManager.Draw(spriteBatch);
      else throw new Exception("This ain't right..!");
    }

    public void SetAnimation(string animationName = null)
    {
      if (animationName == null)
        _animationManager.Stop();
      else
        _animationManager.Play(_animations[animationName]);
    }

    public Sprite(Dictionary<string, Animation> animations)
    {
      _animations = animations;
      _animationManager = new AnimationManager(_animations.First().Value);
    }

    public Sprite(Dictionary<string, Animation> animations, string initialAnimationName)
    {
      _animations = animations;
      _animationManager = new AnimationManager(_animations[initialAnimationName]);
    }

    public Sprite(Texture2D texture)
    {
      _texture = texture;
    }

    public virtual void Update(GameTime gameTime)
    {
      _animationManager.Update(gameTime);
    }

    #endregion
  }
}