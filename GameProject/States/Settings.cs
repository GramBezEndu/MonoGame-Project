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
using Microsoft.Xna.Framework.Audio;
using GameProject.Animations;

namespace GameProject.States
{
	public class Settings : State
	{
		protected List<Button> keybindsButtons = new List<Button>();
		protected List<string> inputKeybindStrings = new List<string>();
		/// <summary>
		/// Represents if any key (and which one by the index) is waiting to be binded
		/// </summary>
		protected List<bool> bindingNow;

		protected Slider musicVolume;
		protected Slider sfxVolume;

		///Represents max font width (from input keybinds strings), so buttons can be placed correctly
		protected Vector2 size = new Vector2(0, 0);

		protected List<Component> keybindingsComponents = new List<Component>();
		protected List<Component> gameplayComponents = new List<Component>();
		protected List<Component> audioComponents = new List<Component>();
		protected List<Component> videoComponents = new List<Component>();

        public Settings(Game1 g, GraphicsDevice gd, ContentManager c) : base(g, gd, c)
		{
			//Shared components
			CreateSharedComponents();

			//Categories buttons
			CreateCategoriesButtons();

			//Keybindings components
			CreateKeybindingsComponents();

			//Gameplay components
			CreateGameplayComponents();

			//Audio components
			CreateAudioComponents();

			//Video components
			CreateVideoComponents();

			//On default show keybindings components
			ShowKeybindingsComponents(this, new EventArgs());
		}

		private void CreateKeybindingsComponents()
		{
			//Make keybinds strings
			foreach (var s in Game.Input.KeyBindings)
			{
				string actualString = s.Key.ToString();
				inputKeybindStrings.Add(actualString);
				Vector2 currentSize = Font.MeasureString(actualString);
				size.X = Math.Max(size.X, currentSize.X);
			}

			var tempButton = new Button(Textures["Button"], Font, Game.Scale);

			//Make keybinds buttons (need to set correct Y still)
			for (int i = 0; i < Game.Input.KeyBindings.Count; i++)
			{
				keybindsButtons.Add(new Button(Textures["Button"], Font, Game.Scale)
				{
					Position = new Vector2(0.255f * Game.Width + size.X, 0.09f * Game.Height + i * tempButton.Height),
					Text = Game.Input.KeyBindings.ElementAt(i).Value.ToString(),
					Click = ChangeKeybind
				}
				);
			}

			//Iterate over strings and make Text components
			for (int i = 0; i < inputKeybindStrings.Count; i++)
			{
				keybindingsComponents.Add(new Text(Font, inputKeybindStrings[i])
				{
					Position = new Vector2(0.255f * Game.Width, 0.107f * Game.Height + i * keybindsButtons[i].Height)
				}
				);
			}

			//Make a list of bools (default false)
			bindingNow = new List<bool>(new bool[Game.Input.KeyBindings.Count]);

			keybindingsComponents.AddRange(keybindsButtons);

			staticComponents.AddRange(keybindingsComponents);
		}

		private void CreateGameplayComponents()
		{
			//Add a speedrun timer checkbox + text
			var speedrunTimerText = new Text(Font, "Speedrun timer")
			{
				Position = new Vector2(0.5f * Game.Width, 0.5f * Game.Height)
			};

			gameplayComponents.Add(speedrunTimerText);

			staticComponents.AddRange(gameplayComponents);
		}

