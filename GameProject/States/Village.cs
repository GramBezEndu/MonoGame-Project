using System;
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
using GameProject.Inventory;

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
			int inventorySlots = 6;
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
			var startingWarriorBreastplate = content.Load<Texture2D>("StartingWarriorBreastplate");
			var startingArcherBreastplate = content.Load<Texture2D>("StartingArcherBreastplate");
			var startingWarriorHelmet = content.Load<Texture2D>("StartingWarriorHelmet");
			var startingArcherHelmet = content.Load<Texture2D>("StartingArcherHelmet");
			var startingBoots = content.Load<Texture2D>("StartingBoots");

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
						player.InventoryManager.AddItem(new StartingWarriorHelmet(startingWarriorHelmet, game.Scale));
						player.InventoryManager.AddItem(new StartingWarriorBreastplate(startingWarriorBreastplate, game.Scale));
						player.InventoryManager.AddItem(new StartingBoots(startingBoots, game.Scale));

						player.InventoryManager.EquipmentManager = new EquipmentManager(inventoryTexture,
							slotTexture,
							font,
							new Vector2(0.02f * game.Width, 0.05f * game.Height),
							game.Scale);
						player.InventoryManager.EquipmentManager.EquipmentSlots = new List<InventorySlot>() {
							new WarriorHelmetSlot(slotTexture, font, game.Scale)
							{
								Position = new Vector2(0.075f * game.Width, 0.07f * game.Height)
							},
							new NecklaceSlot(slotTexture, font, game.Scale)
							{
								Position = new Vector2(0.075f * game.Width, 0.07f * game.Height + slotTexture.Height*game.Scale)
							},
							new WarriorBreastplateSlot(slotTexture, font, game.Scale)
							{
								Position = new Vector2(0.075f * game.Width, 0.07f * game.Height + 2*slotTexture.Height*game.Scale)
							},
							new BootsSlot(slotTexture, font, game.Scale)
							{
								Position = new Vector2(0.075f * game.Width, 0.07f * game.Height + 3*slotTexture.Height*game.Scale)
							},
							new SwordSlot(slotTexture, font, game.Scale)
							{
								Position = new Vector2(0.025f * game.Width, 0.07f * game.Height + 2*slotTexture.Height*game.Scale)
							},
							new ShieldSlot(slotTexture, font, game.Scale)
							{
								Position = new Vector2(0.125f * game.Width, 0.07f * game.Height + 1*slotTexture.Height*game.Scale)
							},
							new RingSlot(slotTexture, font, game.Scale)
							{
								Position = new Vector2(0.125f * game.Width, 0.07f * game.Height + 2*slotTexture.Height*game.Scale)
							}
						};
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
						player.InventoryManager.AddItem(new StartingArcherHelmet(startingArcherHelmet, game.Scale));
						player.InventoryManager.AddItem(new StartingArcherBreastplate(startingArcherBreastplate, game.Scale));
						player.InventoryManager.AddItem(new StartingBoots(startingBoots, game.Scale));

						player.InventoryManager.EquipmentManager = new EquipmentManager(inventoryTexture,
							slotTexture,
							font,
							new Vector2(0.02f * game.Width, 0.05f * game.Height),
							game.Scale);
						player.InventoryManager.EquipmentManager.EquipmentSlots = new List<InventorySlot>() {
							new ArcherHelmetSlot(slotTexture, font, game.Scale)
							{
								Position = new Vector2(0.075f * game.Width, 0.07f * game.Height)
							},
							new NecklaceSlot(slotTexture, font, game.Scale)
							{
								Position = new Vector2(0.075f * game.Width, 0.075f * game.Height + slotTexture.Height*game.Scale)
							},
							new ArcherBreastplateslot(slotTexture, font, game.Scale)
							{
								Position = new Vector2(0.075f * game.Width, 0.07f * game.Height + 2*slotTexture.Height*game.Scale)
							},
							new BootsSlot(slotTexture, font, game.Scale)
							{
								Position = new Vector2(0.075f * game.Width, 0.07f * game.Height + 3*slotTexture.Height*game.Scale)
							},
							new BowSlot(slotTexture, font, game.Scale)
							{
								Position = new Vector2(0.025f * game.Width, 0.07f * game.Height + 2*slotTexture.Height*game.Scale)
							},
							new RingSlot(slotTexture, font, game.Scale)
							{
								Position = new Vector2(0.125f * game.Width, 0.07f * game.Height + 2*slotTexture.Height*game.Scale)
							}
						};
						break;
					}
				case PlayerClasses.Wizard:
					{
						//Not implemented yet
						//player = new Wizard(content.Load<Texture2D>("Archer"), g.Scale);
						if (player is ManaUser)
						{
							(player as ManaUser).InventoryManager.AddItem(new ManaPotion(manaPotionTexture, game.Scale), 2);
						}
						break;
					}
			}

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
				},
				player
			};

			if(player is StaminaUser)
			{
				uiComponents = new List<Component>
				{
				player.InventoryManager,
				player.HealthBar,
				(player as StaminaUser).StaminaBar
				};
			}
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, float scale)
		{
			//Static background batch
			spriteBatch.Begin();
			foreach (var c in components)
				c.Draw(gameTime, spriteBatch);
			spriteBatch.End();

			//Moving objects sprite batch (contains player (he should be last in moving components))
			spriteBatch.Begin(transformMatrix: camera.Transform);
			foreach(var mc in movingComponents)
				mc.Draw(gameTime, spriteBatch);
			spriteBatch.End();

			//Static objects over the moving objects (UI)
			spriteBatch.Begin();
			foreach (var ui in uiComponents)
				ui.Draw(gameTime, spriteBatch);
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
			foreach (var mc in movingComponents)
				mc.Update(gameTime);
			foreach (var ui in uiComponents)
				ui.Update(gameTime);
			camera.Follow(game, player);
		}
	}
}
