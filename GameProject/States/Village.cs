﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameProject.Sprites;
using GameProject.Animations;
using GameProject.Items;

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
			#region local variables
			var playerScale = 1f;
			var inventoryTexture = content.Load<Texture2D>("Inventory");
			var slotTexture = content.Load<Texture2D>("InventorySlot");
			int inventorySlots = 5;
			var healthPotionTexture = content.Load<Texture2D>("HealthPotion");
			var manaPotionTexture = content.Load<Texture2D>("ManaPotion");
			var font = content.Load<SpriteFont>("Font");
			var healthBarTexture = content.Load<Texture2D>("HealthBarBorder");
			var healthTexture = content.Load<Texture2D>("Health");
			var staminaTexture = content.Load<Texture2D>("Stamina");
			var anivilTexture = content.Load<Texture2D>("Anvil");
			var background = content.Load<Texture2D>("VillageBackground");
			var box = content.Load<Texture2D>("Box");
			var startingSwordTexture = content.Load<Texture2D>("StartingSword");
			var startingBowTexture = content.Load<Texture2D>("StartingBow");

			var blackSmithAnimations = new Dictionary<string, Animation>()
			{
				{"Idle", new Animation(content.Load<Texture2D>("Blacksmith"), 3, game.Scale) },
			};
			var shopkeeperAnimations = new Dictionary<string, Animation>()
			{
				{"Idle", new Animation(content.Load<Texture2D>("Shopkeeper"), 1, game.Scale) },
			};
			#endregion
			switch (playerClass)
			{
				case PlayerClasses.Warrior:
					{
						var animations = new Dictionary<string, Animation>()
						{
							{"Idle", new Animation(content.Load<Texture2D>("Warrior"), 1, playerScale*game.Scale) },
							{"WalkRight", new Animation(content.Load<Texture2D>("WalkRight"), 3, playerScale*game.Scale)},
							{"WalkLeft", new Animation(content.Load<Texture2D>("WalkLeft"), 3, playerScale*game.Scale) }
						};
						player = new Warrior(animations, g.Scale)
						{
							Position = new Vector2(0.05f * game.Width, 0.4f * game.Height),
							InventoryManager = new InventoryManager(inventoryTexture, slotTexture, font, inventorySlots, new Vector2(0.75f * game.Width, 0.05f * game.Height), game.Scale),
							HealthBar = new HealthBar(healthBarTexture, healthTexture, new Vector2(0.03f * game.Width, 0.9f * game.Height), game.Scale)
						};
						if (player is StaminaUser)
						{
							(player as StaminaUser).StaminaBar = new StaminaBar(healthBarTexture, staminaTexture, new Vector2(0.03f * game.Width, 0.95f * game.Height), game.Scale);
						}
						player.InventoryManager.AddItem(new StartingSword(startingSwordTexture, game.Scale));
						break;
					}
				case PlayerClasses.Archer:
					{
						var animations = new Dictionary<string, Animation>()
						{
							{"Idle", new Animation(content.Load<Texture2D>("Archer"), 1, playerScale*game.Scale) },
							{"WalkRight", new Animation(content.Load<Texture2D>("WalkRight"), 3, playerScale*game.Scale)},
							{"WalkLeft", new Animation(content.Load<Texture2D>("WalkLeft"), 3, playerScale*game.Scale) }
						};
						player = new Archer(animations, g.Scale)
						{
							Position = new Vector2(0.05f * game.Width, 0.4f * game.Height),
							InventoryManager = new InventoryManager(inventoryTexture, slotTexture, font, inventorySlots, new Vector2(0.75f * game.Width, 0.05f * game.Height), game.Scale),
							HealthBar = new HealthBar(healthBarTexture, healthTexture, new Vector2(0.03f * game.Width, 0.9f * game.Height), game.Scale)
					};
						if (player is StaminaUser)
						{
							(player as StaminaUser).StaminaBar = new StaminaBar(healthBarTexture, staminaTexture, new Vector2(0.03f * game.Width, 0.95f * game.Height), game.Scale);
						}
						player.InventoryManager.AddItem(new StartingBow(startingBowTexture, game.Scale));
						break;
					}
				case PlayerClasses.Wizard:
					{
						//player = new Wizard(content.Load<Texture2D>("Archer"), g.Scale);
						if (player is ManaUser)
						{
							(player as ManaUser).InventoryManager.AddItem(new ManaPotion(manaPotionTexture, game.Scale), 2);
						}
						break;
					}
			}
			player.InventoryManager.EquipmentManager = new EquipmentManager(inventoryTexture,
				slotTexture,
				font,
				new Vector2(0.02f * game.Width, 0.05f * game.Height),
				game.Scale);
			player.InventoryManager.AddItem(new HealthPotion(healthPotionTexture, game.Scale), 2);

			components = new List<Component>
			{
				new Sprite(background, g.Scale)
				{
					Position = new Vector2(0,0)
				},
			};

			movingComponents = new List<Component>
			{
				new Blacksmith(blackSmithAnimations)
				{
					Position = new Vector2(0.813f*game.Width,0.33f*game.Height)
				},
				new Shopkeeper(shopkeeperAnimations)
				{
					Position = new Vector2(0.513f*game.Width,0.40f*game.Height)
				},
				new Sprite(box, g.Scale)
				{
					Position = new Vector2(0.813f*game.Width,0.55f*game.Height)
				},
				new Sprite(anivilTexture, g.Scale)
				{
					Position = new Vector2(0.813f*game.Width,0.45f*game.Height)
				}
			};
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, float scale)
		{
			spriteBatch.Begin();
			foreach (var c in components)
				c.Draw(gameTime, spriteBatch);
			player.InventoryManager.Draw(gameTime, spriteBatch);
			//player.InventoryManager.EquipmentManager.Draw(gameTime, spriteBatch);
			player.HealthBar.Draw(gameTime, spriteBatch);
			if (player is StaminaUser)
			{
				(player as StaminaUser).StaminaBar.Draw(gameTime, spriteBatch);
			}
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
			foreach (var mc in movingComponents)
				mc.Update(gameTime);
			//player.InventoryManager.Update(gameTime);
			camera.Follow(game, player);
		}
	}
}
