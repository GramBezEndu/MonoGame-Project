using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using GameProject.Controls;
using GameProject.Sprites;

namespace GameProject.States
{
	public class MainMenu : State
	{
		public MainMenu(Game1 g, GraphicsDevice gd, ContentManager c) : base(g, gd, c)
		{
			var background = content.Load<Texture2D>("Background");
			var buttonTexture = content.Load<Texture2D>("Button");

			var test = new Text(Font, "Test String");

			staticComponents = new List<Component>
			{
				new Sprite(background, g.Scale)
				{
					Position = new Vector2(0,0)
				},
				new Button(buttonTexture, Font, g.Scale)
				{
					Text = "Play",
					Position = new Vector2(0.01f*g.Width, 0.6f*g.Height),
					Click = PlayClick
				},
				new Button(buttonTexture, Font ,g.Scale)
				{
					Text = "Settings",
					Position = new Vector2(0.01f*g.Width, 0.7f*g.Height),
					Click = SettingsState
				},
				new Button(buttonTexture, Font ,g.Scale)
				{
					Text = "Credits",
					Position = new Vector2(0.01f*g.Width, 0.8f*g.Height),
					Click = CreditsState
				},
				new Button(buttonTexture, Font, g.Scale)
				{
				Text = "Exit",
				Position = new Vector2(0.01f * g.Width, 0.9f * g.Height),
				Click = QuitGame
				},
			};
			g.ChangeBackgroundSong(Songs["MainMenu"]);
		}

		private void CreditsState(object sender, EventArgs e)
		{
			Game.ChangeState(new Credits(Game, graphicsDevice, content));
		}

		private void SettingsState(object sender, EventArgs e)
		{
			Game.ChangeState(new Settings(Game, graphicsDevice, content));
		}

		private void PlayClick(object sender, EventArgs e)
		{
			Game.ChangeState(new Play(Game, graphicsDevice, content));
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
		private void QuitGame(object sender, EventArgs e)
		{
			Game.Exit();
		}
	}
}
