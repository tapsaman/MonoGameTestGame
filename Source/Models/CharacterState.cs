namespace ZA6.Models
{
    public abstract class CharacterState : State
    {
        public Character Character;
        public CharacterState(Character character)
        {
            Character = character;
        }
    }
}