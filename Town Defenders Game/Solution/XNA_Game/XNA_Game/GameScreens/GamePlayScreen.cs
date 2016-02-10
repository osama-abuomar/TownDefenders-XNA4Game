
using System;
using Engine.Utilities.Controls;
using Engine.Utilities.Controls.ImageButtons;
using Microsoft.Xna.Framework;
using Engine.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Engine.Iso_Tile_Engine;
using Microsoft.Xna.Framework.Content;
using Engine.Components.GameLogic;
using Microsoft.Xna.Framework.Input;


namespace XNA_Game.GameScreens
{
    public class GamePlayScreen : BaseGameState
    {
        GameWorld _gameWorld;
       

        public GamePlayScreen(Game game, GameStateManager manager)
            : base(game, manager)
        {
           
            

        }

        public override void Initialize()
        {
            GameRef.IsMouseVisible = true;
            base.Initialize();
        }

       
        protected override void LoadContent()
        {
            ContentManager content = GameRef.Content;
            base.LoadContent();
            
           
       

            // world
            _gameWorld = new GameWorld(ControlManager);


            Camera.ViewWidth = GameRef.Graphics.PreferredBackBufferWidth;
            Camera.ViewHeight = GameRef.Graphics.PreferredBackBufferHeight;
            Camera.WorldWidth = ((_gameWorld.Map.MapWidth - 2) * TileInfo.TileStepX);
            Camera.WorldHeight = ((_gameWorld.Map.MapHeight - 2) * TileInfo.TileStepY);
            // to hide the shark teeth at the edges.
            Camera.DisplayOffset = new Vector2(TileEngineInfo.BaseOffsetX, TileEngineInfo.BaseOffsetY);
            //Camera.CameraLocation = new Vector2(Camera.WorldWidth / 2, Camera.WorldHeight / 2);
            Camera.CameraLocation = Vector2.Zero;

            SoundManager.Instance().StopMusic();
            SoundManager.Instance().SetMusicVolume(20);
            SoundManager.Instance().PlaySong("gameplay");

        }

        // test only
        void btn96_Click(object sender, System.EventArgs e)
        {
            
        }



        public override void UpdateLayout()
        {

        }

        public void UpdateCamera()
        {
            float cameraSpeed = Camera.Speed;
            Vector2 v2Movement = Vector2.Zero;

            //read arrow keys..
            if (InputManager.KeyDown(Keys.A))
            {
                v2Movement += new Vector2(-1 , 0);
            }
            if (InputManager.KeyDown(Keys.D))
            {
                v2Movement += new Vector2(+1 , 0);
            }
            if (InputManager.KeyDown(Keys.W))
            {
                v2Movement += new Vector2(0, -1 );
            }
            if (InputManager.KeyDown(Keys.S))
            {
                v2Movement += new Vector2(0, +1 );
            }
            if (v2Movement != Vector2.Zero)
                v2Movement.Normalize();

            v2Movement *= cameraSpeed;
            Camera.Move(v2Movement);
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.KeyPressed(Keys.P))
                StateManager.PushState(new Dialog(GameRef, StateManager));


            _gameWorld.Update(gameTime);

            ControlManager.Update(gameTime);
            base.Update(gameTime);

            UpdateCamera();
        }

        public override void Draw(GameTime gameTime)
        {
            //just to eliminate the lines that appear due to antialiasing..
            GraphicsDevice.Clear(new Color(100,119,40));

            GameRef.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            _gameWorld.Draw(GameRef.SpriteBatch);
            #region Hilight
            //Vector2 hilightLoc = Camera.ScreenToWorld(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
            //Point hilightPoint = myMap.WorldToMapCell(new Point((int)hilightLoc.X, (int)hilightLoc.Y));

            //int hilightrowOffset = 0;
            //if ((hilightPoint.Y) % 2 == 1)
            //    hilightrowOffset = TileInfo.OddRowXOffset;

            //GameRef.SpriteBatch.Draw(
            //                GameGraphics.GetTexture("mouse_hilight").SourceTexture,
            //                Camera.WorldToScreen(
            //                new Vector2(
            //                    (hilightPoint.X * TileInfo.TileStepX) + hilightrowOffset,
            //                    (hilightPoint.Y + 2) * TileInfo.TileStepY)),
            //                new Rectangle(0, 0, 64, 32),
            //                Color.White * 0.3f,
            //                0.0f,
            //                Vector2.Zero,
            //                1.0f,
            //                SpriteEffects.None,
            //                0.0f);
            #endregion
            

            
            ControlManager.Draw(GameRef.SpriteBatch);
            GameRef.SpriteBatch.End();
            base.Draw(gameTime);

        }



    }
}
