using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Utilities
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class InputManager : Microsoft.Xna.Framework.GameComponent
    {

        private static KeyboardState currentKeyboardState;
        private static KeyboardState lastKeyboardState;

        public static KeyboardState CurrentKeyboardState 
        {
            get { return currentKeyboardState; }
        }

        public static KeyboardState LastKeyboardState 
        {
            get { return lastKeyboardState; }
        }

        public InputManager(Game game): base(game)
        {
            currentKeyboardState = Keyboard.GetState();
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
            lastKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();



            base.Update(gameTime);
        }

        public void Flush()
        {
            lastKeyboardState = currentKeyboardState;
        }

        public static bool KeyPressed(Keys key)
        {
            if (lastKeyboardState.IsKeyUp(key) && 
                currentKeyboardState.IsKeyDown(key))
                return true;
            else return false;
        }

        public static bool KeyReleased(Keys key)
        {
            return lastKeyboardState.IsKeyDown(key) && 
                currentKeyboardState.IsKeyUp(key);
        }

        public static bool KeyDown(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }
    }
}
