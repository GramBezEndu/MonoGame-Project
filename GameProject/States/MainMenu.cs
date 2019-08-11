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
using System.Diagnostics;

namespace GameProject.States
{
	public class MainMenu : State
	{
		public MainMenu(Game1 g, GraphicsDevice gd, ContentManager c) : base(g, gd, c)
		{
			//Initialize components list
			staticComponents = new List<Component>();

			AddBackground(g);

			AddButtons(g);

			g.ChangeBackgroundSong(Songs["MainMenu"]);

			if (Debugger.IsAttached)
				EnableViewingStaticComponentsRectangle(gd);
		}

		private void AddButtons(Game1 g)
		{
			Vector2 statingPosition = new Vector2(0.07f * g.Width, 0.63f * g.Height);
			Vector2 buttonsInterval = new Vector2(0, 0.02f * g.Height);

			var playButton = new Button(Textures["Button"], Font, g.Scale)
			{
				Text = "Play",
				Position = statingPosition,
				Click = PlayClick,
			};

			var settingsButton = new Button(Textures["Button"], Font, g.Scale)
			{
				Text = "Settings",
				Position = new Vector2(statingPosition.X, statingPosition.Y + playButton.Height + buttonsInterval.Y),
				Click = SettingsState
			};

			var creditsButton = new Button(Textures["Button"], Font, g.Scale)
			{
				Text = "Credits",
				Position = new Vector2(statingPosition.X, statingPosition.Y + 2* (playButton.Height + buttonsInterval.Y)),
				Click = CreditsState
			};

			var exitButton = new Button(Textures["Button"], Font, g.Scale)
			{
				Text = "Exit",
				Position = new Vector2(statingPosition.X, statingPosition.Y + 3* (playButton.Height + buttonsInterval.Y)),
				Click = QuitGame
			};

			staticComponents.AddRange(new List<Component>()
			{
				playButton,
				settingsButton,
				creditsButton,
				exitButton
			});
		}

		private void AddBackground(Game1 g)
		{
			staticComponents.Add(new Sprite(Textures["Background"], g.Scale)
			{
				Position = new Vector2(g.Width / 2, g.Height / 2)
			});
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
