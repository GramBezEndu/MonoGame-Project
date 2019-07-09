using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Sprites;
using GameProject.Animations;
using GameProject.Controls;

namespace GameProject.States
{
	public class OptionalRoom : GameState
	{
		Door GoBackDoor;
		Dungeon dungeonLevel;
		public Vector2 OldPlayerPosition { get; set; }
		public OptionalRoom(Game1 g, GraphicsDevice gd, ContentManager c, Player p, int level, Dungeon gameLevel) : base(g, gd, c)
		{
			#region variables
			var background = content.Load<Texture2D>("OptionalRoomBackground");
			var font = content.Load<SpriteFont>("Font");
			var optionalEntranceTexture = content.Load<Texture2D>("OptionalEntrance");
			var pauseBorderTexture = content.Load<Texture2D>("PauseBorder");
			var buttonTexture = content.Load<Texture2D>("Button");
            #endregion
            player = p;
			dungeonLevel = gameLevel;
			//Place door at the end of dungeon level to come back
            GoBackDoor = new Door(g, 
                this,
                new Sprite(optionalEntranceTexture, g.Scale)
                {
            	    Position = new Vector2(1.05f*game.Width, 0.55f * game.Height)
                },
                new Sprite(Keys[Input.KeyBindings["Interact"].ToString()], g.Scale),
                player);
            //GoBackDoor = new Sprite(optionalEntranceTexture, g.Scale)
            //{
            //	Position = new Vector2(0.05f*game.Width, 0.55f * game.Height)
            //};
            //roll a number in 1-100 range
            int value = g.RandomPercent();
            //There is a chance to spawn a boss if level is higher than 10
            //Monsters 70%, Treasure chest 15%, Boss 15%
            if (level > 10)
			{
                //Monsters
                if (value <= 70)
                {
					SpawnSkeletonGroup();
                }
				//Mysterious chest
                else if (value <= 85)
                {
                    SpawnMysteriousChest(new Vector2(0.5f * game.Width, 0.82f * game.Height));
                }
				//Boss not added yet -> equals with empty room
                else
                {

                }
            }
			//Monsters 70%, Treasure chest 30%
			else
			{
                //Monsters
                if(value <= 70)
                {
					SpawnSkeletonGroup();
                }
                //Treasure chest
                else
                {
                    SpawnMysteriousChest(new Vector2(0.5f* game.Width, 0.65f*game.Height));
                }
			}

			//Static background
			staticComponents = new List<Component>
			{
				new Sprite(background, g.Scale)
				{
					Position = new Vector2(0,0)
				},
			};

			movingComponents = new List<Component>()
			{
				GoBackDoor
			};

            foreach (var x in mysteriousChests)
                movingComponents.Add(x);

			collisionSprites.Add(new Sprite(Textures["Wall"], g.Scale)
			{
				Position = new Vector2(-0.5f* game.Width,0)
			}
			);

			collisionSprites.Add(new Sprite(Textures["Wall2"], g.Scale)
			{
				Position = new Vector2(1.2f * game.Width, 0)
			}
);

			//foreach (var x in enemies)
			//    movingComponents.Add(x);
			movingComponents.Add(player);

			pausedComponents = new List<Component>
			{
				new Sprite(pauseBorderTexture, g.Scale)
				{
					Position = new Vector2(0, 0.63f * g.Height),
				},
				new Button(buttonTexture, font, g.Scale)
				{
				Text = "Main Menu",
				Position = new Vector2(0.01f * g.Width, 0.7f * g.Height),
				Click = MainMenuClick
				},
				new Button(buttonTexture, font, g.Scale)
				{
				Text = "Exit",
				Position = new Vector2(0.01f * g.Width, 0.8f * g.Height),
				Click = ExitClick
				}
			};

			if (player is StaminaUser)
			{
                uiComponents.Add(player.InventoryManager);
                uiComponents.Add(player.HealthBar);
                uiComponents.Add((player as StaminaUser).StaminaBar);
            }
			else
				throw new NotImplementedException();
		}

		private void MainMenuClick(object sender, EventArgs e)
		{
			game.ChangeState(new MainMenu(game, graphicsDevice, content));
		}

		private void ExitClick(object sender, EventArgs e)
		{
			game.Exit();
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, float scale)
		{
			//Static background batch
			spriteBatch.Begin();
			foreach (var c in staticComponents)
				c.Draw(gameTime, spriteBatch);
			spriteBatch.End();

			//Moving sprite
			spriteBatch.Begin(transformMatrix: camera.Transform);
			foreach (var cs in collisionSprites)
				cs.Draw(gameTime, spriteBatch);
			foreach (var e in enemies)
				e.Draw(gameTime, spriteBatch);
			//Moving objects sprite batch (contains player (he should be last in movingComponents))
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

		public override void PostUpdate()
		{
			//throw new NotImplementedException();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			//game.ChangeState(new Dungeon(game, graphicsDevice, content, player));
			if (!Paused)
			{
				foreach (var c in staticComponents)
					c.Update(gameTime);
                foreach (var mc in movingComponents)
					mc.Update(gameTime);
                //Go back
                GoBackDoor.Update(gameTime);
                if(GoBackDoor.Activated)
                {                    
                    //Bring back old player position and update gameState for player
                    player.Position = OldPlayerPosition;
					player.gameState = dungeonLevel;
					//We can change state now
                    game.ChangeState(dungeonLevel);

                    return;
                }
				foreach (var ui in uiComponents)
					ui.Update(gameTime);
				camera.Follow(game, player);
			}
			else
			{
				foreach (var pc in pausedComponents)
					pc.Update(gameTime);
			}
		}

		public void Enter(Player player)
		{
			//Assign old position
			OldPlayerPosition = player.Position;
			//change the position in new state
			this.player.Position = new Vector2(0.1f * game.Width, 0.6f * game.Height);
		}
	}
}