		private void CreateVideoComponents()
		{
			//Add a fullscreen checkbox + text
			var fullscreenText = new Text(Font, "Fullscreen")
			{
				Position = new Vector2(0.5f * Game.Width, 0.5f * Game.Height)
			};

			videoComponents.Add(fullscreenText);

			var checkboxAnimations = new Dictionary<string, Animation>()
			{
				{"CheckboxChecked", new Animation(Textures["CheckboxChecked"], 1, Game.Scale) },
				{"Checkbox", new Animation(Textures["Checkbox"], 1, Game.Scale) },
			};

			var fullscreenCheckBox = new Checkbox(Input, checkboxAnimations)
			{
				Position = new Vector2(fullscreenText.Position.X + fullscreenText.Width, fullscreenText.Position.Y),
				Click = ChangeWindowMode,
				Checked = Game.graphics.IsFullScreen
			};
			videoComponents.Add(fullscreenCheckBox);

			//Add a resolution list + text
			var resolutionText = new Text(Font, "Resolution")
			{
				Position = new Vector2(fullscreenText.Position.X, fullscreenText.Position.Y + 3 * fullscreenText.Height)
			};

			videoComponents.Add(resolutionText);

			List<string> Resolutions = new List<string>()
			{
				"1920 x 1080",
				"1600 x 900",
				"1280 x 720",
				"1680 x 1050",
				"1600 x 1200",
				"1600 x 1024",
				"1024 x 768",
				"800 x 600",
			};

			var resolutionList = new SelectableList(Input, Textures["ArrowSelector"], Game.Scale, Font, Resolutions)
			{
				Position = new Vector2(resolutionText.Position.X + resolutionText.Width, resolutionText.Position.Y),
				OnValueChange = ChangeResolution
			};

			string actualRes = String.Format("{0} x {1}", Game.Width, Game.Height);

			resolutionList.ChangeSelectedOption(actualRes);

			videoComponents.Add(resolutionList);

			staticComponents.AddRange(videoComponents);
		}

		private void CreateAudioComponents()
		{
			//Add a music volume slider + text component
			var musicVolumeText = new Text(Font, "Music volume ")
			{
				Position = new Vector2(0.5f * Game.Width, 0.2f * Game.Height)
			};

			audioComponents.Add(musicVolumeText);

			musicVolume = new Slider(Input, Textures["SliderBorder"], Textures["SliderFilled"], Font, Game.Scale)
			{
				Position = new Vector2(musicVolumeText.Position.X + musicVolumeText.Width, musicVolumeText.Position.Y),
				Click = ChangeMusicVolume,
				CurrentValue = MediaPlayer.Volume
			};

			audioComponents.Add(musicVolume);

			//Add a sound volume slider + text component
			var sfxVolumeText = new Text(Font, "Sfx volume ")
			{
				Position = new Vector2(musicVolumeText.Position.X, musicVolumeText.Position.Y + 2 * musicVolumeText.Height)
			};

			audioComponents.Add(sfxVolumeText);

			sfxVolume = new Slider(Input, Textures["SliderBorder"], Textures["SliderFilled"], Font, Game.Scale)
			{
				Position = new Vector2(sfxVolumeText.Position.X + sfxVolumeText.Width, sfxVolumeText.Position.Y),
				Click = ChangeSfxVolume,
				CurrentValue = SoundEffect.MasterVolume
			};

			audioComponents.Add(sfxVolume);

			staticComponents.AddRange(audioComponents);
		}

		private void CreateSharedComponents()
		{
			staticComponents = new List<Component>
			{
				new Sprite(Textures["Background"], Game.Scale)
				{
					Position = new Vector2(0,0)
				},
				new Sprite(Textures["BackgroundBig"], Game.Scale)
				{
					Position = new Vector2(1f/4f*Game.Width, 0.03f*Game.Height)
				},
				new Button(Textures["Button"], Font, Game.Scale)
				{
				Text = "Back",
				Position = new Vector2(0.01f * Game.Width, 0.9f * Game.Height),
				Click = Back
				},
				new Button(Textures["Button"], Font, Game.Scale)
				{
				Position = new Vector2(0.7f * Game.Width, 0.9f * Game.Height),
				Text = "Restore to defaults",
				Click = RestoreToDefaults
				},
			};
		}

