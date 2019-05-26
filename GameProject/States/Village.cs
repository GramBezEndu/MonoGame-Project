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
using GameProject.Inventory;
using GameProject.Controls;

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
        List<Door> optionalEntrances;
		public Village(Game1 g, GraphicsDevice gd, ContentManager c, PlayerClasses playerClass) : base(g, gd, c)
		{
			int inventorySlots = 12;
			CreatePlayer(g, gd, playerClass, inventorySlots);

			AddCommonItemsToPlayer();

			var blackSmithAnimations = new Dictionary<string, Animation>()
			{
				{"Idle", new Animation(content.Load<Texture2D>("Blacksmith"), 3, game.Scale) },
			};

			//Static Textures["VillageBackground"]
			staticComponents = new List<Component>
			{
				new Sprite(Textures["VillageBackground"], g.Scale)
				{
					Position = new Vector2(0,0)
				},
			};

			var dungeonEntrance = new Sprite(Textures["DungeonEntrance"], g.Scale)
			{
				Position = new Vector2(1.113f * game.Width, 0.4f * game.Height)
			};

			var interactionButton = new Sprite(Keys[Input.KeyBindings["Interact"].ToString()], g.Scale)
			{
				Position = dungeonEntrance.Position
			};

			optionalEntrances = new List<Door>()
			{
				new Door(game, this, dungeonEntrance, interactionButton, player),
			};

			var blacksmithSprite = new Sprite(blackSmithAnimations)
			{
				Position = new Vector2(0.813f * game.Width, 0.33f * game.Height)
			};
			var blacksmithButton = new Sprite(Keys[Input.KeyBindings["Interact"].ToString()], game.Scale);

			movingComponents = new List<Component>(optionalEntrances)
			{
				new Blacksmith(game, this, blacksmithSprite, blacksmithButton, player),
				new Sprite(Textures["box"], g.Scale)
				{
					Position = new Vector2(0.813f*game.Width,0.55f*game.Height)
				},
				new Sprite(Textures["Anvil"], g.Scale)
				{
					Position = new Vector2(0.813f*game.Width,0.45f*game.Height)
				},
				player,
			};

			SpawnShopkeeper(new Vector2(0.513f * game.Width, 0.4f * game.Height));

			var trainingDummyAnimations = new Dictionary<string, Animation>()
			{
				{"Idle", new Animation(content.Load<Texture2D>("TrainingDummy"), 1, game.Scale)}
			};

			enemies.Add(new TrainingDummy(game, this, Font, trainingDummyAnimations)
			{
				Position = new Vector2(0.95f * game.Width, 0.4f * game.Height)
			}
			);

			pausedComponents = new List<Component>
			{
				new Sprite(Textures["PauseBorder"], g.Scale)
				{
					Position = new Vector2(0, 0.63f * g.Height),
				},
				new Button(Textures["Button"], Font, g.Scale)
				{
				Text = "Main Menu",
				Position = new Vector2(0.01f * g.Width, 0.7f * g.Height),
				Click = MainMenuClick
				},
				new Button(Textures["Button"], Font, g.Scale)
				{
				Text = "Exit",
				Position = new Vector2(0.01f * g.Width, 0.8f * g.Height),
				Click = ExitClick
				}
			};
		}

		private void AddCommonPlayerUiComponents()
		{
			uiComponents.Add(player.InventoryManager);
			uiComponents.Add(player.HealthBar);
		}

		private void AddStaminaBarUi()
		{
			uiComponents.Add((player as StaminaUser).StaminaBar);
		}

		private void AddManaBarUi()
		{
			throw new NotImplementedException();
			//uiComponents.Add((player as ManaUser).);
		}

		private void AddCommonItemsToPlayer()
		{
			player.InventoryManager.AddItem(
				new HealthPotion(Textures["HealthPotion"], game.Scale)
				{
					Quantity = 5
				}
			);
		}

		private void CreatePlayer(Game1 g, GraphicsDevice gd, PlayerClasses playerClass, int inventorySlots)
		{
			switch (playerClass)
			{
				case PlayerClasses.Warrior:
					{
						CreateWarrior(g, gd, inventorySlots);
						break;
					}
				case PlayerClasses.Archer:
					{
						CreateArcher(g, gd, inventorySlots);
						break;
					}
				case PlayerClasses.Wizard:
					{
						CreateWizard();
						break;
					}
			}
		}

		private void CreateWizard()
		{
			//Not implemented yet
			throw new NotImplementedException();
			//player = new Wizard(content.Load<Texture2D>("Archer"), g.Scale);
			if (player is ManaUser)
			{
				(player as ManaUser).InventoryManager.AddItem(new ManaPotion(Textures["ManaPotion"], game.Scale)
				{
					Quantity = 5
				});
			}
			AddCommonPlayerUiComponents();
			AddManaBarUi();
		}

		private void CreateArcher(Game1 g, GraphicsDevice gd, int inventorySlots)
		{
			var animations = new Dictionary<string, Animation>()
						{
							{"Idle", new Animation(content.Load<Texture2D>("Archer"), 1, game.Scale) },
							{"WalkRight", new Animation(content.Load<Texture2D>("WalkRight"), 3, game.Scale)},
							{"WalkLeft", new Animation(content.Load<Texture2D>("WalkLeft"), 3, game.Scale) }
						};
			player = new Archer(this, animations, Input, g.Scale)
			{
				Position = new Vector2(0.05f * game.Width, 0.4f * game.Height)
			};
			player.InventoryManager = new InventoryManager(gd, player, Textures["Inventory"], Textures["InventorySlot"], Textures["Gold"], Font, inventorySlots, new Vector2(0.55f * game.Width, 0.05f * game.Height), game.Scale);
			player.HealthBar = new HealthBar(Textures["HealthBarBorder"], Textures["Health"], Font, new Vector2(0.03f * game.Width, 0.9f * game.Height), game.Scale);
			if (player is StaminaUser)
			{
				(player as StaminaUser).StaminaBar = new StaminaBar(Textures["HealthBarBorder"], Textures["Stamina"], Font, new Vector2(0.03f * game.Width, 0.95f * game.Height), game.Scale);
			}
			player.InventoryManager.AddItem(new StartingBow(Textures["StartingBow"], game.Scale));
			player.InventoryManager.AddItem(new StartingArcherHelmet(Textures["StartingArcherHelmet"], game.Scale));
			player.InventoryManager.AddItem(new StartingArcherBreastplate(Textures["StartingArcherBreastplate"], game.Scale));
			player.InventoryManager.AddItem(new StartingBoots(Textures["StartingBoots"], game.Scale));

			player.InventoryManager.EquipmentManager = new EquipmentManager(Textures["Inventory"],
				Textures["InventorySlot"],
				Font,
				new Vector2(0.02f * game.Width, 0.05f * game.Height),
				game.Scale)
			{
				EquipmentSlots = new List<InventorySlot>()
							{
								new ArcherHelmetSlot(gd, player, Textures["InventorySlot"], Font, game.Scale)
								{
									Position = new Vector2(0.075f * game.Width, 0.07f * game.Height)
								},
								new NecklaceSlot(gd, player, Textures["InventorySlot"], Font, game.Scale)
								{
									Position = new Vector2(0.075f * game.Width, 0.075f * game.Height + Textures["InventorySlot"].Height*game.Scale)
								},
								new ArcherBreastplateSlot(gd, player, Textures["InventorySlot"], Font, game.Scale)
								{
									Position = new Vector2(0.075f * game.Width, 0.07f * game.Height + 2*Textures["InventorySlot"].Height*game.Scale)
								},
								new BootsSlot(gd, player, Textures["InventorySlot"], Font, game.Scale)
								{
									Position = new Vector2(0.075f * game.Width, 0.07f * game.Height + 3*Textures["InventorySlot"].Height*game.Scale)
								},
								new BowSlot(gd, player, Textures["InventorySlot"], Font, game.Scale)
								{
									Position = new Vector2(0.025f * game.Width, 0.07f * game.Height + 2*Textures["InventorySlot"].Height*game.Scale)
								},
								new RingSlot(gd, player, Textures["InventorySlot"], Font, game.Scale)
								{
									Position = new Vector2(0.125f * game.Width, 0.07f * game.Height + 2*Textures["InventorySlot"].Height*game.Scale)
								}
							}
			};
			AddCommonPlayerUiComponents();
			AddStaminaBarUi();
		}

		private void CreateWarrior(Game1 g, GraphicsDevice gd, int inventorySlots)
		{
			var animations = new Dictionary<string, Animation>()
						{
							{"Idle", new Animation(content.Load<Texture2D>("Warrior"), 1, game.Scale) },
							{"WalkRight", new Animation(content.Load<Texture2D>("WalkRight"), 3, game.Scale)},
							{"WalkLeft", new Animation(content.Load<Texture2D>("WalkLeft"), 3, game.Scale) },
							{"BlockRight", new Animation(content.Load<Texture2D>("BlockRight"), 1, game.Scale) },
							{"BlockLeft", new Animation(content.Load<Texture2D>("BlockLeft"), 1, game.Scale) },
							{"BlockUp", new Animation(content.Load<Texture2D>("BlockUp"), 1, game.Scale) },
							{"FastAttack", new Animation(content.Load<Texture2D>("WarriorFastAttack"), 3, game.Scale, 0.1f) },
							{"NormalAttack", new Animation(content.Load<Texture2D>("WarriorNormalAttack"), 3, game.Scale, 0.25f) }
						};
			player = new Warrior(this, animations, Input, g.Scale, Font)
			{
				Position = new Vector2(0.05f * game.Width, 0.4f * game.Height)
			};
			player.InventoryManager = new InventoryManager(gd, player, Textures["Inventory"], Textures["InventorySlot"], Textures["Gold"], Font, inventorySlots, new Vector2(0.55f * game.Width, 0.05f * game.Height), game.Scale);
			player.HealthBar = new HealthBar(Textures["HealthBarBorder"], Textures["Health"], Font, new Vector2(0.03f * game.Width, 0.9f * game.Height), game.Scale);
			if (player is StaminaUser)
			{
				(player as StaminaUser).StaminaBar = new StaminaBar(Textures["HealthBarBorder"], Textures["Stamina"], Font, new Vector2(0.03f * game.Width, 0.95f * game.Height), game.Scale);
			}
			player.InventoryManager.AddItem(new StartingSword(Textures["StartingSword"], game.Scale));
			player.InventoryManager.AddItem(new StartingWarriorHelmet(Textures["StartingWarriorHelmet"], game.Scale));
			player.InventoryManager.AddItem(new StartingWarriorBreastplate(Textures["StartingWarriorBreastplate"], game.Scale));
			player.InventoryManager.AddItem(new StartingBoots(Textures["StartingBoots"], game.Scale));
			player.InventoryManager.AddItem(new StartingShield(Textures["StartingShield"], game.Scale));

			player.InventoryManager.EquipmentManager = new EquipmentManager(Textures["Inventory"],
				Textures["InventorySlot"],
				Font,
				new Vector2(0.02f * game.Width, 0.05f * game.Height),
				game.Scale)
			{
				EquipmentSlots = new List<InventorySlot>()
							{
								new WarriorHelmetSlot(gd, player, Textures["InventorySlot"], Font, game.Scale)
								{
									Position = new Vector2(0.075f * game.Width, 0.07f * game.Height)
								},
								new NecklaceSlot(gd, player, Textures["InventorySlot"], Font, game.Scale)
								{
									Position = new Vector2(0.075f * game.Width, 0.07f * game.Height + Textures["InventorySlot"].Height*game.Scale)
								},
								new WarriorBreastplateSlot(gd, player, Textures["InventorySlot"], Font, game.Scale)
								{
									Position = new Vector2(0.075f * game.Width, 0.07f * game.Height + 2*Textures["InventorySlot"].Height*game.Scale)
								},
								new BootsSlot(gd, player, Textures["InventorySlot"], Font, game.Scale)
								{
									Position = new Vector2(0.075f * game.Width, 0.07f * game.Height + 3*Textures["InventorySlot"].Height*game.Scale)
								},
								new SwordSlot(gd, player, Textures["InventorySlot"], Font, game.Scale)
								{
									Position = new Vector2(0.025f * game.Width, 0.07f * game.Height + 2*Textures["InventorySlot"].Height*game.Scale)
								},
								new ShieldSlot(gd, player, Textures["InventorySlot"], Font, game.Scale)
								{
									Position = new Vector2(0.125f * game.Width, 0.07f * game.Height + 1*Textures["InventorySlot"].Height*game.Scale),
								},
								new RingSlot(gd, player, Textures["InventorySlot"], Font, game.Scale)
								{
									Position = new Vector2(0.125f * game.Width, 0.07f * game.Height + 2*Textures["InventorySlot"].Height*game.Scale)
								}
							}
			};
			AddCommonPlayerUiComponents();
			AddStaminaBarUi();
		}

		private void ExitClick(object sender, EventArgs e)
		{
			game.Exit();
		}

		private void MainMenuClick(object sender, EventArgs e)
		{
			game.ChangeState(new MainMenu(game, graphicsDevice, content));
		}

        private void NextState()
        {
			GameState newState = new Dungeon(game, game.GraphicsDevice, game.Content, player);
			game.ChangeState(newState);
			player.gameState = newState;
        }

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, float scale)
		{
				//Static Textures["VillageBackground"] batch
				spriteBatch.Begin();
				foreach (var c in staticComponents)
					c.Draw(gameTime, spriteBatch);
				spriteBatch.End();

				//Moving objects sprite batch (contains player (he should be last in moving staticComponents))
				spriteBatch.Begin(transformMatrix: camera.Transform);
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
				if(Paused)
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
			if (!Paused)
			{
				foreach (var c in staticComponents)
					c.Update(gameTime);
				foreach (var mc in movingComponents)
					mc.Update(gameTime);
                foreach(var i in optionalEntrances)
                {
                    if (i.Activated == true)
                    {
                        i.Activated = false;
                        NextState();
                    }
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
	}
}
