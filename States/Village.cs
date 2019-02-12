using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameProject.Sprites;

namespace GameProject.States
{
	public enum PlayerClasses
	{
		Warrior,
		Archer,
		Wizard,
	}
	public class Village : GameState
	{
		public Village(Game1 g, GraphicsDevice gd, ContentManager c, PlayerClasses playerClass) : base(g, gd, c)
		{
			switch (playerClass)
			{
				case PlayerClasses.Warrior:
					{
						player = new Warrior(content.Load<Texture2D>("Warrior"), g.Scale);
						break;
					}
				case PlayerClasses.Archer:
					{
						player = new Archer(content.Load<Texture2D>("Archer"), g.Scale);
						break;
					}
				case PlayerClasses.Wizard:
					{
						player = new Wizard(content.Load<Texture2D>("Archer"), g.Scale);
						break;
					}
			}
			player.Position = new Vector2(0.05f * game.Width, 0.4f * game.Height);

			var background = content.Load<Texture2D>("VillageBackground");
			components = new List<Component>
			{
				new Sprite(background, g.Scale)
				{
					Position = new Vector2(0,0)
				},
			};

			var box = content.Load<Texture2D>("Box");

			movingComponents = new List<Component>
			{
				new Sprite(box, g.Scale)
				{
					Position = new Vector2(0.5f*game.Width,0.7f*game.Height)
				},
			};
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, float scale)
		{
			spriteBatch.Begin();
			foreach (var c in components)
				c.Draw(gameTime, spriteBatch);
			//spriteBatch.DrawString(font, player.Position.ToString(), new Vector2(10, 10), Color.White);
			spriteBatch.End();

			spriteBatch.Begin(transformMatrix: camera.Transform);
			foreach(var mc in movingComponents)
				mc.Draw(gameTime, spriteBatch);
			player.Draw(gameTime, spriteBatch);
			spriteBatch.End();
		}

		public override void PostUpdate()
		{
			//throw new NotImplementedException();
		}

		public override void Update(GameTime gameTime)
		{
			foreach (var c in components)
				c.Update(gameTime);
			player.Update(gameTime);
			camera.Follow(game, player);
		}
	}
}
