using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Sprites;

namespace GameProject.States
{
	public class DungeonLobby : GameState
	{
		List<Door> Entrances = new List<Door>();
		public DungeonLobby(Game1 g, GraphicsDevice gd, ContentManager c, Player p) : base(g, gd, c)
		{
			player = p;
			//Reset player position
			player.Position = new Vector2(0.05f * Game.Width, 0.6f * Game.Height);
			//Static background
			staticComponents = new List<Component>
			{
				new Sprite(Textures["DungeonLobbyBackground"], g.Scale)
				{
					Position = new Vector2(0,0)
				},
			};
			var doorSprite = new Sprite(Textures["DungeonEntrance"], Game.Scale)
			{
				Position = new Vector2(0.3f * Game.Width, 0.55f * Game.Height)
			};
			var interactButton = new Sprite(Keys[Input.KeyBindings["Interact"].ToString()], Game.Scale);
			Entrances.Add(new Door(Game, this, doorSprite, interactButton, player));
			movingComponents = new List<Component>(Entrances)
			{
				player
			};

			if (player is StaminaUser)
			{
				uiComponents.Add(player.InventoryManager);
				uiComponents.Add(player.HealthBar);
				uiComponents.Add((player as StaminaUser).StaminaBar);
			}
			else
				throw new NotImplementedException();

			//Wall at beginning of level
			Sprite wall = new Sprite(Textures["Wall"], g.Scale)
			{
				Position = new Vector2(-0.5f * Game.Width, 0)
			};

			//Wall at the end of the level
			Sprite endWall = new Sprite(Textures["Wall2"], g.Scale)
			{
				Position = new Vector2(1f * Game.Width, 0)
			};

			collisionSprites.Add(wall);
			collisionSprites.Add(endWall);

			Game.ChangeBackgroundSong(null);
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Begin();
			foreach (var sc in staticComponents)
				sc.Draw(gameTime, spriteBatch);
			spriteBatch.End();

			spriteBatch.Begin(transformMatrix: camera.Transform);
			foreach (var cs in collisionSprites)
				cs.Draw(gameTime, spriteBatch);
			foreach (var e in enemies)
				e.Draw(gameTime, spriteBatch);
			foreach (var mc in movingComponents)
				mc.Draw(gameTime, spriteBatch);
			spriteBatch.End();

			//Static objects over the moving objects (UI)
			spriteBatch.Begin();
			foreach (var ui in uiComponents)
				ui.Draw(gameTime, spriteBatch);
			//paused UI
			if (Paused)
			{
				foreach (var pc in pausedComponents)
					pc.Draw(gameTime, spriteBatch);
			}
			spriteBatch.End();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if(!Paused)
			{
				foreach (var sc in staticComponents)
					sc.Update(gameTime);

				foreach (var mc in movingComponents)
					mc.Update(gameTime);
				foreach (var i in Entrances)
				{
					if (i.Activated == true)
					{
						i.Activated = false;
						NextState();
						return;
					}
				}
				foreach (var ui in uiComponents)
					ui.Update(gameTime);
				camera.Follow(Game, player);
			}
			else
			{
				foreach (var pc in pausedComponents)
					pc.Update(gameTime);
			}
		}

		private void NextState()
		{
			GameState newState = new SkeletonDungeon(Game, Game.GraphicsDevice, Game.Content, player);
			Game.ChangeState(newState);
			player.gameState = newState;
		}

		public override void PostUpdate()
		{
			//throw new NotImplementedException();
		}
	}
}
