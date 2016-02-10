using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Specialized;

namespace Engine.Utilities.Controls
{
  
    public class ButtonMenu : DrawableGameComponent
    {
        readonly SpriteFont _spriteFont;
        readonly SpriteBatch _spriteBatch;
        readonly Texture2D _buttonImage;
        KeyboardState _oldState, _newState;
        int _selectedIndex = 0;
        private readonly StringCollection _menuItems = new StringCollection();

        public ButtonMenu(Game game, SpriteFont spriteFont, Texture2D buttonImage)
            : base(game)
        {
            HiliteColor = Color.White;
            Position = new Vector2();
            NormalColor = Color.Black;
            _spriteFont = spriteFont;
            _buttonImage = buttonImage;
            _spriteBatch =
            (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
        }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = (int)MathHelper.Clamp(
                value,
                0,
                _menuItems.Count - 1);
            }
        }

        public Color NormalColor { get; set; }

        public Color HiliteColor { get; set; }

        public Vector2 Position { get; set; }

        public void SetMenuItems(string[] items)
        {
            _menuItems.Clear();
            _menuItems.AddRange(items);
            CalculateBounds();
        }
        private void CalculateBounds()
        {
            Width = _buttonImage.Width;
            Height = 0;
            foreach (string item in _menuItems)
            {
                Vector2 size = _spriteFont.MeasureString(item);
                Height += 5;
                Height += _buttonImage.Height;
            }
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {
            _newState = Keyboard.GetState();
            if (CheckKey(Keys.Down))
            {
                _selectedIndex++;
                if (_selectedIndex == _menuItems.Count)
                    _selectedIndex = 0;
            }
            if (CheckKey(Keys.Up))
            {
                _selectedIndex--;
                if (_selectedIndex == -1)
                {
                    _selectedIndex = _menuItems.Count - 1;
                }
            }
            _oldState = _newState;
            base.Update(gameTime);
        }


        private bool CheckKey(Keys theKey)
        {
            return _oldState.IsKeyDown(theKey) && _newState.IsKeyUp(theKey);
        }


        public override void Draw(GameTime gameTime)
        {
            Vector2 textPosition;
            Rectangle buttonRectangle = new Rectangle(
            (int)Position.X,
            (int)Position.Y,
            _buttonImage.Width,
            _buttonImage.Height);
            Color myColor;
            for (int i = 0; i < _menuItems.Count; i++)
            {
                if (i == SelectedIndex)
                    myColor = HiliteColor;
                else
                    myColor = NormalColor;
                _spriteBatch.Draw(_buttonImage,
                buttonRectangle,
                Color.White);
                textPosition = new Vector2(
                buttonRectangle.X + (_buttonImage.Width / 2),
                buttonRectangle.Y + (_buttonImage.Height / 2));
                Vector2 textSize = _spriteFont.MeasureString(_menuItems[i]);
                textPosition.X -= textSize.X / 2;
                textPosition.Y -= _spriteFont.LineSpacing / 2;
                _spriteBatch.DrawString(_spriteFont,
                _menuItems[i], textPosition,
                myColor);
                buttonRectangle.Y += _buttonImage.Height;
                buttonRectangle.Y += 5;
            }
            base.Draw(gameTime);
        }
    }
}
