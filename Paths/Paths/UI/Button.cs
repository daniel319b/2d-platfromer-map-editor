using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Paths
{
    /// <summary>
    /// This class represents a button which the user can click on.
    /// </summary>
    class Button : GameObject
    {

        /// <summary>
        /// This event fires when the user has clicked the button. 
        /// </summary>
        public event EventHandler Clicked;

        /// <summary>
        /// The color of the text.
        /// </summary>
        public Color TextColor = Color.Black;
        /// <summary>
        /// Constructs a new Button with only a texture.
        /// </summary>
        /// <param name="position">The position of the button.</param>
        /// <param name="texture">The texture of the button.</param>
        public Button(Vector2 position, Texture2D texture)
            : base(position, texture) 
        {
          
        }
      
        /// <summary>
        /// Creates a new Button with the default texture and a specified text.
        /// </summary>
        /// <param name="position">The position of the button.</param>
        /// <param name="text">The text that will be displayed on the button.</param>
        public Button(Vector2 position, string text)
            : base(position)
        {
            this.text = text;
        }

        /// Creates a new Button with a specified texture and a specified text.
        /// </summary>
        /// <param name="position">The position of the button.</param>
        /// <param name="texture">The texture of the button.</param>
        /// <param name="text">The text that will be displayed on the button.</param>
        public Button(Vector2 position, Texture2D texture, string text)
            : base(position, texture)
        {
            this.text = text;
        }

        public override void Update()
        {
            if (text != null)
                boundBox = new Rectangle((int)position.X, (int)position.Y, (int)Font.Regular.MeasureString(text).X+20 , (int)Font.Regular.MeasureString(text).Y +10);
            else
                boundBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            if (IsClicked())
                onLeftClick();
        }

        public bool IsClicked()
        {
            return Input.MouseRectangle.Intersects(boundBox) && Input.LeftButtonPressed();
        }

        /// <summary>
        /// Fires up the event for the left click on the button.
        /// </summary>
        public void onLeftClick()
        {
            if (Clicked != null)
                Clicked(this, EventArgs.Empty);
        }


        public override void Draw(SpriteBatch sb)
        {
            if (text != null)
            {
                if (texture != null)
                {
                    sb.Draw(texture, boundBox, Color.White);
                    sb.DrawString(Font.Regular, text, Position + new Vector2 (10,5), TextColor, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                }
                else//if only text.
                    sb.DrawString(Font.Regular, text, position, TextColor);
            }
            else//if there is no text draw the regular texture.
                base.Draw(sb);
        }
    }
}
