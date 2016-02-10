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


namespace Engine.Utilities
{
    public abstract partial class GameState : DrawableGameComponent
    {
        public List<GameComponent> Components { get; private set; }

        public GameState Tag { get; private set; }

        protected GameStateManager StateManager;

        protected GameState(Game game, GameStateManager manager)
            : base(game)
        {
            StateManager = manager;
            Components = new List<GameComponent>();
            Tag = this;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public virtual void UpdateLayout()
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (GameComponent component in Components)
            {
                if (component.Enabled)
                    component.Update(gameTime);
            }
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            foreach (var component in Components)
            {
                if (component is DrawableGameComponent)
                {
                    var drawComponent = component as DrawableGameComponent;
                    if (drawComponent.Visible)
                        drawComponent.Draw(gameTime);
                }
            }
            base.Draw(gameTime);
        }

        internal protected virtual void StateChange(object sender, EventArgs e)
        {
            if (StateManager.CurrentState == Tag)
                Show();
            else
                Hide();
        }
        protected virtual void Show()
        {
            Visible = true;
            Enabled = true;
            foreach (var component in Components)
            {
                component.Enabled = true;
                if (component is DrawableGameComponent)
                    ((DrawableGameComponent)component).Visible = true;
            }
        }
        protected virtual void Hide()
        {
            Visible = false;
            Enabled = false;
            foreach (var component in Components)
            {
                component.Enabled = false;
                if (component is DrawableGameComponent) ((DrawableGameComponent)component).Visible = false;
            }
        }
    }
}

