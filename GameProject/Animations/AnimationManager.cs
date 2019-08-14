using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject.Sprites;

namespace GameProject.Animations
{
	public class AnimationManager
	{
		Sprite Sprite;
		public Animation animation;
		private float millisecondsTimer;
		public Vector2 Position { get; set; }
		/// <summary>
		/// If not declared, animation manager will play first animation from dictionary
		/// </summary>
		/// <param name="a"></param>
		public AnimationManager(Sprite spriteReference, Animation a)
		{
			animation = a;
			Sprite = spriteReference;
		}
		public void Draw(SpriteBatch spriteBatch)
		{
			if(Sprite.FlipHorizontally)
			{
				spriteBatch.Draw(animation.Texture,
				Position,
				new Rectangle(animation.CurrentFrame * animation.FrameWidth,
					0,
					animation.FrameWidth,
					animation.FrameHeight),
				Sprite.Color,
				animation.Rotation,
				animation.Origin,
				animation.Scale,
				SpriteEffects.FlipHorizontally,
				0f);
			}
			else
			{
				spriteBatch.Draw(animation.Texture,
				Position,
				new Rectangle(animation.CurrentFrame * animation.FrameWidth,
					0,
					animation.FrameWidth,
					animation.FrameHeight),
				Sprite.Color,
				animation.Rotation,
				animation.Origin,
				animation.Scale,
				SpriteEffects.None,
				0f);
			}

		}
		public void Play(Animation a)
		{
			if (a == animation)
				return;
			//The sprite's rectangle changed
			if (Sprite.ShowRectangle && (a.FrameWidth != animation.FrameWidth || a.FrameHeight != animation.FrameHeight))
			{
				animation = a;
				Sprite.SetSpriteRectangle();
			}
			else
			{
				animation = a;
			}
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
					animation.OnAnimationEnd?.Invoke(animation, new EventArgs());
				}
			}
		}
	}
}
