using Microsoft.Xna.Framework;
using TapsasEngine;
using TapsasEngine.Models;
using ZA6.Models;
using TapsasEngine.Sprites;

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
            Interactable = true;
            Trigger += Open;

            var dataStore = Static.GetStoreByType(DataStoreType.Session);
            Opened = dataStore.Get("chest " + _chestId + " opened");

            if (!Opened)
            {
                Sprite = new Sprite(Img.ObjectTexture, new Rectangle(0, 16, 16, 16));
            }
            else
            {
                Sprite = new Sprite(Img.ObjectTexture, new Rectangle(16, 16, 16, 16));
            }
        }

        private void Open(Character _)
        {
            if (Opened)
                return;
            
            SFX.ChestOpen.Play();
            Sprite = new Sprite(Img.ObjectTexture, new Rectangle(16, 16, 16, 16));
            Opened = true;

            var dataStore = Static.GetStoreByType(DataStoreType.Session);
            dataStore.Save("chest " + _chestId + " opened", true);

            var item = Collectible.ByID(ItemID);

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
                item.ItemLevel = MapLevel.Air;
                item.Position = Hitbox.Rectangle.Center - new Vector2(item.Hitbox.Rectangle.Width / 2, item.Hitbox.Rectangle.Height);
                Static.Scene.Add(item);

                Static.EventSystem.Load(new Event[]
                {
                    new AnimateEvent(new Animations.Move(item, new Vector2(0, -10), 0.6f)),
                    new TextEvent(new Dialog(item.CollectText), this),
                    new RunEvent(item.Collect)
                });
            }
        }

    }
}