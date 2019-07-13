using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
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
		/// Represents if any key (and which one by the index) is waiting to be binded
		/// </summary>
		List<bool> bindingNow;

		Slider musicVolume;

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
                    Position = new Vector2(1f/4f*Game.Width, 0.03f*Game.Height)
                },
                new Button(buttonTexture, buttonFont, g.Scale)
                {
                Text = "Back",
                Position = new Vector2(0.01f * g.Width, 0.9f * g.Height),
                Click = Back
                },
            };

            //Make keybinds strings
            foreach (var s in Game.Input.KeyBindings)
            {
				string actualString = s.Key.ToString();
				inputKeybindStrings.Add(actualString);
                Vector2 currentSize = font.MeasureString(actualString);
                size.X = Math.Max(size.X, currentSize.X);
            }

			var tempButton = new Button(buttonTexture, font, g.Scale);

            //Make keybinds buttons (need to set correct Y still)
            for (int i = 0; i < Game.Input.KeyBindings.Count; i++)
            {
                keybindsButtons.Add(new Button(buttonTexture, font, g.Scale)
                {
                    Position = new Vector2(0.255f * Game.Width + size.X, 0.033f * Game.Height + i*tempButton.Height),
                    Text = Game.Input.KeyBindings.ElementAt(i).Value.ToString(),
					Click = ChangeKeybind
                }
                );
            }

			//Iterate over strings and make Text components
			for (int i = 0; i < inputKeybindStrings.Count; i++)
			{
				staticComponents.Add(new Text(font, inputKeybindStrings[i])
				{
					Position = new Vector2(0.255f * Game.Width, 0.05f * Game.Height + i * keybindsButtons[i].Height)
				}
				);	
			}

			//Make a list of bools (default false)
			bindingNow = new List<bool>(new bool[Game.Input.KeyBindings.Count]);

			staticComponents.AddRange(keybindsButtons);

			//Add a music volume slider + text component
			var musicVolumeText = new Text(font, "Music volume: ")
			{
				Position = new Vector2(0.5f * g.Width, 0.2f * g.Height)
			};

			staticComponents.Add(musicVolumeText);

			musicVolume = new Slider(Input, Textures["SliderBorder"], Textures["SliderFilled"], font, g.Scale)
			{
				Position = new Vector2(musicVolumeText.Position.X + musicVolumeText.Width, musicVolumeText.Position.Y),
				Click = ChangeMusicVolume,
				CurrentValue = MediaPlayer.Volume
			};

			staticComponents.Add(musicVolume);

			//Add a restore to defaults button
			staticComponents.Add(new Button(buttonTexture, font, Game.Scale)
			{
				Position = new Vector2(0.7f * g.Width, 0.9f * g.Height),
				Text = "Restore to defaults",
				Click = RestoreToDefaults
			}
			);

			//Add a fullscreen checkbox + text
			var fullscreenText = new Text(font, "Fullscreen")
			{
				Position = new Vector2(musicVolumeText.Position.X, musicVolumeText.Position.Y + 2*musicVolumeText.Height)
			};

			staticComponents.Add(fullscreenText);
			var fullscreenCheckBox = new Checkbox(c1, c2, g.Scale)
			{
				Position = new Vector2(fullscreenText.Position.X + fullscreenText.Width, fullscreenText.Position.Y),
				Click = ChangeWindowMode
			};
			staticComponents.Add(fullscreenCheckBox);
		}

		private void ChangeWindowMode(object sender, EventArgs e)
		{
			if((sender as Checkbox).Checked)
			{
				//Change to fullscreen
				Game.SetFullscreen();
			}
			else
			{
				//Change to windowed mode
				Game.SetWindowedMode();
			}
		}

		private void ChangeMusicVolume(object sender, EventArgs e)
		{
			MediaPlayer.Volume = (sender as Slider).CurrentValue;
		}

		private void RestoreToDefaults(object sender, EventArgs e)
		{
			//Restore keybindings
			Game.Input.RestoreToDefaults();
			for (int i = 0; i < Game.Input.KeyBindings.Count; i++)
			{
				//Update strings in keybinds buttons
				keybindsButtons[i].Text = Game.Input.KeyBindings.ElementAt(i).Value.ToString();
				//Reset bindingNow list
				bindingNow[i] = false;
			}
			//Restore music volume
			MediaPlayer.Volume = 0.5f;
			musicVolume.CurrentValue = 0.5f;

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

			if (Game.Input.KeyBindings.ContainsKey(inputKeybindStrings[index]))
			{
				Game.Input.KeyBindings[inputKeybindStrings[index]] = null;
				bindingNow[index] = true;
				keybindsButtons[index].Text = "Press any key";
			}
			else
				throw new Exception("Keybind not found");
		}

		private void Back(object sender, EventArgs e)
		{
			//If selected keys by player are ok we can go back to Main Menu
			if(ValidateKeys())
			{
				Game.ChangeState(new MainMenu(Game, graphicsDevice, content));
			}
			//We should display a window with message that current settings are not correct
			else
			{
				CreateMessage("Selected keybindings are not correct. Make sure there is no duplicates or not assigned keys.");
			}
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Begin();
			foreach (var c in staticComponents)
				c.Draw(gameTime, spriteBatch);
			spriteBatch.End();
		}

		/// <summary>
		/// Checks if new keybindings are correct
		/// </summary>
		public bool ValidateKeys()
		{
			//Check for duplicates
			var test = Input.KeyBindings.Where(i => Input.KeyBindings.Any(t => t.Key != i.Key && t.Value == i.Value)).ToDictionary(i => i.Key, i => i.Value);
			if (test.Count > 0)
				return false;
			//Check for not assigned values
			foreach (var x in Game.Input.KeyBindings)
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
			//Should we check for keybind?
			if(bindingNow.Contains(true))
			{
				//Check if user pressed keybind
				var key = Input.FirstPressedkey();
				if (key == null)
					return;
				else
				{
					int index = bindingNow.IndexOf(true);
					Game.Input.KeyBindings[inputKeybindStrings[index]] = key;
					keybindsButtons[index].Text = key.ToString();
					bindingNow[index] = false;
				}
			}
		}
	}
}
