using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Engine.Assets;
using Engine.Iso_Tile_Engine;
using Engine.Utilities;
using Engine.Utilities.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNA_Game.GameScreens
{
    public class GameOptionsScreen : BaseGameState
    {

        private PictureBox _backgroundImage;
        private Label _pageTitle;

        private SwitchBox _resolution;
        private SwitchBox _viewMode;
        private SwitchBox _volume;
        private SwitchBox _music;
        private SwitchBox _soundEffects;
        private LinkLabel _back;


        public GameOptionsScreen(Game game, GameStateManager manager)
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
            _pageTitle.Text = "General Game Options..";
            ControlManager.Add(_pageTitle);

            _resolution = new SwitchBox("Resolution", new List<string> { "720p", "1080p" });
            _resolution.ItemChanged += new EventHandler(_resolution_ItemChanged);
            _resolution.Hover += new EventHandler(Hover);
            ControlManager.Add(_resolution);

            _viewMode = new SwitchBox("View Mode", new List<string> { "Window", "Full Screen" });
            _viewMode.ItemChanged += new EventHandler(_viewMode_ItemChanged);
            _viewMode.Hover +=new EventHandler(Hover);
            ControlManager.Add(_viewMode);

            var volumes = new List<string>();
            for (int i = 0; i < 105; i += 5)
                volumes.Add(i.ToString());
            volumes.Reverse();
            _volume = new SwitchBox("Audio Volume", volumes);
            _volume.ItemChanged += new EventHandler(_volume_ItemChanged);
            _volume.Hover += new EventHandler(Hover);
            ControlManager.Add(_volume);

            _music = new SwitchBox("Gameplay Music", new List<string> { "On", "Off" });
            _music.ItemChanged += new EventHandler(_music_ItemChanged);
            _music.Hover += new EventHandler(Hover);
            ControlManager.Add(_music);

            _soundEffects = new SwitchBox("Sound Effects", new List<string> { "On", "Off" });
            _soundEffects.ItemChanged += new EventHandler(_soundEffects_ItemChanged);
            _soundEffects.Hover += new EventHandler(Hover);
            ControlManager.Add(_soundEffects);

            _back = new LinkLabel();
            _back.Text = "Back to Main Menu";
            _back.Size = _back.SpriteFont.MeasureString(_back.Text);
            _back.Selected += new EventHandler(_back_Selected);
            _back.Click += new EventHandler(_back_Selected);
            _back.Hover += new EventHandler(Hover);
            ControlManager.Add(_back);

            UpdateLayout();

            ControlManager.NextControl();
            ControlManager.FocusChanged += new EventHandler(ControlManager_FocusChanged);
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

            Vector2 backPos = new Vector2(XPos - xOffsetFromSwitchBoxes, YStartPos + YPosIncrement);
            _back.SetPosition(backPos);

            Vector2 titlePos = new Vector2(XPos - xOffsetFromSwitchBoxes, (int)(7f / 20f * GameRef.ScreenRectangle.Height) - 2*YPosIncrement);
            _pageTitle.SetPosition(titlePos);
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

        void _soundEffects_ItemChanged(object sender, EventArgs e)
        {
            string option = _soundEffects.CurrentItem;
            switch (option)
            {
                case "On":
                    SoundManager.Instance().EnableSounds();
                    break;
                case "Off":
                    SoundManager.Instance().DisableSounds();
                    break;
            }
        }

        void _music_ItemChanged(object sender, EventArgs e)
        {
            string option = _music.CurrentItem;
            switch (option)
            {
                case "On":
                    SoundManager.Instance().EnableMusic();
                    break;
                case "Off":
                    SoundManager.Instance().DisableMusic();
                    break;
            }
        }

        void _volume_ItemChanged(object sender, EventArgs e)
        {
            int chosenVolume = int.Parse(_volume.CurrentItem);
            SoundManager.Instance().SetMusicVolume(chosenVolume);

        }

        void _viewMode_ItemChanged(object sender, EventArgs e)
        {
            SwitchBox switchBox = sender as SwitchBox;
            switch (switchBox.CurrentItem)
            {
                case "Full Screen":
                    if (!GameRef.Graphics.IsFullScreen)
                        GameRef.Graphics.ToggleFullScreen();
                    break;
                case "Window":
                    if (GameRef.Graphics.IsFullScreen)
                        GameRef.Graphics.ToggleFullScreen();
                    break;
            }
        }

        void _resolution_ItemChanged(object sender, EventArgs e)
        {
            SwitchBox switchBox = sender as SwitchBox;
            switch (switchBox.CurrentItem)
            {
                case "1080p":
                    if (GameRef.ScreenWidth == 1366)
                    {
                        TileEngineInfo.SquaresAcross = 32;
                        TileEngineInfo.SquaresDown = 70;
                        GameRef.ScreenWidth = 1920;
                        GameRef.ScreenHeight = 1080;
                        GameRef.ScreenRectangle = new Rectangle(0,0,1920,1080);
                        GameRef.Graphics.PreferredBackBufferHeight = 1080;
                        GameRef.Graphics.PreferredBackBufferWidth = 1920;
                        GameRef.Graphics.ApplyChanges();
                        _backgroundImage.DestinationRectangle = GameRef.ScreenRectangle;

                       GameRef.UpdateStatesLayout();

                    }
                    break;
                case "720p":
                    if (GameRef.ScreenWidth == 1920)
                    {
                        TileEngineInfo.SquaresAcross = 30;
                        TileEngineInfo.SquaresDown = 51;
                        GameRef.ScreenWidth = 1366;
                        GameRef.ScreenHeight = 768;
                        GameRef.ScreenRectangle = new Rectangle(0, 0, 1366, 768);
                        GameRef.Graphics.PreferredBackBufferHeight = 768;
                        GameRef.Graphics.PreferredBackBufferWidth = 1366;
                        GameRef.Graphics.ApplyChanges();
                        _backgroundImage.DestinationRectangle = GameRef.ScreenRectangle;

                        GameRef.UpdateStatesLayout();
                    }
                    break;
            }
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



