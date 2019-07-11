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
		Texture2D SliderFilled;
		Text HowMuchBarFilled;
		float maxValue = 1f;
		float minValue = 0f;
		public float CurrentValue {get; set;} = 0.5f;

		string Percentage;
		SpriteFont Font;
		Input input;

		public Slider(Input i,Texture2D sliderBorder, Texture2D sliderFilled, SpriteFont f, float scale) : base(sliderBorder, scale)
		{
			SliderFilled = sliderFilled;
			Percentage = string.Format("{0}%", (CurrentValue / Math.Abs(maxValue - minValue)) * 100);
			HowMuchBarFilled = new Text(f, Percentage);
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
				HowMuchBarFilled.Position = new Vector2(this.Position.X + this.Width / 2 - Font.MeasureString(Percentage).X/2, this.Position.Y);
				HowMuchBarFilled.Draw(gameTime, spriteBatch);
			}
		}

		public override void Update(GameTime gameTime)
		{
			var mouseRectangle = new Rectangle(input.CurrentMouseState.X, input.CurrentMouseState.Y, 1, 1);
			if (mouseRectangle.Intersects(Rectangle))
			{
				//User clicked somewhere on slider
				if(input.CurrentMouseState.LeftButton == ButtonState.Released && input.PreviousMouseState.LeftButton == ButtonState.Pressed)
				{

				}
			}
		}
	}
}
