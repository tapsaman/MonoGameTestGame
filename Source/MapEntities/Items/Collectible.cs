using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using TapsasEngine;
using ZA6.Items;
using ZA6.Models;

namespace ZA6
{
    public abstract class Collectible : MapObject
    {
        public override MapLevel Level { get => ItemLevel; }
        public MapLevel ItemLevel;

        public abstract string ItemID { get; }
        public abstract string CollectText { get; }

        public int CollectCount {
            get => Static.GameData.GetInt("collected ItemID:" + ItemID);
            set => Static.GameData.Save("collected ItemID:" + ItemID, value);
        }

        public SoundEffect PickUpSound { get; protected set; } = SFX.Heart; 

        public static Collectible ByID(string itemID)
        {
            switch (itemID)
            {
                case "heart":
                    return new Heart();
                case "mushroom":
                    return new Mushroom();
                default:
                    return null;
            }
        }

        public Collectible()
        {
            Hitbox.Color = Color.Orange;
            Interactable = false;
            TriggeredOnTouch = true;
            Trigger += OnCollect;
        }

        public void OnCollect(Character character)
        {
            if (character == Static.Player)
            {
                if (CollectCount == 0)
                    CollectEvent();
                else
                    Collect();
            }
        }

        public virtual void Collect()
        {
            PickUpSound.Play();
            Static.Scene.Remove(this);
            CollectCount += 1;
        }

        protected void CollectEvent()
        {
            Music.Pause(this);
            SFX.ItemGet.Play();
            Position = Static.Player.Position + new Vector2(7, -25);
            Static.Player.AnimatedSprite.SetAnimation("GotItem");

            Static.EventSystem.Load(
                new Event[]
                {
                    /*new RunEvent(
                        () => { Static.Player.AnimatedSprite.SetAnimation("GotItem"); }
                    ),*/
                    new WaitEvent(2f),
                    new TextEvent(new Dialog(CollectText), this),
                    new RunEvent(Collect),
                    new RunEvent(() =>
                        {
                            Static.Player.Moving = true;
                            Music.Resume(this);
                        }
                    )
                }
            );
        }
    }
}
