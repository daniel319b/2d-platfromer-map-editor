using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using System.Xml;
using Microsoft.Xna.Framework.Content;

namespace Paths
{
    public class Map
    {
        #region Description
        /// <summary>
        /// The name of the map.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The dimensions of the map(in tiles)
        /// </summary>
        public Point Dimensions { get; set; }
        /// <summary>
        /// The tile size in pixels.
        /// </summary>
        public Point TileSize { get; set; }
        /// <summary>
        /// The tile sheet assest name.
        /// </summary>
        public string TileSheetAssestName { get; set; }
        /// <summary>
        /// The collision tile texture assest name.
        /// </summary>
        public string CollisionTileTextureName { get; set; }
        /// <summary>
        /// A list of the background layers.(Assest name)
        /// </summary>
        public List<string> BackgroundLayerTextures { get; set; }
        #endregion

        #region Graphical Properties
        

        /// <summary>
        /// Defines the basic ground tiles array.
        /// </summary>
        public int[] BaseLayer { get; set; }
        /// <summary>
        /// Defines the objects array, used for decoration.
        /// </summary>
        public List<GameObject> ObjectLayer { get; set; }
        /// <summary>
        /// Defines the Collision array.
        /// </summary>
        public int[] CollisionLayer { get; set; }

        [ContentSerializerIgnore]
        public List<Texture2D> BackgroundLayer;

        /// <summary>
        /// The texture of the tile set.
        /// </summary>
        [ContentSerializerIgnore]
        public Texture2D TileSetTexture { get; set; }

        /// <summary>
        /// The collision tile texture.
        /// </summary>
        [ContentSerializerIgnore]
        public Texture2D CollisionTileTexture { get; set; }

        #endregion


        [ContentSerializerIgnore]
        public Texture2D rect { get; set; }

        [ContentSerializerIgnore]
        public bool DrawGrid { get; set; }

        [ContentSerializerIgnore]
        public Player Player { get; set; }

        public event EventHandler Created;
        
      
    
        [ContentSerializerIgnore]
        public Rectangle Bounds
        {
            get { return new Rectangle(0, 0, Dimensions.X * Tile.Width, Dimensions.Y * Tile.Height); }
        }

        public Vector2 SpawnPoint;

        public Map() 
        {
           
        }

        public Map(string tilesSet,int rows,int columns)
        {
            Dimensions = new Point(columns, rows);
            TileSheetAssestName = tilesSet;
            TileSetTexture = TextureManager.SetTexture (tilesSet);
             BaseLayer = new int[rows * columns];
             CollisionLayer = new int[rows * columns];
            //Reset the base layer.
            for (int i = 0; i < BaseLayer.Length; i++)
                BaseLayer[i] = -1;
  
            CollisionTileTexture = TextureManager.SetTexture("square");
            CollisionTileTextureName = "square";
            DrawBackgroundLayer = true;
            DrawBaseLayer = true;
            DrawObjectLayer = true;
        }

        public void SaveToFile(string name)
        {
            //System.IO.StreamWriter a = new System.IO.StreamWriter(Environment.CurrentDirectory+"/"+name+".txt");
            //for (int i = 0; i < height; i++)
            //{
            //    for (int j = 0; j < width; j++)
            //        a.Write(tiles[i, j]+" ");
            //    a.WriteLine();
            //}
            //a.Close();
        }

