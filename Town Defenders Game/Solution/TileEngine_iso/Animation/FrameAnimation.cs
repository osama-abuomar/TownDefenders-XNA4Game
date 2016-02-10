using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Engine.Animation
{
    public class FrameAnimation : ICloneable
    {
        // The first frame of the Animation.  We will calculate other
        // frames on the fly based on this frame.
        private Rectangle _rectInitialFrame;


        // Number of frames in the Animation
        private int _iFrameCount = 1;


        // The frame currently being displayed. 
        // This value ranges from 0 to iFrameCount-1
        private int _iCurrentFrame = 0;


        // Amount of time (in seconds) to display each frame
        private float _fFrameLength = 0.2f;


        // Amount of time that has passed since we last animated
        private float _fFrameTimer = 0.0f;


        // The number of times this animation has been played
        private int _iPlayCount = 0;


        // The animation that should be played after this animation
        private string _sNextAnimation = null;

        /// 
        /// The number of frames the animation contains
        /// 
        public int FrameCount
        {
            get { return _iFrameCount; }
            set { _iFrameCount = value; }
        }

        /// 
        /// The time (in seconds) to display each frame
        /// 
        public float FrameLength
        {
            get { return _fFrameLength; }
            set { _fFrameLength = value; }
        }

        /// 
        /// The frame number currently being displayed
        /// 
        public int CurrentFrame
        {
            get { return _iCurrentFrame; }
            set { _iCurrentFrame = (int)MathHelper.Clamp(value, 0, _iFrameCount - 1); }
        }

        public int FrameWidth
        {
            get { return _rectInitialFrame.Width; }
        }

        public int FrameHeight
        {
            get { return _rectInitialFrame.Height; }
        }

        /// 
        /// The rectangle associated with the current
        /// animation frame.
        /// 
        public Rectangle FrameRectangle
        {
            get
            {
                return new Rectangle(
                    _rectInitialFrame.X + (_rectInitialFrame.Width * _iCurrentFrame),
                    _rectInitialFrame.Y, _rectInitialFrame.Width, _rectInitialFrame.Height);
            }
        }

        public int PlayCount
        {
            get { return _iPlayCount; }
            set { _iPlayCount = value; }
        }

        public string NextAnimation
        {
            get { return _sNextAnimation; }
            set { _sNextAnimation = value; }
        }

        public FrameAnimation(Rectangle firstFrame, int frames)
        {
            _rectInitialFrame = firstFrame;
            _iFrameCount = frames;
        }

        public FrameAnimation(int x, int y, int width, int height, int frames)
        {
            _rectInitialFrame = new Rectangle(x, y, width, height);
            _iFrameCount = frames;
        }

        public FrameAnimation(int x, int y, int width, int height, int frames, float frameLength)
        {
            _rectInitialFrame = new Rectangle(x, y, width, height);
            _iFrameCount = frames;
            _fFrameLength = frameLength;
        }

        public FrameAnimation(int x, int y,
            int width, int height, int frames,
            float frameLength, string strNextAnimation)
        {
            _rectInitialFrame = new Rectangle(x, y, width, height);
            _iFrameCount = frames;
            _fFrameLength = frameLength;
            _sNextAnimation = strNextAnimation;
        }

        public void Update(GameTime gameTime)
        {
            _fFrameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_fFrameTimer > _fFrameLength)
            {
                _fFrameTimer = 0.0f;
                _iCurrentFrame = (_iCurrentFrame + 1) % _iFrameCount;
                if (_iCurrentFrame == 0)
                    _iPlayCount = (int)MathHelper.Min(_iPlayCount + 1, int.MaxValue);
            }
        }

        object ICloneable.Clone()
        {
            return new FrameAnimation(this._rectInitialFrame.X, this._rectInitialFrame.Y,
                                      this._rectInitialFrame.Width, this._rectInitialFrame.Height,
                                      this._iFrameCount, this._fFrameLength, _sNextAnimation);
        }
    }
}
