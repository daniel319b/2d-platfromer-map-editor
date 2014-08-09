using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Paths
{
    class CheckBox : GameObject
    {
        public bool Checked { get; set; }

        public Texture2D ONTexture, OFFTexture;

        public CheckBox(Vector2 pos) : base(pos)
        {
            ONTexture = TextureManager.SetTexture("UI/CheckboxON");
            OFFTexture = TextureManager.SetTexture("UI/CheckboxOFF");
            texture = OFFTexture;         
        }   

        public override void Update()
        {
            base.Update();
            if (Input.MouseRectangle.Intersects(BoundBox) && Input.LeftButtonPressed()) 
                Checked = !Checked;//Invert
            if (Checked == true)
                texture = ONTexture;
            else
                texture = OFFTexture;
        }

       
    }
}
