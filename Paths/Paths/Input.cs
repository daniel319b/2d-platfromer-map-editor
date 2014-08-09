using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Paths
{
    /// <summary>
    /// This class handles the input from the user.
    /// </summary>
   public class Input
    {
        static KeyboardState oldKeyState;
        static MouseState oldMouseState;
        static KeyboardState    keyState;
        static MouseState   mouseState;
       

        #region Keyboard
        /// <summary>
        /// Checks if a specified key has been pressed.
        /// </summary>
        public static bool KeyPressed(Keys key)
        {           
            if (oldKeyState.IsKeyUp(key) && keyState.IsKeyDown(key))
                return true;
            return false;
        }

        /// <summary>
        /// Checks if a specified key is down.
        /// </summary>
        public static bool KeyDown(Keys key)
        {
            if (oldKeyState.IsKeyDown(key) && keyState.IsKeyDown(key))
                return true;
            return false;
        }

        /// <summary>
        /// Checks is a specified key is up.
        /// </summary>
        public static bool KeyUp(Keys key)
        {
            if (oldKeyState.IsKeyUp(key) && keyState.IsKeyUp(key))
                return true;
            return false;
        }

        #endregion

        #region Update
        /// <summary>
        /// Updates the Keyboard and the Mouse.
        /// </summary>
        public static void Update()
        {
            //Keyboard Update
            oldKeyState = keyState;
            keyState = Keyboard.GetState();
             
            //Mouse Update
            oldMouseState = mouseState;
            PrevMousePosition = MousePosition;
            mouseState = Mouse.GetState();
            MousePosition = new Vector2(mouseState.X, mouseState.Y);
            mouseRectangle  =  new Rectangle ((int)MousePosition.X,(int)MousePosition.Y,10,10);
        }
        #endregion

        #region Mouse
 
        public static bool LeftButtonDown()
        {
            return mouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool RightButtonDown()
        {
            return mouseState.RightButton == ButtonState.Pressed;
        }

        public static bool LeftButtonReleased()
        {
            return mouseState.LeftButton == ButtonState.Released;
        }

        public static bool RightButtonReleased()
        {
            return mouseState.RightButton == ButtonState.Released;
        }

        public static bool RightButtonPressed()
        {
            return oldMouseState.RightButton == ButtonState.Pressed &&
                      mouseState.RightButton == ButtonState.Released;
        }

        public static bool LeftButtonPressed()
        {
            return oldMouseState.LeftButton == ButtonState.Pressed &&
                      mouseState.LeftButton == ButtonState.Released;
        }

        /// <summary>
        /// Returns the mouse position.
        /// </summary>
        /// <returns>The mouse position</returns>
        public static Vector2 MousePosition
        {
            get;
            private set;
        }

       /// <summary>
       /// The mouse position in the previous frame.
       /// </summary>
        public static Vector2 PrevMousePosition
        {
            get;
            private set;
        }

        static Rectangle mouseRectangle;
        /// <summary>
        /// Returns the mouse rectangle bounds.
        /// </summary>
        public static  Rectangle MouseRectangle
        {
            get { return mouseRectangle; }
            set { mouseRectangle = value; }
        }

        /// <summary>
        /// The mouse texture.
        /// </summary>
        public static Texture2D MouseTexture { get; set; }
        
       /// <summary>
       /// Draws the mouse.
       /// </summary>
       public static void DrawMouse(SpriteBatch spriteBatch)
       {
           spriteBatch.Begin();
           spriteBatch.Draw(MouseTexture, MousePosition, Color.White);
           spriteBatch.End();
       }
        #endregion
    }
}
