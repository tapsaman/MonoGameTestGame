/*

source:
https://community.monogame.net/t/2d-rotating-polygon-collision-example-using-planes/9778

*/

// it uses the following names spaces as well.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace ZA6
{
public class PolygonCollisionExample : Game
{
    public static GraphicsDeviceManager graphics;
    public static SpriteBatch spriteBatch;
    public static SpriteFont font;

    Rectangle posRectangle0 = new Rectangle(100, 100, 100, 100);
    Rectangle posRectangle1 = new Rectangle(220, 100, 100, 100);

    Rectangle posRectangle3 = new Rectangle(100, 250, 100, 100);
    Rectangle posRectangle4 = new Rectangle(220, 250, 100, 100);

    float rotationcc = 0.4f;
    float rotationccw = -.4f;

    public PolygonCollisionExample()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
    }
    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }
    protected override void LoadContent()
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        ExampleHelperClass.SetUp(graphics, ref spriteBatch);

    }
    protected override void UnloadContent()
    {
        // TODO: Unload any non ContentManager content here
    }
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        rotationcc += .003f;
        rotationcc = ExampleHelperClass.AlignRotation(rotationcc);
        rotationccw -= .003f;
        rotationccw = ExampleHelperClass.AlignRotation(rotationccw);

        base.Update(gameTime);
    }
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        spriteBatch.Begin();

        Color color0 = Color.Beige;
        if (ExampleHelperClass.IsRectangleWithinCollisionRectangle(posRectangle0, rotationcc, false, posRectangle1, rotationccw, false))
        {
            color0 = Color.Red;
        }
        else
        {
            color0 = Color.Green;
        }
        //spriteBatch.Draw(MiscDraw.dotTexture, posRectangle0, MiscDraw.dotRectangle, Color.Black, rotationcc, Vector2.Zero, SpriteEffects.None, 0);
        //spriteBatch.Draw(MiscDraw.dotTexture, posRectangle1, MiscDraw.dotRectangle, Color.Black, rotationccw, Vector2.Zero, SpriteEffects.None, 0);

        ExampleHelperClass.DrawBasicSquare(posRectangle0, 1, color0, false, false, rotationcc);
        ExampleHelperClass.DrawBasicSquare(posRectangle1, 1, color0, false, false, rotationccw);

        Color color1 = Color.Black;
        if (ExampleHelperClass.IsRectangleWithinCollisionRectangle(posRectangle3, rotationcc, true, posRectangle4, rotationccw, true))
        {
            color1 = Color.Red;
        }
        else
        {
            color1 = Color.Green;
        }
        //MiscDraw.DrawCenteredRotatedRectangle(MiscDraw.dotTexture, posRectangle3, MiscDraw.dotRectangle, rotationcc, Color.AliceBlue);
        //MiscDraw.DrawCenteredRotatedRectangle(MiscDraw.dotTexture, posRectangle4, MiscDraw.dotRectangle, rotationccw, Color.AliceBlue);

        ExampleHelperClass.DrawBasicSquare(posRectangle3, 1, color1, false, true, rotationcc);
        ExampleHelperClass.DrawBasicSquare(posRectangle4, 1, Color.Blue, false, true, rotationccw);

        spriteBatch.End();

        base.Draw(gameTime);
    }
    }

    public static class ExampleHelperClass
    {
        public const float PI = (float)(Math.PI);
        public const float PI2 = (float)(Math.PI * 2);
        public const float PIHALF = (float)(Math.PI * .5f);
        public const float TORADIANS = PI2 / 360.0f;
        public const float TODEGREES = 360.0f / PI2;

        static SpriteBatch spriteBatch;
        public static Texture2D dotTexture;
        public static Rectangle dotRectangle = new Rectangle(0, 0, 1, 1);

        public static void SetUp(GraphicsDeviceManager gdm, ref SpriteBatch sb)
        {
            spriteBatch = sb;
            if (dotTexture == null)
            {
                dotTexture = TextureDotCreate(gdm.GraphicsDevice);
            }
        }

        public static Texture2D TextureDotCreate(GraphicsDevice device)
        {
            Color[] data = new Color[1];
            data[0] = new Color(255, 255, 255, 255);

            return TextureFromColorArray(device, data, 1, 1);
        }
        public static Texture2D TextureFromColorArray(GraphicsDevice device, Color[] data, int width, int height)
        {
            if (width > 2047 || height > 2047)
            {
                Console.WriteLine(" TextureFromColorArray(...) -> Big ass array to texture !");
            }
            Texture2D tex = new Texture2D(device, width, height);
            tex.SetData<Color>(data);
            return tex;
        }

        public static void DrawBasicPoint(Vector2 p, Color c)
        {
            Rectangle screendrawrect = new Rectangle((int)p.X, (int)p.Y, 2, 2);
            spriteBatch.Draw(dotTexture, screendrawrect, new Rectangle(0, 0, 1, 1), c, 0.0f, Vector2.One, SpriteEffects.None, 0);
        }
        public static void DrawLine(Vector2 postion, int length, int linethickness, float rot, Color c)
        {
            Rectangle screendrawrect = new Rectangle((int)postion.X, (int)postion.Y, linethickness, length);
            spriteBatch.Draw(dotTexture, screendrawrect, new Rectangle(0, 0, 1, 1), c, rot, Vector2.Zero, SpriteEffects.None, 0);
        }
        public static void DrawBasicLine(Vector2 s, Vector2 e, int thickness, Color linecolor, float rot)
        {
            float distance = Vector2.Distance(s, e);
            float direction = (float)Atan2Xna(e.X - s.X, e.Y - s.Y);
            //direction = DirectionToRadians(e.X - s.X, e.Y - s.Y);
            direction += rot;
            Rectangle screendrawrect = new Rectangle((int)s.X, (int)s.Y, thickness, (int)distance);
            spriteBatch.Draw(dotTexture, screendrawrect, new Rectangle(0, 0, 1, 1), linecolor, direction, Vector2.Zero, SpriteEffects.None, 0);
        }
        public static void DrawBasicLine(Vector2 s, Vector2 e, int thickness, Color linecolor)
        {
            float distance = Vector2.Distance(e, s); ;
            float direction = (float)Atan2Xna(e.X - s.X, e.Y - s.Y);
            Rectangle screendrawrect = new Rectangle((int)s.X, (int)s.Y, thickness, (int)distance);
            spriteBatch.Draw(dotTexture, screendrawrect, new Rectangle(0, 0, 1, 1), linecolor, direction, Vector2.Zero, SpriteEffects.None, 0);
        }


        public static void DrawBasicSquare(Rectangle r, int thickness, Color col, bool draw_filled, bool drawCentered, float rotation)
        {
            if (drawCentered)
                DrawBasicSquare(r, thickness, col, draw_filled, rotation, new Vector2(r.X + r.Width * .5f, r.Y + r.Height * .5f));
            else
                DrawBasicSquare(r, thickness, col, draw_filled, rotation, new Vector2(r.X, r.Y));
        }
        public static void DrawBasicSquare(Rectangle r, int thickness, Color col, bool draw_filled, float rotation, Vector2 rotOrigin)
        {
            if (draw_filled == false)
            {
                float q = rotation;
                Vector2 A = new Vector2(r.Left, r.Top);
                Vector2 B = new Vector2(r.Right, r.Top);
                Vector2 C = new Vector2(r.Right, r.Bottom);
                Vector2 D = new Vector2(r.Left, r.Bottom);
                Vector2 o = rotOrigin;
                A = Rotate2dPointAboutOriginOnZaxis(A, o, q);
                B = Rotate2dPointAboutOriginOnZaxis(B, o, q);
                C = Rotate2dPointAboutOriginOnZaxis(C, o, q);
                D = Rotate2dPointAboutOriginOnZaxis(D, o, q);
                ExampleHelperClass.DrawLine(A, r.Width, thickness, q, col);
                ExampleHelperClass.DrawLine(B, r.Height, thickness, q + PIHALF, col);
                ExampleHelperClass.DrawLine(C, r.Width, thickness, q + (PIHALF * 2), col);
                ExampleHelperClass.DrawLine(D, r.Height, thickness, q + (PIHALF * 3), col);
            }
            else
            {
                spriteBatch.Draw(dotTexture, r, new Rectangle(0, 0, 1, 1), col, rotation, rotOrigin, SpriteEffects.None, 0);
            }
        }

        /* collision stuff */

        // didnt test
        public static bool IsUnrotatedRectanglesColliding(Rectangle r0, Rectangle r1)
        {
            Point cd = r1.Center - r0.Center;
            if (cd.X < 0) { cd.X = -cd.X; }
            if (cd.Y < 0) { cd.Y = -cd.Y; }
            Point gwh = new Point( (int)((r0.Width + r1.Width) * .5f), (int)((r0.Height + r1.Height) * .5f));
            if(cd.X<gwh.X && cd.Y<gwh.Y)
                return true;
            else
                return false;
        }

        // rotated simple origins
        public static bool IsRectangleWithinCollisionRectangle(Rectangle r0, float rotationR0, bool centerOriginR0, Rectangle r1, float rotationR1, bool centerOriginR1)
        {
            Vector2 org0 = new Vector2(r0.X, r0.Y);
            if (centerOriginR0)
                org0 = new Vector2(r0.X + r0.Width * .5f, r0.Y + r0.Height * .5f);

            Vector2 org1 = new Vector2(r1.X, r1.Y);
            if (centerOriginR1)
                org1 = new Vector2(r1.X + r1.Width * .5f, r1.Y + r1.Height * .5f);

            return IsRectangleWithinCollisionRectangle(r0, rotationR0, org0, r1, rotationR1, org1);
        }

        // rotated 0,0 is the coordinate system origin
        public static bool IsRectangleWithinCollisionRectangle(Rectangle r0, float rotationR0, Vector2 rotOriginR0, Rectangle r1, float rotationR1, Vector2 rotOriginR1)
        {
            // This can be any shape its not limited to rectangles its basically a polygon boundry check.
            // typically you use arrays however im going to use variables step by step.

            // get rotated points of rectangle 1
            Vector2 A0 = new Vector2(r0.Left, r0.Top);
            Vector2 B0 = new Vector2(r0.Right, r0.Top);
            Vector2 C0 = new Vector2(r0.Right, r0.Bottom);
            Vector2 D0 = new Vector2(r0.Left, r0.Bottom);
            // optimally you store the shapes points in clockwise (cw) or ccw order.
            // we could also do this with just two rotations saving a lot of this extra work
            A0 = Rotate2dPointAboutOriginOnZaxis(A0, rotOriginR0, rotationR0);
            B0 = Rotate2dPointAboutOriginOnZaxis(B0, rotOriginR0, rotationR0);
            C0 = Rotate2dPointAboutOriginOnZaxis(C0, rotOriginR0, rotationR0);
            D0 = Rotate2dPointAboutOriginOnZaxis(D0, rotOriginR0, rotationR0);

            // get rotated points of rectangle 2
            Vector2 A1 = new Vector2(r1.Left, r1.Top);
            Vector2 B1 = new Vector2(r1.Right, r1.Top);
            Vector2 C1 = new Vector2(r1.Right, r1.Bottom);
            Vector2 D1 = new Vector2(r1.Left, r1.Bottom);
            A1 = Rotate2dPointAboutOriginOnZaxis(A1, rotOriginR1, rotationR1);
            B1 = Rotate2dPointAboutOriginOnZaxis(B1, rotOriginR1, rotationR1);
            C1 = Rotate2dPointAboutOriginOnZaxis(C1, rotOriginR1, rotationR1);
            D1 = Rotate2dPointAboutOriginOnZaxis(D1, rotOriginR1, rotationR1);

            // you can return true with just one match but this is left to demonstrate.
            bool match = false;

            // first rectangle
            // well use this to give more visual info to the user
            if(IsPointWithinRectangleDisplay(A0, B0, C0, D0, A1, false)) { match = true; }
            // in 2d also with just rectangles we could do this with just 2 points a target and a normal
            // per rectangle but since this is a demo
            if (IsPointWithinRectangle(A0, B0, C0, D0, B1, false)) { match = true; }
            if (IsPointWithinRectangle(A0, B0, C0, D0, C1, false)) { match = true; }
            if (IsPointWithinRectangle(A0, B0, C0, D0, D1, false)) { match = true; }
            // second rectangle
            if (IsPointWithinRectangle(A1, B1, C1, D1, A0, false)) { match = true; }
            if (IsPointWithinRectangle(A1, B1, C1, D1, B0, false)) { match = true; }
            if (IsPointWithinRectangle(A1, B1, C1, D1, C0, false)) { match = true; }
            if (IsPointWithinRectangle(A1, B1, C1, D1, D0, false)) { match = true; }

            if (match)
                return true;
            else
                return false;
        }
        public static bool IsPointWithinRectangle(Vector2 A, Vector2 B, Vector2 C, Vector2 D, Vector2 collision_point, bool reversewindingalsotrue)
        {
            int numberofplanescrossed = 0;
            if (HasPointCrossedPlane2d(A, B, collision_point)) { numberofplanescrossed++; } else { numberofplanescrossed--; }
            if (HasPointCrossedPlane2d(B, C, collision_point)) { numberofplanescrossed++; } else { numberofplanescrossed--; }
            if (HasPointCrossedPlane2d(C, D, collision_point)) { numberofplanescrossed++; } else { numberofplanescrossed--; }
            if (HasPointCrossedPlane2d(D, A, collision_point)) { numberofplanescrossed++; } else { numberofplanescrossed--; }
            if ((reversewindingalsotrue == false && numberofplanescrossed >= 4) || (reversewindingalsotrue && numberofplanescrossed <= -4))
            { return true; }
            else
            { return false; }
        }
        public static bool HasPointCrossedPlane2d(Vector2 start, Vector2 end, Vector2 collision_point)
        {
            Vector2 B = (end - start);
            Vector2 A = (collision_point - start);
            // We only need the signed result
            // cross right and dot 
            float sign = A.X * -B.Y + A.Y * B.X;
            if (sign > 0.0f)
                return true;
            else
                return false;
        }

        public static Vector2 Rotate2dPointAboutOriginOnZaxis(Vector2 p, Vector2 o, double q)
        {
            //x' = x*cos s - y*sin s , y' = x*sin s + y*cos s 
            double x = p.X - o.X; // transform locally to the orgin
            double y = p.Y - o.Y;
            double rx = x * Math.Cos(q) - y * Math.Sin(q);
            double ry = x * Math.Sin(q) + y * Math.Cos(q);
            p.X = (float)rx + o.X; // translate back to non local
            p.Y = (float)ry + o.Y;
            return p;
        }

        public static void DrawCenteredRotatedRectangle(Texture2D textureobj, Rectangle screenrect, Rectangle texturerect, float rot, Color color)
        {
            // xna implicitly set the origin to the top left point of the rectangle.
            // which i argued is not proper however it is made that way anyways.
            // 0 0 should be the top left of the coordinate system not the local object origin.
            // if your going to pick a arbitrary rotation origin and name it center put it in the center of the local object.
            screenrect.X += (int)(screenrect.Width * .5f);
            screenrect.Y += (int)(screenrect.Height * .5f);
            Vector2 toff = new Vector2(texturerect.Width * .5f, texturerect.Height * .5f);
            spriteBatch.Draw(textureobj, screenrect, texturerect, color, rot, toff, SpriteEffects.None, 0);
        }

        public static float Atan2Xna(float difx, float dify)
        {
            return (float)Math.Atan2(difx, dify) * -1;
        }

        public static float AlignRotation(float rotation)
        {
            if (rotation > ExampleHelperClass.PI2) { rotation -= ExampleHelperClass.PI2; }
            if (rotation < 0f) { rotation += ExampleHelperClass.PI2; }
            return rotation;
        }


        // visual version
        public static bool IsPointWithinRectangleDisplay(Vector2 A, Vector2 B, Vector2 C, Vector2 D, Vector2 collision_point, bool reversewindingalsotrue)
        {
            int numberofplanescrossed = 0;
            if (HasPointCrossedPlane2d(A, B, collision_point)) { DisplayRightCrossLine(A, B, collision_point, true); numberofplanescrossed++; } else { DisplayRightCrossLine(A, B, collision_point, false); numberofplanescrossed--; }
            if (HasPointCrossedPlane2d(B, C, collision_point)) { DisplayRightCrossLine(B, C, collision_point, true); numberofplanescrossed++; } else { DisplayRightCrossLine(B, C, collision_point, false); numberofplanescrossed--; }
            if (HasPointCrossedPlane2d(C, D, collision_point)) { DisplayRightCrossLine(C, D, collision_point, true); numberofplanescrossed++; } else { DisplayRightCrossLine(C, D, collision_point, false); numberofplanescrossed--; }
            if (HasPointCrossedPlane2d(D, A, collision_point)) { DisplayRightCrossLine(D, A, collision_point, true); numberofplanescrossed++; } else { DisplayRightCrossLine(D, A, collision_point, false); numberofplanescrossed--; }
            if ((reversewindingalsotrue == false && numberofplanescrossed >= 4) || (reversewindingalsotrue && numberofplanescrossed <= -4))
            { return true; }
            else
            { return false; }
        }
        // visual
        public static void DisplayRightCrossLine(Vector2 start, Vector2 end, Vector2 targetPosition, bool trueorfalse)
        {
            Vector2 B = (end - start);
            Vector2 visualstart = (end - start) * .5f + start;
            Vector2 visualNormal = new Vector2(-B.Y, B.X);
            if (trueorfalse)
            {
                DrawBasicLine(visualstart, (.2f * visualNormal + visualstart), 1, Color.Red);
                DrawBasicLine(visualstart, targetPosition, 1, Color.Red);
            }
            else
            {
                DrawBasicLine(visualstart, (.2f * visualNormal + visualstart), 1, Color.Green);
                DrawBasicLine(visualstart, targetPosition, 1, Color.Green);
            }
        }
    }
}