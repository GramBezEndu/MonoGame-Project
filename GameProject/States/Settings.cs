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
	public class Settings : State
	{
		SpriteFont font;
		public Settings(Game1 g, GraphicsDevice gd, ContentManager c) : base(g, gd, c)
		{
			var background = content.Load<Texture2D>("Background");
			var buttonTexture = content.Load<Texture2D>("Button");
			var buttonFont = content.Load<SpriteFont>("Font");
			var settingsTexture = content.Load<Texture2D>("SettingsBackground");
			var keyBorderTexture = content.Load<Texture2D>("SettingsBorder");
            var c1 = content.Load<Texture2D>("Checkbox");
            var c2 = content.Load<Texture2D>("CheckboxChecked");
			font = content.Load<SpriteFont>("Font");

			staticComponents = new List<Component>
			{
				new Sprite(background, g.Scale)
				{
					Position = new Vector2(0,0)
				},
				new Sprite(settingsTexture, g.Scale)
				{
					Position = new Vector2(1f/4f*game.Width, 0.03f*game.Height)
				},
				new Button(keyBorderTexture, buttonFont, g.Scale)
				{
					Position = new Vector2(0.25f*game.Width, 0.03f*game.Height)
				},
				new Button(buttonTexture, buttonFont, g.Scale)
				{
				Text = "Back",
				Position = new Vector2(0.01f * g.Width, 0.9f * g.Height),
				Click = Back
				},
                new Checkbox(c1, c2, g.Scale)
                {
                    Position = new Vector2(0.25f*game.Width, 0.6f*game.Height)
                }
			};
		}

		private void Back(object sender, EventArgs e)
		{
			game.ChangeState(new MainMenu(game, graphicsDevice, content));
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, float scale)
		{
			spriteBatch.Begin();
			foreach (var c in staticComponents)
				c.Draw(gameTime, spriteBatch);
			for(int i=0;i<game.Input.KeyBindings.Count;i++)
			{
				spriteBatch.DrawString(font, game.Input.KeyBindings.ElementAt(i).ToString(), new Vector2(game.Width/2, (i * 0.04f + 0.05f) * game.Height), Color.Black);
			}
			spriteBatch.End();
		}

		public override void PostUpdate()
		{
			//throw new NotImplementedException();
		}

		public override void Update(GameTime gameTime)
		{
			foreach (var c in staticComponents)
				c.Update(gameTime);
		}
	}
}
