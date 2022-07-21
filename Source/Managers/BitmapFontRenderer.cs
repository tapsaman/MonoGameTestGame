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

    

    public static class BitmapFontRenderer
    {
        public static BitmapFont Font;

        public static void DrawString(SpriteBatch spriteBatch, string text, Vector2 position)
        {
            var startPosition = position;

            foreach (int code in text)
            {
                if (code == 10)
                {
                    // 10 = LF = line feed
                    // Used for new lines
                    position.X = startPosition.X;
                    position.Y += Font.LineHeight;
                    continue;
                }
                else if (code == 13)
                {
                    // 13 = CR = carriage return
                    // Ignored
                    continue;
                }
                else if (!Font.Chars.ContainsKey(code))
                {
                    Sys.LogError("Undefined symbol at " + code + " (" + (char)code + ") for " + Font.ToString());
                    Font.Chars[code] = Font.Chars[Font.UndefinedSymbolCode];
                }

                BitmapFontChar c = Font.Chars[code];

                spriteBatch.Draw(
                    Font.Texture,
                    position + c.Offset,
                    c.SourceRectangle,
                    Color.White
                );

                position.X += c.XAdvance + Font.LetterSpacing;
            }
        }
    }
}