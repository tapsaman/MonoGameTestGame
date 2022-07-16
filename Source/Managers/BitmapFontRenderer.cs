using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTestGame.Models;

namespace MonoGameTestGame.Managers
{
    /*
    Info on XML importing:
        https://www.cs.usfca.edu/~galles/cs420S13/lecture/UsingXNA4XML/XNA_XML.html
    
    Bitmap font files are generated with BMFont
    XML can then be find-replaced with regex:
        <char id="(\d+)" x="(\d+)" y="(\d+)" width="(\d+)" height="(\d+)" xoffset="(-?\d+)" yoffset="(-?\d+)" xadvance="(\d+)" page="0" chnl="15" />
        <Item><Key>$1</Key><Value>$2 $3 $4 $5 $6 $7 $8</Value></Item>
    */

    public class BitmapFontChar
    {
        public Rectangle SourceRectangle;
        public Vector2 Offset;
        public int XAdvance;
        public BitmapFontChar(int[] props)
        {
            SourceRectangle = new Rectangle(props[0], props[1], props[2], props[3]);
            Offset = new Vector2(props[4], props[5]);
            XAdvance = props[6];
        }
    }

    public static class BitmapFontRenderer
    {
        private static Dictionary<int, BitmapFontChar> _chars;
        private static Texture2D _texture;
        public static void Load()
        {
            //_font = StaticData.Content.Load<BitmapFont>("Fonts/bitmapfonttest");
            Dictionary<int, int[]> rawCharData = StaticData.Content.Load<Dictionary<int, int[]>>("Fonts/bitmapfonttest");
            _chars = new Dictionary<int, BitmapFontChar>();
            _texture = StaticData.Content.Load<Texture2D>("Fonts/bitmapfonttest_0");

            foreach(var item in rawCharData)
            {
                _chars[item.Key] = new BitmapFontChar(item.Value);
            }
        }

        public static void DrawString(SpriteBatch spriteBatch, string text, Vector2 position, Color color)
        {
            var startPosition = position;

            foreach (int code in text)
            {
                if (code == 10)
                {
                    // New line character
                    position.X = startPosition.X;
                    position.Y += 12;
                }
                else
                { 
                    var c = _chars[code];

                    spriteBatch.Draw(
                        _texture,
                        position + c.Offset,
                        c.SourceRectangle,
                        color
                    );

                    position.X += c.XAdvance;
                }
            }
        }
    }
}