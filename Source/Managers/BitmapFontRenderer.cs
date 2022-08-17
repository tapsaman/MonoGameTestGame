using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine;

namespace ZA6.Managers
{
    public static class BitmapFontRenderer
    {
        public static BitmapFont Font;

        public static Point CalculateSize(string text, float yCrop = 0)
        {
            int w = 0, h = 0;
            int line = 0;
            int greatestWidth = 0;

            for (int i = 0; i < text.Length; i++)
            {
                int code = text[i];
                int xAdvance;

                if (code == 10)
                {
                    // 10 = LF = line feed
                    // Used for new lines
                    if (w > greatestWidth) 
                        greatestWidth = w;

                    w = 0;
                    h += Font.LineHeight;

                    if (line == 0)
                    {
                        h -= (int)yCrop;
                    }

                    line++;
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
                    var c = Font.Chars[Font.UndefinedSymbolCode];
                    xAdvance = c.XAdvance;
                }
                else
                {
                    var c = Font.Chars[code];
                    xAdvance = c.XAdvance;
                }

                w += xAdvance;
                
                if (i != text.Length - 1 && (text[i + 1] != 10 || text[i + 1] != 13))
                {
                    // Not last letter of line
                    w += Font.LetterSpacing;
                }
            }

            return new Point(w, h);
        }

        public static void DrawString(SpriteBatch spriteBatch, string text, Vector2 position, float yCrop = 0, int? highlightRow = null)
        {
            position.Round();
            var startX = position.X;
            int line = 0;
            //position -= new Vector2(0, yCrop);

            if (highlightRow == line)
            {
                Static.Renderer.ChangeToHighlightEffect();
            }

            foreach (int code in text)
            {
                BitmapFontChar c;
                int xAdvance;
                Rectangle sourceRectangle;
                Vector2 offset;

                if (code == 10)
                {
                    // 10 = LF = line feed
                    // Used for new lines
                    position.X = startX;
                    position.Y += Font.LineHeight;
                    if (line == 0)
                    {
                        position.Y -= yCrop;
                    }
                    line++;

                    if (highlightRow == line)
                    {
                        Static.Renderer.ChangeToHighlightEffect();
                    }
                    else
                    {
                        Static.Renderer.ChangeToDefault();
                    }
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
                    c = Font.Chars[Font.UndefinedSymbolCode];
                    xAdvance = c.XAdvance;
                    sourceRectangle = c.SourceRectangle;
                    offset = c.Offset;
                }
                else
                {
                    c = Font.Chars[code];
                    xAdvance = c.XAdvance;
                    sourceRectangle = c.SourceRectangle;
                    offset = c.Offset;
                }

                if (line == 0 && yCrop != 0)
                {
                    int charYCrop = Math.Max(0, (int)(yCrop - c.Offset.Y));
                    sourceRectangle = new Rectangle(
                        sourceRectangle.X,
                        sourceRectangle.Y + charYCrop,
                        sourceRectangle.Width,
                        sourceRectangle.Height - charYCrop
                    );
                    offset = new Vector2(c.Offset.X, c.Offset.Y + charYCrop - yCrop);
                    //offset = c.Offset;
                }

                spriteBatch.Draw(
                    Font.Texture,
                    position + offset,
                    sourceRectangle,
                    Color.White
                );

                position.X += xAdvance + Font.LetterSpacing;
            }

            Static.Renderer.ChangeToDefault();
        }
    }
}