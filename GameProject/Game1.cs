using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject.States;

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

		public void ChangeState(State state)
		{
			nextState = state;
		}

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			graphics.PreferredBackBufferWidth = 1600; //1600
			graphics.PreferredBackBufferHeight = 900; //900
			IsMouseVisible = true;
			//Fullscreen check
			if (false)
			{
				graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
				graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
				graphics.IsFullScreen = true;
			}
			Width = graphics.PreferredBackBufferWidth;
			Height = graphics.PreferredBackBufferHeight;
			Scale = (float)Width / LogicalWidth;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
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
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			if(nextState != null)
			{
				currentState = nextState;
				nextState = null;
			}

			// TODO: Add your update logic here
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

			base.Draw(gameTime);
		}
	}
}
