using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameProject
{
    public abstract class State
    {
        protected ContentManager content;
        public GraphicsDevice graphicsDevice { get; protected set; }
        protected Game1 game;
        public Input Input { get; private set; }
        protected List<Component> staticComponents;
        /// <summary>
        /// Key textures
        /// </summary>
        protected Dictionary<string, Texture2D> Keys = new Dictionary<string, Texture2D>();
        public SpriteFont Font { get; protected set; }
        /// <summary>
        /// Contains all textures from main content folder
        /// </summary>
        public Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();

		public Dictionary<string, Song> Songs = new Dictionary<string, Song>();

		private void LoadTextures()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(content.RootDirectory);
            if (!directoryInfo.Exists)
                throw new DirectoryNotFoundException();
            FileInfo[] files = directoryInfo.GetFiles("*.*");
            foreach (FileInfo file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file.Name);
                if (key == "Font")
                    continue;
                //Keys[key] = content.Load<Texture2D>(directoryInfo.ToString() + key);
                Textures[key] = content.Load<Texture2D>(Directory.GetCurrentDirectory() + "/Content/" + key);
            }
        }

		private void LoadSongs()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(content.RootDirectory + "/Songs/");
			if (!directoryInfo.Exists)
				throw new DirectoryNotFoundException();
			FileInfo[] files = directoryInfo.GetFiles("*.*");
			foreach (FileInfo file in files)
			{
				string key = Path.GetFileNameWithoutExtension(file.Name);
				//Keys[key] = content.Load<Texture2D>(directoryInfo.ToString() + key);
				Songs[key] = content.Load<Song>(Directory.GetCurrentDirectory() + "/Content/Songs/" + key);
			}
		}

        public State(Game1 g, GraphicsDevice gd, ContentManager c)
        {
            content = c;
            game = g;
            graphicsDevice = gd;
            Input = game.Input;
            LoadKeyTextures();
            LoadTextures();
			LoadSongs();
            Font = content.Load<SpriteFont>("Font");
        }
        /// <summary>
        /// Load all key textures
        /// </summary>
        private void LoadKeyTextures()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(content.RootDirectory + "/Keys/");
            if (!directoryInfo.Exists)
                throw new DirectoryNotFoundException();
            FileInfo[] files = directoryInfo.GetFiles("*.*");
            foreach (FileInfo file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file.Name);
                //Keys[key] = content.Load<Texture2D>(directoryInfo.ToString() + key);
                Keys[key] = content.Load<Texture2D>(Directory.GetCurrentDirectory() + "/Content/Keys/" + key);
            }
        }

        public abstract void Update(GameTime gameTime);
		public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch, float scale);
		/// <summary>
		/// Remove not needed resources etc.
		/// </summary>
		public abstract void PostUpdate();
	}
}
