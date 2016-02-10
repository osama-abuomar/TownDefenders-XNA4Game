using System;
using Engine.Assets;
using Microsoft.Xna.Framework;
using Engine.Utilities;
using Engine.Utilities.Controls;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace XNA_Game.GameScreens
{
    public class StartMenuScreen : BaseGameState
    {
        PictureBox _backgroundImage;

        LinkLabel _newGame;
        LinkLabel _loadGame;
        LinkLabel _options;
        LinkLabel _editMap;
        LinkLabel _multiplayer;
        LinkLabel _back;
        LinkLabel _exit;





        public StartMenuScreen(Game game, GameStateManager manager)
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


            _newGame = new LinkLabel();
            _newGame.Text = "Start a New Game";
            _newGame.Size = _newGame.SpriteFont.MeasureString(_newGame.Text);
            _newGame.Selected += new EventHandler(menuItem_Selected);
            _newGame.Click += new EventHandler(menuItem_Selected);
            _newGame.Hover += new EventHandler(LinkLabel_Hover);
            ControlManager.Add(_newGame);

            _loadGame = new LinkLabel();
            _loadGame.Text = "Load Saved Games..";
            _loadGame.Size = _loadGame.SpriteFont.MeasureString(_loadGame.Text);
            _loadGame.Selected += new EventHandler(menuItem_Selected);
            _loadGame.Hover += new EventHandler(LinkLabel_Hover);
            _loadGame.Click += new EventHandler(menuItem_Selected);
            ControlManager.Add(_loadGame);

            _options = new LinkLabel();
            _options.Text = "General Options";
            _options.Size = _options.SpriteFont.MeasureString(_options.Text);
            _options.Selected += menuItem_Selected;
            _options.Hover += new EventHandler(LinkLabel_Hover);
            _options.Click += new EventHandler(menuItem_Selected);
            ControlManager.Add(_options);

            _editMap = new LinkLabel();
            _editMap.Text = "Edit Custom Senario..";
            _editMap.Size = _editMap.SpriteFont.MeasureString(_editMap.Text);
            _editMap.Selected += menuItem_Selected;
            _editMap.Hover += new EventHandler(LinkLabel_Hover);
            _editMap.Click += new EventHandler(menuItem_Selected);
            ControlManager.Add(_editMap);

            _multiplayer = new LinkLabel();
            _multiplayer.Text = "Multiplayer Game";
            _multiplayer.Size = _multiplayer.SpriteFont.MeasureString(_multiplayer.Text);
            _multiplayer.Selected += menuItem_Selected;
            _multiplayer.Hover += new EventHandler(LinkLabel_Hover);
            _multiplayer.Click += new EventHandler(menuItem_Selected);
            ControlManager.Add(_multiplayer);

            _back = new LinkLabel();
            _back.Text = "To Title Screen";
            _back.Size = _back.SpriteFont.MeasureString(_back.Text);
            _back.Selected += menuItem_Selected;
            _back.Hover += new EventHandler(LinkLabel_Hover);
            _back.Click += new EventHandler(menuItem_Selected);
            ControlManager.Add(_back);

            _exit = new LinkLabel();
            _exit.Text = "Exit to Windows";
            _exit.Size = _exit.SpriteFont.MeasureString(_exit.Text);
            _exit.Selected += menuItem_Selected;
            _exit.Hover += new EventHandler(LinkLabel_Hover);
            _exit.Click += new EventHandler(menuItem_Selected);
            ControlManager.Add(_exit);

            ControlManager.NextControl();
            ControlManager.FocusChanged += new EventHandler(ControlManager_FocusChanged);

            UpdateLayout();

            ControlManager_FocusChanged(_newGame, null);
        }


        public override void UpdateLayout()
        {
            _backgroundImage.DestinationRectangle = GameRef.ScreenRectangle;


            float YStartPos = (int)(3f / 10f * GameRef.ScreenRectangle.Height);
            float YPosIncrement = _newGame.SpriteFont.MeasureString(_newGame.Text).Y + 15;
            Vector2 position;
            LinkLabel ll;

            foreach (var control in ControlManager)
            {
                if (control is LinkLabel)
                {
                    ll = (control as LinkLabel);
                    position = new Vector2(
                        GameRef.ScreenRectangle.Width / 2 - ll.SpriteFont.MeasureString(ll.Text).X / 2,
                        YStartPos);
                    ll.SetPosition(position);
                    YStartPos += YPosIncrement;
                }
            }
        }


        void LinkLabel_Hover(object sender, EventArgs e)
        {
            ControlManager.LoseFocus();
            ((Control)sender).HasFocus = true;
        }


        void ControlManager_FocusChanged(object sender, EventArgs e)
        {

        }


        private void menuItem_Selected(object sender, EventArgs e)
        {

            if (sender == _newGame)
            {
                StateManager.PushState(GameRef.NewGameScreen);
            }
            if (sender == _loadGame)
            {

            }
            if (sender == _options)
            {
                StateManager.PushState(GameRef.GameOptionsScreen);
            }
            if (sender == _editMap)
            {

            }
            if (sender == _multiplayer)
            {

            }
            if (sender == _back)
            {
                StateManager.PopState();
            }
            if (sender == _exit)
            {
                SoundManager.Instance().StopMusic();
                GameRef.Exit();
            }
        }


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
