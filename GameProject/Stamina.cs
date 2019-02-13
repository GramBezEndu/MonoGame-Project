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
	public class Stamina : Sprite
	{
		public int MaxStamina { get; set; }
		public int CurrentStamina { get; set; }
		public Stamina(Texture2D healthTexture, float scale) : base(healthTexture, scale)
		{
			MaxStamina = 50;
			CurrentStamina = 21;
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture,
				Position,
				new Rectangle((int)Position.X, (int)Position.Y, (int)(texture.Width * CurrentStamina / MaxStamina), (int)(texture.Height)),
				Color.White,
				0f,
				Vector2.Zero,
				Scale,
				SpriteEffects.None,
				0f);
		}
	}
}
