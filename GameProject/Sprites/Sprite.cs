﻿using System;
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
		public Color Color { get; set; } = Color.White;
		public bool FlipHorizontally { get; set; }
		//protected Vector2 position;
		public Vector2 Scale { get; set; }

		public bool ShowRectangle { get; set; }
		protected GraphicsDevice graphicsDevice;
		protected Texture2D rectangleTexture;
		protected Vector2 rectangleOrigin;

		protected float rotation = 0f;
		public Vector2 Origin;

		//public Vector2 Velocity { get; set; }
		public Sprite(Texture2D t, Vector2 scale)
		{
			texture = t;
			Scale = scale;
			Origin = new Vector2(texture.Width / 2, texture.Height / 2);
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
			animationManager = new AnimationManager(this, a.First().Value);
            //Set scale even if there are animations (the scale will equal scale of first animation, so you should provide same scale for every animation)
            Scale = animations.First().Value.Scale;
		}

		/// <summary>
		/// Call this method if you want to draw sprite's rectnagle
		/// </summary>
		public void EnableSpriteRectangleDrawing(GraphicsDevice gd)
		{
			graphicsDevice = gd;
			SetSpriteRectangle();
			ShowRectangle = true;
		}

		private void SetSpriteRectangle()
		{
			var data = new List<Color>();

			if (texture != null)
			{
				for (int y = 0; y < texture.Height; y++)
				{
					for (int x = 0; x < texture.Width; x++)
					{
						if (y == 0 || // On the top
							x == 0 || // On the left
							y == texture.Height - 1 || // on the bottom
							x == texture.Width - 1) // on the right
						{
							data.Add(Color.Red); // white
						}
						else
						{
							data.Add(Color.Transparent);
						}
					}
				}

				rectangleTexture = new Texture2D(graphicsDevice, texture.Width, texture.Height);
			}
			else if (animationManager != null)
			{
				for (int y = 0; y < animationManager.animation.FrameHeight; y++)
				{
					for (int x = 0; x < animationManager.animation.FrameWidth; x++)
					{
						if (y == 0 || // On the top
							x == 0 || // On the left
							y == animationManager.animation.FrameHeight - 1 || // on the bottom
							x == animationManager.animation.FrameWidth - 1) // on the right
						{
							data.Add(Color.Red); // white
						}
						else
						{
							data.Add(Color.Transparent);
						}
					}
				}

				rectangleTexture = new Texture2D(graphicsDevice, animationManager.animation.FrameWidth, animationManager.animation.FrameHeight);
			}
			rectangleTexture.SetData<Color>(data.ToArray());
			rectangleOrigin = new Vector2(rectangleTexture.Width/2, rectangleTexture.Height/2);
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
					return (int)(texture.Height * Scale.Y);
				else if (animationManager != null)
					return (int)(animationManager.animation.FrameHeight * animationManager.animation.Scale.Y);
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
					return (int)(texture.Width * Scale.X);
				else if (animationManager != null)
					return (int)(animationManager.animation.FrameWidth * animationManager.animation.Scale.X);
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
					return new Rectangle((int)(Position.X - (Origin.X *Scale.X)), (int)(Position.Y - (Origin.Y * Scale.Y)), (int)(texture.Width * Scale.X), (int)(texture.Height * Scale.Y));
				else if (animationManager != null)
					return new Rectangle((int)(Position.X - (animationManager.animation.Origin.X * animationManager.animation.Scale.X)), (int)(Position.Y - (animationManager.animation.Origin.Y * animationManager.animation.Scale.Y)), (int)(animationManager.animation.FrameWidth * animationManager.animation.Scale.X), (int)(animationManager.animation.FrameHeight * animationManager.animation.Scale.Y));
				else throw new Exception("Invalid rectangle sprite");
			}
		}
		public override void Update(GameTime gameTime)
		{
			//regular sprite does not require any update
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if(!Hidden)
			{
				//Sprite section
				if (texture != null)
				{
					if(FlipHorizontally)
						spriteBatch.Draw(texture, Position, null, Color, rotation, Origin, Scale, SpriteEffects.FlipHorizontally, 0f);
					else
						spriteBatch.Draw(texture, Position, null, Color, rotation, Origin, Scale, SpriteEffects.None, 0f);
				}
				else if (animationManager != null)
				{
					animationManager.Draw(spriteBatch);
				}
				else
					throw new Exception("Invalid sprite");
				//Rectangle section
				if(ShowRectangle)
				{
					spriteBatch.Draw(rectangleTexture, Position, null, Color.White, rotation, rectangleOrigin, Scale, SpriteEffects.None, 0f);
				}
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
		}

		#endregion
	}
}
