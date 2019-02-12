using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace GameProject
{
	public abstract class State
	{
		protected ContentManager content;
		protected GraphicsDevice graphicsDevice;
		protected Game1 game;
		protected List<Component> components;
		public State(Game1 g, GraphicsDevice gd, ContentManager c)
		{
			content = c;
			game = g;
			graphicsDevice = gd;
		}
		public abstract void Update(GameTime gameTime);
		public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch, float scale);
		/// <summary>
		/// Remove not needed resources
		/// </summary>
		public abstract void PostUpdate();
	}
}
