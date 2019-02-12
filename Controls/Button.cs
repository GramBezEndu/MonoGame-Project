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
		public Button(Texture2D t, SpriteFont f, float scale) : base(t,scale)
		{
			texture = t;
			font = f;
			PenColor = Color.Black;
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			var color = Color.White;

			if (isHovering)
				color = Color.Gray;

			spriteBatch.Draw(texture, Position, null, color, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
			if (!string.IsNullOrEmpty(Text))
			{
				var x = (rectangle.X + (rectangle.Width / 2) - font.MeasureString(Text).X /2);
				var y = (rectangle.Y + (rectangle.Height / 2) - font.MeasureString(Text).Y / 2);

				////Check this (needs a fix)
				//spriteBatch.DrawString(font, Text, new Vector2(x, y), PenColor, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
				spriteBatch.DrawString(font, Text, new Vector2(x, y), PenColor);
			}
		}
		public override void Update(GameTime gameTime)
		{
			previousState = currentState;
			currentState = Mouse.GetState();

			var mouseRectangle = new Rectangle(currentState.X, currentState.Y, 1, 1);

			isHovering = false;

			if(mouseRectangle.Intersects(rectangle))
			{
				isHovering = true;

				if(currentState.LeftButton == ButtonState.Released && previousState.LeftButton == ButtonState.Pressed)
				{
					Click?.Invoke(this, new EventArgs());
				}
			}
		}
	}
}
