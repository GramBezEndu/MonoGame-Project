using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Controls;

namespace GameProject
{
	public class SpeedrunTimer : Component
	{
		/// <summary>
		/// Current time in milliseconds
		/// </summary>
		public float CurrentTime { get; set; }
		public Vector2 Position { get; set; }

		SpriteFont font;
		Text timer;

		public SpeedrunTimer(SpriteFont f)
		{
			font = f;
			timer = new Text(f, CurrentTime.ToString())
			{
				Position = new Vector2(0, 0)
			};
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if(!Hidden)
				timer.Draw(gameTime, spriteBatch);
		}

		public override void Update(GameTime gameTime)
		{
			if(!Hidden)
			{
				CurrentTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
				//Update timer string
				timer.Message = CurrentTime.ToString();
				timer.Update(gameTime);
			}
		}
	}
}
