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
	/// </summary>
	public class MovingText : Text
	{
		//GameTimer moveTime = new GameTimer(0.1f);
		float distanceBase;
		public MovingText(Game1 g, SpriteFont font, string message) : base(font, message)
		{
			distanceBase = 5f * g.Scale;
		}

		public override void Update(GameTime gameTime)
		{
			if(!Hidden)
			{
				base.Update(gameTime);
				Position += new Vector2(0, -distanceBase);
			}
		}
	}
}
