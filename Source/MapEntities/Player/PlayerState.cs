namespace ZA6.Models
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