using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace Paths
{
    /// <summary>
    /// This is the base class for all the objects in the game.
    /// It handles basic graphics and physics things.
    /// </summary>
    public class GameObject
    {
        #region Fields and Properties

        
        protected  Vector2 position;
        protected Vector2 origin, velocity;
        protected Texture2D texture;
        protected Rectangle boundBox;
        protected float rotation = 0.0f, scale = 1.0f, opacity = 1.0f;
        protected int health;
        protected bool alive = true;
        protected string text;
        protected Color color = Color.White;
      
        /// <summary>
        /// The position of the object in the world.
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Origin property used for the center of the game object
        /// </summary>
        public Vector2 Origin
        {
            get { return origin;}        
        }

        /// <summary>
        /// Object's velocity to update the position.
        /// </summary>
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        /// <summary>
        /// The current texture of the object.
        /// </summary>
        [ContentSerializerIgnore]
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; TextureName = texture.Name; }
        }

        public string TextureName
        { get; set; }

        /// <summary>
        /// Used for drawing a text instead of a texture.
        /// </summary>
        [ContentSerializerIgnore]
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// Color defaulted to White.  The Color property uses the Alpha property
        /// to automatically use the opacity.
        /// </summary>
          [ContentSerializerIgnore]
        public Color Color
        {
            get
            {
                return color * opacity;
            }
            set { color = value; }
        }

        /// <summary>
        /// The opacity of the texture.1 is fully visible, 0 is fully transparent.
        /// </summary>
        public float Opacity
        {
            get { return opacity; }
            set { opacity = value; }
        }
        /// <summary>
        /// The scale of the GameObject 
        /// Bigger values to make the object bigger.
        /// Smaller values to make the object smaller.
        /// 1 for the default.
        /// </summary>
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        /// <summary>
        /// A rectangle representing the bounds of the object - for collision.
        /// </summary>
        [ContentSerializerIgnore]
        public Rectangle BoundBox
        {
            get { return boundBox; }
            protected set { boundBox = value; }
        }

        /// <summary>
        /// The rotation of the object - where the object is looking.
        /// </summary>
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        /// <summary>
        /// Returns if the object is alive or not.
        /// </summary>
          [ContentSerializerIgnore]
        public bool Alive
        {
            get { return alive; }
            set { alive = value; }
        }

           /// <summary>
        /// Transform matrix for correct rectangle collision and
        /// per-pixel collision
        /// </summary>
        [ContentSerializerIgnore] 
        public Matrix Transform
        {
            get;
            private set;
        }
        
        #endregion


        #region Constructors
        /// <summary>
        /// An empty Constructor for a Game Object.
        /// </summary>
        public GameObject() { }
        /// <summary>
        /// Constructs a GameObject with a specific position.
        /// </summary>
        /// <param name="pos">The position of the object.</param>
        public GameObject(Vector2 pos) { Position = pos; }
        /// <summary>
        /// Constructs a GameObject with specific position and texture.
        /// </summary>
        /// <param name="pos">The position of the object.</param>
        /// <param name="text">The texture of the object.</param>
        public GameObject(Vector2 pos, Texture2D text) { Position = pos; Texture = text; }

        #endregion


        #region Update and Draw Methods
        /// <summary>
        /// Updates the object's components and position.
        /// </summary>
        public virtual void Update()
        {
            if(texture != null)
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            CalculateMatrix();
            CalculateBoundingRectangle();
        }

        /// <summary>
        /// Calculates the transform matrix of the object with the origin,
        /// rotation, scale, and position.  This will need to be done every
        /// game loop because chances are the position changed.
        /// </summary>
        private void CalculateMatrix()
        {
            Transform = Matrix.CreateTranslation(new Vector3(-Origin, 0)) *
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateScale(1.0f) *
                Matrix.CreateTranslation(new Vector3(position, 0));
        }

        /// <summary>
        /// Calculates the bounding rectangle of the object using the object's transform
        /// matrix to make a correct rectangle.
        /// </summary>
        private void CalculateBoundingRectangle()
        {
            if (texture != null)
            {
                boundBox = new Rectangle(0, 0, texture.Width, texture.Height);
            }
            else
                boundBox = new Rectangle(0, 0, (int)Font.Regular.MeasureString(text).X, (int)Font.Regular.MeasureString(text).Y);

                Vector2 leftTop = Vector2.Transform(new Vector2(boundBox.Left, boundBox.Top), Transform);
                Vector2 rightTop = Vector2.Transform(new Vector2(boundBox.Right, boundBox.Top), Transform);
                Vector2 leftBottom = Vector2.Transform(new Vector2(boundBox.Left, boundBox.Bottom), Transform);
                Vector2 rightBottom = Vector2.Transform(new Vector2(boundBox.Right, boundBox.Bottom), Transform);

                Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                                  Vector2.Min(leftBottom, rightBottom));
                Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                                          Vector2.Max(leftBottom, rightBottom));

                boundBox = new Rectangle((int)min.X, (int)min.Y,
                    (int)(max.X - min.X), (int)(max.Y - min.Y));
            
        }

        /// <summary>
        /// Draws the object only if he's alive.
        /// </summary>
        /// <param name="sb">The spritebatch for drawing.</param>
        public virtual void Draw(SpriteBatch sb)
        {
            if (Alive)
            {               
             //   sb.Begin();
                if (texture != null)
                    sb.Draw(texture, position, null, Color, rotation, origin, scale, SpriteEffects.None, 0.0f);
                else
                    sb.DrawString(Font.Regular, text, position, Color);
               // sb.End();
            }
        }

        /// <summary>
        /// Draws the object only if he's alive.
        /// </summary>
        /// <param name="sb">The spritebatch for drawing.</param>
        /// <param name="transform">The transformation matrix which will apply.</param>
        public virtual void Draw(SpriteBatch sb,Matrix transform)
        {
            if (Alive)
            {
                sb.Begin(SpriteSortMode.Deferred, null, null, null, null, null, transform);
                if (texture != null)
                    sb.Draw(texture, position, null, Color, rotation, origin, scale, SpriteEffects.None, 0.0f);
             //   else
              //      sb.DrawString(Font.Regular, text, position, Color);
                sb.End();
            }
        }
        #endregion
    }
}
