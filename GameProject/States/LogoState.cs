using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameProject.Sprites;
using System.Diagnostics;

namespace GameProject.States
{
	public class LogoState : State
	{
		GameTimer stateTimer;
		public LogoState(Game1 g, GraphicsDevice gd, ContentManager c) : base(g, gd, c)
		{
			staticComponents = new List<Component>()
			{
				new Sprite(Textures["Logo"], g.Scale)
				{
					Position = new Vector2(g.Width/2, g.Height/2)
				}
			};
			stateTimer = new GameTimer(2.5f);

			if (Debugger.IsAttached)
				EnableViewingStaticComponentsRectangle(gd);
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Begin();
			foreach (var c in staticComponents)
				c.Draw(gameTime, spriteBatch);
			spriteBatch.End();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if(stateTimer.Enabled == false)
			{
				stateTimer.Start();
			}
			foreach (var c in staticComponents)
				c.Update(gameTime);
			stateTimer.Update(gameTime);
			if(stateTimer.CurrentTime <= 0)
			{
				Game.ChangeState(new MainMenu(Game, graphicsDevice, Game.Content));
			}
		}

		public override void PostUpdate()
		{
			//
		}
	}
}
