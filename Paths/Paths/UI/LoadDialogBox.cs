using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Paths
{
    class LoadDialogBox : Form
    {
        public Button btnLoad;
        public List<GameObject> maps;
        public GameObject selectedMap;

        public LoadDialogBox(Vector2 pos)
            : base(pos, TextureManager.SetTexture("UI/LoadDialogBox"))
        {
            maps = new List<GameObject>();
            for (int i = 0; i < getMaps().Count; i++)
            {
                GameObject g = new GameObject(Position + new Vector2(10,40 + 30 * i));
                g.Text = getMaps()[i];
                g.Update();//this is a static object so it only needs 1 update.
                g.Color = Color.Blue;
                maps.Add(g);
            }

            btnLoad = new Button(Position + new Vector2(290,220),TextureManager.SetTexture ("UI/Button"),"Load Selected Map");
            AddControls(btnLoad);
        }

        public override void Update()
        {
            if (Input.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Enter))//If Enter So save.
                btnLoad.onLeftClick();
            else if (Input.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))//If ESC so dont save.
                btnClose.onLeftClick();

            base.Update();
            
            foreach (GameObject name in maps)
            {
                if (Input.MouseRectangle.Intersects(name.BoundBox))
                {
                    name.Color = Color.Blue * 0.6f;
                    if (Input.LeftButtonDown())
                    {
                        selectedMap = name;
                        name.Color = Color.Black;
                    }
                }
                else
                    name.Color = Color.Blue;
                if(selectedMap != null)
                   selectedMap.Color = Color.Red;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            ScreenWriter.Write("Maps Available:", Position + new Vector2(10, 10), Color.Black);
            foreach (GameObject a in maps)
                a.Draw(sb);
        }
        /// <summary>
        /// Gets all the maps names form the current directory.
        /// </summary>
        /// <returns></returns>
        List<string> getMaps()
        {
            List<string> maps = new List<string> ();
            string[] files = System.IO.Directory.GetFiles(Environment.CurrentDirectory, "*.xml");
            foreach (string s in files)
            {
                string s2 = s.Substring(Environment.CurrentDirectory.Length+1);
                maps.Add(s2.Substring(0,s2.Length -4));//subtring minus the .xml
            }
            return maps;
        }
    }
}
