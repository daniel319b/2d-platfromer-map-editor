using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Paths
{
    /// <summary>
    /// A helper class that can write text on screen.
    /// </summary>
    public static class ScreenWriter
    {
        public static SpriteBatch SpriteBatch
        { get; set; }

        public static void Write(string text, Vector2 position, Color color)
        {
          //  SpriteBatch.Begin();
            SpriteBatch.DrawString(Font.Regular, text, position, color);
           // SpriteBatch.End();
        }      
    }
}
