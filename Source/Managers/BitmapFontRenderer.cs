using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTestGame.Managers
{
    public static class BitmapFontRenderer
    {
        public static BitmapFont Font;

        public static void DrawString(SpriteBatch spriteBatch, string text, Vector2 position, float yCrop = 0)
        {
            var startX = position.X;
            int line = 0;
            //position -= new Vector2(0, yCrop);

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
        }
    }
}