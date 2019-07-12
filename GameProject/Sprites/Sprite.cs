using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject.Animations;

namespace GameProject.Sprites
{
	/// <summary>
	/// Store and draw object
	/// </summary>
	public class Sprite : Component
	{
		protected Texture2D texture;
		public AnimationManager animationManager { get; protected set; }
		public Dictionary<string, Animation> animations { get; protected set; }
		protected Vector2 position;
		public Vector2 Velocity;
		public bool FlipHorizontally { get; set; }
		//protected Vector2 position;
		public float Scale { get; set; }
		//public Vector2 Velocity { get; set; }
		public Sprite(Texture2D t, float scale)
		{
			texture = t;
			Scale = scale;
		}
		/// <summary>
		/// Clone dictionary (used for cloning animations [every sprite needs to have own animations due to OnAnimationEnd function])
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="original"></param>
		/// <returns></returns>
		public static Dictionary<TKey, TValue> CloneDictionaryCloningValues<TKey, TValue>
   (Dictionary<TKey, TValue> original) where TValue : ICloneable
		{
			Dictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>(original.Count,
																	original.Comparer);
			foreach (KeyValuePair<TKey, TValue> entry in original)
			{
				ret.Add(entry.Key, (TValue)entry.Value.Clone());
			}
			return ret;
		}
		public Sprite(Dictionary<string,Animation> a)
		{
			animations = a.ToDictionary(x => x.Key, x => (Animation)x.Value.Clone());
			animationManager = new AnimationManager(a.First().Value);
            //Set scale even if there are animations (the scale will equal scale of first animation, so you should provide same scale for every animation)
            Scale = animations.First().Value.Scale;
		}
		public Vector2 Position
		{
			get { return position; }
			set
			{
				position = value;
				if (animationManager != null)
					animationManager.Position = value;
			}
		}
        /// <summary>
        /// Returns sprite Height (includes scaling)
        /// </summary>
		public int Height
		{
			get
			{
				if (texture != null)
					return (int)(texture.Height * Scale);
				else if (animationManager != null)
					return (int)(animationManager.animation.FrameHeight * animationManager.animation.Scale);
				else throw new Exception("Invalid sprite");
			}
		}
        /// <summary>
        /// Returns sprite Width (includes scaling)
        /// </summary>
		public int Width
		{
			get
			{
				if (texture != null)
					return (int)(texture.Width * Scale);
				else if (animationManager != null)
					return (int)(animationManager.animation.FrameWidth * animationManager.animation.Scale);
				else throw new Exception("Invalid sprite");
			}
		}

		/// <summary>
		/// Returns the object Rectangle (includes scaling)
		/// </summary>
		public Rectangle Rectangle
		{
			get
			{
				if (texture != null)
					return new Rectangle((int)Position.X, (int)Position.Y, (int)(texture.Width * Scale), (int)(texture.Height * Scale));
				else if (animationManager != null)
					return new Rectangle((int)Position.X, (int)Position.Y, (int)(animationManager.animation.FrameWidth * animationManager.animation.Scale), (int)(animationManager.animation.FrameHeight * animationManager.animation.Scale));
				else throw new Exception("Invalid rectangle sprite");
			}
		}
		public override void Update(GameTime gameTime)
		{
			//regular sprite does not require any update

			//Animated sprites -> check if sprite is should be flipped horizontally
			if(animationManager != null)
			{
				animationManager.FlipHorizontally = this.FlipHorizontally;
			}
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if(!Hidden)
			{
				if (texture != null)
				{
					if(FlipHorizontally)
						spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.FlipHorizontally, 0f);
					else
						spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
				}
				else if (animationManager != null)
					animationManager.Draw(spriteBatch);
				else
					throw new Exception("Invalid sprite");
				//spriteBatch.Draw()
			}
		}

		#region Collision
		protected bool IsTouchingLeft(Sprite sprite)
		{
			return this.Rectangle.Right + this.Velocity.X > sprite.Rectangle.Left &&
			  this.Rectangle.Left < sprite.Rectangle.Left &&
			  this.Rectangle.Bottom > sprite.Rectangle.Top &&
			  this.Rectangle.Top < sprite.Rectangle.Bottom;
		}

		protected bool IsTouchingRight(Sprite sprite)
		{
			return this.Rectangle.Left + this.Velocity.X < sprite.Rectangle.Right &&
			  this.Rectangle.Right > sprite.Rectangle.Right &&
			  this.Rectangle.Bottom > sprite.Rectangle.Top &&
			  this.Rectangle.Top < sprite.Rectangle.Bottom;
		}

		protected bool IsTouchingTop(Sprite sprite)
		{
			return this.Rectangle.Bottom + this.Velocity.Y > sprite.Rectangle.Top &&
			  this.Rectangle.Top < sprite.Rectangle.Top &&
			  this.Rectangle.Right > sprite.Rectangle.Left &&
			  this.Rectangle.Left < sprite.Rectangle.Right;
		}

		protected bool IsTouchingBottom(Sprite sprite)
		{
			return this.Rectangle.Top + this.Velocity.Y < sprite.Rectangle.Bottom &&
			  this.Rectangle.Bottom > sprite.Rectangle.Bottom &&
			  this.Rectangle.Right > sprite.Rectangle.Left &&
			  this.Rectangle.Left < sprite.Rectangle.Right;
		}

		public bool IsTouching(Sprite sprite)
		{
            //We use Intersects method here, we do not care how we are touching the object
            //Intersects also checks if rectangle is inside other rectangle
            //and we do not have a method for it
            if (this.Rectangle.Intersects(sprite.Rectangle))
                return true;
            else
                return false;
			//if (IsTouchingRight(sprite))
			//	return true;
			//if (IsTouchingLeft(sprite))
			//	return true;
			//if (IsTouchingTop(sprite))
			//	return true;
			//if (IsTouchingBottom(sprite))
			//	return true;
			//else
			//	return false;
		}

		#endregion
	}
}
