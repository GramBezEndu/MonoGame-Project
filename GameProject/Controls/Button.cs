using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject.Sprites;

namespace GameProject.Controls
{
	public class Button : Sprite
	{
		MouseState currentState;
		MouseState previousState;
		SpriteFont font;
		bool isHovering;
		public EventHandler Click { get; set; }
		public Color PenColor { get; set; }
		public string Text { get; set; }
		public Button(Texture2D t, SpriteFont f, Vector2 scale) : base(t,scale)
		{
			texture = t;
			font = f;
			PenColor = Color.Black;
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
            //We do not draw button when it is hidden
            if(!Hidden)
            {
				base.Draw(gameTime, spriteBatch);
				//var color = Color.White;

				//if (isHovering)
				//    color = Color.Gray;

				//spriteBatch.Draw(texture, Position, null, color, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
				if (!string.IsNullOrEmpty(Text))
				{
					var x = (Rectangle.X + (Rectangle.Width / 2) - font.MeasureString(Text).X / 2);
					var y = (Rectangle.Y + (Rectangle.Height / 2) - font.MeasureString(Text).Y / 2);

					////Check this (needs a fix)
					//spriteBatch.DrawString(font, Text, new Vector2(x, y), PenColor, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
					spriteBatch.DrawString(font, Text, new Vector2(x, y), PenColor);
				}
			}
		}
		public override void Update(GameTime gameTime)
		{
            //We do not check for clicks etc. when button is hidden
            if(!Hidden)
            {
                previousState = currentState;
                currentState = Mouse.GetState();

                var mouseRectangle = new Rectangle(currentState.X, currentState.Y, 1, 1);

                if (mouseRectangle.Intersects(Rectangle))
                {
                    isHovering = true;
					Color = Color.Gray;

                    if (currentState.LeftButton == ButtonState.Released && previousState.LeftButton == ButtonState.Pressed)
                    {
                        Click?.Invoke(this, new EventArgs());
                    }
                }
				else
				{
					isHovering = false;
					Color = Color.White;
				}
            }
		}
	}
}
