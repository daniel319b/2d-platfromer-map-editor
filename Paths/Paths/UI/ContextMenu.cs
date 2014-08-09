using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Paths
{
    class ContextMenu : GameObject
    {
        List<Button> entries;
        Color selectedColor = Color.Blue;

        public Button btnDelete, btnCopy, btnProperties;

        public ContextMenu(Vector2 position, Texture2D texture)
            : base(position, texture)
        {
            entries = new List<Button>();

            btnDelete = new Button(Position + new Vector2(10, 10),"Delete");
            btnCopy = new Button(Position + new Vector2(10, 50),"Copy");
            btnProperties = new Button(Position + new Vector2(10, 90),"Properties");

            entries.Add(btnDelete);
            entries.Add(btnCopy);
            entries.Add(btnProperties);
        }

        public override void Update()
        {
            boundBox = new Rectangle((int)position.X, (int)position.X, texture.Width, texture.Height);
            foreach (Button entry in entries)
            {
                if (Input.MouseRectangle.Intersects(entry.BoundBox))
                    entry.TextColor = selectedColor;    
                else
                    entry.TextColor = Color.Black;
                entry.Update();

            }
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            foreach (Button entry in entries)
                entry.Draw(sb);
        }

    }
}
