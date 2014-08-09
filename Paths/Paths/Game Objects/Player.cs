using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Paths
{
    public class Player 
    {
        public Vector2 position, velocity, acceleration,prevPosition;
        public Texture2D texture;

        public Rectangle bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height); }
        }

        public Map map { get; set; }

        bool isJumping, isOnGround;

        public Player(Texture2D tex, Vector2 pos,Map map)
        {
            texture = tex;
            position = pos;
            this.map = map;
            map.Created += new EventHandler(map_Created);
        }

        void map_Created(object sender, EventArgs e)
        {
            position = map.SpawnPoint;
        }

        public void Update(GameTime gameTime)
        {
            map.Created += new EventHandler(map_Created);
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (!isOnGround)
                acceleration = new Vector2(0, 1400);
          //  else
           // {
          //      velocity = Vector2.Zero;
          //      acceleration = Vector2.Zero;
          //  }
            
            ApplyPhysics(time);
            HandleInput();
            HandleCollisions();
        }

        private void HandleInput()
        {
            if (Input.KeyDown(Keys.D))
                position.X += 2;
            if (Input.KeyDown(Keys.A))
                position.X -= 2;
            if (Input.KeyPressed(Keys.W) && isOnGround )
            {
                isOnGround = false;
                position.Y -= 5f;
                velocity.Y = -465f;
            }
        }

        private void HandleCollisions()
        {
            int leftTile = (int)Math.Floor((float)bounds.Left / Tile.Width);
            int rightTile = (int)Math.Ceiling(((float)bounds.Right / Tile.Width)) - 1;
            int topTile = (int)Math.Floor((float)bounds.Top / Tile.Height);
            int bottomTile = (int)Math.Ceiling(((float)bounds.Bottom / Tile.Height)) - 1;

            isOnGround = false;

            for (int y = topTile; y <= bottomTile; y++)
            {
                for (int x = leftTile; x <= rightTile; x++)
                {
                    TileCollision collision = map.GetTileCollision(x, y);
                    //if it's collidable.
                    //if (collision != TileCollision.Passable)
                    //{
                    //    Rectangle tileBounds = map.GetTileBounds(x, y);
                    //    Rectangle intersect = Rectangle.Intersect(tileBounds, bounds);
                    //    if (intersect != Rectangle.Empty)
                    //    {
                    //        int YDepth = intersect.Height;
                    //        int XDepth = intersect.Width;

                    //        if (YDepth < XDepth || collision == TileCollision.Platform)
                    //        {
                    //            if (bounds.Bottom <= tileBounds.Bottom)
                    //            {
                    //               // position.Y -= YDepth;
                    //                isOnGround = true;
                    //                acceleration = Vector2.Zero;
                    //                velocity.Y = 0;
                    //            }
                    //            if (collision == TileCollision.Impassable || isOnGround)
                    //                position.Y -= YDepth;
                    //        }
                    //        else if (collision == TileCollision.Impassable)
                    //        {
                    //            if (position.X < tileBounds.X)//right direction
                    //                position.X -= XDepth;
                    //            else if (position.X > tileBounds.X)  //left direction
                    //                position.X += XDepth;
                    //        }
                    //    }
                    //}

                    Rectangle tileBounds = map.GetTileBounds(x, y);
                    Rectangle intersect = Rectangle.Intersect(tileBounds, bounds);

                    if (collision != TileCollision.Passable && intersect != Rectangle.Empty)
                    {
                        int YDepth = intersect.Height;
                        int XDepth = intersect.Width;

                        if (collision == TileCollision.Impassable && YDepth > XDepth)//X Axis-Dont check for platforms.
                        {
                            //right direction
                            if (position.X < tileBounds.X)
                                position.X -= XDepth;
                            //left direction
                            else
                                position.X += XDepth;
                          
                        }
                        else if (YDepth < XDepth || collision == TileCollision.Impassable)//Y Axis
                        {
                            //we are standing on the tile.(above the tile)
                            //if (position.Y < tileBounds.Y-10 && velocity.Y >= 0)
                            if (bounds.Bottom +10< tileBounds.Bottom && velocity.Y >= 0)
                            {
                                position.Y -= YDepth - 1;
                                isOnGround = true;
                                acceleration = Vector2.Zero;
                                velocity.Y = 0;
                            }
                            //our head touches the bottom of the tile.(below the tile)
                            else if (position.Y > tileBounds.Y && collision == TileCollision.Impassable && velocity.Y < 0)
                            {
                                position.Y += YDepth;
                                velocity.Y = 0;
                            }
                        }


                        
                    }


                    //if (map.isCollideableTile(x, y))
                    //{
                    //    isOnGround = false;

                    //    Rectangle d = Rectangle.Intersect(bounds, tileBounds);
                    //    if (d != Rectangle.Empty)
                    //    {
                    //        if (d.Width > d.Height)
                    //        {
                    //            if (bounds.Y < tileBounds.Y)//at the top of a platform
                    //            {
                    //                position.Y -= d.Height - 1;
                    //                isOnGround = true;

                    //            }
                    //            else
                    //                position.Y += d.Height;

                    //            acceleration = Vector2.Zero;
                    //            velocity.Y = 0;
                    //        }
                    //        else
                    //        {
                    //            if (position.X < tileBounds.X)
                    //                position.X -= d.Width;
                    //            else
                    //                position.X += d.Width;
                    //            velocity.X = 0;
                    //        }

                    //    }


                    //}
                    
                }
            }
        }

        
        private void ApplyPhysics(float time)
        {
            velocity += acceleration * time;
            position += velocity * time;
        }

        
        public void Draw(SpriteBatch sb)
        {
            sb.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Camera.Transform);
            //sb.Begin();
            sb.Draw(texture, position, Color.White);
            sb.End();
        }

        public static void Spawn(Vector2 vector2)
        {
            
        }
    }
}
