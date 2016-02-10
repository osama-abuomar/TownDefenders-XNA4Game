using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Engine.Utilities.Controls
{
    public abstract class Control
    {
        public event EventHandler Selected;
        public event EventHandler Click;
        public event EventHandler Hover;
      
        public string Name { get; set; }
        public string Text { get; set; }
        public Vector2 Size { get; set; }
        protected Vector2 Position { get; set; }
        public abstract void SetPosition(Vector2 position);
        public object Value { get; set; }
        public bool HasFocus { get; set; }
        public bool Enabled { get; set; }
        public bool Visible { get; set; }
        public bool TabStop { get; set; }
        public SpriteFont SpriteFont { get; set; }
        public Color Color { get; set; }
        public string Type { get; set; }

        //protected readonly Color NormalColor = new Color(120, 53, 10);
        public static readonly Color NormalColor = new Color(90, 0, 0);
        public static readonly Color SelectColor = new Color(90, 40, 0);
        public static readonly Color LabelColor = Color.Black;

        protected virtual void OnClick()
        {
            EventHandler handler = Click;
            
            if (handler != null) handler(this, EventArgs.Empty);
        }

        protected virtual void OnHover()
        {
            EventHandler handler = Hover;
            if (handler != null) handler(this, EventArgs.Empty);
        }


        protected Control()
        {
            Color = Color.White;
            Enabled = true;
            Visible = true; 
            SpriteFont = ControlManager.SpriteFont;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void HandleInput();

        protected virtual void OnSelected(EventArgs e)
        {
            if (Selected != null)
            {
                Selected(this, e);
            }
        }
    }
}
