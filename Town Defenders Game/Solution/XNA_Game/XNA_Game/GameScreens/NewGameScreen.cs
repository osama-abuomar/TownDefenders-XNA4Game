using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Engine.Assets;
using Engine.Utilities;
using Engine.Utilities.Controls;
using Microsoft.Xna.Framework;

namespace XNA_Game.GameScreens
{
    public class NewGameScreen : BaseGameState
    {

        private PictureBox _backgroundImage;
        private Label _pageTitle;

        private SwitchBox _resources;
        private SwitchBox _population;
        private SwitchBox _mapSize;
        private SwitchBox _mapStyle;
        private SwitchBox _allowCheats;
        private LinkLabel _startGame;
        private LinkLabel _back;


        public NewGameScreen(Game game, GameStateManager manager)
            : base(game, manager)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            base.LoadContent();

            _backgroundImage = new PictureBox(GameGraphics.GetTexture("bg").SourceTexture, GameRef.ScreenRectangle);
            ControlManager.Add(_backgroundImage);

            _pageTitle = new Label();
            _pageTitle.Text = "New Game Settings..";
            ControlManager.Add(_pageTitle);

            _resources = new SwitchBox("Player Resources", new List<string>() { "High", "Normal", "Low" });
            _resources.ItemChanged +=new EventHandler(_resources_ItemChanged);
            _resources.Hover+=new EventHandler(Hover);
            ControlManager.Add(_resources);

            _population = new SwitchBox("Max Population", new List<string>() { "50", "100", "150", "200", "500" });
            _population.ItemChanged +=new EventHandler(_population_ItemChanged);
            _population.Hover += new EventHandler(Hover);
            ControlManager.Add(_population);

            _mapSize = new SwitchBox("Map Size", new List<string>() { "Small", "Normal" ,"Large" });
            _mapSize.ItemChanged +=new EventHandler(_mapSize_ItemChanged);
            _mapSize.Hover += new EventHandler(Hover);
            ControlManager.Add(_mapSize);

             _mapStyle = new SwitchBox("Map Terrain Style", new List<string>() { "Spring", "Desert" ,"Forest" });
            _mapStyle.ItemChanged +=new EventHandler(_mapStyle_ItemChanged);
            _mapStyle.Hover += new EventHandler(Hover);
            ControlManager.Add(_mapStyle);

             _allowCheats = new SwitchBox("Allow Cheats", new List<string>() { "Yes", "No"  });
            _allowCheats.ItemChanged +=new EventHandler(_allowCheats_ItemChanged);
            _allowCheats.Hover += new EventHandler(Hover);
            ControlManager.Add(_allowCheats);

            _startGame = new LinkLabel();
            _startGame.Text = "Start Game";
            _startGame.Size = _startGame.SpriteFont.MeasureString(_startGame.Text);
            _startGame.Selected += new EventHandler(_startGame_Selected);
            _startGame.Click += new EventHandler(_startGame_Selected);
            _startGame.Hover += new EventHandler(Hover);
            ControlManager.Add(_startGame);

            _back = new LinkLabel();
            _back.Text = "Back to Main Menu";
            _back.Size = _back.SpriteFont.MeasureString(_back.Text);
            _back.Selected += new EventHandler(_back_Selected);
            _back.Click += new EventHandler(_back_Selected);
            _back.Hover += new EventHandler(Hover);
            ControlManager.Add(_back);



            ControlManager.NextControl();
            ControlManager.FocusChanged += new EventHandler(ControlManager_FocusChanged);


            UpdateLayout();

            ControlManager_FocusChanged(_resources, null);
        }


        public override void UpdateLayout()
        {
            _backgroundImage.DestinationRectangle = GameRef.ScreenRectangle;


            float YStartPos = (int)(7f / 20f * GameRef.ScreenRectangle.Height);
            float XPos = (int)(GameRef.ScreenRectangle.Width == 1080 ? (2f / 5f) * GameRef.ScreenRectangle.Width : (1f / 3f) * GameRef.ScreenRectangle.Width);
            float YPosIncrement = _back.SpriteFont.MeasureString(_back.Text).Y + 15;
            Vector2 position;
            SwitchBox sb;


            foreach (var control in ControlManager)
            {
                if (control is SwitchBox)
                {
                    sb = (control as SwitchBox);
                    position = new Vector2(
                        XPos,
                        YStartPos);
                    sb.SetPosition(position);
                    YStartPos += YPosIncrement;
                }
            }


            const int xOffsetFromSwitchBoxes = 30;

            Vector2 _startGamePos = new Vector2(XPos - xOffsetFromSwitchBoxes, YStartPos + YPosIncrement);
            _startGame.SetPosition(_startGamePos);

            YStartPos += YPosIncrement;

            Vector2 backPos = new Vector2(XPos - xOffsetFromSwitchBoxes, YStartPos + YPosIncrement);
            _back.SetPosition(backPos);

            Vector2 titlePos = new Vector2(XPos - xOffsetFromSwitchBoxes, (int)(7f / 20f * GameRef.ScreenRectangle.Height) - 2*YPosIncrement);
            _pageTitle.SetPosition(titlePos);
           



        }


        void _startGame_Selected(object sender, EventArgs e)
        {
            StateManager.PushState(GameRef.GamePlayScreen);
        }

        void _allowCheats_ItemChanged(object sender, EventArgs e)
        {
            
        }

        void _mapStyle_ItemChanged(object sender, EventArgs e)
        {
            
        }

        void _mapSize_ItemChanged(object sender, EventArgs e)
        {
           
        }

        void _population_ItemChanged(object sender, EventArgs e)
        {
            
        }

        void _resources_ItemChanged(object sender, EventArgs e)
        {
           
        }

        private void Hover(object sender, EventArgs e)
        {
            ControlManager.LoseFocus();
            ((Control)sender).HasFocus = true;
        }

        #region Event Handlers
        void _back_Selected(object sender, EventArgs e)
        {
            StateManager.PopState();
        }


        void ControlManager_FocusChanged(object sender, EventArgs e)
        {
            //TODO play a sound..
        }

        #endregion


        public override void Update(GameTime gameTime)
        {
            ControlManager.Update(gameTime);
            base.Update(gameTime);
        }



        public override void Draw(GameTime gameTime)
        {
            GameRef.SpriteBatch.Begin();
            base.Draw(gameTime);
            ControlManager.Draw(GameRef.SpriteBatch);
            GameRef.SpriteBatch.End();
        }
    }
}



