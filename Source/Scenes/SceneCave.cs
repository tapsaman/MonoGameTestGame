namespace ZA6
{
    public class SceneCave : Scene
    {
        public SceneCave() {}
        
        protected override void Load()
        {
            if (!Static.SessionData.Get("spoken to klaus"))
                Add(new Klaus() { Position = TileMap.ConvertTileXY(15, 7) });
            
            Add(new TreasureChest("cave") { Position = TileMap.ConvertTileXY(10, 14) });
        }
    }
}