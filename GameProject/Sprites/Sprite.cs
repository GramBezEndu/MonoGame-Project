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
		protected AnimationManager animationManager;
		protected Dictionary<string, Animation> animations;
		protected Vector2 position;
		public Vector2 Velocity;
		//protected Vector2 position;
		public float Scale { get; set; }
		//public Vector2 Velocity { get; set; }
		public Sprite(Texture2D t, float scale)
		{
			texture = t;
			Scale = scale;
		}
		public Sprite(Dictionary<string,Animation> a)
		{
			animations = a;
			animationManager = new AnimationManager(a.First().Value);
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
		/// Returns the object rectangle (includes scaling)
		/// </summary>
		public Rectangle rectangle
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
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (texture != null)
				spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
			else if (animationManager != null)
				animationManager.Draw(spriteBatch);
			else
				throw new Exception("Invalid sprite");
			//spriteBatch.Draw()
		}
	}
}
