using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Paths
{
    class SaveDialogBox : Form
    {
         Map map;

        public SaveDialogBox(Vector2 pos,Map map)
            : base(pos, TextureManager.SetTexture("UI/DialogBox"))
        {
            this.map = map;
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            ScreenWriter.Write("Map Successfully Saved To \n            "+map.Name+".xml", Position + new Vector2(10, 30), Color.Black);  
        }
    }
}
