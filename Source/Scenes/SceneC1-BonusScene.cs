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

            _moogle = new Moogle() { Position = TileMap.ConvertTileXY(4, 38), Direction = Direction.Right };
            _moogle.Trigger += TalkTMoogle;
            Add(_moogle);

            var lockedChest = new LockedChest() { Position = TileMap.ConvertTileXY(4, 34) };
            lockedChest.Trigger += (Character _) => { SceneData.Save("tried chest", true); };
            Add(lockedChest);

            SceneData.Save("tried chest", true);
        }

        public override void Start()
        {
            base.Start();

            if (!Static.SessionData.Get("met moogle"))
            {
                Static.SessionData.Save("met moogle", true);
                MeetMoogle();
            }
            else
            {
                MeetMoogle();
            }
        }

        private void MeetMoogle()
        {
            var moogleStartPos = _moogle.Position + new Vector2(0, 3);
            var yDiff = Player.Position.Y - moogleStartPos.Y;
            var doorPos = TileMap.ConvertTileXY(26, 36);

            Static.EventSystem.Load(new Event[]
            {
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

                //new WaitEvent(1f),

                new AnimateEvent(new Animations.Walk.To(_moogle, moogleStartPos, 180f)) { ID = "walk1", Wait = false },
                new AnimateEvent(new Animations.Walk.To(_moogle, doorPos + TileMap.ConvertTileXY(2, 2), 180f)) { ID = "walk2", WaitForID = "walk1", Wait = false },
                new FaceEvent(_moogle, Player) { WaitForID = "walk2" },

                //new WaitEvent(1f),

                new AnimateEvent(new Animations.Walk(Player, TileMap.ConvertTileXY(2, 0), 180f)),
                new AnimateEvent(new Animations.Walk.To(Player, moogleStartPos, 180f)),
                new AnimateEvent(new Animations.Walk.To(Player, doorPos + TileMap.ConvertTileXY(0, 2), 180f)),
                //new AnimateEvent(new Animations.Walk.To(_moogle, doorPos + TileMap.ConvertTileXY(0, 2), 100f)) { Wait = false, ID = "walk" },
                //new AnimateEvent(new Animations.Walk.To(Player, doorPos + TileMap.ConvertTileXY(0, 4), 100f)),

                /*new AnimateEvent(new Animations.Walk(_moogle, new Vector2(0, -yDiff), 100f)) { Wait = false, ID = "walk" },
                new FaceEvent(_moogle, Player) { WaitForID = "walk" },
                new AnimateEvent(new Animations.Walk(Player, TileMap.ConvertTileXY(2, 0), 100f)),
                new AnimateEvent(new Animations.Walk(Player, new Vector2(0, -(yDiff - TileMap.TileHeight * 2)), 100f)),*/
                new AskEvent(Dialog.Ask("So want to play the game??", "Ye", "Nah bro"), _moogle)
                {
                    IfOption1 = new Event[]
                    {
                        new AnimateEvent(new Animations.Jump(_moogle)) { Wait = false },
                        new TextEvent(new Dialog("Ok let's go!!!\n\n\nai ain't playing!!!"), _moogle),
                        new AnimateEvent(new Animations.Walk(Player, TileMap.ConvertTileXY(0, -3))),
                        new AnimateEvent(new Animations.Walk(_moogle, TileMap.ConvertTileXY(-2, 0))),
                        new FaceEvent(_moogle, Direction.Up),
                    },
                    IfOption2 = new Event[]
                    {
                        new TextEvent(new Dialog("Um... Ok.", "I'll just wait for...\nother hero guys wandering\nhere."), _moogle),
                        new AnimateEvent(new Animations.Walk.To(Player, TileMap.PlayerStartPosition)),
                    }
                }
            });
        }

        private void TalkTMoogle(Character _)
        {
            Static.EventSystem.Load(new Event[]
            {
                new ConditionEvent(DataStoreType.Scene, "tried chest")
                {
                    IfFalse = new Event[]
                    {
                        new ConditionEvent(DataStoreType.Scene, "moogle speak toggle")
                        {
                            IfFalse = new Event[]
                            {
                                new TextEvent(new Dialog("Just get a... get to the\notherside my guy."), _moogle),
                                new SaveValueEvent(DataStoreType.Scene, "moogle speak toggle", true)
                            },
                            IfTrue = new Event[]
                            {
                                new TextEvent(new Dialog("Are you ok"), _moogle),
                                new SaveValueEvent(DataStoreType.Scene, "moogle speak toggle", false)
                            }
                        }
                    },
                    IfTrue = new Event[]
                    {
                        new ConditionEvent(DataStoreType.Scene, "moogle checked chest")
                        {
                            IfFalse = new Event[]
                            {
                                new TextEvent(new Dialog("Did u try open chest yet ?"), _moogle),
                                new AskEvent(Dialog.Ask("Ye it does not open", "Ye it doesnt open", "How is this a minigame??"), _moogle)
                                {
                                    IfOption2 = new Event[]
                                    {
                                        new TextEvent(new Dialog("Go chek the treasure again\nbro"), _moogle),
                                        new SaveValueEvent(DataStoreType.Scene, "tried chest", false)
                                    },
                                    IfOption1 = new Event[]
                                    {
                                        new TextEvent(new Dialog("Damn that sck man"), _moogle),
                                        new WaitEvent(1f),
                                        new AnimateEvent(new Animations.Walk(_moogle, TileMap.ConvertTileXY(-20, 0), 180f)),
                                        new FaceEvent(Player, Direction.Left),
                                        new AnimateEvent(new Animations.Jump.To(_moogle, TileMap.ConvertTileXY(0, -4))),
                                        new FaceEvent(_moogle, Direction.Left),
                                        new WaitEvent(1f),
                                        new AnimateEvent(new Animations.Jump.To(_moogle, TileMap.ConvertTileXY(0, 4))),
                                        new AnimateEvent(new Animations.Walk(_moogle, TileMap.ConvertTileXY(20, 0), 180f)),
                                        new FaceEvent(Player, Direction.Down),
                                        new FaceEvent(_moogle, Direction.Up),
                                        new TextEvent(new Dialog("Ye sorry abou that kupo\nIt was locked"), _moogle),
                                        new WaitEvent(1f),
                                        new SaveValueEvent(DataStoreType.Scene, "moogle checked chest", true),
                                    }
                                }
                            },
                            IfTrue = new Event[]
                            {
                                new TextEvent(new Dialog("Watcha want now"), _moogle),
                                new AskEvent(Dialog.Ask("", "Can i get out of here", "Jus chillin"), _moogle)
                                {
                                    IfOption2 = new Event[]
                                    {
                                        new TextEvent(new Dialog("Eeeeeyyyyyyyy"), _moogle),
                                    },
                                    IfOption1 = new Event[]
                                    {
                                        new TextEvent(new Dialog("Oh ok guy"), _moogle),
                                        new AnimateEvent(new Animations.Walk(_moogle, TileMap.ConvertTileXY(-2, 0))),
                                        new FaceEvent(_moogle, Direction.Right),
                                        new AnimateEvent(new Animations.Walk.To(Player, TileMap.ConvertTileXY(26, 40))),
                                        new FaceEvent(_moogle, Direction.Down),
                                        new AnimateEvent(new Animations.Walk(Player, TileMap.ConvertTileXY(-2, 0))),
                                        new FaceEvent(Player, Direction.Up),
                                        new TextEvent(new Dialog("The mini game game costs 199\nrupees to play\n kupo.\nGotta feed the kids and all that"), _moogle),
                                        new RunEvent(() => { Static.Game.Hud.Rupees -= 199; }),
                                        new WaitEvent(RupeeHUD.UPDATE_TIME * 199),
                                        new TextEvent(new Dialog("Good by!"), _moogle),
                                        new AnimateEvent(new Animations.Jump.To(_moogle, TileMap.ConvertTileXY(2, 0))),
                                        new AnimateEvent(new Animations.Jump.To(_moogle, TileMap.ConvertTileXY(50, 0))),
                                        new AnimateEvent(new Animations.Walk.To(Player, TileMap.PlayerStartPosition)),
                                    }
                                }
                            }
                        }
                    }
                }
            });
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