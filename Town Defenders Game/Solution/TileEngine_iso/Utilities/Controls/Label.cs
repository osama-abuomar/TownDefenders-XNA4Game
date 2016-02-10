
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Utilities.Controls
{
    public class Label:Control
    {

        

        public Label()
        {
            Color = LabelColor;
            TabStop = false;
        }

        public override void SetPosition(Vector2 position)
        {
            Position = position;
        }

        public override void Update(GameTime gameTime)
        {
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible) return;
            spriteBatch.DrawString(SpriteFont, Text, Position, Color);
        }
        public override void HandleInput()
        {
        }
    }
}
