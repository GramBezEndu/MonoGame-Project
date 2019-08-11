using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using GameProject.States;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Audio;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameProject
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		public GraphicsDeviceManager graphics { get; private set; }
		SpriteBatch spriteBatch;
		const int LogicalWidth = 1920;
		const int LogicalHeight = 1080;
		/// <summary>
		/// Actual screen width
		/// </summary>
		public int Width
		{
			get
			{
				return graphics.PreferredBackBufferWidth;
			}
		}
		/// <summary>
		/// Actual screen height
		/// </summary>
		public int Height
		{
			get
			{
				return graphics.PreferredBackBufferHeight;
			}
		}
		/// <summary>
		/// Scaling objects
		/// </summary>
		public Vector2 Scale
		{
			get
			{
				return new Vector2((float)Width/LogicalWidth, (float)Height/LogicalHeight);
			}
		}

        State currentState;
		State nextState;

        SpriteFont font;

		public Random Random { get; private set; }

		public Input Input { get; private set; }

		/// <summary>
		/// Contains actually played song
		/// </summary>
		private Song currentBackgroundSong { get; set; }
		private Song nextBackgroundSong { get; set; }

		public void ChangeBackgroundSong(Song song)
		{
			//We do not allow to change song to the same song
			if (IsThisSongPlaying(song))
				return;
			//If parameter is null we stop playing music
			if (song == null)
			{
				MediaPlayer.Stop();
				return;
			}
			nextBackgroundSong = song;
		}
		
		public bool IsThisSongPlaying(Song song)
		{
			if (currentBackgroundSong == song)
				return true;
			else
				return false;
		}

		public void ChangeState(State state)
		{
			currentState.DisposeMessages();
			nextState = state;
		}

        /// <summary>
        /// Generates a number in 1-100 range (includes 1 and 100)
        /// </summary>
        /// <returns></returns>
        public int RandomPercent()
        {
            var value = this.Random.Next(1, 101);
            return value;
        }
        /// <summary>
        /// Generate random number from min to max range (includes min and max)
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public int RandomNumber(int min, int max)
        {
            var value = this.Random.Next(min, max + 1);
            return value;
        }
		/// <summary>
		/// Draw a random float between 2 and 3
		/// </summary>
		/// <returns></returns>
		public float RandomCriticalMultiplier()
		{
			float rnd = (float)Random.NextDouble();
			return 2f + rnd;
		}

		public void ChangeResolution(int width, int height)
		{
			//if(Math.Round((float)width/height, 2) != 1.78)
			//{
			//	throw new Exception("Only 16:9 resolutions");
			//}
			//Do not switch to the same resolution
			if(width == Width && Height == height)
			{
				return;
			}
			graphics.PreferredBackBufferWidth = width;
			graphics.PreferredBackBufferHeight = height;
			graphics.ApplyChanges();
		}

		public Game1()
		{
			//Show dialog box on unhandled exceptions
			Application.ThreadException += new ThreadExceptionEventHandler(UnhandledThreadException);
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledException);

			Window.Title = "Defeat The Vapula";
			graphics = new GraphicsDeviceManager(this);
			Window.ClientSizeChanged += OnResize;
			Content.RootDirectory = "Content";
			//Loop songs
			MediaPlayer.IsRepeating = true;
			//Default volume for songs
			MediaPlayer.Volume = 0.5f;
			//Default volume for sfx
			SoundEffect.MasterVolume = 0.5f;
			graphics.HardwareModeSwitch = true;
			graphics.PreferredBackBufferWidth = 1600; //1600
			graphics.PreferredBackBufferHeight = 900; //900
			IsMouseVisible = true;
			//Fullscreen check (will only work for 16:9 monitors)
			if (false)
			{
				graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
				graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
				//graphics.PreferredBackBufferWidth = 1280;
				//graphics.PreferredBackBufferHeight = 800;
				graphics.IsFullScreen = true;
			}
			graphics.ApplyChanges();
			//Note:
			//Game is caped at 60fps but slower frame rate means slower enemies, player and projectiles movement
		}

		private void UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			List<string> buttons = new List<string>() { "Ok" };
			Microsoft.Xna.Framework.Input.MessageBox.Show("Unhandled error", (e.ExceptionObject as Exception).Message, buttons);
			if(!Debugger.IsAttached)
				this.Exit();
		}

		private void UnhandledThreadException(object sender, ThreadExceptionEventArgs e)
		{
			List<string> buttons = new List<string>() { "Ok" };
			Microsoft.Xna.Framework.Input.MessageBox.Show("Unhandled error", e.Exception.Message, buttons);
			if (!Debugger.IsAttached)
				this.Exit();
		}

		private void OnResize(object sender, EventArgs e)
		{
			//throw new NotImplementedException();
		}

		/// <summary>
		/// Sets fullscreen mode
		/// </summary>
		public void SetFullscreen()
		{
			if(graphics.IsFullScreen)
			{
				return;
			}
			else
			{
				var x = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes;
				//graphics.HardwareModeSwitch = true;
				//graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
				//graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
				//graphics.PreferredBackBufferWidth = Width;
				//graphics.PreferredBackBufferHeight = Height;
				graphics.IsFullScreen = true;
				//this.Window.IsBorderless = false;

				//graphics.PreferredBackBufferWidth = Width;
				//graphics.PreferredBackBufferHeight = Height;


				//Workaround for fullscreen issue where:
				//First swap to fullscreen mode does not work properly
				//But next swaps to fullscreen do not generate this issue

				//Note: This fix works for the first time,
				//But later alt tabs are broken instead
				//IntPtr hWnd = this.Window.Handle;
				//var control = System.Windows.Forms.Control.FromHandle(hWnd);
				//var form = control.FindForm();
				//form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
				//form.WindowState = System.Windows.Forms.FormWindowState.Maximized;

				graphics.ApplyChanges();
			}
		}

		/// <summary>
		/// Sets windowed mode
		/// </summary>
		public void SetWindowedMode()
		{
			if (!graphics.IsFullScreen)
			{
				return;
			}
			else
			{
				graphics.IsFullScreen = false;
				//graphics.PreferredBackBufferWidth = Width;
				//graphics.PreferredBackBufferHeight = Height;
				graphics.ApplyChanges();
				//graphics.ToggleFullScreen();
				//graphics.ApplyChanges();
				//Thread.Sleep(500);
			}
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any staticComponents
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
			Random = new Random();
			//Load input from file here
			Input = new Input();
			currentState = new GameProject.States.LogoState(this, graphics.GraphicsDevice, Content);
			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Font");
			// TODO: use this.Content to load your game content here
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			if(nextState != null)
			{
				currentState = nextState;
				nextState = null;
			}

			if(nextBackgroundSong != null)
			{
				MediaPlayer.Stop();
				currentBackgroundSong = nextBackgroundSong;
				nextBackgroundSong = null;
				if(currentBackgroundSong != null)
					MediaPlayer.Play(currentBackgroundSong);
			}

			// TODO: Add your update logic here

			//If the window has no focus, we do not update
			if(this.IsActive)
				currentState.Update(gameTime);
			else
			{
				if(currentState is GameState)
				{
					(currentState as GameState).Paused = true;
				}
			}

			currentState.PostUpdate();
			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			// TODO: Add your drawing code here
			GraphicsDevice.Clear(Color.Black);
			currentState.Draw(gameTime, spriteBatch);
			//if(graphics.PreferredBackBufferWidth != Width || graphics.PreferredBackBufferHeight != Height)
			//{
			//	graphics.PreferredBackBufferWidth = Width;
			//	graphics.PreferredBackBufferHeight = Height;
			//	graphics.ApplyChanges();
			//}
			//Begin new sprite batch for debug info
			spriteBatch.Begin();

			if (Debugger.IsAttached)
            {
				DateTime compilationDate = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).LastWriteTime;
				spriteBatch.DrawString(font, "Debug Mode Enabled", new Vector2(0, 0), Color.Red);
				spriteBatch.DrawString(font, "version: " + compilationDate.ToString(), new Vector2(0, 0.05f* Height), Color.Red);
				spriteBatch.DrawString(font, "current state: " + currentState.ToString(), new Vector2(0, 0.1f * Height), Color.Red);
				spriteBatch.DrawString(font, "graphics.HardwareModeSwitch: " + graphics.HardwareModeSwitch.ToString(), new Vector2(0, 0.15f * Height), Color.Red);
				spriteBatch.DrawString(font, "graphics.IsFullScreen: " + graphics.IsFullScreen.ToString(), new Vector2(0, 0.2f * Height), Color.Red);
				spriteBatch.DrawString(font, "graphics.Width: " + graphics.PreferredBackBufferWidth.ToString(), new Vector2(0, 0.25f * Height), Color.Red);
				spriteBatch.DrawString(font, "graphics.Height: " + graphics.PreferredBackBufferHeight.ToString(), new Vector2(0, 0.3f * Height), Color.Red);
				spriteBatch.DrawString(font, "Selected Width: " + Width.ToString(), new Vector2(0, 0.35f * Height), Color.Red);
				spriteBatch.DrawString(font, "Selected Height: " + Height.ToString(), new Vector2(0, 0.4f * Height), Color.Red);
				spriteBatch.DrawString(font, "Client Bounds Width: " + Window.ClientBounds.Width.ToString(), new Vector2(0, 0.45f * Height), Color.Red);
				spriteBatch.DrawString(font, "Client Bounds Height: " + Window.ClientBounds.Height.ToString(), new Vector2(0, 0.5f * Height), Color.Red);
				spriteBatch.DrawString(font, "Scale: " + Scale.ToString(), new Vector2(0, 0.55f * Height), Color.Red);
				spriteBatch.DrawString(font, "Draw FPS: " + (1 / gameTime.ElapsedGameTime.TotalSeconds).ToString(), new Vector2(0, 0.6f * Height), Color.Red);
				if(currentState is GameState)
					spriteBatch.DrawString(font, "Displaying tutorial: " + (currentState as GameState).IsDisplayingTutorial, new Vector2(0, 0.7f * Height), Color.Red);
			}
            spriteBatch.End();

            base.Draw(gameTime);
		}
	}
}
