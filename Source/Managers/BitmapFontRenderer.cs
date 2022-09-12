using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TapsasEngine;

namespace ZA6.Managers
{
    public static class BitmapFontRenderer
    {
        public static BitmapFont Font;
        public static Effect[] FontEffects;

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

        public static void DrawString(SpriteBatch spriteBatch, string text, Vector2 position, float yCrop = 0, Color? color = null)
        {
            position.Round();
            var startX = position.X;
            int line = 0;
            //position -= new Vector2(0, yCrop);

            /*if (highlightRow == line)
            {
                Static.Renderer.ChangeToHighlightEffect();
            }*/

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                ushort code = c;

                BitmapFontChar fontChar;
                int xAdvance;
                Rectangle sourceRectangle;
                Vector2 offset;

                if (c == '\n')
                {
                    // \n = 10 = LF line feed
                    // Used for new lines

                    position.X = startX;
                    position.Y += Font.LineHeight;

                    if (line == 0)
                    {
                        position.Y -= yCrop;
                    }

                    line++;

                    continue;

                    /*if (highlightRow == line)
                    {
                        Static.Renderer.ChangeToHighlightEffect();
                    }
                    else
                    {
                        Static.Renderer.ChangeToDefault();
                    }*/
                }
                else if (c == '\f')
                {
                    // 12 = \f = fromfeed page break
                    // Apply effect

                    if (text.Length > i + 2)
                    {
                        string effectNumber = text.Substring(i + 1, 2);
                        Static.Renderer.ChangeToEffect(FontEffects[Int32.Parse(effectNumber)]);
                    }
                    
                    i += 2;

                    continue;
                }
                else if (code == 13)
                {
                    // 13 = \r = CR carriage return
                    // Ignored

                    continue;
                }
                else if (!Font.Chars.ContainsKey(code))
                {
                    Sys.LogError("Undefined symbol at " + code + " (" + (char)code + ") for " + Font.ToString());
                    Font.Chars[code] = Font.Chars[Font.UndefinedSymbolCode];

                    fontChar = Font.Chars[Font.UndefinedSymbolCode];
                    xAdvance = fontChar.XAdvance;
                    sourceRectangle = fontChar.SourceRectangle;
                    offset = fontChar.Offset;
                }
                else
                {
                    fontChar = Font.Chars[code];
                    xAdvance = fontChar.XAdvance;
                    sourceRectangle = fontChar.SourceRectangle;
                    offset = fontChar.Offset;
                }

                if (line == 0 && yCrop != 0)
                {
                    int charYCrop = Math.Max(0, (int)(yCrop - offset.Y));
                    sourceRectangle = new Rectangle(
                        sourceRectangle.X,
                        sourceRectangle.Y + charYCrop,
                        sourceRectangle.Width,
                        sourceRectangle.Height - charYCrop
                    );
                    offset = new Vector2(offset.X, offset.Y + charYCrop - yCrop);
                    //offset = c.Offset;
                }

                spriteBatch.Draw(
                    Font.Texture,
                    position + offset,
                    sourceRectangle,
                    color ?? Color.White
                );

                position.X += xAdvance + Font.LetterSpacing;
            }

            Static.Renderer.ChangeToDefault();
        }
    }
}