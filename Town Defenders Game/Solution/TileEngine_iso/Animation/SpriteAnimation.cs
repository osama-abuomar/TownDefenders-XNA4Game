using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Engine.Iso_Tile_Engine;

namespace Engine.Animation
{
    public class SpriteAnimation
    {
        // The texture that holds the images for this sprite
        readonly Texture2D _t2DTexture;

        // True if animations are being played
        bool _bAnimating = true;

        // If set to anything other than Color.White, will colorize
        // the sprite with that color.
        Color _colorTint = Color.White;

        // Screen Position of the Sprite
        Vector2 _v2Position = new Vector2(0, 0);
        Vector2 _v2LastPosition = new Vector2(0, 0);

        // Dictionary holding all of the FrameAnimation objects
        // associated with this sprite.
        readonly Dictionary<string, FrameAnimation> _faAnimations = new Dictionary<string, FrameAnimation>();

        // Which FrameAnimation from the dictionary above is playing
        string _sCurrentAnimation = null;

        // If true, the sprite will automatically rotate to align itself
        // with the angle difference between it's new position and
        // it's previous position.  In this case, the 0 rotation point
        // is to the right (so the sprite should start out facing to
        // the right.
        bool _bRotateByPosition = false;

        // How much the sprite should be rotated by when drawn
        // Value is in Radians, and 0 indicates no rotation.
        float _fRotation = 0f;

        // Calcualted center of the sprite
        Vector2 _v2Center;

        // Calculated width and height of the sprite
        int _iWidth;
        int _iHeight;

        ///
        /// Vector2 representing the position of the sprite's upper left
        /// corner pixel.
        ///
        public Vector2 Position
        {
            get { return _v2Position; }
            set
            {
                _v2LastPosition = _v2Position;
                _v2Position = value;
                UpdateRotation();
            }
        }

        ///
        /// The X position of the sprite's upper left corner pixel.
        ///
        public int X
        {
            get { return (int)_v2Position.X; }
            set
            {
                _v2LastPosition.X = _v2Position.X;
                _v2Position.X = value;
                UpdateRotation();
            }
        }

        ///
        /// The Y position of the sprite's upper left corner pixel.
        ///
        public int Y
        {
            get { return (int)_v2Position.Y; }
            set
            {
                _v2LastPosition.Y = _v2Position.Y;
                _v2Position.Y = value;
                UpdateRotation();
            }
        }

        ///
        /// Width (in pixels) of the sprite animation frames
        ///
        public int Width
        {
            get { return _iWidth; }
        }

        ///
        /// Height (in pixels) of the sprite animation frames
        ///
        public int Height
        {
            get { return _iHeight; }
        }

        ///
        /// If true, the sprite will automatically rotate in the direction
        /// of motion whenever the sprite's Position changes.
        ///
        public bool AutoRotate
        {
            get { return _bRotateByPosition; }
            set { _bRotateByPosition = value; }
        }

        ///
        /// The degree of rotation (in radians) to be applied to the
        /// sprite when drawn.
        ///
        public float Rotation
        {
            get { return _fRotation; }
            set { _fRotation = value; }
        }

        ///
        /// Screen coordinates of the bounding box surrounding this sprite
        ///
        public Rectangle BoundingBox
        {
            get { return new Rectangle(X, Y, _iWidth, _iHeight); }
        }

        ///
        /// The texture associated with this sprite.  All FrameAnimations will be
        /// relative to this texture.
        ///
        public Texture2D Texture
        {
            get { return _t2DTexture; }
        }

        ///
        /// Color value to tint the sprite with when drawing.  Color.White
        /// (the default) indicates no tinting.
        ///
        public Color Tint
        {
            get { return _colorTint; }
            set { _colorTint = value; }
        }

        ///
        /// True if the sprite is (or should be) playing animation frames.  If this value is set
        /// to false, the sprite will not be drawn (a sprite needs at least 1 single frame animation
        /// in order to be displayed.
        ///
        public bool IsAnimating
        {
            get { return _bAnimating; }
            set { _bAnimating = value; }
        }

