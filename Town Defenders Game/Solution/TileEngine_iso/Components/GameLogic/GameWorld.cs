using System;
using System.Collections.Generic;
using Engine.Assets;
using Engine.Components.GameLogic.Movement;
using Engine.Components.GameLogic.Player;
using Engine.Components.GameLogic.Selection;
using Engine.Components.GameObjects.Clouds;
using Engine.Components.GameObjects.Structures.Tree;
using Engine.Components.GameObjects.Views;
using Engine.Iso_Tile_Engine;
using Engine.Utilities.Controls;
using Engine.Utilities.Controls.Panels.Dock.Manager;
using Engine.Utilities.Controls.Panels.StatusBar;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.Components.GameObjects;

namespace Engine.Components.GameLogic
{
   
    public class GameWorld
    {
        //TODO all GamePlayer (s)
        public GamePlayer MainPlayer;
        public TileMap Map;
        public CloudManager CloudManager;
        //TODO to be Tree manager..
        private readonly List<Tree> _trees;
        private readonly PictureBox _border;

        public ControlManager ControlManagerRef; 
        public ISelectionManager SelectionManager;
        public UnitMovementManager MovementManager;
        public DockManager DockManager;
        public UpperStatusBar UpperStatusBar;
      

        public GameWorld(ControlManager controlManagerRef)
        {
            ControlManagerRef = controlManagerRef;
            Map = new TileMap(200, 400);
            MainPlayer = new GamePlayer(this);
            MainPlayer.Name = "Osama Abulail";
            int cloudsCount = (Map.MapWidth*Map.MapHeight)/(20*20);
            CloudManager = new CloudManager(Map, cloudsCount, CloudDirection.West, 0.5f);
            MovementManager = new UnitMovementManager(this);
            SelectionManager = new UnitSelectionManager(this);
            DockManager = new DockManager(ControlManagerRef);
            UpperStatusBar = new UpperStatusBar(new Vector2(10, 0), MainPlayer, ControlManagerRef);

            _trees = new List<Tree>();

            // generate random trees.
            Random r = new Random();
            for (int i = 0; i < 4000; i++)
            {
                Point treeLoc = new Point(r.Next(0,Map.MapWidth), r.Next(0,Map.MapHeight));
                if (Map.MapCellAt(treeLoc).Walkable)
                {
                    Tree t = new Tree("tree" + r.Next(1, 26), this, treeLoc);
                    _trees.Add(t);
                }
            }


            // border
            _border = new PictureBox(GameGraphics.GetTexture("gameplay_border").SourceTexture,
               new Rectangle(0, 0, 1366, 768));
            ControlManagerRef.Add(_border);

        }

        public void Update(GameTime gameTime)
        {
            SelectionManager.Update(gameTime);
            MovementManager.Update(gameTime);
            MainPlayer.Update(gameTime);
            CloudManager.Update(gameTime);
            UpperStatusBar.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var t in _trees)
            {
                t.Draw(spriteBatch);

            }
          
            Map.Draw(spriteBatch);
            MainPlayer.Draw(spriteBatch);
            CloudManager.Draw(spriteBatch);
          
        }

    }
}
