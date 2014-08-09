using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Paths
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player player;

        Map map;
        Editor editor;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 960;
            graphics.PreferredBackBufferWidth = 1020;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = true;
            base.Initialize();
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ScreenWriter.SpriteBatch = spriteBatch;
            TextureManager.Content = Content;
            Font.Regular = Content.Load<SpriteFont>("font");
            

            map = new Map("Map/Tiles", 22, 42);
            map.rect = Content.Load<Texture2D>("rect");
            player = new Player(Content.Load<Texture2D>("Map/stick"), new Vector2(20),map);
            
           
          
            
            map.Name = "Map2";
            map.SpawnPoint = new Vector2(100);
            //map.Dimensions = new Point(21, 22);
            map.BackgroundLayerTextures = new List<string>();
            map.BackgroundLayerTextures.Add("Map/bg_2_01");
           
            map.TileSize = new Point(32, 32);
            map.ObjectLayer = new List<GameObject>();

            map.BackgroundLayer = new List<Texture2D>();
            foreach (string name in map.BackgroundLayerTextures)
                map.BackgroundLayer.Add(Content.Load<Texture2D>(name));
            
            Camera.ScreenBounds = GraphicsDevice.Viewport.Bounds;
            Camera.Position = Vector2.Zero;
            

            editor = new Editor(new Vector2(0, 705), Content.Load<Texture2D>("UI/EditorBack"), map);
            editor.graphicsDevice = GraphicsDevice;
            editor.Player = player;
        }



        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            map = editor.Map;
            player.map = map;
            player.Update(gameTime);
            editor.Update();
            Input.Update();

            if (Input.KeyDown(Keys.Right))
                Camera.Position += new Vector2(2, 0);
            else if (Input.KeyDown(Keys.Left))
                Camera.Position += new Vector2(-2, 0);
            else if (Input.KeyDown(Keys.Up))
                Camera.Position += new Vector2(0, 2);
            else if (Input.KeyDown(Keys.Down))
                Camera.Position += new Vector2(0, -2);

            if (Input.KeyDown(Keys.OemPlus))
                Camera.Scale += 0.01f;
            if (Input.KeyDown(Keys.OemMinus))
                Camera.Scale -= 0.01f;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
          
            map.Draw(spriteBatch);
            player.Draw(spriteBatch);
            
            editor.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
