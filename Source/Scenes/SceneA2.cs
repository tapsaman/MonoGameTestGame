using Microsoft.Xna.Framework;
using TapsasEngine;
using TapsasEngine.Enums;
using ZA6.Models;

namespace ZA6
{
    public class SceneA2 : Scene
    {
        private Petteri _petteri;
        
        protected override void Load()
        {
            if (Static.GameData.GetString("scenario") != null && !Static.GameData.Get("spoken to petteri"))
            {
                _petteri = new Petteri()
                {
                    Position = TileMap.ConvertTileXY(18, 28)
                };
                _petteri.Trigger += TalkToPetteri;
                Add(_petteri);
            }
        }

        private void TalkToPetteri(Character _)
        {
            Static.GameData.Save("spoken to petteri", true);
            Static.EventSystem.Load(new Event[]
            {
                new TextEvent(new Dialog("i know it is here                              .\nmy hole"), _petteri),
                new FaceEvent(_petteri, Player),
                new TextEvent(new Dialog("have you seen it?"), _petteri),
                new FaceEvent(_petteri, Direction.Down),
            });
        }
    }
}