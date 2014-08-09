using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Paths
{
    public class Camera
    {
        public static Vector2 Position;    
        static Rectangle bounds;
        static float scale = 1;

        public static float Scale
        {
            get { return Camera.scale; }
            set { Camera.scale = value; }
        }

        public static Rectangle ScreenBounds
        {
            get { return bounds; }
            set { bounds = value; Position = new Vector2(value.Width / 2, value.Height / 2); }
        }

        public static bool InView(Rectangle rec)
        {
            Rectangle bounds2 = new Rectangle((int)(Position.X - (bounds.Width / 2.0f)), (int)(Position.Y -( bounds.Height / 2.0f)), bounds.Width, bounds.Height);
            if (rec.Intersects(bounds2))
                return true;
            return false;
        }

        public static Matrix Transform
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(-Position, 0)) *
                       Matrix.CreateScale(Scale)*
                       Matrix.CreateTranslation(0,0/*ScreenBounds.Width / 2, ScreenBounds.Height / 2*/, 0); }
        }

        public static Vector2 ToWordCoords(Vector2 Position)
        {
            return Vector2.Transform(Position,Matrix.Invert(Transform));
        }



        
    }
}
