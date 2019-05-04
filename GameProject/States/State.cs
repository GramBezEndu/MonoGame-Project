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

namespace GameProject
{
    public abstract class State
    {
        protected ContentManager content;
        protected GraphicsDevice graphicsDevice;
        protected Game1 game;
        public Input Input { get; private set; }
        protected List<Component> staticComponents;
        /// <summary>
        /// Key textures
        /// </summary>
        protected Dictionary<string, Texture2D> Keys = new Dictionary<string, Texture2D>();
        public State(Game1 g, GraphicsDevice gd, ContentManager c)
        {
            content = c;
            game = g;
            graphicsDevice = gd;
            Input = game.Input;
            LoadKeyTextures();
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