        ///
        /// The FrameAnimation object of the currently playing animation
        ///
        public FrameAnimation CurrentFrameAnimation
        {
            get
            {
                if (!string.IsNullOrEmpty(_sCurrentAnimation))
                    return _faAnimations[_sCurrentAnimation];
                else
                    return null;
            }
        }

        ///
        /// The string name of the currently playing animaton.  Setting the animation
        /// resets the CurrentFrame and PlayCount properties to zero.
        ///
        public string CurrentAnimation
        {
            get { return _sCurrentAnimation; }
            set
            {
                if (_faAnimations.ContainsKey(value))
                {
                    _sCurrentAnimation = value;
                    _faAnimations[_sCurrentAnimation].CurrentFrame = 0;
                    _faAnimations[_sCurrentAnimation].PlayCount = 0;
                }
            }
        }

        public Vector2 DrawOffset { get; set; }
        public float DrawDepth { get; set; }

        public SpriteAnimation(Texture2D texture)
        {
            DrawOffset = Vector2.Zero;
            DrawDepth = 0.0f;

            _t2DTexture = texture;
        }

        void UpdateRotation()
        {
            if (_bRotateByPosition)
            {
                _fRotation = (float)Math.Atan2(_v2Position.Y - _v2LastPosition.Y, _v2Position.X - _v2LastPosition.X);
            }
        }

        public void AddAnimation(string name, int X, int Y, int width, int Height, int frames, float frameLength)
        {
            _faAnimations.Add(name, new FrameAnimation(X, Y, width, Height, frames, frameLength));
            _iWidth = width;
            _iHeight = Height;
            _v2Center = new Vector2(x: _iWidth / 2, y: _iHeight / 2);
        }

        public void AddAnimation(string name, int X, int Y, int width, int Height, int frames,
           float frameLength, string nextAnimation)
        {
            _faAnimations.Add(name, new FrameAnimation(X, Y, width, Height, frames, frameLength, nextAnimation));
            _iWidth = width;
            _iHeight = Height;
            _v2Center = new Vector2(x: _iWidth / 2, y: _iHeight / 2);
        }

        public FrameAnimation GetAnimationByName(string name)
        {
            return _faAnimations.ContainsKey(name) ? _faAnimations[name] : null;
        }

        public void MoveBy(float x, float y)
        {
            _v2LastPosition = _v2Position;
            _v2Position.X += x;
            _v2Position.Y += y;
            UpdateRotation();
        }

        public void Update(GameTime gameTime)
        {
            // Don't do anything if the sprite is not animating
            if (!_bAnimating) return;
            // If there is not a currently active animation
            if (CurrentFrameAnimation == null)
            {
                // Make sure we have an animation associated with this sprite
                if (_faAnimations.Count > 0)
                {
                    // Set the active animation to the first animation
                    // associated with this sprite
                    var sKeys = new string[_faAnimations.Count];
                    _faAnimations.Keys.CopyTo(sKeys, 0);
                    CurrentAnimation = sKeys[0];
                }
                else
                {
                    return;
                }
            }

            // Run the Animation's update method
            CurrentFrameAnimation.Update(gameTime);

            // Check to see if there is a "followup" animation named for this animation
            if (!String.IsNullOrEmpty(CurrentFrameAnimation.NextAnimation))
            {
                // If there is, see if the currently playing animation has
                // completed a full animation loop
                if (CurrentFrameAnimation.PlayCount > 0)
                {
                    // If it has, set up the next animation
                    CurrentAnimation = CurrentFrameAnimation.NextAnimation;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, int xOffset, int yOffset)
        {
            if (_bAnimating)
                spriteBatch.Draw(_t2DTexture,
                    Camera.WorldToScreen(_v2Position) + _v2Center + DrawOffset + new Vector2(xOffset, yOffset),
                    CurrentFrameAnimation.FrameRectangle, _colorTint,
                    _fRotation, _v2Center, 1f, SpriteEffects.None, DrawDepth);
        }
    }
}