        public void LoadFromFile(string path)
        {
            //System.IO.StreamReader reader = new System.IO.StreamReader(Environment.CurrentDirectory + "/" + path + ".txt");
            //List<string[]> lines = new List<string[]>();
            //string line = reader.ReadLine();
            //int width = line.Split().Length-1;

            //while (line != null)
            //{
            //    lines.Add(line.Split ());
            //    line = reader.ReadLine();
            //}
            //reader.Close();

            //tiles = new int[lines.Count, width];
            //for (int i = 0; i < lines.Count; i++)
            //    for (int j = 0; j < width; j++)
            //    { 
            //        tiles[i, j] = int.Parse(lines[i][j].ToString());

                   
            //    }

            //for (int i = 0; i < lines.Count; i++)
            //    for (int j = 0; j < width; j++)
            //    {
            //        if (tiles[i,j] == -1)
            //        {
            //            Rectangle r = GetTileBounds(j, i-1);
            //            SpawnPoint = new Vector2(r.X, r.Y);
            //            break;
            //        }
            //    }
            //if (Created != null)
            //    Created(this, EventArgs.Empty);
        }

        #region Draw
        /// <summary>
        /// Draw's the map and all it's layers: Base,Object,Background,Collision.
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            if (!DrawBaseLayer && !DrawObjectLayer && !DrawBackgroundLayer && !DrawCollisionLayer)
                return;

            sb.Begin(SpriteSortMode.Deferred,BlendState.AlphaBlend,null,null,null,null,Camera.Transform);
            //sb.Begin();
            //Draw the background layer.
            if (DrawBackgroundLayer)
                foreach (Texture2D tex in BackgroundLayer)
                    sb.Draw(tex, Vector2.Zero, Color.White);

            for (int y = 0; y < Dimensions.Y; y++)
                for (int x = 0; x < Dimensions.X; x++)
                {
                    //Draw the base layer.
                    if (DrawBaseLayer)
                    {
                        Rectangle sourceRectangle = GetBaseLayerSourceRectangle(x, y);
                        if (sourceRectangle != Rectangle.Empty)
                            sb.Draw(TileSetTexture, Tile.Size * new Vector2(x, y), sourceRectangle, Color.White);
                    }
                    //Draw the collision layer.
                    if (DrawCollisionLayer && GetTileCollision(x, y) != TileCollision.Passable)
                    {
                        Color color = GetTileCollision(x, y) == TileCollision.Impassable ? Color.Red : Color.Blue;
                        sb.Draw(CollisionTileTexture, Tile.Size * new Vector2(x, y), color * 0.4f);
                    }
                   
                    if (DrawGrid)//Draw the grid.
                        sb.Draw(rect, new Rectangle((int)(Tile.Size * new Vector2(x, y)).X, (int)(Tile.Size * new Vector2(x, y)).Y, Tile.Width, Tile.Height), Color.White);
                }
            //Draw the object layer.
            if (DrawObjectLayer)
                foreach (GameObject o in ObjectLayer)
                    o.Draw(sb);
            sb.End();
           
        }//Draw
        #endregion

        #region Methods
        /// <summary>
        /// Gets the bounding rectangle of a tile in the map.
        /// </summary>
        /// <param name="x">The x coordinate of the tile.(in tiles)</param>
        /// <param name="y">The y coordinate of the tile.(in tiles)</param>
        public Rectangle GetTileBounds(int x, int y)
        {
            return new Rectangle(x * Tile.Width, y * Tile.Height, Tile.Width, Tile.Height);
        }

  
       
        /// <summary>
        /// Returns the id of a tile in a given map position.
        /// </summary>
        /// <param name="x">The x coordinate of the position.</param>
        /// <param name="y">The y coordinate of the position.</param>
        /// <param name="layer">The layer you want to get the value form.</param>
        /// <returns></returns>
        public int GetTileID(int x, int y, int[] layer)
        {
            if ((x >= Dimensions.X || x < 0) ||
                (y >= Dimensions.Y || y < 0))
            {
                throw new IndexOutOfRangeException("index was out of the map's bounds");
            }
            return layer[y * Dimensions.X + x];
        }

        /// <summary>
        /// Sets a tile in a given layer to a new tile value.
        /// </summary>
        /// <param name="x">The x coordinate of the position.</param>
        /// <param name="y">The y coordinate of the position.</param>
        /// <param name="tileValue">The new tile value</param>
        /// <param name="layer">The layer in which the tile will be set.</param>
        public void SetTile(int x, int y,int tileValue, int[] layer)
        {
            if (x < 0 || y < 0) return;
            if (x > Dimensions.X || y > Dimensions.Y) return;
            if (y * Dimensions.X + x >= layer.Length) return;
            layer[y * Dimensions.X + x] = tileValue;
        }
       

