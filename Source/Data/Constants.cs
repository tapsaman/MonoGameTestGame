namespace MonoGameTestGame
{
    public enum Resolution
    {
        FullScreen,
        _256x224, // Native SNES reso
        _512x448, // Native x2 
        _768x672, // Native x3
        _1024x896, // Native x4 
        _1234X1080 // Native fitted for 1080 height
    }

    public enum MapCode
    {
        A1,
        A2,
        B1,
        C1
    }

    public enum TransitionType
    {
        Pan,
        FadeToBlack
    }

    public enum EventStore
    {
        Scene,
        Game
    }
}