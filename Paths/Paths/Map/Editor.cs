using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Paths
{
    class Editor
    {
        #region Field and Properties

        /// <summary>
        /// The texture that holds the tile set.
        /// </summary>
        public  Texture2D TileSet { get; set; }
        /// <summary>
        /// The position of the Editor.
        /// </summary>
        public Vector2 Position { get; set; }
        /// <summary>
        /// The bounds of the editor window.
        /// </summary>
        public Rectangle Bounds { get; set; }
        /// <summary>
        /// The current map that you are editing.
        /// </summary>
        public Map Map { get; set; }
        /// <summary>
        /// The current player that is the game.
        /// </summary>
        public Player Player { get; set; }

        Texture2D rectangleTexture,backgroundTexture;

        Vector2 tileSetPosition;
        Rectangle tileSetRectangle;

        /// <summary>
        /// The current selected ground tile that will be added to the map when clicked.
        /// </summary>
        int CurrentGroundTile;
        /// <summary>
        /// The current selected Collision Tile.
        /// </summary>
        TileCollision CurrentCollisionTile = TileCollision.Passable;

        //Gui Elements
        Button btnSave, btnLoad, btnMapProperties, btnAddObject,btnAddBackground;
        LoadDialogBox loadDialog;
        SaveDialogBox saveDialog;
        CheckBox chkBoxGrid,chkBoxCollisionlayer, chkBoxBackgroundLayer, chkBoxGroundLayer, chkBoxObjectLayer,chkBoxSpawnPoint;
        MapPropertiesWindow mapPropertiesWindow;
        ObjectPropertiesWindow objectPropertiesWindow;

        ContextMenu ctxMenu;

        List<GameObject> GUIElements = new List<GameObject>();
        /// <summary>
        /// Are you currently dragging an object?
        /// </summary>
        bool dragging;

        GameObject platformTile, blockTile,spawnPoint,objectToDrag;
        #endregion

        #region Constructor and Initialization

        public Editor(Vector2 pos,Texture2D Background,Map map)
        {
            Map = map;
            Position = pos;
            TileSet = map.TileSetTexture;
            backgroundTexture = Background;
            Bounds = new Rectangle((int)Position.X, (int)Position.Y, backgroundTexture.Width+500, backgroundTexture.Height+300);
            tileSetPosition = Position + new Vector2(20,40);
            tileSetRectangle = new Rectangle((int)tileSetPosition.X, (int)tileSetPosition.Y, TileSet.Width, TileSet.Height);

            #region GUI Elements Initialization

            //Check Boxes Initialization --------------------------------------------------------------------
            chkBoxGrid = new CheckBox(Position + new Vector2(880, 230));
            chkBoxCollisionlayer = new CheckBox(Position + new Vector2(200, 40));
            chkBoxBackgroundLayer = new CheckBox(Position + new Vector2(200, 80));
            chkBoxGroundLayer = new CheckBox(Position + new Vector2(430, 40));
            chkBoxObjectLayer = new CheckBox(Position + new Vector2(430, 80));
            chkBoxSpawnPoint = new CheckBox(Position + new Vector2(430, 120));
            AddGUIElements(chkBoxGrid, chkBoxCollisionlayer, chkBoxBackgroundLayer,chkBoxGroundLayer,chkBoxObjectLayer,chkBoxSpawnPoint);
            //-----------------------------------------------------------------------------------------------

            //Buttons Initialization ------------------------------------------------------------------------
            btnSave = new Button(Position + new Vector2(890, 10),TextureManager.SetTexture("UI/Button"), "Save Map");
            btnLoad = new Button(Position + new Vector2(890, 50),TextureManager.SetTexture("UI/Button"), "Load Map");
            btnMapProperties = new Button(Position + new Vector2(700, 10),TextureManager.SetTexture("UI/Button"), "Map Properties");
            btnAddObject = new Button(Position + new Vector2(700, 50), TextureManager.SetTexture("UI/Button"), "Add New Object");
            btnAddBackground = new Button(Position + new Vector2(700, 100),TextureManager.SetTexture("UI/Button"),"Add New Background");

            btnSave.Clicked += new EventHandler(btnSave_Clicked);
            btnLoad.Clicked += new EventHandler(btnLoad_Clicked);
            btnMapProperties.Clicked += new EventHandler(btnMapProperties_Show_Clicked);
            btnAddObject.Clicked += (sender, args) => ShowOpenFileDialog();
            btnAddBackground.Clicked += (sender, args) => ShowOpenFileDialog();
            AddGUIElements(btnSave, btnLoad, btnMapProperties,btnAddObject,btnAddBackground);
            //-------------------------------------------------------------------------------------------------

            platformTile = new GameObject(Position + new Vector2(210, 200), TextureManager.SetTexture("square")); platformTile.Color = Color.Blue * 0.5f;
            blockTile = new GameObject(Position + new Vector2(210, 155), TextureManager.SetTexture("square")); blockTile.Color = Color.Red * 0.5f;
            spawnPoint = new GameObject(map.SpawnPoint, TextureManager.SetTexture("SpawnPoint"));
            AddGUIElements(blockTile, platformTile); 
            
            #endregion

            rectangleTexture = TextureManager.SetTexture("rect");
            
            map.ObjectLayer.Add(new GameObject (new Vector2(100),TextureManager.SetTexture ("tree")));
            map.ObjectLayer.Add(new GameObject(new Vector2(300), TextureManager.SetTexture("bench")));
        }


        /// <summary>
        /// Adds gui elements to the list to Update and Draw
        /// </summary>
        /// <param name="elements">The GUI elements.</param>
        private void AddGUIElements(params GameObject[] elements)
        {
            GUIElements.AddRange(elements);
        }

        #endregion

        #region Event Handlers

        void btnMapProperties_Show_Clicked(object sender, EventArgs e)
        {
            //Create a new MapProperties Window.
            mapPropertiesWindow = new MapPropertiesWindow(new Vector2(100),Map);
            FormsManager.AddForm(mapPropertiesWindow);
        }

        void btnLoad_Clicked(object sender, EventArgs e)
        {
            //Create a dialog box with a list of all the map files in the directory.
            //make the user choose on and then click ok. and load the map. 
            //Create Map.Load(int[] map)
            loadDialog = new LoadDialogBox(new Vector2(150, 200));
            loadDialog.btnLoad.Clicked += new EventHandler(frmLoad_btnLoad_Clicked);
            FormsManager.AddForm(loadDialog);
        }

        void frmLoad_btnLoad_Clicked(object sender, EventArgs e)
        {
            if (loadDialog.selectedMap != null)
                Map = Map.Deserialize(loadDialog.selectedMap.Text+".xml");
            Player.position = Map.SpawnPoint;
            FormsManager.RemoveForm(loadDialog);
        }

        void btnSave_Clicked(object sender, EventArgs e)
        {
            Map.Serialize();
            saveDialog = new SaveDialogBox(new Vector2(150,200),Map);
            FormsManager.AddForm(saveDialog);
        }

        void ctxMenu_btnCopy_Clicked(object sender, EventArgs e)
        {
            GameObject a = new GameObject(Input.MousePosition, objectToDrag.Texture);
            Map.ObjectLayer.Add(a);
            ctxMenu = null;
        }

        void ctxMenu_btnDelete_Clicked(object sender, EventArgs e)
        {
            Map.ObjectLayer.Remove(objectToDrag);
            ctxMenu = null;
        }

        void ctxMenu_PropertiesClicked(object sender, EventArgs e)
        {
            if (objectToDrag == null) return;//if no object selected, dont show the window.
            objectPropertiesWindow = new ObjectPropertiesWindow(new Vector2(100), TextureManager.SetTexture("UI/LoadDialogBox"), objectToDrag);
            FormsManager.AddForm(objectPropertiesWindow);
            ctxMenu = null;
        }

        #endregion

        #region Update Methods

        public void Update()
        {
            if (FormsManager.GetForms().Length > 0)
            {
                FormsManager.Update();
                return;
            }

            UpdateMapObjects();
            HandleClicks();

            SetUpCheckBoxes();
            UpdateSpawnPoint();

            if (ctxMenu != null)
                ctxMenu.Update();
            if (ctxMenu != null && Input.LeftButtonPressed() && !Input.MouseRectangle.Intersects(ctxMenu.BoundBox))
                ctxMenu = null;
            

            //Update each GUI element.
            foreach (GameObject element in GUIElements)
                element.Update();
           
        }

        /// <summary>
        /// Shows the OpenFileDialogBox for the user to select a new texture.
        /// </summary>
        private void ShowOpenFileDialog()
        {
            System.Windows.Forms.OpenFileDialog openDialogBox = new System.Windows.Forms.OpenFileDialog();
            openDialogBox.Filter = "| *.png"; openDialogBox.Title = "Choose A Texture To Load";
            System.IO.Stream fileStream = null; 
            if (openDialogBox.ShowDialog() == System.Windows.Forms.DialogResult.OK) 
            {
                try
                {
                    if ((fileStream = openDialogBox.OpenFile()) != null)
                    {

                        using (fileStream)
                        {
                            if (btnAddObject.IsClicked())
                               Map.ObjectLayer.Add(new GameObject(new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2),
                                                                  Texture2D.FromStream(graphicsDevice, fileStream)) 
                                                                  {   //set the the texture name to the file name, remove the ".png"
                                                                      TextureName = openDialogBox.SafeFileName.Substring(0,openDialogBox.SafeFileName.Length -4) }//object initalizer.
                                                                  );
                            else if(btnAddBackground.IsClicked())
                                Map.BackgroundLayer.Add(Texture2D.FromStream(graphicsDevice,fileStream));   
                        }
                        
                    }

                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.ToString());
                }
            }
            openDialogBox.Dispose();
        }

        /// <summary>
        /// Handles mouse clicks in the game window.
        /// </summary>
        private void HandleClicks()
        {

            if (Input.LeftButtonPressed())//Handle left click on the editor.
            {
                if (Input.MouseRectangle.Intersects(tileSetRectangle))//clicked on the tile set- select the tile.
                {
                    CurrentGroundTile = getClickedTileCode();//get the clicked tile.
                    CurrentCollisionTile = 0;//deselect the Collision tile.
                }
                if (Input.MouseRectangle.Intersects(platformTile.BoundBox))//clicked on the platform collision tile - select it.
                {
                    CurrentCollisionTile = TileCollision.Platform;//Set the collision tile to Platform.
                    chkBoxCollisionlayer.Checked = true;//Show the collision layer.
                }
                if (Input.MouseRectangle.Intersects(blockTile.BoundBox))
                {
                    CurrentCollisionTile = TileCollision.Impassable;//Set the collision tile to Impassable block.
                    chkBoxCollisionlayer.Checked = true;//Show the collision layer.
                }
            }
            if (Input.MouseRectangle.Intersects(Map.Bounds))
                HandleMapClick();
        }

        /// <summary>
        /// Updates the spawn point.
        /// </summary>
        private void UpdateSpawnPoint()
        {
            //Update the Spawn Point object.
            spawnPoint.Update();
            if (Input.MouseRectangle.Intersects(spawnPoint.BoundBox) && Input.LeftButtonDown() && !dragging && chkBoxSpawnPoint.Checked)
            {
                objectToDrag = spawnPoint;
                dragging = true;
            }
            else if (Input.LeftButtonReleased())
                dragging = false;
            Map.SpawnPoint = spawnPoint.Position;
        }

        /// <summary>
        /// Sets up the check boxes.
        /// </summary>
        private void SetUpCheckBoxes()
        {
            //Set up the check boxes and the map drawing conditions -----------------------------------------------------
            Map.DrawCollisionLayer = chkBoxCollisionlayer.Checked;
            Map.DrawGrid = chkBoxGrid.Checked;
            Map.DrawBackgroundLayer = chkBoxBackgroundLayer.Checked;
            Map.DrawBaseLayer = chkBoxGroundLayer.Checked;
            Map.DrawObjectLayer = chkBoxObjectLayer.Checked;
            //------------------------------------------------------------------------------------------------------
        }

        /// <summary>
        /// Handles a map mouse click.
        /// </summary>
        private void HandleMapClick()
        {
            //Input.MousePosition += Camera.Position;
            int x = (int)Camera.ToWordCoords(Input.MousePosition).X / Tile.Width;//Get the x coordinate of the clicked tile position.
            int y = (int)Camera.ToWordCoords(Input.MousePosition).Y / Tile.Height;//Get the y coordinate of the clicked tile position.
            if (Input.LeftButtonDown() && !dragging)
            {
                if(CurrentCollisionTile == 0)//set ground tile only if no collision tile selected.
                   Map.SetTile(x, y, CurrentGroundTile, Map.BaseLayer);
                else//only set collision tile if not 0
                   Map.SetTile(x, y, (int)CurrentCollisionTile, Map.CollisionLayer);
            }
            if (Input.RightButtonDown() && !dragging)//Delete tile
            {
                Map.SetTile(x, y, -1, Map.BaseLayer);//Delete the tile at the position.
                Map.SetTile(x, y, (int)TileCollision.Passable, Map.CollisionLayer);//Set this collision tile to Passable.
            }
        }

        /// <summary>
        /// Gets the clicked tile code that the user clicked on.
        /// </summary>
        /// <returns></returns>
        private int getClickedTileCode()
        {
            int coordx = (int)(Input.MousePosition.X - tileSetPosition.X) / Tile.Width;//Get the relative xCoord to the tileset.
            int coordy = (int)(Input.MousePosition.Y - tileSetPosition.Y) / Tile.Height;//Get the relative yCoord to the tileset.
            return coordx + coordy * (TileSet.Width / Tile.Width);
        }

        /// <summary>
        /// Updates the map object layer.
        /// And handles the mouse dragging of the objects.
        /// </summary>
        private void UpdateMapObjects()
        {
            if (chkBoxObjectLayer.Checked)
            {
                foreach (GameObject i in Map.ObjectLayer.Reverse<GameObject>())
                {
                    i.Update();
                    if (Input.LeftButtonDown() && Input.MouseRectangle.Intersects(i.BoundBox) && !dragging)
                    {
                        dragging = true;
                        objectToDrag = i;
                    }
                    else if (Input.LeftButtonReleased())
                        dragging = false;
                    if (Input.RightButtonPressed() && Input.MouseRectangle.Intersects(i.BoundBox))
                    {
                        ctxMenu = new ContextMenu(Input.MousePosition, TextureManager.SetTexture("UI/DialogBox"));
                        ctxMenu.btnProperties.Clicked += new EventHandler(ctxMenu_PropertiesClicked);
                        ctxMenu.btnDelete.Clicked += new EventHandler(ctxMenu_btnDelete_Clicked);
                        ctxMenu.btnCopy.Clicked += new EventHandler(ctxMenu_btnCopy_Clicked);
                    }
                }
            }

            if (dragging)
                objectToDrag.Position += Input.MousePosition - Input.PrevMousePosition;
        }

        #endregion

        #region Draw Methods

        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(backgroundTexture, Position, Color.White);//Draw the editor's background.
            sb.Draw(TileSet, tileSetPosition, Color.White);//Draw the tile set.
            drawTileSetGrid(sb);//Draw the rectangle seperation of the tiles in the tile set.

            //Draw each GUI element.
            foreach (GameObject element in GUIElements)
                element.Draw(sb);
            if (chkBoxSpawnPoint.Checked)
                spawnPoint.Draw(sb);
      
            DrawGUIText();
            HighlightCurrentTile(sb);
            FormsManager.Draw(sb);

            if (ctxMenu != null)
                ctxMenu.Draw(sb);
            ScreenWriter.Write("Mouse World Pos:"+Camera.ToWordCoords(Input.MousePosition), new Vector2(100), Color.Black);
            sb.End();
           
        }

       private void HighlightCurrentTile(SpriteBatch sb)
       {
           //if the ground tiles selected.
           if (CurrentCollisionTile == TileCollision.Passable)
               sb.Draw(blockTile.Texture, new Vector2(tileSetPosition.X + CurrentGroundTile * Tile.Width, tileSetPosition.Y), Color.White * 0.25f);
           
       }

       

        private void DrawGUIText()
        {
            ScreenWriter.Write("Tile Set:", Position + new Vector2(20, 10), Color.Black);
            ScreenWriter.Write("Draw Grid", chkBoxGrid.Position + new Vector2(30, -10), Color.Black);
            ScreenWriter.Write("Collision Layer", chkBoxCollisionlayer.Position + new Vector2(30, -10), Color.Black);
            ScreenWriter.Write("Background Layer", chkBoxBackgroundLayer.Position + new Vector2(30, -10), Color.Black);
            ScreenWriter.Write("Base Layer", chkBoxGroundLayer.Position + new Vector2(30, -10), Color.Black);
            ScreenWriter.Write("Object Layer", chkBoxObjectLayer.Position + new Vector2(30, -10), Color.Black);
            ScreenWriter.Write("Spawn Point", chkBoxSpawnPoint.Position + new Vector2(30, -10), Color.Black);
            ScreenWriter.Write("Collision Tiles", Position + new Vector2(185, 110), Color.Black);
            ScreenWriter.Write("- Solid Block", Position + new Vector2(240, 143), Color.Black);
            ScreenWriter.Write("- Platform", Position + new Vector2(240, 188), Color.Black);

        }


        private void drawTileSetGrid(SpriteBatch sb)
        {
            for (int i = 0; i < TileSet.Height / Tile.Height; i++)
                for (int j = 0; j < TileSet.Width / Tile.Width; j++)
                {
                    Vector2 pos = new Vector2(Tile.Width * j, Tile.Height * i);
                    Rectangle Destination = new Rectangle((int)(tileSetPosition.X + pos.X), (int)(tileSetPosition.Y + pos.Y), Tile.Width, Tile.Height);
                    sb.Draw(rectangleTexture, Destination, Color.White);
                }
        }
        #endregion




        public GraphicsDevice graphicsDevice { get; set; }
    }
}
