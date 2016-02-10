using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Engine.Utilities.Controls
{
    public class SwitchBox:Control
    {
        public event EventHandler ItemChanged;
        //private event EventHandler LBClick;
        //private event EventHandler RBClick;

        //private void OnLBClick()
        //{
        //    EventHandler handler = LBClick;
        //    if (handler != null) handler(this, EventArgs.Empty);
        //}

        //private void OnRBClick()
        //{
        //    EventHandler handler = RBClick;
        //    if (handler != null) handler(this, EventArgs.Empty);
        //}

        private readonly PictureBox _leftArrow;
        private readonly PictureBox _rightArrow;

        private PictureBox _indexer;
        public bool Indexer;

        private List<string> _menuItems;
        private int _currentItem;

        public string Caption;

        private const int CaptionMaxWidth = 200;
        private const int MenuMaxWidth = 200;

      
        private readonly int _arrowWidth;
        private readonly int _arrowHeight;

        


        public SwitchBox(string caption, List<string> menuItems )
        {
            // set position initially to zero.
            var la = GameGraphics.GetTexture("left_arrow").SourceTexture;
            _leftArrow = new PictureBox(la, new Rectangle(0, 0, la.Width, la.Height));
            var ra = GameGraphics.GetTexture("right_arrow").SourceTexture;
            _rightArrow = new PictureBox(ra, new Rectangle(0, 0, ra.Width, ra.Height));

            _arrowWidth = ra.Width; // or la
            _arrowHeight = ra.Height;

            SetMenuItems(menuItems);

            _currentItem = 0;

            Caption = caption;

            HasFocus = false;
            TabStop = true;
            Indexer = true;

            SetPosition(Vector2.Zero);
           
        }
        /// <summary>
        /// Automatically sets the positions of the SwitchBox visual components
        /// </summary>
        /// <param name="position"></param>
        public override void SetPosition(Vector2 position)
        {
            Position = position;
            _leftArrow.SetPosition(new Vector2(position.X + CaptionMaxWidth, position.Y+5));
            _rightArrow.SetPosition(new Vector2(position.X + CaptionMaxWidth + _arrowWidth + MenuMaxWidth, position.Y+5));

            var indexerTexture = GameGraphics.GetTexture("left_indexer").SourceTexture;
            var indexerXPosition = Position.X - indexerTexture.Width - 18; // some offset
            var indexerYPosition = Position.Y + 3;
            var indexerPosition = new Vector2(indexerXPosition, indexerYPosition);
            _indexer = new PictureBox(indexerTexture, new Rectangle(0, 0, indexerTexture.Width, indexerTexture.Height));
            _indexer.SetPosition(indexerPosition);

            _leftArrowButtonArea = _leftArrow.DestinationRectangle;
            _rightArrowButtonArea = _rightArrow.DestinationRectangle;
        }

        public void SetMenuItems(List<string> items)
        {
            if(_menuItems == null)
                _menuItems = new List<string>();
            if(items.Count == 0)
                throw new Exception("err: empty menu passed!");
            _menuItems.Clear();
            _menuItems.AddRange(items);
        }

        public string CurrentItem
        {
            get { return _menuItems[_currentItem]; }
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.MouseClickedOverArea(MouseActiveArea))
            {
                SoundManager.Instance().PlaySoundEffect("select");
                OnClick();
            }
            if (InputManager.MouseHoverOverArea(MouseActiveArea))
            {
                OnHover();
            }
            if (InputManager.MouseEnteredArea(MouseActiveArea))
            {
                SoundManager.Instance().PlaySoundEffect("focus");
            }
        }

        protected Rectangle MouseActiveArea
        {
            get
            {
                return new Rectangle(
                    (int) Position.X,
                    (int) Position.Y,
                    CaptionMaxWidth + _leftArrow.DestinationRectangle.Width + MenuMaxWidth + _rightArrow.DestinationRectangle.Width,
                    _arrowHeight);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //current viewed item's X position is changed depending on the length of the text (since it is centered alignment)
            float itemXPosition = Position.X + CaptionMaxWidth + _arrowWidth + MenuMaxWidth/2 -
                                  SpriteFont.MeasureString(CurrentItem).X/2;
            float itemYPosition = Position.Y;
            Vector2 itemPosition = new Vector2(itemXPosition, itemYPosition);

            // draw the arrows..
            _leftArrow.Draw(spriteBatch);
            _rightArrow.Draw(spriteBatch);
            // draw the caption..
            spriteBatch.DrawString(SpriteFont, Caption, Position, HasFocus? SelectColor:NormalColor);
            // draw the menue item..
            spriteBatch.DrawString(SpriteFont, CurrentItem, itemPosition, NormalColor);
            // draw indexer..
            if (HasFocus && Indexer)
            {
                _indexer.Draw(spriteBatch);
            }
        }

        private void NextItem()
        {
            _currentItem++;
            if (_currentItem == _menuItems.Count)
                _currentItem = 0;
        }

        private void PreviousItem()
        {
            _currentItem--;
            if (_currentItem < 0)
                _currentItem = _menuItems.Count - 1;
        }

        private Rectangle _leftArrowButtonArea;

        private Rectangle _rightArrowButtonArea;

        public override void HandleInput()
        {
            if (HasFocus)
            {
                if (InputManager.KeyReleased(Keys.Enter))
                    base.OnSelected(null);

                if (InputManager.KeyReleased(Keys.Left))
                {
                    PreviousItem();
                    this.OnItemChanged(null);
                }

                if (InputManager.KeyReleased(Keys.Right))
                {
                    NextItem();
                    this.OnItemChanged(null);
                }

                //already focused by hovering
                if (InputManager.MouseClickedOverArea(_leftArrowButtonArea))
                {
                    SoundManager.Instance().PlaySoundEffect("select");
                    PreviousItem();
                    this.OnItemChanged(null);
                }

                if (InputManager.MouseClickedOverArea(_rightArrowButtonArea))
                {
                    SoundManager.Instance().PlaySoundEffect("select");
                    NextItem();
                    this.OnItemChanged(null);
                }

                if (InputManager.MouseClickedOverArea(CaptionArea))
                {
                    SoundManager.Instance().PlaySoundEffect("select");
                    NextItem();// or previous
                    this.OnItemChanged(null);
                }

                if (InputManager.MouseExitedArea(MouseActiveArea))
                {
                    HasFocus = false;
                }
            }
        }

        protected Rectangle CaptionArea
        {
            get
            {
                return new Rectangle(
                    (int) Position.X,
                    (int) Position.Y,
                    CaptionMaxWidth,
                    _arrowHeight);
            }

        }

        protected virtual void OnItemChanged(EventArgs e)
        {
            if (ItemChanged != null)
            {
                ItemChanged(this, e);
            }
        }

    }


}
