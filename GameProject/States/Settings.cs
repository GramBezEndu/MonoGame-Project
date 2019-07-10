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
	public class Settings : State
	{
		SpriteFont font;
		List<Button> keybindsButtons = new List<Button>();
        List<string> inputKeybindStrings = new List<string>();
		/// <summary>
		/// Represents if any key (and which one) is waiting to be binded
		/// </summary>
		List<bool> bindingNow;

        ///Represents max font width (from input keybinds strings), so buttons can be placed correctly
        Vector2 size = new Vector2(0, 0);
        public Settings(Game1 g, GraphicsDevice gd, ContentManager c) : base(g, gd, c)
		{
			var background = content.Load<Texture2D>("Background");
			var buttonTexture = content.Load<Texture2D>("Button");
			var buttonFont = content.Load<SpriteFont>("Font");
			var settingsTexture = content.Load<Texture2D>("BackgroundBig");
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
                    Position = new Vector2(0.55f*game.Width, 0.6f*game.Height)
                }
            };

            //Make keybinds strings
            foreach (var s in game.Input.KeyBindings)
            {
                inputKeybindStrings.Add(s.Key.ToString());
                Vector2 currentSize = font.MeasureString(s.Key.ToString());
                size.X = Math.Max(size.X, currentSize.X);
            }

            var tempButton = new Button(buttonTexture, font, g.Scale);

            //Make keybinds buttons (need to set correct Y still)
            for (int i = 0; i < game.Input.KeyBindings.Count; i++)
            {
                keybindsButtons.Add(new Button(buttonTexture, font, g.Scale)
                {
                    Position = new Vector2(0.255f * game.Width + size.X, 0.033f * game.Height + i*tempButton.Height),
                    Text = game.Input.KeyBindings.ElementAt(i).Value.ToString(),
					Click = ChangeKeybind
                }
                );
            }

			//Make a list of bools (default false)
			bindingNow = new List<bool>(new bool[game.Input.KeyBindings.Count]);

			staticComponents.AddRange(keybindsButtons);

			//Add a restore to defaults button
			staticComponents.Add(new Button(buttonTexture, font, game.Scale)
			{
				Position = new Vector2(0.7f * g.Width, 0.9f * g.Height),
				Text = "Restore to defaults",
				Click = RestoreToDefaults
			}
			);
		}

		private void RestoreToDefaults(object sender, EventArgs e)
		{
			game.Input.RestoreToDefaults();
			for (int i = 0; i < game.Input.KeyBindings.Count; i++)
			{
				//Update strings in keybinds buttons
				keybindsButtons[i].Text = game.Input.KeyBindings.ElementAt(i).Value.ToString();
				//Reset bindingNow list
				bindingNow[i] = false;
			}
		}

		private void ChangeKeybind(object sender, EventArgs e)
		{
			//Check if we can bind now
			if(bindingNow.Contains(true))
			{
				//Other key is being binded now
				return;
			}

			//Set keybind to null on click
			var button = sender as Button;

			int index = keybindsButtons.IndexOf(button);

			if (game.Input.KeyBindings.ContainsKey(inputKeybindStrings[index]))
			{
				game.Input.KeyBindings[inputKeybindStrings[index]] = null;
				bindingNow[index] = true;
				keybindsButtons[index].Text = "Press any key";
			}
			else
				throw new Exception("Keybind not found");
			//while (game.Input.CurrentState.GetPressedKeys().Length == 0)
			//{

			//}
		}

		private void Back(object sender, EventArgs e)
		{
			//If selected keys by player are ok we can go back to Main Menu
			if(ValidateKeys())
			{
				game.ChangeState(new MainMenu(game, graphicsDevice, content));
			}
			//If not, we restore keybindings to default
			else
			{
				RestoreToDefaults(null, null);
				game.ChangeState(new MainMenu(game, graphicsDevice, content));
			}
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, float scale)
		{
			spriteBatch.Begin();
			foreach (var c in staticComponents)
				c.Draw(gameTime, spriteBatch);

            //Iterate over string and draw in correct position (they might be "Text" components, maybe want to change it later)
			for(int i=0;i<inputKeybindStrings.Count;i++)
			{
                //Should be done with on Y with Max(font.MeasureString, button.Y) (needs a fix later)
				spriteBatch.DrawString(font, inputKeybindStrings[i], new Vector2(0.255f*game.Width, 0.05f * game.Height + i * keybindsButtons[i].Height), Color.Black);
			}
			spriteBatch.End();
		}

		/// <summary>
		/// Checks if new keybindings are correct
		/// </summary>
		public bool ValidateKeys()
		{
			foreach(var x in game.Input.KeyBindings)
			{
				if (x.Value == null)
					return false;
			}
			return true;
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
