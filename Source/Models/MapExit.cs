using TapsasEngine.Enums;

namespace ZA6
{    
    public class MapExit
    {
        public Direction Direction;
        public string MapName;
        public TransitionType TransitionType;

        public MapExit()
        {
            TransitionType = TransitionType.Pan;
        }

        public MapExit(Direction direction, string mapName, TransitionType transitionType)
        {
            Direction = direction;
            MapName = mapName;
            TransitionType = transitionType;
        }
    }
}