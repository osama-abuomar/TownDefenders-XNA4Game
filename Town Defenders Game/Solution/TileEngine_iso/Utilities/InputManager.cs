using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Iso_Tile_Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Engine.Utilities
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class InputManager : Microsoft.Xna.Framework.GameComponent
    {

        private static KeyboardState _currentKeyboardState;
        private static KeyboardState _lastKeyboardState;

        private static MouseState _currentMouseState;
        private static MouseState _lastMouseState;

        public static Rectangle ScreenRectangle;

        private const double DoubleClickDelay = 300; // ms
        private static List<double> LeftBtnClicksFromNow = new List<double>();


        public InputManager(Game game, Rectangle screenRect)
            : base(game)
        {
            _currentKeyboardState = Keyboard.GetState();
            _currentMouseState = Mouse.GetState();

            //todo must be updated if resolution changed
            ScreenRectangle = screenRect;
        }


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            _lastKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            _lastMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            if (_CLEARmouseLbHeldDownSinceLastLbPressNEXTFRAME)
            {
                _CLEARmouseLbHeldDownSinceLastLbPressNEXTFRAME = false;
                _mouseLbHeldDownSinceLastLbPress = false;
            }
            

            if (MouseLeftButtonClicked())
            {
                LeftBtnClicksFromNow.Add(0.0);
            }

            var itemsToRemove = new List<int>();
            for (int i = 0; i < LeftBtnClicksFromNow.Count; i++)
            {
                LeftBtnClicksFromNow[i] += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (LeftBtnClicksFromNow[i] > DoubleClickDelay)
                    itemsToRemove.Add(i); // because I can't remove in a for loop
            }
            foreach (var item in itemsToRemove)
            {
                LeftBtnClicksFromNow.RemoveAt(item);
            }

            // needed by MouseClickedOverArea method..
            if (_currentMouseState.LeftButton == ButtonState.Pressed && !_lockPressTime)
            {
                _mouseLbHeldDownSinceLastLbPress = true;
                _mouseLbLastPressLocation = new Point(_currentMouseState.X, _currentMouseState.Y);
                _lockPressTime = true;
            }
            if (_currentMouseState.LeftButton == ButtonState.Released)
            {
                _CLEARmouseLbHeldDownSinceLastLbPressNEXTFRAME = true;
                _lockPressTime = false;
            }


            base.Update(gameTime);
        }

        public void Flush()
        {
            _lastKeyboardState = _currentKeyboardState;
            _lastMouseState = _currentMouseState;
        }

        public static bool KeyPressed(Keys key)
        {
            if (_lastKeyboardState.IsKeyUp(key) &&
                _currentKeyboardState.IsKeyDown(key))
                return true;
            return false;
        }

        public static bool KeyReleased(Keys key)
        {
            return _lastKeyboardState.IsKeyDown(key) &&
                _currentKeyboardState.IsKeyUp(key);
        }

        public static bool KeyDown(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key);
        }

        
        private static bool MouseEventWithinScreenRect()
        {
            var currentMouseLocation = new Point(_currentMouseState.X, _currentMouseState.Y);
            var lastMouseLocation = new Point(_lastMouseState.X, _lastMouseState.Y);
            if (ScreenRectangle.Contains(currentMouseLocation) &&
                ScreenRectangle.Contains(lastMouseLocation))
                return true;
            return false;
        }

        public static bool MouseLeftButtonClicked()
        {
            if (!MouseEventWithinScreenRect()) return false;

            return (_lastMouseState.LeftButton == ButtonState.Pressed
                && _currentMouseState.LeftButton == ButtonState.Released );
        }

        public static bool MouseLeftButtonDoubleClicked()
        {
            if (!MouseEventWithinScreenRect()) return false;

            return MouseLeftButtonClicked() &&
                (LeftBtnClicksFromNow.Count > 1);
        }

        public static bool MouseRightButtonClicked()
        {
            if (!MouseEventWithinScreenRect()) return false;

            return (_lastMouseState.RightButton == ButtonState.Pressed
                && _currentMouseState.RightButton == ButtonState.Released);
        }

        private static Point _lastMousePos;
        private static Point _currentMousePos;

        public static bool MouseHoverOverArea(Rectangle area)
        {
            if (!MouseEventWithinScreenRect()) return false;

            _lastMousePos = new Point(_lastMouseState.X, _lastMouseState.Y);
            _currentMousePos = new Point(_currentMouseState.X, _currentMouseState.Y);

            var isWithinArea = area.Contains(_lastMousePos) && area.Contains(_currentMousePos);

            if (isWithinArea && _lastMousePos != _currentMousePos) return true;
            else return false;
        }


        private static bool _mouseLbHeldDownSinceLastLbPress;
        private static bool _CLEARmouseLbHeldDownSinceLastLbPressNEXTFRAME;
        private static bool _lockPressTime = false;

        private static Point _mouseLbLastPressLocation;
        /// <summary>
        /// press and release
        /// </summary>
        /// <param name="area"></param>
        public static bool MouseClickedOverArea(Rectangle area)
        {
            if (!MouseEventWithinScreenRect()) return false;

            var currentMouseLocation = new Point(_currentMouseState.X, _currentMouseState.Y);
            return
                area.Contains(_mouseLbLastPressLocation) &&
                _mouseLbHeldDownSinceLastLbPress &&
                area.Contains(currentMouseLocation) &&
                _currentMouseState.LeftButton == ButtonState.Released;
        }

        /// <summary>
        /// only press, make a sound 
        /// </summary>
        /// <param name="area"></param>
        public static bool MousePressedOverArea(Rectangle area)
        {
            if (!MouseEventWithinScreenRect()) return false;

            var currentMouseLocation = new Point(_currentMouseState.X, _currentMouseState.Y);
            return 
                area.Contains(currentMouseLocation) &&
                _currentMouseState.LeftButton == ButtonState.Pressed; ;
        }

        /// <summary>
        /// carefull: to return the button visual state to be released taking no action
        /// </summary>
        /// <param name="area"></param>
        public static bool MouseReleasedOutsideOfPressedArea(Rectangle area)
        {
            if (!MouseEventWithinScreenRect()) return false;

            var currentMouseLocation = new Point(_currentMouseState.X, _currentMouseState.Y);
            return
                area.Contains(_mouseLbLastPressLocation) &&
                _mouseLbHeldDownSinceLastLbPress &&
                !(area.Contains(currentMouseLocation)) &&
                _currentMouseState.LeftButton == ButtonState.Released;
            
        }

        public static bool MouseExitedArea(Rectangle area)
        {
            if (!MouseEventWithinScreenRect()) return false;

            var currentMouseLocation = new Point(_currentMouseState.X, _currentMouseState.Y);
            var lastMouseLocation = new Point(_lastMouseState.X, _lastMouseState.Y);
            return
                area.Contains(lastMouseLocation) &&
                !area.Contains(currentMouseLocation);
        }

        public static bool MouseEnteredArea(Rectangle area)
        {
            if (!MouseEventWithinScreenRect()) return false;

            var currentMouseLocation = new Point(_currentMouseState.X, _currentMouseState.Y);
            var lastMouseLocation = new Point(_lastMouseState.X, _lastMouseState.Y);
            return
                !area.Contains(lastMouseLocation) &&
                area.Contains(currentMouseLocation);
        }

        public static bool MouseOverArea(Rectangle area)
        {
            if (!MouseEventWithinScreenRect()) return false;

            var currentMouseLocation = new Point(_currentMouseState.X, _currentMouseState.Y);
            return
                area.Contains(currentMouseLocation);
        }

        public static Vector2 GetMouseWorldLocation()
        {
            var clickPosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            var clickWorldPosition = Camera.ScreenToWorld(clickPosition);
            return clickWorldPosition;
        }
    }
}
