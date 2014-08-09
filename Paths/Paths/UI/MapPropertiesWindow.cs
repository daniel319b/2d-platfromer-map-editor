using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Paths
{
    class MapPropertiesWindow : Form
    {
        public Button btnUpdate;

        public TextBox txtName, txtTileX, txtTileY,txtSelected;
        Map map;

       
       
        public MapPropertiesWindow(Vector2 pos,Map map)
            : base(pos, TextureManager.SetTexture("UI/MapSettingBack"))
        {
            this.map = map;
            btnUpdate = new Button(Position + new Vector2(100, 130),TextureManager.SetTexture ("UI/Button"), "Update Info");
            txtSelected = txtName = new TextBox(Position + new Vector2(120, 10), TextureManager.SetTexture("UI/txtBox"));
            txtTileX = new TextBox(Position + new Vector2(120, 50), TextureManager.SetTexture("UI/txtBox"));
            txtTileY = new TextBox(Position + new Vector2(120, 90), TextureManager.SetTexture("UI/txtBox"));
            txtTileY.maxLength = 3; txtTileX.maxLength = 3;

            txtName.Text = map.Name;
            txtTileX.Text = map.TileSize.X.ToString ();
            txtTileY.Text = map.TileSize.Y.ToString ();
         
            AddControls(txtName, txtTileX, txtTileY,btnUpdate);
            btnUpdate.Clicked += new EventHandler(btnUpdate_Clicked);
        }

        void btnUpdate_Clicked(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtTileX.Text == "" || txtTileY.Text == "")
                return;
            map.Name = txtName.Text;
            map.TileSize = new Point(int.Parse(txtTileX.Text), int.Parse(txtTileY.Text));
            Tile.Height = map.TileSize.Y;
            Tile.Width = map.TileSize.X;
            FormsManager.RemoveForm(this);
        }

    
        public override void Update()
        {
            if (Input.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Enter))//If Enter So save.
                btnUpdate.onLeftClick();
            else if (Input.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))//If ESC so dont save.
                btnClose.onLeftClick();

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
            ScreenWriter.Write("Map Name:", Position + new Vector2(10, 15), Color.Black);
            ScreenWriter.Write("Tile Width:", Position + new Vector2(10, 50), Color.Black);
            ScreenWriter.Write("Tile Height:", Position + new Vector2(10, 95), Color.Black);
        }
    }
}
