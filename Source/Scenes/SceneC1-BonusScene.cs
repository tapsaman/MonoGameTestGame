using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using ZA6.Models;

namespace ZA6
{
    public class SceneC1 : Scene
    {
        private Texture2D _overlay;
        private Moogle _moogle;
        private Event[] _startEvent;

        public SceneC1()
        {
            Theme = Static.Content.Load<Song>("linktothepast/forest");
        }

        protected override void Load()
        {
            _overlay = Static.Content.Load<Texture2D>("linktothepast/shadedwoodtransparency");
            _moogle = new Moogle();
            _moogle.Position = TileMap.ConvertTileXY(4, 38);
            _moogle.Direction = Direction.Right;
            var yDiff = Player.Position.Y - _moogle.Position.Y;
            var doorPos = TileMap.ConvertTileXY(26, 36);

            //Add(new Bubble() { Position = TileMap.ConvertTileXY(16, 20) });
            //Add(new Bari() { Position = TileMap.ConvertTileXY(10, 24) });

            _startEvent = new Event[]
            {
                //new AnimateEvent(new Animations.Walk(Player, TileMap.ConvertTileXY(2, 0))),
                //new AnimateEvent(new Animations.Jump(_moogle)),
                new WaitEvent(0.5f),
                new FaceEvent(Player, _moogle),
                new WaitEvent(0.5f),
                new FaceEvent(_moogle, Player),
                new WaitEvent(1f),
                new AnimateEvent(new Animations.Jump(_moogle)),
                new TextEvent(new Dialog("oh sh*t a customer"), _moogle),
                new AnimateEvent(new Animations.Walk.Timed(_moogle, new Vector2(0, yDiff), 0.5f)),
                new FaceEvent(_moogle, Player),
                new FaceEvent(Player, _moogle),
                new TextEvent(new Dialog("Wellcome to the mini game\naction!"), _moogle),

                new AnimateEvent(new Animations.Walk.To(_moogle, doorPos + TileMap.ConvertTileXY(0, 2), 100f)) { Wait = false, ID = "walk" },
                new FaceEvent(_moogle, Player) { WaitForID = "walk" },
                new AnimateEvent(new Animations.Walk.To(Player, doorPos + TileMap.ConvertTileXY(0, 4), 100f)),

                /*new AnimateEvent(new Animations.Walk(_moogle, new Vector2(0, -yDiff), 100f)) { Wait = false, ID = "walk" },
                new FaceEvent(_moogle, Player) { WaitForID = "walk" },
                new AnimateEvent(new Animations.Walk(Player, TileMap.ConvertTileXY(2, 0), 100f)),
                new AnimateEvent(new Animations.Walk(Player, new Vector2(0, -(yDiff - TileMap.TileHeight * 2)), 100f)),*/
                new AskEvent(Dialog.Ask("So want to play the game??", "Ye", "Nah bro"), _moogle)
                {
                    IfOption1 = new Event[]
                    {
                        new AnimateEvent(new Animations.Jump(_moogle)) { Wait = false },
                        new TextEvent(new Dialog("Ok let's go!!!", "ai ain't playing!!!"), _moogle),
                        new AnimateEvent(new Animations.Walk(Player, TileMap.ConvertTileXY(3, 0))),
                        new FaceEvent(Player, Direction.Left),
                        new AnimateEvent(new Animations.Walk(_moogle, TileMap.ConvertTileXY(0, 2))),
                        new AnimateEvent(new Animations.Walk(_moogle, TileMap.ConvertTileXY(-2, 0))),
                        new FaceEvent(_moogle, Direction.Right),
                        new AnimateEvent(new Animations.Walk(Player, TileMap.ConvertTileXY(-3, 0))),
                        new AnimateEvent(new Animations.Walk(Player, TileMap.ConvertTileXY(0, -4))),
                        new AnimateEvent(new Animations.Walk(_moogle, TileMap.ConvertTileXY(2, 0))),
                        new AnimateEvent(new Animations.Walk(_moogle, TileMap.ConvertTileXY(0, -2))),
                    },
                    IfOption2 = new Event[]
                    {
                        new TextEvent(new Dialog("Um... Ok.", "I'll just wait for...\nother hero guys wandering\nhere."), _moogle),
                        new AnimateEvent(new Animations.Walk(Player, TileMap.ConvertTileXY(0, 4))),
                        new AnimateEvent(new Animations.Walk(Player, TileMap.ConvertTileXY(-4, 3))),
                    }
                }
            };

            Add(_moogle);
        }

        public override void Start()
        {
            base.Start();
            Static.EventSystem.Load(_startEvent);
        }

        public override void DrawOverlay(SpriteBatch spriteBatch)
        {
            // Reduce native size for panning
            var overlayPosition = OverlayOffset + DrawOffset * 0.5f - Static.NativeSize;
            
            spriteBatch.Draw(
                _overlay,
                overlayPosition,
                new Rectangle(0, 0, Width * 3, Height * 2),
                new Color(255, 255, 255, 0.5f)
            );
        }
    }
}