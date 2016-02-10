using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Engine.Utilities.Controls
{
    public class ControlManager : List<Control>
    {
        public event EventHandler FocusChanged;

        int _selectedControl;
        public static SpriteFont SpriteFont { get; private set; }

        public ControlManager(SpriteFont spriteFont)
        {
            _selectedControl = 0;
            SpriteFont = spriteFont;
        }

        public ControlManager(SpriteFont spriteFont, int capacity)
            : base(capacity)
        {
            _selectedControl = 0;
            SpriteFont = spriteFont;
        }

        public ControlManager(SpriteFont spriteFont, IEnumerable<Control> collection) :
            base(collection)
        {
            _selectedControl = 0;
            SpriteFont = spriteFont;
        }


        /// <summary>
        /// update enabled controls, and move focus if Keys up or down pressed
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (Count == 0)
                return;
            foreach (Control c in this)
            {
                if (c.Enabled)
                    c.Update(gameTime);
                if (c.HasFocus)
                    c.HandleInput();
            }

            

            if (InputManager.KeyPressed(Keys.Up))
            {
                LoseFocus();
                PreviousControl();

            }
            if (InputManager.KeyPressed(Keys.Down))
            {
                LoseFocus();
                NextControl();
            }
        }
        /// <summary>
        /// Draw visible controls
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Control c in this)
            {
                if (c.Visible)
                    c.Draw(spriteBatch);
            }
        }

        public void LoseFocus()
        {
            //_selectedControl = 0;

            foreach (var control in this)
            {
                control.HasFocus = false;
            }
        }


        /// <summary>
        /// Move focus to next control that is a tab stop and enabled
        /// </summary>
        public void NextControl()
        {
            if (Count == 0)
                return;

            SoundManager.Instance().PlaySoundEffect("focus");

            int currentControl = _selectedControl;
            this[_selectedControl].HasFocus = false;
            do
            {
                _selectedControl++;
                if (_selectedControl == Count)
                    _selectedControl = 0;
                if (this[_selectedControl].TabStop && this[_selectedControl].Enabled)
                {
                    if (FocusChanged != null)
                        FocusChanged(this[_selectedControl], null);
                    break;
                }
            } while (currentControl != _selectedControl);
            this[_selectedControl].HasFocus = true;
        }
        /// <summary>
        /// Move focus to previous control
        /// </summary>
        public void PreviousControl()
        {
            if (Count == 0)
                return;

            SoundManager.Instance().PlaySoundEffect("focus");

            int currentControl = _selectedControl;
            this[_selectedControl].HasFocus = false;
            do
            {
                _selectedControl--;
                if (_selectedControl < 0)
                    _selectedControl = Count - 1;
                if (this[_selectedControl].TabStop && this[_selectedControl].Enabled)
                {
                    if (FocusChanged != null)
                        FocusChanged(this[_selectedControl], null);
                    break;
                }
            } while (currentControl != _selectedControl);
            this[_selectedControl].HasFocus = true;
        }
    }
}
