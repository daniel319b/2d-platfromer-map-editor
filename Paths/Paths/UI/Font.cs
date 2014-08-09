using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Paths
{
    /// <summary>
    /// A helper class that contains Fonts.
    /// </summary>
    public class Font
    {
        static SpriteFont regular;
        static SpriteFont title, menuEntry;

        /// <summary>
        /// Regular font for drawing in game.
        /// </summary>
        public static SpriteFont Regular
        {
            get { return Font.regular; }
            set { Font.regular = value; }
        }
    
        /// <summary>
        /// A menu entry font.
        /// </summary>
        public static SpriteFont MenuEntry
        {
            get { return Font.menuEntry; }
            set { Font.menuEntry = value; }
        }

        /// <summary>
        /// A menu title font.
        /// </summary>
        public static SpriteFont Title
        {
            get { return Font.title; }
            set { Font.title = value; }
        }
    }
}
