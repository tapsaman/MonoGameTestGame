using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Managers;

namespace MonoGameTestGame.Models
{
    public abstract class PlayerState : State
    {
        public Player Player;
        public PlayerState(Player player)
        {
            Player = player;
        }
    } 
}