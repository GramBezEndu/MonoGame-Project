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
	/// Text moving up
	/// Note: These objects are not being delted in State class now when they go off screen
	/// </summary>
	public class MovingText : Text
	{
		//GameTimer moveTime = new GameTimer(0.1f);
		public float BaseDistance { get; set; }
		public MovingText(Game1 g, SpriteFont font, string message) : base(font, message)
		{
			BaseDistance = 5f * g.Scale;
		}

		public override void Update(GameTime gameTime)
		{
			if(!Hidden)
			{
				base.Update(gameTime);
				Position += new Vector2(0, -BaseDistance);
			}
		}
	}
}
