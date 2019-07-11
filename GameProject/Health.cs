using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject.Sprites;

namespace GameProject
{
	public class Health : Sprite
	{
		public int MaxHealth { get; set; }
		public int CurrentHealth { get; set; }
		private SpriteFont font;
		public Health(Texture2D healthTexture, SpriteFont f, float scale) : base(healthTexture, scale)
		{
			font = f;
			MaxHealth = 20;
			CurrentHealth = MaxHealth;
		}
		//public override Rectangle Rectangle
		//{
		//	get { return Rectangle; }
		//	set
		//	{

		//	}
		//}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture,
				Position,
				new Rectangle(0, 0, (int)(texture.Width * CurrentHealth / MaxHealth), texture.Height),
				//new Rectangle((int)Position.X, (int)Position.Y, (int)(texture.Width * CurrentHealth / MaxHealth), (int)(texture.Height)),
				Color.White,
				0f,
				Vector2.Zero,
				Scale,
				SpriteEffects.None,
				0f);
			//Maybe this should be in update, but w/e
			string healthString = String.Format("{0}/{1}", CurrentHealth, MaxHealth);
			spriteBatch.DrawString(font, healthString, Position, Color.White);
		}
	}
}
