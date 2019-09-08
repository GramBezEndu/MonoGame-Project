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

		protected float rotation = 0f;
		public Vector2 Origin = Vector2.Zero;

		//public Vector2 Velocity { get; set; }
		public Sprite(Texture2D t, Vector2 scale)
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

		public void Rotate(int degrees)
		{
			if (texture != null)
			{
				Origin = new Vector2(Width / 2, Height / 2);
				rotation = (float)((Math.PI / 180) * degrees);
			}
			else if (animationManager != null)
			{
				animationManager.animation.Origin = new Vector2(Width / 2, Height / 2);
				animationManager.animation.Rotation = (float)((Math.PI / 180) * degrees);
				Origin = animationManager.animation.Origin;
				rotation = animationManager.animation.Rotation;
			}
			else
				throw new Exception("Invalid sprite");
		}

		public void SetSpriteRectangle()
		{
			var data = new List<Color>();

			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					if (y == 0 || // On the top
						x == 0 || // On the left
						y == Height - 1 || // on the bottom
						x == Width - 1) // on the right
					{
						data.Add(Color.Red);
					}
					else
					{
						data.Add(Color.Transparent);
					}
				}
			}

			rectangleTexture = new Texture2D(graphicsDevice, Width, Height);
			rectangleTexture.SetData<Color>(data.ToArray());
		}

        protected Texture2D SetRectangleTextureForTexture(Rectangle rectangle)
        {
            var data = new List<Color>();

            for (int y = 0; y < rectangle.Height; y++)
            {
                for (int x = 0; x < rectangle.Width; x++)
                {
                    if (y == 0 || // On the top
                        x == 0 || // On the left
                        y == rectangle.Height - 1 || // on the bottom
                        x == rectangle.Width - 1) // on the right
                    {
                        data.Add(Color.Red);
                    }
                    else
                    {
                        data.Add(Color.Transparent);
                    }
                }
            }

            var rectangleText = new Texture2D(graphicsDevice, rectangle.Width, rectangle.Height);
            rectangleText.SetData<Color>(data.ToArray());
            return rectangleText;
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
					return new Rectangle((int)(Position.X - Origin.X), (int)(Position.Y - Origin.Y), (int)(texture.Width * Scale.X), (int)(texture.Height * Scale.Y));
				else if (animationManager != null)
					return new Rectangle((int)(Position.X - Origin.X), (int)(Position.Y - Origin.Y), (int)(animationManager.animation.FrameWidth * animationManager.animation.Scale.X), (int)(animationManager.animation.FrameHeight * animationManager.animation.Scale.Y));
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
					spriteBatch.Draw(rectangleTexture, Position, null, Color.White, rotation, Origin, new Vector2(1f, 1f), SpriteEffects.None, 0f);
				}
			}
		}

		#region Collision
        //Collision on rotated sprite's is not implemented
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
