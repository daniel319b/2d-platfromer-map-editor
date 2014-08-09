using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Paths
{
    class Form : GameObject
    {
        protected List<GameObject> Controls;

        /// <summary>
        /// The button which closes the form.
        /// </summary>
        public Button btnClose;

        Vector2 prevPosition;

        public Form(Vector2 position, Texture2D texture)
            : base(position, texture)
        {
            btnClose = new Button(Position + new Vector2(Texture.Width - 50, 10), TextureManager.SetTexture("ExitButton"));
            btnClose.Clicked += new EventHandler(btnClose_Clicked);
            Controls = new List<GameObject>();
            Controls.Add(btnClose);
        }

        protected virtual void btnClose_Clicked(object sender, EventArgs e)
        {
            FormsManager.RemoveForm(this);
        }

        public override void Update()
        {
            prevPosition = position;
            boundBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
           
            HandleDragging();
            foreach (GameObject c in Controls)
                c.Update();
            if (Input.KeyPressed(Keys.Escape) || Input.KeyPressed(Keys.Enter))//If the user clicked ESC or ENTER, Close the form.
                btnClose.onLeftClick();
        }


        private void HandleDragging()
        {
            if (Input.MouseRectangle.Intersects(boundBox) && Input.LeftButtonDown())
                Position += Input.MousePosition - Input.PrevMousePosition;
            //Update each of the controls' positions.
            foreach (GameObject c in Controls)
                c.Position += position - prevPosition;
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            foreach (GameObject c in Controls)
                c.Draw(sb);
        }

        protected void AddControls(params GameObject[] controls)
        {
            Controls.AddRange(controls);
        }
    }
}
