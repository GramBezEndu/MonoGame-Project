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
using GameProject.Controls;
using Microsoft.Xna.Framework.Audio;

namespace GameProject
{
    public abstract class State
    {
        protected ContentManager content;
        public GraphicsDevice graphicsDevice { get; protected set; }
        public Game1 Game { get; protected set; }
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

		/// <summary>
		/// Contatins all skeleton archer textures (includes arrow)
		/// </summary>
		public Dictionary<string, Texture2D> SkeletonArcherTextures = new Dictionary<string, Texture2D>();

		public Dictionary<string, Song> Songs = new Dictionary<string, Song>();

		public Dictionary<string, SoundEffect> SoundEffects = new Dictionary<string, SoundEffect>();

		/// <summary>
		/// Current message that is displayed
		/// </summary>
		public Message Message;

		/// <summary>
		/// Loads textures from main directory to Textures dictionary
		/// </summary>
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
                Textures[key] = content.Load<Texture2D>(Directory.GetCurrentDirectory() + "/Content/" + key);
            }
        }

		/// <summary>
		/// Loads skeleton archer textures to SkeletonArcherTextures dictionary
		/// </summary>
		private void LoadSkeletonArcherTextures()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(content.RootDirectory + "/Skeleton/Archer/");
			if (!directoryInfo.Exists)
				throw new DirectoryNotFoundException();
			FileInfo[] files = directoryInfo.GetFiles("*.*");
			foreach (FileInfo file in files)
			{
				string key = Path.GetFileNameWithoutExtension(file.Name);
				if (key == "Font")
					continue;
				SkeletonArcherTextures[key] = content.Load<Texture2D>(Directory.GetCurrentDirectory() + "/Content/Skeleton/Archer/" + key);
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

		public void LoadSoundEffects()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(content.RootDirectory + "/Sounds/");
			if (!directoryInfo.Exists)
				throw new DirectoryNotFoundException();
			FileInfo[] files = directoryInfo.GetFiles("*.*");
			foreach (FileInfo file in files)
			{
				string key = Path.GetFileNameWithoutExtension(file.Name);
				//Keys[key] = content.Load<Texture2D>(directoryInfo.ToString() + key);
				SoundEffects[key] = content.Load<SoundEffect>(Directory.GetCurrentDirectory() + "/Content/Sounds/" + key);
			}
		}

        public State(Game1 g, GraphicsDevice gd, ContentManager c)
        {
            content = c;
            Game = g;
            graphicsDevice = gd;
            Input = Game.Input;
            LoadKeyTextures();
            LoadTextures();
			LoadSkeletonArcherTextures();
			LoadSongs();
			LoadSoundEffects();
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

        public virtual void Update(GameTime gameTime)
		{
			Input.Update(gameTime);
			//Add message to display if there is any
			AddMessage();
		}

		public virtual void AddMessage()
		{
			if(Message != null)
			{
				IEnumerable<Component> msg = staticComponents.Where(x => x is Message);
				foreach(var m in msg)
				{
					m.Hidden = true;
				}
				staticComponents.Add(Message);
				Message = null;
			}
		}

		public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
		/// <summary>
		/// Remove not needed resources etc.
		/// </summary>
		public abstract void PostUpdate();

		public void CreateMessage(string msg)
		{
			Message = new Message(Game, graphicsDevice, Font, msg, SoundEffects["MessageNotification"]);
		}
	}
}
