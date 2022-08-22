namespace ZA6
{
    public enum MapLevel : ushort
    {
        Ground,
        Character,
        Air
    }

    public enum TransitionType
    {
        Pan,
        FadeToBlack,
        Doorway
    }

    public enum DataStoreType
    {
        Scene,
        Session,
        Game
    }
}