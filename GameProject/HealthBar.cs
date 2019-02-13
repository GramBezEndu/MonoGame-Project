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
	public class HealthBar : Sprite
	{
		public Health Health { get; set; }
		//private Texture2D healthTexture;
		//private Rectangle healthRectangle;
		public HealthBar(Texture2D healthBardBorder, Texture2D healthTexture, float scale) : base(healthBardBorder, scale)
		{
			Health = new Health(healthTexture, scale);
			//this.healthTexture = healthTexture;
			//healthRectangle = new Rectangle(0, 0, healthTexture.Width)
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			Health.Position = this.Position;
			Health.Draw(gameTime, spriteBatch);
			base.Draw(gameTime, spriteBatch);
		}
	}
}
