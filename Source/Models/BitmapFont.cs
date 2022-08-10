using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZA6
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
                var rawCharData = Static.Content.Load<Dictionary<int, int[]>>("Fonts/linktothepast/linktothepast");
                Load(rawCharData);
                Texture = Static.Content.Load<Texture2D>("Fonts/linktothepast/linktothepast_0");
                LineHeight = 16;
                LetterSpacing = 1;
            }
        }

        public class Courier : BitmapFont
        {
            public Courier()
            {
                Texture = Static.Content.Load<Texture2D>("Fonts/bitmapfonttest_0");
                var rawCharData = Static.Content.Load<Dictionary<int, int[]>>("Fonts/bitmapfonttest");
                Load(rawCharData);
            }
        }
    }
}