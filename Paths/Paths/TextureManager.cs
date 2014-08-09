using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Paths
{
    /// <summary>
    /// A helper class that can load textures.
    /// </summary>
    public class TextureManager
    {
        public static ContentManager Content
        {
            get;
            set;
        }

        /// <summary>
        /// Set a texture from the Content to an object.
        /// </summary>
        /// <param name="assestName">The assest name as it's in the Content menu.</param>
        /// <returns></returns>
        public static Texture2D SetTexture(string assestName)
        {
           Texture2D tex =  Content.Load<Texture2D>(assestName);
           tex.Name = assestName;
           return tex;
        }
    }
}
