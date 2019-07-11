using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Controls
{
	/// <summary>
	/// Displays text with background for a limited time
	/// </summary>
	public class Message : Text
	{
		GameTimer displayTimer;
		public Message(GraphicsDevice gd, SpriteFont font, string message) : base(font, message)
		{
			displayTimer = new GameTimer(3f);
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (displayTimer.CurrentTime >= 0)
				base.Draw(gameTime, spriteBatch);
		}

		public override void Update(GameTime gameTime)
		{
			//Enable timer on first update
			if (displayTimer.Enabled == false)
				displayTimer.Start();
			displayTimer.Update(gameTime);
		}
	}
}
