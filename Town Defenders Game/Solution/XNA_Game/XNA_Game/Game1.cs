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
using Engine.Utilities;
using XNA_Game.GameScreens;
using Engine.Utilities.Debug;

namespace XNA_Game
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        public GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;

        public GameStateManager StateManager;
        public TitleScreen TitleScreen;
        public StartMenuScreen StartMenuScreen;
        public GameOptionsScreen GameOptionsScreen;
        public NewGameScreen NewGameScreen;
        public GamePlayScreen GamePlayScreen;
        /// <summary>
        /// //just to loop all of the states 
        /// </summary>
        private List<GameState> _allGameStates; 

        public int ScreenWidth = 1366;
        public int ScreenHeight = 768;
        public Rectangle ScreenRectangle;

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this)
                {
                    PreferredBackBufferWidth = ScreenWidth,
                    PreferredBackBufferHeight = ScreenHeight
                };
            ScreenRectangle = new Rectangle(
                0,
                0,
                ScreenWidth,
                ScreenHeight);

            Content.RootDirectory = "Content";

            _allGameStates = new List<GameState>();

            Components.Add(new InputManager(this, ScreenRectangle));
            StateManager = new GameStateManager(this);
            Components.Add(StateManager);

            TitleScreen = new TitleScreen(this, StateManager);
            StartMenuScreen = new StartMenuScreen(this, StateManager);
            GameOptionsScreen = new GameOptionsScreen(this, StateManager);
            NewGameScreen = new NewGameScreen(this, StateManager);
            GamePlayScreen = new GamePlayScreen(this, StateManager);
            
            //just to loop all of the states 
            _allGameStates.Add(TitleScreen);
            _allGameStates.Add(StartMenuScreen);
            _allGameStates.Add(GameOptionsScreen);
            _allGameStates.Add(NewGameScreen);
            _allGameStates.Add(GamePlayScreen);

            StateManager.ChangeState(TitleScreen);
           
    
            Window.Title = "Osama Abulail";
            Components.Add(new FrameRateCounter(this));
            
           
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

            base.Initialize();
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);
           
            // TODO: use this.Content to load your game content here

            Extentions.LoadAllGameContent(Content);

          
            SoundManager.Instance().SetSoundsVolume(100);
            SoundManager.Instance().SetMusicVolume(100);
            SoundManager.Instance().PlaySong("intro");
            

           
            //SoundManager.Instance().PlaySong(SoundManager.GameSongs.BGMusic1);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
          
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 

        #region Fields for testing
        //Rectangle Area = new Rectangle(0, 0, 200, 200);
        //private int counter = 0;
        #endregion

        protected override void Update(GameTime gameTime)
        {

            #region Test Code

            
           ////Console.Clear();
           // if(InputManager.MouseReleasedOutsideOfPressedArea(Area))
           //     Console.WriteLine("Oops " + counter++ );
            

            #endregion

         

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public void UpdateStatesLayout()
        {
            foreach (var gameState in _allGameStates)
            {
                gameState.UpdateLayout();
            }
        }
    }
}
