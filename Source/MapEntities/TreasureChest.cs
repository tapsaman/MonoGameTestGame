using Microsoft.Xna.Framework;
using TapsasEngine.Models;
using ZA6.Models;
using ZA6.Sprites;

namespace ZA6
{
    public class TreasureChest : MapObject
    {
        public bool Opened;
        public string ItemID;
        private string _chestId;

        public TreasureChest(string id)
        {
            _chestId = id;
            Hitbox.Load(16, 16);
            Hittable = false;
            Colliding = true;
            Trigger += Open;

            var dataStore = Static.GetStoreByType(DataStoreType.Session);
            Opened = dataStore.Get("chest " + _chestId + " opened");

            if (!Opened)
            {
                Interactable = true;
                Sprite.SetTexture(Img.ObjectTexture, new Rectangle(0, 16, 16, 16));
            }
            else
            {
                Interactable = false;
                Sprite.SetTexture(Img.ObjectTexture, new Rectangle(16, 16, 16, 16));
            }
        }

        private void Open(Character _)
        {
            if (Opened)
                return;
            
            SFX.ChestOpen.Play();
            Sprite.SetTexture(Img.ObjectTexture, new Rectangle(16, 16, 16, 16));
            Opened = true;

            var dataStore = Static.GetStoreByType(DataStoreType.Session);
            dataStore.Save("chest " + _chestId + " opened", true);

            var item = ItemIDToItem(ItemID);

            if (item == null)
            {
                Static.EventSystem.Load(new Event[]
                {
                    new WaitEvent(0.6f),
                    new TextEvent(new Dialog("There's nothing in here..."), this)
                });
            }
            else
            {
                item.Position = Hitbox.Rectangle.Center - new Vector2(item.Hitbox.Rectangle.Width / 2, item.Hitbox.Rectangle.Height);
                Static.Scene.Add(item);

                Static.EventSystem.Load(new Event[]
                {
                    new AnimateEvent(new Animations.Move(item, new Vector2(0, -10), 0.6f)),
                    new TextEvent(new Dialog(item.PickUpText), this),
                    new RunEvent(() => {
                        item.OnPickUp();
                    }),
                    new RemoveEvent(item)
                });
            }
        }

        public static Item ItemIDToItem(string itemID)
        {
            switch (itemID)
            {
                case "Heart":
                    return new Items.Heart();
                default:
                    return null;
            }
        }
    }
}