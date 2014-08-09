using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Paths
{
    class ObjectPropertiesWindow : Form
    {
        public TextBox txtPositionX,txtPositionY, txtTexture, txtRotation, txtOpacity, txtScale;
        TextBox txtSelected;

        GameObject selectedObject;

        public ObjectPropertiesWindow(Vector2 position, Texture2D texture,GameObject selectedObject)
            : base(position, texture)
        {
            this.selectedObject = selectedObject;
         
            //Initialize textboxes.
            txtSelected = txtPositionX = new TextBox(Position + new Vector2(100, 15)); txtPositionX.Width = 100;
            txtPositionY = new TextBox(Position + new Vector2(220, 15)); txtPositionY.Width = 100;            
            txtTexture = new TextBox(Position + new Vector2(150, 65)); txtTexture.Width += 50;
            txtRotation = new TextBox(Position + new Vector2(100, 115)); txtRotation.Width = 100;
            txtOpacity = new TextBox(Position + new Vector2(95, 165)); txtOpacity.Width = 100;
            txtScale = new TextBox(Position + new Vector2(75, 215)); txtScale.Width = 100;
            AddControls(txtPositionX, txtPositionY, txtTexture, txtRotation, txtOpacity, txtScale); 

            //set up the text boxes text to the oject's properties.
            txtPositionX.Text = selectedObject.Position.X.ToString();
            txtPositionY.Text = selectedObject.Position.Y.ToString();
            txtTexture.Text = selectedObject.TextureName;
            txtRotation.Text = selectedObject.Rotation.ToString();
            txtScale.Text = selectedObject.Scale.ToString();
            txtOpacity.Text = selectedObject.Opacity.ToString();
        }

        protected override void btnClose_Clicked(object sender, EventArgs e)
        {
            Vector2 newPos = new Vector2(float.Parse(txtPositionX.Text), float.Parse(txtPositionY.Text));
            selectedObject.Position = newPos;
            selectedObject.Rotation = float.Parse(txtRotation.Text);
            selectedObject.Scale = float.Parse(txtScale.Text);
            selectedObject.Opacity = float.Parse(txtOpacity.Text);
            base.btnClose_Clicked(sender, e);
        }

        public override void Update()
        {   
            base.Update();
            foreach (GameObject control in Controls)
            {
                TextBox t = control as TextBox;
                if (t == null) continue;

                t.Opacity = 0.4f;//fade out the non selected text boxes.
                t.Enabled = false;//Do not enable the non selected text boxes.
                if (Input.MouseRectangle.Intersects(t.BoundBox) && Input.LeftButtonPressed())
                    if (t != txtSelected)
                        txtSelected = t;//if the user clicked a text box, select it.
                txtSelected.Enabled = true;
            }
            txtSelected.Opacity = 1;
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);     
            WriteText(sb);
        }

        private void WriteText(SpriteBatch sb)
        {
            ScreenWriter.Write("Position:",Position + new Vector2(10, 20), Color.Black);
            ScreenWriter.Write("Texture Name:", Position + new Vector2(10, 70), Color.Black);
            ScreenWriter.Write("Rotation:",Position + new Vector2(10, 120), Color.Black);
            ScreenWriter.Write("Opacity:",Position + new Vector2(10, 170), Color.Black);
            ScreenWriter.Write("Scale:", Position + new Vector2(10, 220), Color.Black);
        }
    }
}
