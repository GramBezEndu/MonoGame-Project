using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject.States;
using System;
using System.Diagnostics;
using System.IO;

namespace GameProject
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		const int LogicalWidth = 1600;
		const int LogicalHeight = 900;
		/// <summary>
		/// Actual screen width
		/// </summary>
		public int Width { get; private set; }
		/// <summary>
		/// Actual screen height
		/// </summary>
		public int Height { get; private set; }
		/// <summary>
		/// Scaling objects
		/// </summary>
		public float Scale { get; private set; }

        State currentState;
		State nextState;

        SpriteFont font;

		public Random Random { get; private set; }

		public Input Input { get; private set; }

		public void ChangeState(State state)
		{
			nextState = state;
		}

        /// <summary>
        /// Generates a number in 1-100 range
        /// </summary>
        /// <returns></returns>
        public int RandomPercent()
        {
            var value = this.Random.Next(1, 101);
            return value;
        }
        /// <summary>
        /// Generate random number from min to max range
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public int RandomNumber(int min, int max)
        {
            var value = this.Random.Next(min, max + 1);
            return value;
        }

		public Game1()
		{
			Window.Title = "Defeat The Vapula";
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
            //Note:
            //Screen resolution affects enemies aggro
            //Handling this issue is not included yet
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
			Width = graphics.PreferredBackBufferWidth;
			Height = graphics.PreferredBackBufferHeight;
			Scale = (float)Width / LogicalWidth;
            //Note:
            //Game is caped at 60fps but slower frame rate means slower player movement
            //Handling this issue is not included yet
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
			currentState = new MainMenu(this, graphics.GraphicsDevice, Content);

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

			// TODO: Add your update logic here

			//If the window had no focus, we do not update
			if(this.IsActive)
				currentState.Update(gameTime);

			currentState.PostUpdate();
			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			// TODO: Add your drawing code here
			currentState.Draw(gameTime, spriteBatch, Scale);
            //Create new sprite batch for version info + debug display
            spriteBatch.Begin();
            if (Debugger.IsAttached)
            {
				DateTime compilationDate = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).LastWriteTime;
				spriteBatch.DrawString(font, "Debug Mode Enabled", new Vector2(0, 0), Color.Red);
				spriteBatch.DrawString(font, "version: " + compilationDate.ToString(), new Vector2(0, 0.05f* Height), Color.Red);
				spriteBatch.DrawString(font, "current state: " + currentState.ToString(), new Vector2(0, 0.1f * Height), Color.Red);
            }
            spriteBatch.End();

            base.Draw(gameTime);
		}
	}
}