        /// <summary>
        /// Gets the source rectangle of a given tile from the base layer.
        /// </summary>
        /// <param name="x">The x coordinate of the tile.(in tiles)</param>
        /// <param name="y">The y coordinate of the tile.(in tiles)</param>
        public Rectangle GetBaseLayerSourceRectangle(int x, int y)
        {
            if ((x >= Dimensions.X || x < 0) ||
               (y >= Dimensions.Y || y < 0))
            {
                return Rectangle.Empty;//If out of bounds, return empty.
            }

            int tileValue = GetTileID(x, y, BaseLayer);//get the tile value
            int tilesPerRow = TileSetTexture.Width / Tile.Width;//how much tiles in a row.

            //if it's not a tile that we should draw,return empty.
            if (tileValue < 0) return Rectangle.Empty;
            
            int y2 = tileValue / tilesPerRow;//get the y coordinate.
            int x2 = tileValue % tilesPerRow;//get the x coordinate.
            return new Rectangle(x2 * Tile.Width, y2 * Tile.Height, Tile.Width, Tile.Height);
        }

        /// <summary>
        /// Checks if the given tile is blocked.
        /// </summary>
        /// <param name="x">The x coordinate of the tile.(in tiles)</param>
        /// <param name="y">The y coordinate of the tile.(in tiles)</param>
        public bool isBlocked(int x, int y)
        {
            if (x >= Dimensions.X || x < 0) return true;
            if (y >= Dimensions.Y || y < 0) return true;
            //if the collision tile is 0, no collision.
            return GetTileID(x,y,CollisionLayer) == 1;
        }

        public TileCollision GetTileCollision(int x, int y)
        {
            if (x >= Dimensions.X || x < 0) return TileCollision.Impassable;
            if (y >= Dimensions.Y || y < 0) return TileCollision.Impassable;
            return (TileCollision)GetTileID(x, y, CollisionLayer);
        }
        /// <summary>
        /// Serialize the map to an XML file called "(MapName).xml"
        /// </summary>
        public void Serialize()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(Name + ".xml", settings))
            {
                IntermediateSerializer.Serialize(writer, this, null);
                
            }
        }

        public static Map Deserialize(string fileName)
        {
            Map res = new Map();
          
            using (XmlReader r = XmlReader.Create(fileName))
            {
                res = IntermediateSerializer.Deserialize<Map>(r, null);
            }
            res.BackgroundLayer = new List<Texture2D>();
            foreach (string name in res.BackgroundLayerTextures)
                res.BackgroundLayer.Add(TextureManager.SetTexture(name));
            foreach (GameObject o in res.ObjectLayer)
                o.Texture = TextureManager.SetTexture(o.TextureName);
            res.TileSetTexture = TextureManager.SetTexture(res.TileSheetAssestName);
            res.CollisionTileTexture = TextureManager.SetTexture(res.CollisionTileTextureName);
            
            return res;


        }

        #endregion
        
        /// <summary>
        /// Draw the base layer?
        /// </summary>
        [ContentSerializerIgnore]
        public bool DrawBaseLayer { get; set; }
        /// <summary>
        /// Draw the object layer?
        /// </summary>
        [ContentSerializerIgnore]
        public bool DrawObjectLayer { get; set; }
        /// <summary>
        /// Draw the background layer?
        /// </summary>
        [ContentSerializerIgnore]
        public bool DrawBackgroundLayer { get; set; }
        /// <summary>
        /// Draw the collision layer?
        /// </summary>
        [ContentSerializerIgnore]
        public bool DrawCollisionLayer { get; set; }



    }
}