		private void CreateCategoriesButtons()
		{
			var keybindingsSettingButton = new Button(Textures["Button"], Font, Game.Scale)
			{
				Text = "Keybindings",
				Position = new Vector2(0.255f * Game.Width, 0.033f * Game.Height),
				Click = ShowKeybindingsComponents
			};
			var gameplaySettingButton = new Button(Textures["Button"], Font, Game.Scale)
			{
				Text = "Gameplay",
				Position = new Vector2(keybindingsSettingButton.Position.X + keybindingsSettingButton.Width, keybindingsSettingButton.Position.Y),
				Click = ShowGameplayComponents
			};
			var audioSettingButton = new Button(Textures["Button"], Font, Game.Scale)
			{
				Text = "Audio",
				Position = new Vector2(gameplaySettingButton.Position.X + gameplaySettingButton.Width, gameplaySettingButton.Position.Y),
				Click = ShowAudioComponents
			};
			var videoSettingsButton = new Button(Textures["Button"], Font, Game.Scale)
			{
				Text = "Video",
				Position = new Vector2(audioSettingButton.Position.X + audioSettingButton.Width, audioSettingButton.Position.Y),
				Click = ShowVideoComponents
			};

			staticComponents.Add(keybindingsSettingButton);
			staticComponents.Add(gameplaySettingButton);
			staticComponents.Add(audioSettingButton);
			staticComponents.Add(videoSettingsButton);
		}

		private void ShowKeybindingsComponents(object sender, EventArgs e)
		{
			foreach (var c in keybindingsComponents)
				c.Hidden = false;
			foreach (var c in gameplayComponents)
				c.Hidden = true;
			foreach (var c in audioComponents)
				c.Hidden = true;
			foreach (var c in videoComponents)
				c.Hidden = true;
		}

		private void ShowGameplayComponents(object sender, EventArgs e)
		{
			foreach (var c in keybindingsComponents)
				c.Hidden = true;
			foreach (var c in gameplayComponents)
				c.Hidden = false;
			foreach (var c in audioComponents)
				c.Hidden = true;
			foreach (var c in videoComponents)
				c.Hidden = true;
		}

		protected virtual void ShowVideoComponents(object sender, EventArgs e)
		{
			foreach (var c in keybindingsComponents)
				c.Hidden = true;
			foreach (var c in gameplayComponents)
				c.Hidden = true;
			foreach (var c in audioComponents)
				c.Hidden = true;
			foreach (var c in videoComponents)
				c.Hidden = false;
		}

		private void ShowAudioComponents(object sender, EventArgs e)
		{
			foreach (var c in keybindingsComponents)
				c.Hidden = true;
			foreach (var c in gameplayComponents)
				c.Hidden = true;
			foreach (var c in audioComponents)
				c.Hidden = false;
			foreach (var c in videoComponents)
				c.Hidden = true;
		}

		private void ChangeResolution(object sender, EventArgs e)
		{
			//Get the resolution from SelectedOption message
			string msg = (sender as SelectableList).SelectedOption?.Message;
			msg.Replace(" ", string.Empty);

			string[] Resolutions = msg.Split('x');
			int width = Int32.Parse(Resolutions[0]);
			int height = Int32.Parse(Resolutions[1]);

			if(Game.Width != width && Game.Height != height)
			{
				Game.ChangeResolution(width, height);
				Game.ChangeState(new Settings(Game, Game.GraphicsDevice, content));
			}
		}

		private void ChangeSfxVolume(object sender, EventArgs e)
		{
			SoundEffect.MasterVolume = (sender as Slider).CurrentValue;
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
			//Restore sfx volume
			SoundEffect.MasterVolume = 0.5f;
			sfxVolume.CurrentValue = 0.5f;
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

		protected virtual void Back(object sender, EventArgs e)
		{
			//If selected keys by player are ok we can go back to Main Menu
			if(ValidateKeys())
			{
				Game.ChangeState(new MainMenu(Game, graphicsDevice, content));
			}
			//We should display a window with message that current settings are not correct
			else
			{
				CreateMessage("Selected keybindings are not correct. Make sure there is no duplicates or not assigned actions.");
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
