using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject.Controls
{
	public class TextButton : Text, ICloneable
	{
		Input Input;
		public Color HoveringColor { get; set; } = Color.Red;
		/// <summary>
		/// Contains selected color (while not hovering)
		/// </summary>
		public Color DefaultColor { get; set; } = Color.White;
		bool IsHovering;
		public EventHandler Click { get; set; }
		public Rectangle Rectangle
		{
			get
			{
				return new Rectangle((int)Position.X, (int)Position.Y, (int)(Width), (int)(Height));
			}
		}
		public TextButton(Input i, SpriteFont font, string message) : base(font, message)
		{
			Input = i;
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (!Hidden)
			{
				if(IsHovering)
					spriteBatch.DrawString(Font, Message, Position, HoveringColor);
				else
					spriteBatch.DrawString(Font, Message, Position, Color);
			}
		}

		public override void Update(GameTime gameTime)
		{
			if(!Hidden)
			{
				base.Update(gameTime);
				var mouseRectangle = new Rectangle(Input.CurrentMouseState.X, Input.CurrentMouseState.Y, 1, 1);

				IsHovering = false;

				if (mouseRectangle.Intersects(Rectangle))
				{
					IsHovering = true;
					if (Input.CurrentMouseState.LeftButton == ButtonState.Released && Input.PreviousMouseState.LeftButton == ButtonState.Pressed)
					{
						Click?.Invoke(this, new EventArgs());
					}
				}
			}
		}

		public object Clone()
		{
			return MemberwiseClone();
		}
	}
}
