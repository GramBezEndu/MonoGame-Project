using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameProject.Controls;
using GameProject.Sprites;

namespace GameProject.States
{
	public class Credits : State
	{
		public Credits(Game1 g, GraphicsDevice gd, ContentManager c) : base(g, gd, c)
		{
			var background = content.Load<Texture2D>("Background");
			var buttonTexture = content.Load<Texture2D>("Button");
			var buttonFont = content.Load<SpriteFont>("Font");

			staticComponents = new List<Component>();

			staticComponents.Add(new Sprite(background, g.Scale)
			{
				Position = new Vector2(g.Width / 2, g.Height / 2)
			});

			var back = new Button(buttonTexture, buttonFont, g.Scale)
			{
				Text = "Back",
				Click = MainMenuState
			};

			var interval = new Vector2(0, 0.02f * g.Height);

			Vector2 pos = new Vector2(0.07f * g.Width, 0.63f * g.Height + 3* interval.Y + 3* back.Height);

			back.Position = new Vector2(pos.X, pos.Y);

			staticComponents.Add(back);
		}

		private void MainMenuState(object sender, EventArgs e)
		{
			Game.ChangeState(new MainMenu(Game, graphicsDevice, content));
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Begin();
			foreach (var c in staticComponents)
				c.Draw(gameTime, spriteBatch);
			spriteBatch.End();
		}

		public override void PostUpdate()
		{
			//throw new NotImplementedException();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			foreach (var c in staticComponents)
				c.Update(gameTime);
		}
	}
}
