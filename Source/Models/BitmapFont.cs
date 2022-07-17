using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTestGame
{
    
    public abstract class BitmapFont
    {
        public Dictionary<int, BitmapFontChar> Chars { get; private set; }
        public Texture2D Texture { get; protected set; }
        public int LineHeight { get; protected set; } = 12;
        public int LetterSpacing { get; protected set; } = 0;
        public int UndefinedSymbolCode { get; protected set; } = 205;

        public BitmapFont()
        {
            Chars = new Dictionary<int, BitmapFontChar>();
        }

        public void Load(Dictionary<int, int[]> rawCharData)
        {
            foreach(var item in rawCharData)
            {
                Chars[item.Key] = new BitmapFontChar(item.Value);
            }
        }

        public class LinkToThePast : BitmapFont
        {
            public LinkToThePast()
            {
                var rawCharData = StaticData.Content.Load<Dictionary<int, int[]>>("Fonts/linktothepast/linktothepast");
                Load(rawCharData);
                Texture = StaticData.Content.Load<Texture2D>("Fonts/linktothepast/linktothepast_0");
                LineHeight = 16;
                LetterSpacing = 1;
            }
        }

        public class Courier : BitmapFont
        {
            public Courier()
            {
                Texture = StaticData.Content.Load<Texture2D>("Fonts/bitmapfonttest_0");
                var rawCharData = StaticData.Content.Load<Dictionary<int, int[]>>("Fonts/bitmapfonttest");
                Load(rawCharData);
            }
        }
    }

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
}