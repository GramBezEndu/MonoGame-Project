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

			staticComponents = new List<Component>
			{
				new Sprite(background, g.Scale)
				{
					Position = new Vector2(0,0)
				},
				new Button(buttonTexture,buttonFont,g.Scale)
				{
					Text = "Back",
					Position = new Vector2(0.01f*g.Width, 0.9f*g.Height),
					Click = MainMenuState
				}
			};
		}

		private void MainMenuState(object sender, EventArgs e)
		{
			game.ChangeState(new MainMenu(game, graphicsDevice, content));
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
