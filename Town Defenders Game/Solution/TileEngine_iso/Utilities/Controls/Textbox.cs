using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Engine.Utilities.Controls
{
   
    public class Textbox : DrawableGameComponent
    {
        private readonly Texture2D _textboxTexture;
        private readonly Texture2D _cursor;
        private readonly SpriteFont _spriteFont;
        private readonly SpriteBatch _spriteBatch;
        private readonly ContentManager _content;

        private readonly Keys[] _keysToCheck = new Keys[]
            {
                Keys.A, Keys.B, Keys.C, Keys.D, Keys.E,
                Keys.F, Keys.G, Keys.H, Keys.I, Keys.J,
                Keys.K, Keys.L, Keys.M, Keys.N, Keys.O,
                Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T,
                Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y,
                Keys.Z, Keys.Back, Keys.Space
            };

        private Vector2 _cursorPosition;
        private Vector2 _textPosition;
        private Vector2 _textboxPosition;
        private TimeSpan _blinkTime;
        private bool _blink;
        private KeyboardState _currentKeyboardState;
        private KeyboardState _lastKeyboardState;

        public Textbox(Game game, SpriteFont spriteFont)
            : base(game)
        {
            _spriteBatch =
                (SpriteBatch) Game.Services.GetService(typeof (SpriteBatch));
            _content =
                (ContentManager) Game.Services.GetService(typeof (ContentManager));
            this._spriteFont = spriteFont;
            _textboxTexture = _content.Load<Texture2D>(@"GUI\textbox");
            _cursor = _content.Load<Texture2D>(@"GUI\cursor");
            _textboxPosition = new Vector2();
            _cursorPosition = new Vector2(
                _textboxPosition.X + 5,
                _textboxPosition.Y + 5);
            _textPosition = new Vector2(
                _textboxPosition.X + 5,
                _textboxPosition.Y + 5);
            _blink = false;
            Text = "";
        }

        public string Text { get; set; }

        public Vector2 Position
        {
            get { return _textboxPosition; }
            set
            {
                _textboxPosition = value;
                SetTextPosition();
            }
        }

        private void SetTextPosition()
        {
            _cursorPosition = new Vector2(
                _textboxPosition.X + 5,
                _textboxPosition.Y + 5);
            _textPosition = new Vector2(
                _textboxPosition.X + 5,
                _textboxPosition.Y + 5);
        }

        public int Height
        {
            get { return _textboxTexture.Height; }
        }

        public int Width
        {
            get { return _textboxTexture.Width; }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            _currentKeyboardState = Keyboard.GetState();
            _blinkTime += gameTime.ElapsedGameTime;
            if (_blinkTime > TimeSpan.FromMilliseconds(500))
            {
                _blink = !_blink;
                _blinkTime -= TimeSpan.FromMilliseconds(500);
            }
            foreach (Keys key in _keysToCheck)
            {
                if (CheckKey(key))
                {
                    AddKeyToText(key);
                    break;
                }
            }
            base.Update(gameTime);
            Vector2 textSize = _spriteFont.MeasureString(Text);
            _cursorPosition.X = _textPosition.X + textSize.X;
            _lastKeyboardState = _currentKeyboardState;
        }

        private void AddKeyToText(Keys key)
        {
            string newChar = "";
            if (Text.Length >= 16 && key != Keys.Back)
                return;
            switch (key)
            {
                case Keys.A:
                    newChar += "a";
                    break;
                case Keys.B:
                    newChar += "b";
                    break;
                case Keys.C:
                    newChar += "c";
                    break;
                case Keys.D:
                    newChar += "d";
                    break;
                case Keys.E:
                    newChar += "e";
                    break;
                case Keys.F:
                    newChar += "f";
                    break;
                case Keys.G:
                    newChar += "g";
                    break;
                case Keys.H:
                    newChar += "h";
                    break;
                case Keys.I:
                    newChar += "i";
                    break;
                case Keys.J:
                    newChar += "j";
                    break;
                case Keys.K:
                    newChar += "k";
                    break;
                case Keys.L:
                    newChar += "l";
                    break;
                case Keys.M:
                    newChar += "m";
                    break;
                case Keys.N:
                    newChar += "n";
                    break;
                case Keys.O:
                    newChar += "o";
                    break;
                case Keys.P:
                    newChar += "p";
                    break;
                case Keys.Q:
                    newChar += "q";
                    break;
                case Keys.R:
                    newChar += "r";
                    break;
                case Keys.S:
                    newChar += "s";
                    break;
                case Keys.T:
                    newChar += "t";
                    break;
                case Keys.U:
                    newChar += "u";
                    break;
                case Keys.V:
                    newChar += "v";
                    break;
                case Keys.W:
                    newChar += "w";
                    break;
                case Keys.X:
                    newChar += "x";
                    break;
                case Keys.Y:
                    newChar += "y";
                    break;
                case Keys.Z:
                    newChar += "z";
                    break;
                case Keys.Space:
                    newChar += " ";
                    break;
                case Keys.Back:
                    if (Text.Length != 0)
                        Text = Text.Remove(Text.Length - 1);
                    return;
            }
            if (_currentKeyboardState.IsKeyDown(Keys.RightShift) ||
                _currentKeyboardState.IsKeyDown(Keys.LeftShift))
            {
                newChar = newChar.ToUpper();
            }
            Text += newChar;
        }

        private bool CheckKey(Keys theKey)
        {
            return _lastKeyboardState.IsKeyDown(theKey) &&
                   _currentKeyboardState.IsKeyUp(theKey);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Draw(_textboxTexture, _textboxPosition, Color.White);
            if (!_blink)
                _spriteBatch.Draw(_cursor, _cursorPosition, Color.White);
            _spriteBatch.DrawString(_spriteFont, Text, _textPosition, Color.Black);
            base.Draw(gameTime);
        }

        public void Show()
        {
            Enabled = true;
            Visible = true;
        }

        public void Hide()
        {
            Enabled = false;
            Visible = false;
        }
    }
}