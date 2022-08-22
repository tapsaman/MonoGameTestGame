using Microsoft.Xna.Framework.Graphics;

namespace ZA6.UI
{
    public class Row : UIContainer
    {
        public Row()
        {
            Padding = 5;
        }

        protected override void CalculateSize()
        {
            if (Components.Length == 0)
                return;
            
            var width = (Width - Padding * (Components.Length - 1)) / Components.Length;

            foreach (var component in Components)
            {
                component.Width = width;
                component.Height = Height;
            }
        }

        public override bool DetermineKeyInput()
        {
            //if (base.DetermineKeyInput())
            //    return true;
            
            var dir = Input.P1.GetDirectionVector();
            
            if (dir.X < 0)
            {
                FocusPrevious();
                return true;
            }
            else if (dir.X > 0)
            {
                FocusNext();
                return true;
            }

            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var position = Position;

            foreach (var component in Components)
            {
                component.Position = position;
                component.Draw(spriteBatch);
                position.X += component.Width + Padding;
            }
        }
    }
}