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
	public class Slider : Sprite
	{
		public EventHandler Click { get; set; }
		Texture2D SliderFilled;
		Text HowMuchBarFilled;
		float maxValue = 1f;
		float minValue = 0f;
		public float CurrentValue
		{
			get => _currentValue;
			set
			{
				_currentValue = value;
				SetPercentage();
				HowMuchBarFilled.Message = GetPercentage();
			}
		}
		private float _currentValue;

		private string percentage;
		public string GetPercentage()
		{
			return percentage;
		}

		public void SetPercentage()
		{
			percentage = string.Format("{0}%", (CurrentValue / Math.Abs(maxValue - minValue)) * 100);
		}


		SpriteFont Font;
		Input input;

		public Slider(Input i, Texture2D sliderBorder, Texture2D sliderFilled, SpriteFont f, float scale) : base(sliderBorder, scale)
		{
			SliderFilled = sliderFilled;
			SetPercentage();
			HowMuchBarFilled = new Text(f, percentage);
			Font = f;
			input = i;
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (!Hidden)
			{
				//Draws border
				base.Draw(gameTime, spriteBatch);
				//Draws how much slider is filled
				spriteBatch.Draw(SliderFilled,
					Position,
					new Rectangle(0, 0, (int)(SliderFilled.Width * CurrentValue / Math.Abs(maxValue - minValue)), SliderFilled.Height),
					Color.White,
					0f,
					Vector2.Zero,
					Scale,
					SpriteEffects.None,
					0f);
				//Writes % how much slider is filled
				HowMuchBarFilled.Position = new Vector2(this.Position.X + this.Width / 2 - Font.MeasureString(GetPercentage()).X / 2, this.Position.Y);
				HowMuchBarFilled.Draw(gameTime, spriteBatch);
			}
		}

		public override void Update(GameTime gameTime)
		{
			var mouseRectangle = new Rectangle(input.CurrentMouseState.X, input.CurrentMouseState.Y, 1, 1);
			if (mouseRectangle.Intersects(Rectangle))
			{
				Vector2 mousePos = new Vector2(input.CurrentMouseState.X, input.CurrentMouseState.Y);
				//Left mouse button is pressed
				if (input.CurrentMouseState.LeftButton == ButtonState.Pressed)
				{
					Vector2 positionRelated = mousePos - this.Position;
					int width = (int)positionRelated.X;

					CurrentValue = (float)Math.Round(((float)width / this.Width), 2);
					Click?.Invoke(this, new EventArgs());
				}
			}
		}
	}
}
