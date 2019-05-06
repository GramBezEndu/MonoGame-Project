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
        List<string> inputKeybindStrings = new List<string>();
        //Will represent max font width (from input keybinds strings)
        Vector2 size = new Vector2(0, 0);
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

            //Make keybinds strings
            foreach (var s in game.Input.KeyBindings)
            {
                inputKeybindStrings.Add(s.Key.ToString());
                Vector2 currentSize = font.MeasureString(s.Key.ToString());
                size.X = Math.Max(size.X, currentSize.X);
            }

            //Make keybinds buttons (need to set correct Y still)
            for (int i = 0; i < inputKeybindStrings.Count; i++)
            {
                staticComponents.Add(new Button(buttonTexture, font, g.Scale)
                {
                    Position = new Vector2(0.255f * game.Width + size.X, 0)
                }
                );
            }
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

            //Make it a list of strings in constructor
			for(int i=0;i<inputKeybindStrings.Count;i++)
			{
                //Should be done with measure string (needs a fix later)
				spriteBatch.DrawString(font, inputKeybindStrings.ElementAt(i), new Vector2(0.255f*game.Width, (i * 0.04f + 0.05f) * game.Height), Color.Black);
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
