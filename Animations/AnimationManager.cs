using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject.Animations
{
	public class AnimationManager
	{
		private Animation animation;
		private float millisecondsTimer;
		public Vector2 Position { get; set; }
		public AnimationManager(Animation a)
		{
			animation = a;
		}
		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(
				animation.Texture,
				Position,
				new Rectangle(animation.CurrentFrame * animation.FrameWidth, 0, animation.FrameWidth, animation.FrameHeight),
				Color.White);
		}
		public void Play(Animation a)
		{
			if (a == animation)
				return;
			animation = a;
			animation.CurrentFrame = 0;
			millisecondsTimer = 0f;
		}
		public void Stop()
		{
			millisecondsTimer = 0f;
			animation.CurrentFrame = 0;
		}
		public void Update(GameTime gameTime)
		{
			millisecondsTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (millisecondsTimer > animation.FrameTime)
			{
				millisecondsTimer = 0f;
				animation.CurrentFrame++;

				if(animation.CurrentFrame >= animation.FrameCount)
				{
					animation.CurrentFrame = 0;
				}
			}
		}
	}
}
