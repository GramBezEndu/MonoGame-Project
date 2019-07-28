using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject
{
	public class StaminaBar : Sprite
	{
		public Stamina Stamina { get; set; }
		public StaminaBar(Texture2D staminaBorderTexture, Texture2D staminaTexture, SpriteFont font, Vector2 position, Vector2 scale) : base(staminaBorderTexture, scale)
		{
			Position = position;
			Stamina = new Stamina(staminaTexture, font, scale)
			{
				Position = position
			};
			//this.healthTexture = healthTexture;
			//healthRectangle = new Rectangle(0, 0, healthTexture.Width)
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			//Health.Position = this.Position;
			Stamina.Draw(gameTime, spriteBatch);
			base.Draw(gameTime, spriteBatch);
		}
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}
	}
}
