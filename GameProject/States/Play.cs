﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Controls;
using GameProject.Sprites;

namespace GameProject.States
{
	public class Play : State
	{
		public Play(Game1 g, GraphicsDevice gd, ContentManager c) : base(g, gd, c)
		{
			var background = content.Load<Texture2D>("Background");
			var buttonTexture = content.Load<Texture2D>("Button");

			staticComponents = new List<Component>
			{
				new Sprite(background, g.Scale)
				{
					Position = new Vector2(0,0)
				},
				new Button(buttonTexture, Font,g.Scale)
				{
					Text = "New Game",
					Position = new Vector2(0.01f*g.Width, 0.6f*g.Height),
					Click = NewGameClick
				},
				new Button(buttonTexture, Font, g.Scale)
				{
					Text = "Load Game",
					Position = new Vector2(0.01f*g.Width, 0.7f*g.Height),
					Click = LoadGameClick
				},
				new Button(buttonTexture, Font, g.Scale)
				{
					Text = "Delete Game",
					Position = new Vector2(0.01f*g.Width, 0.8f*g.Height),
					Click = DeleteGameClick
				},
				new Button(buttonTexture, Font, g.Scale)
				{
					Text = "Back",
					Position = new Vector2(0.01f*g.Width, 0.9f*g.Height),
					Click = MainMenuState
				}
			};
		}

		private void DeleteGameClick(object sender, EventArgs e)
		{
			CreateMessage("Option unavailable");
		}

		private void LoadGameClick(object sender, EventArgs e)
		{
			CreateMessage("Option unavailable");
		}

		private void MainMenuState(object sender, EventArgs e)
		{
			Game.ChangeState(new MainMenu(Game, graphicsDevice, content));
		}

		private void NewGameClick(object sender, EventArgs e)
		{
			Game.ChangeState(new ClassSelect(Game, graphicsDevice, content));
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
