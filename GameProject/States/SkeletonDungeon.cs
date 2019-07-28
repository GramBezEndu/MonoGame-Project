﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using GameProject.Sprites;
using GameProject.Animations;
using GameProject.Items;
using GameProject.Inventory;
using GameProject.Controls;


namespace GameProject.States
{
	public class SkeletonDungeon : GameState
	{
		float dungeonLevelStringTimer = 2f;
		int currentLevel;
		string dungeonLevelText;
		Door nextLevelEntrance;
        //Note:
        //Optional room changes every time we enter it
        //It needs a change
		List<Door> optionalEntrances = new List<Door>();
        List<OptionalRoom> optionalRooms = new List<OptionalRoom>();
		int levelWidth;
		int optionalRoomsQuantity;
		public SkeletonDungeon(Game1 g, GraphicsDevice gd, ContentManager c, Player p, int level = 1) : base(g, gd, c)
        {
            currentLevel = level;
            dungeonLevelText = String.Format("Dungeon Level {0}", currentLevel);
            player = p;
			//Reset player position
            player.Position = new Vector2(0.05f * Game.Width, 0.6f * Game.Height);

            //Roll a level width
            levelWidth = g.Random.Next((currentLevel + 10) / 5, (currentLevel + 10) / 2) * Game.Width;

			//Place next level entrance at the end
			nextLevelEntrance = new Door(
                Game,
                this,
                new Sprite(Textures["DungeonEntrance"], g.Scale)

				{
                    Position = new Vector2(levelWidth, 0.55f * Game.Height)
                },
                new Sprite(Keys[Input.KeyBindings["Interact"].ToString()], g.Scale),
                player);

			movingComponents = new List<Component>()
			{
				nextLevelEntrance,
			};

			//Spawn Statue of Gods at the end of the level
			//SpawnShopkeeper(new Vector2(levelWidth - 0.68f * Game.Width, 0.59f * Game.Height));
			//SpawnBlacksmith(new Vector2(levelWidth - 0.43f * Game.Width, 0.5f * Game.Height));
			SpawnStatueOfGods(new Vector2(levelWidth - 0.22f * Game.Width, 0.65f * Game.Height));

			//Draw optional rooms quantity
			optionalRoomsQuantity = g.Random.Next(0, currentLevel / 2);
            //optionalRoomsQuantity = 3;

            //Place entrances to optional rooms (can be less rooms than drawn if they are touching each other)
            for (int i = 0; i < optionalRoomsQuantity; i++)
            {
                var temp = new Sprite(Textures["OptionalEntrance"], g.Scale)
                {
                    Position = new Vector2(g.Random.Next(0, levelWidth), 0.55f * Game.Height)
                };
                var temp2 = new Sprite(Keys[Input.KeyBindings["Interact"].ToString()], g.Scale);

                bool canBeAdded = true;
                foreach (var x in optionalEntrances)
                {
					//Bug: It can touch shopkeeper/statue of gods/blacksmith
                    //If it is touching any optional entrance or next level entrance it can't be added
                    if (temp.IsTouching(x.MainSprite) || temp.IsTouching(nextLevelEntrance.MainSprite))
                    {
                        canBeAdded = false;
                        break;
                    }
                }
                if (canBeAdded)
                {
                    optionalEntrances.Add(new Door(g, this, temp, temp2, p));
					optionalRooms.Add(new OptionalRoom(g, gd, c, p, level, this));
                }
            }

            //Static background
            staticComponents = new List<Component>
            {
                new Sprite(Textures["DungeonBackground"], g.Scale)
				{
                    Position = new Vector2(0,0)
                },
            };

			SpawnSkeletonGroup();

            //Wall at beginning of level
            Sprite wall = new Sprite(Textures["Wall"], g.Scale)
            {
                Position = new Vector2(-0.5f* Game.Width, 0)
            };

            //Wall at the end of the level
            Sprite endWall = new Sprite(Textures["Wall2"], g.Scale)
            {
                Position = new Vector2(levelWidth + 0.1f * Game.Width, 0)
            };

			collisionSprites.Add(wall);
            collisionSprites.Add(endWall);

			//foreach (var x in enemies)
			//    movingComponents.Add(x);

			movingComponents.Add(PickUpPrompt);
			movingComponents.Add(player);

            if (player is StaminaUser)
            {
                uiComponents.Add(player.InventoryManager);
                uiComponents.Add(player.HealthBar);
                uiComponents.Add((player as StaminaUser).StaminaBar);
            }
            else
                throw new NotImplementedException();

			//100% chance for normal boss
			if(level == 10)
			{

			}
			//100% chance for mysterious chest
			else if(level % 10 == 0)
			{
				SpawnMysteriousChest(new Vector2(levelWidth - 0.4f * Game.Width, 0.82f * Game.Height));
			}
        }

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			//Static background batch
			spriteBatch.Begin();
			foreach (var c in staticComponents)
				c.Draw(gameTime, spriteBatch);
			spriteBatch.End();

			//Moving objects sprite batch (contains player (he should be last in movingComponents))
			spriteBatch.Begin(transformMatrix: camera.Transform);
			foreach (var cs in collisionSprites)
				cs.Draw(gameTime, spriteBatch);
            foreach (var mc in optionalEntrances)
                mc.Draw(gameTime, spriteBatch);
			foreach (var e in enemies)
				e.Draw(gameTime, spriteBatch);
            foreach (var mc in movingComponents)
				mc.Draw(gameTime, spriteBatch);
            spriteBatch.End();

			//Static objects over the moving objects (UI)
			spriteBatch.Begin();
			foreach (var ui in uiComponents)
				ui.Draw(gameTime, spriteBatch);
			//Current dungeon level display text
			if (dungeonLevelStringTimer > 0)
			{
                float halfTextWidth = Font.MeasureString(dungeonLevelText).X/2;
				spriteBatch.DrawString(Font, dungeonLevelText, new Vector2(Game.Width / 2 - halfTextWidth, Game.Height / 3), Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
				//spriteBatch.DrawString(font, currentLevel.ToString(), new Vector2(game.Width / 2, game.Height / 2), Color.Red);
			}
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
				if(dungeonLevelStringTimer > 0)
					dungeonLevelStringTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				foreach (var c in staticComponents)
					c.Update(gameTime);
				foreach (var mc in movingComponents)
					mc.Update(gameTime);
                //Need seperate list, you can't update optional entrance before updating input which is updated in player
                //Need to rethink this concept
                foreach (var oe in optionalEntrances)
                    oe.Update(gameTime);
				for(int i=0;i<optionalEntrances.Count;i++)
				{
					if(optionalEntrances[i].Activated)
					{
						EnterOptionalRoom(i);
					}
				}
                //Next level entrance
                nextLevelEntrance.Update(gameTime);
                if(nextLevelEntrance.Activated)
                {
					NextDungeonLevel();
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
		private void EnterOptionalRoom(int i)
		{
			GameState newState = optionalRooms[i];
			optionalRooms[i].Enter(player);
			Game.ChangeState(newState);
			player.gameState = newState;
		}
		private void NextDungeonLevel()
		{
			GameState newState = new SkeletonDungeon(Game, graphicsDevice, content, player, currentLevel + 1);
			Game.ChangeState(newState);
			player.gameState = newState;
		}
	}
}