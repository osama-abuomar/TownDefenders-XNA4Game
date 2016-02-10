using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Engine.Utilities.Controls.ImageButtons
{
    public class ImageButton96 : PictureBox
    {
        
        protected Rectangle MouseActiveArea
        {
            get
            {
                return DestinationRectangle;
            }
        }


        public ImageButton96(Texture2D image, Vector2 position)
            :base(image, new Rectangle((int) position.X, (int) position.Y, 96, 96), 
            ControlInfo.IconDrawDepth)
        {
            
           
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (InputManager.MouseHoverOverArea(MouseActiveArea))
            {
                OnHover();
            }
            if (InputManager.MouseEnteredArea(MouseActiveArea))
            {
                HasFocus = true;
                SoundManager.Instance().PlaySoundEffect("focus");
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible) return;

            base.Draw(spriteBatch);
            
            //draw border..
            spriteBatch.Draw(
                  GameGraphics.GetTexture("button_border_96").SourceTexture,
                  new Vector2(DestinationRectangle.X, DestinationRectangle.Y),
                  SourceRectangle,
                  Color.White,
                  0.0f,
                  Vector2.Zero,
                  1.0f,
                  SpriteEffects.None,
                  ControlInfo.BorderDrawDepth
                 );

            if (HasFocus)
            {
                //draw overlay..
                spriteBatch.Draw(
                      GameGraphics.GetTexture("button_press_96").SourceTexture,
                      new Vector2(DestinationRectangle.X, DestinationRectangle.Y),
                      SourceRectangle,
                      Color.White,
                      0.0f,
                      Vector2.Zero,
                      1.0f,
                      SpriteEffects.None,
                      ControlInfo.OverlayDrawDepth
                     );
            }

        }

        public override void HandleInput()
        {
           base.HandleInput();
           if (HasFocus)
           {
               if (InputManager.KeyReleased(Keys.Enter))
               {
                   SoundManager.Instance().PlaySoundEffect("select");
                   base.OnSelected(null);
               }

               if (InputManager.MouseClickedOverArea(MouseActiveArea))
               {
                   SoundManager.Instance().PlaySoundEffect("select");
                   OnClick();
               }

               if (InputManager.MouseExitedArea(MouseActiveArea))
               {
                   HasFocus = false;
               }
           }

        }
    }
}
