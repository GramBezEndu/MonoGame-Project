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
		public Vector2 Position;
		//protected Vector2 position;
		public float Scale { get; set; }
		//public Vector2 Velocity { get; set; }
		public Sprite(Texture2D t, float scale)
		{
			texture = t;
			Scale = scale;
		}
		//public Vector2 Position
		//{
		//	get { return position; }
		//	set
		//	{
		//		position = value;
		//		if (animationManager != null)
		//			animationManager.Position = value;
		//	}
		//}
		/// <summary>
		/// Returns the object rectangle (includes scaling)
		/// </summary>
		public Rectangle rectangle
		{
			get { return new Rectangle((int)Position.X, (int)Position.Y, (int)(texture.Width*Scale), (int)(texture.Height*Scale)); }
		}
		public override void Update(GameTime gameTime)
		{
			//regular sprite does not require any update
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
			//spriteBatch.Draw()
		}
	}
}
