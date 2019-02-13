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
		public Health(Texture2D healthTexture, float scale) : base(healthTexture, scale)
		{
			MaxHealth = 20;
			CurrentHealth = 5;
		}
		//public override Rectangle rectangle
		//{
		//	get { return rectangle; }
		//	set
		//	{

		//	}
		//}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture,
				Position,
				new Rectangle((int)Position.X, (int)Position.Y, (int)(texture.Width * CurrentHealth / MaxHealth), (int)(texture.Height)),
				Color.White,
				0f,
				Vector2.Zero,
				Scale,
				SpriteEffects.None,
				0f);
		}
	}
}
