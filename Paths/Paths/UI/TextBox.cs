using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Paths
{
    /// <summary>
    /// This class represents a Text Box.
    /// </summary>
    class TextBox : GameObject
    {
        /// <summary>
        /// The max length of the text that the text box can hold.
        /// </summary>
        public int maxLength { get; set; }
        /// <summary>
        /// The text color.
        /// </summary>
        public Color textColor { get; set; }
        /// <summary>
        /// The width of the text box.
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// The height of the text box.
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Is this text box enabled? Can it get input?
        /// </summary>
        public bool Enabled { get; set; }

        int timer = 0;

        /// <summary>
        /// Creates a new Text Box with a specified texture.
        /// </summary>
        /// <param name="pos">The position of the text box.</param>
        /// <param name="tex">The texture of the text box.</param>
        public TextBox(Vector2 pos, Texture2D tex)
            : base(pos, tex) 
        {
            Initialize();
        }

        /// <summary>
        /// Creates a new Text Box with the defalut texture.
        /// </summary>
        /// <param name="pos"></param>
        public TextBox(Vector2 pos)
            : base(pos, TextureManager.SetTexture("UI/txtBox"))
        {
            Initialize();
        }

        private void Initialize()
        {
            maxLength = 14;
            text = "";
            textColor = Color.Black;
            Width = texture.Width;
            Height = texture.Height;
        }

        public override void Update()
        {
            boundBox = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            if (Enabled)//Only update (get input) if enabled.
            {
                foreach (Keys key in Keyboard.GetState().GetPressedKeys())//llop through every pressed key.
                {
                    if (Input.KeyPressed(key))
                    {
                        AddKeyToText(key);//Add to the text if pressed.
                    }
                    if (Input.KeyDown(Keys.Back) && timer > 8)
                    {
                        if (text.Length != 0) //backspace    
                            text = text.Remove(text.Length - 1);//removes the last character.
                        timer = 0;
                    }
                    timer++;
                }
            }
        }

        private void AddKeyToText(Keys key)
        {
            string newChar = "";

            if (text.Length >= maxLength && key != Keys.Back)
                return;


            if (key >= Keys.A && key <= Keys.Z)
            {
                newChar = key.ToString().ToLower();
                if (Input.KeyDown(Keys.RightShift) || Input.KeyDown(Keys.LeftShift))
                    newChar = newChar.ToUpper();
            }
            else if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
            {
                newChar = ((int)(key - Keys.NumPad0)).ToString();

            }
            else if (key >= Keys.D0 && key <= Keys.D9)
            {
                string num = ((int)(key - Keys.D0)).ToString();
                #region Special Keys
                if (Input.KeyDown(Keys.RightShift) || Input.KeyDown(Keys.LeftShift))
                {
                    if (num == "1")
                        newChar = "!";
                    else if (num == "2")
                        newChar = "@";
                    else if (num == "3")
                        newChar = "#";
                    else if (num == "4")
                        newChar = "$";
                    else if (num == "5")
                        newChar = "%";
                    else if (num == "6")
                        newChar = "^";
                    else if (num == "7")
                        newChar = "&";
                    else if (num == "8")
                        newChar = "*";
                    else if (num == "9")
                        newChar = "(";
                    else if (num == "0")
                        newChar = ")";
                }
                #endregion
                else
                    newChar = num;
            }
            else if (key == Keys.OemPeriod)
                newChar += ".";
            else if (key == Keys.OemTilde)
                newChar += " ' ";
            else if (key == Keys.Space)
                newChar += " ";
            else if (key == Keys.OemMinus)
                newChar += "-";
            else if (key == Keys.OemPlus)
                newChar += "+";
            else if (key == Keys.OemQuestion && Input.KeyDown(Keys.RightShift) || Input.KeyDown(Keys.LeftShift))
                newChar += "?";
            else if (key == Keys.Enter)
                newChar = "\n";


            text += newChar;//add the char to the output text      
        }

        public override void Draw(SpriteBatch sb)
        { 
            sb.Draw(texture, boundBox, Color.White * opacity);
            sb.DrawString(Font.Regular, text, new Vector2(boundBox.X+10, BoundBox.Y+5), textColor, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }
        
        

       
    }
}
