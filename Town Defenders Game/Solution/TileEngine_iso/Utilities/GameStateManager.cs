using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace Engine.Utilities
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class GameStateManager : Microsoft.Xna.Framework.GameComponent
    {
        public event EventHandler OnStateChange;

        readonly Stack<GameState> _gameStates = new Stack<GameState>();
        const int StartDrawOrder = 5000;
        const int DrawOrderInc = 100;
        int _drawOrder;
        public GameState CurrentState
        {
            get { return _gameStates.Peek(); }
        }

        public GameStateManager(Game game)
            : base(game)
        {
            _drawOrder = StartDrawOrder;
        }


    


        public override void Initialize()
        {
            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        private void MakeVisible()
        {
            foreach (var gameState in _gameStates)
            {
                gameState.Visible = true;
            }
        }

        public void PopState()
        {
            if (_gameStates.Count > 0)
            {
                RemoveState();
                _drawOrder -= DrawOrderInc;
                if (OnStateChange != null)
                    OnStateChange(this, null);
            }
            MakeVisible();
        }
        private void RemoveState()
        {
            GameState state = _gameStates.Peek();
            OnStateChange -= state.StateChange;
            Game.Components.Remove(state);
            _gameStates.Pop();
        }
        public void PushState(GameState newState)
        {
            
            _drawOrder += DrawOrderInc;
            newState.DrawOrder = _drawOrder;
            AddState(newState);
            if (OnStateChange != null)
                OnStateChange(this, null);

            MakeVisible();
        }
        private void AddState(GameState newState)
        {
            _gameStates.Push(newState);
            Game.Components.Add(newState);
            OnStateChange += newState.StateChange;
        }
        public void ChangeState(GameState newState)
        {
           
            while (_gameStates.Count > 0)
                RemoveState();
            newState.DrawOrder = StartDrawOrder;
            _drawOrder = StartDrawOrder;
            AddState(newState);
            if (OnStateChange != null)
                OnStateChange(this, null);
        }
    }
}
