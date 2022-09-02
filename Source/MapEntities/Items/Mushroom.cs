using Microsoft.Xna.Framework;
using TapsasEngine;
using TapsasEngine.Sprites;
using ZA6.Managers;
using ZA6.Models;

namespace ZA6.Items
{
    public class Mushroom : Collectible
    {
        public Mushroom()
        {
            Sprite = new Sprite(Img.ObjectTexture, new Rectangle(64, 16, 16, 16));
            Hitbox.Load(16, 16);
        }

        public override void Collect()
        {
            Music.Stop();
            SFX.ItemGet.Play();
            Static.Player.Moving = false;
            Position = Static.Player.Position + new Vector2(7, -25);
            Static.Player.AnimatedSprite.SetAnimation("GotItem");
            Static.SessionData.Save("eaten mushroom", true);

            Static.EventSystem.Load(
                new Event[]
                {
                    /*new RunEvent(
                        () => { Static.Player.AnimatedSprite.SetAnimation("GotItem"); }
                    ),*/
                    new WaitEvent(2f),
                    new TextEvent(new Dialog("You foun mushroom !!"), this),
                    new RunEvent(base.Collect),
                    new RunEvent(
                        () => { Static.Player.Moving = true; }
                    )
                }
            );

            /*Static.EventSystem.Load(
                new Event[]
                {
                    new AnimateEvent(new Animations.High())
                },
                EventSystem.Settings.Parallel | EventSystem.Settings.SustainSceneChange
            );*/
        }
    }
}
