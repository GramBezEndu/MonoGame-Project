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
		/// <summary>
		/// Note: Need to rethink concept with adding elements to uiComponents etc.
		/// Maybe it will be better to provide references to different spriteBatches to drawing objects
		/// </summary>
		/// <param name="g"></param>
		/// <param name="gd"></param>
		/// <param name="c"></param>
		/// <param name="playerClass"></param>
		public Village(Game1 g, GraphicsDevice gd, ContentManager c, PlayerClasses playerClass) : base(g, gd, c)
		{
			int inventorySlots = 12;
			CreatePlayer(g, gd, playerClass, inventorySlots);

			AddCommonItemsToPlayer();

			//Static Textures (VillageBackground)
			staticComponents = new List<Component>
			{
				new Sprite(Textures["VillageBackground"], g.Scale)
				{
					Position = new Vector2(0,0)
				},
			};

			var dungeonEntrance = new Sprite(Textures["DungeonLobbyEntrance"], g.Scale)
			{
				Position = new Vector2(1.113f * Game.Width, 0.4f * Game.Height)
			};

			var interactionButton = new Sprite(Keys[Input.KeyBindings["Interact"].ToString()], g.Scale)
			{
				Position = dungeonEntrance.Position
			};

			optionalEntrances = new List<Door>()
			{
				new Door(Game, this, dungeonEntrance, interactionButton, player),
			};

			movingComponents = new List<Component>(optionalEntrances)
			{
				PickUpPrompt,
				player,
			};

			SpawnShopkeeper(new Vector2(0.3f * Game.Width, 0.4f * Game.Height));
			SpawnBlacksmith(new Vector2(0.52f * Game.Width, 0.33f * Game.Height));
			SpawnTrainingDummy(new Vector2(0.7f * Game.Width, 0.4f * Game.Height));
			SpawnStatueOfGods(new Vector2(0.9f * Game.Width, 0.47f * Game.Height));
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
				new HealthPotion(Textures["HealthPotion"], Game.Scale)
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
				(player as ManaUser).InventoryManager.AddItem(new ManaPotion(Textures["ManaPotion"], Game.Scale)
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
							{"Idle", new Animation(content.Load<Texture2D>("Archer"), 1, Game.Scale) },
							{"WalkRight", new Animation(content.Load<Texture2D>("WalkRight"), 3, Game.Scale)},
							{"WalkLeft", new Animation(content.Load<Texture2D>("WalkLeft"), 3, Game.Scale) }
						};
			player = new Archer(this, animations, Input, g.Scale)
			{
				Position = new Vector2(0.05f * Game.Width, 0.4f * Game.Height)
			};
			player.InventoryManager = new InventoryManager(this, gd, player, Textures["Inventory"], Textures["InventorySlot"], Textures["Gold"], Textures["Trashcan"], Font, inventorySlots, new Vector2(0.55f * Game.Width, 0.05f * Game.Height), Game.Scale);
			//Create AccessSlotsManager
			player.InventoryManager.AccessSlotsManager = new AccessSlotsManager(this, gd, player, Textures["InventorySlot"], Font, g.Scale, new Vector2(0.03f * Game.Width, 0.8f * Game.Height), Input);
			player.HealthBar = new HealthBar(Textures["HealthBarBorder"], Textures["Health"], Font, new Vector2(0.03f * Game.Width, 0.9f * Game.Height), Game.Scale);
			if (player is StaminaUser)
			{
				(player as StaminaUser).StaminaBar = new StaminaBar(Textures["HealthBarBorder"], Textures["Stamina"], Font, new Vector2(0.03f * Game.Width, 0.95f * Game.Height), Game.Scale);
			}
			player.InventoryManager.AddItem(new StartingBow(gd, player, Textures["InventorySlot"], Font, Textures["StartingBow"], Game.Scale));
			player.InventoryManager.AddItem(new StartingArcherHelmet(gd, player, Textures["InventorySlot"], Font, Textures["StartingArcherHelmet"], Game.Scale));
			player.InventoryManager.AddItem(new StartingArcherBreastplate(gd, player, Textures["InventorySlot"], Font, Textures["StartingArcherBreastplate"], Game.Scale));
			player.InventoryManager.AddItem(new StartingBoots(gd, player, Textures["InventorySlot"], Font, Textures["StartingBoots"], Game.Scale));

			player.InventoryManager.EquipmentManager = new EquipmentManager(Textures["Inventory"],
				Textures["InventorySlot"],
				Font,
				new Vector2(0.02f * Game.Width, 0.05f * Game.Height),
				Game.Scale)
			{
				EquipmentSlots = new List<InventorySlot>()
							{
								new ArcherHelmetSlot(gd, player, Textures["InventorySlot"], Font, Game.Scale)
								{
									Position = new Vector2(0.075f * Game.Width, 0.07f * Game.Height)
								},
								new ArcherBreastplateSlot(gd, player, Textures["InventorySlot"], Font, Game.Scale)
								{
									Position = new Vector2(0.075f * Game.Width, 0.07f * Game.Height + Textures["InventorySlot"].Height*Game.Scale.Y)
								},
								new NecklaceSlot(gd, player, Textures["InventorySlot"], Font, Game.Scale)
								{
									Position = new Vector2(0.025f * Game.Width, 0.07f * Game.Height + 2*Textures["InventorySlot"].Height*Game.Scale.Y)
								},
								new BootsSlot(gd, player, Textures["InventorySlot"], Font, Game.Scale)
								{
									Position = new Vector2(0.075f * Game.Width, 0.07f * Game.Height + 3*Textures["InventorySlot"].Height*Game.Scale.Y)
								},
								new BowSlot(gd, player, Textures["InventorySlot"], Font, Game.Scale)
								{							
									Position = new Vector2(0.025f * Game.Width, 0.07f * Game.Height + Textures["InventorySlot"].Height*Game.Scale.Y)
								},
								new RingSlot(gd, player, Textures["InventorySlot"], Font, Game.Scale)
								{
									Position = new Vector2(0.125f * Game.Width, 0.07f * Game.Height + 2*Textures["InventorySlot"].Height*Game.Scale.Y)
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
							{"Die", new Animation(Textures["Die"], 3, Game.Scale)},
							{"Dead", new Animation(Textures["Dead"], 1, Game.Scale)},
							{"Idle", new Animation(content.Load<Texture2D>("Warrior"), 1, Game.Scale) },
							{"Walk", new Animation(content.Load<Texture2D>("Walk"), 3, Game.Scale)},
							{"Block", new Animation(content.Load<Texture2D>("Block"), 1, Game.Scale) },
							{"BlockUp", new Animation(content.Load<Texture2D>("BlockUp"), 1, Game.Scale) },
							{"FastAttack", new Animation(content.Load<Texture2D>("WarriorFastAttack"), 3, Game.Scale, 0.1f) },
							{"NormalAttack", new Animation(content.Load<Texture2D>("WarriorNormalAttack"), 3, Game.Scale, 0.15f) }
						};
			player = new Warrior(this, animations, Input, g.Scale, Font)
			{
				Position = new Vector2(0.05f * Game.Width, 0.4f * Game.Height)
			};
			player.InventoryManager = new InventoryManager(this, gd, player, Textures["Inventory"], Textures["InventorySlot"], Textures["Gold"], Textures["Trashcan"], Font, inventorySlots, new Vector2(0.55f * Game.Width, 0.05f * Game.Height), Game.Scale);
			//Create AccessSlotsManager
			player.InventoryManager.AccessSlotsManager = new AccessSlotsManager(this, gd, player, Textures["InventorySlot"], Font, g.Scale, new Vector2(0.03f * Game.Width, 0.8f * Game.Height), Input);
			player.HealthBar = new HealthBar(Textures["HealthBarBorder"], Textures["Health"], Font, new Vector2(0.03f * Game.Width, 0.9f * Game.Height), Game.Scale);
			if (player is StaminaUser)
			{
				(player as StaminaUser).StaminaBar = new StaminaBar(Textures["HealthBarBorder"], Textures["Stamina"], Font, new Vector2(0.03f * Game.Width, 0.95f * Game.Height), Game.Scale);
			}
			//var startingSword = new StartingSword(gd, player, Textures["InventorySlot"], Font, Textures["StartingSword"], game.Scale);
			//startingSword.Upgrade(new LegendaryImprovementScroll(game, Textures["LegendaryImprovementScroll"], game.Scale));
			//player.InventoryManager.AddItem(startingSword);
			//var startingHelmet = new StartingWarriorHelmet(gd, player, Textures["InventorySlot"], Font, Textures["StartingWarriorHelmet"], game.Scale);
			//startingHelmet.Upgrade(new LegendaryImprovementScroll(game, Textures["LegendaryImprovementScroll"], game.Scale));
			//player.InventoryManager.AddItem(startingHelmet);
			player.InventoryManager.AddItem(new StartingSword(gd, player, Textures["InventorySlot"], Font, Textures["StartingSword"], Game.Scale));
			player.InventoryManager.AddItem(new StartingWarriorHelmet(gd, player, Textures["InventorySlot"], Font, Textures["StartingWarriorHelmet"], Game.Scale));
			player.InventoryManager.AddItem(new StartingWarriorBreastplate(gd, player, Textures["InventorySlot"], Font, Textures["StartingWarriorBreastplate"], Game.Scale));
			player.InventoryManager.AddItem(new StartingBoots(gd, player, Textures["InventorySlot"], Font, Textures["StartingBoots"], Game.Scale));
			player.InventoryManager.AddItem(new StartingShield(Textures["StartingShield"], Game.Scale));

			player.InventoryManager.EquipmentManager = new EquipmentManager(Textures["Inventory"],
				Textures["InventorySlot"],
				Font,
				new Vector2(0.02f * Game.Width, 0.05f * Game.Height),
				Game.Scale)
			{
				EquipmentSlots = new List<InventorySlot>()
							{
								new WarriorHelmetSlot(gd, player, Textures["InventorySlot"], Font, Game.Scale)
								{
									Position = new Vector2(0.075f * Game.Width, 0.07f * Game.Height)
								},
								new WarriorBreastplateSlot(gd, player, Textures["InventorySlot"], Font, Game.Scale)
								{
									Position = new Vector2(0.075f * Game.Width, 0.07f * Game.Height + Textures["InventorySlot"].Height*Game.Scale.Y)
								},
								new NecklaceSlot(gd, player, Textures["InventorySlot"], Font, Game.Scale)
								{
									Position = new Vector2(0.025f * Game.Width, 0.07f * Game.Height + 2*Textures["InventorySlot"].Height*Game.Scale.Y)
								},
								new BootsSlot(gd, player, Textures["InventorySlot"], Font, Game.Scale)
								{
									Position = new Vector2(0.075f * Game.Width, 0.07f * Game.Height + 3*Textures["InventorySlot"].Height*Game.Scale.Y)
								},
								new SwordSlot(gd, player, Textures["InventorySlot"], Font, Game.Scale)
								{
									Position = new Vector2(0.025f * Game.Width, 0.07f * Game.Height + Textures["InventorySlot"].Height*Game.Scale.Y)
								},
								new ShieldSlot(gd, player, Textures["InventorySlot"], Font, Game.Scale)
								{
									Position = new Vector2(0.125f * Game.Width, 0.07f * Game.Height + 1*Textures["InventorySlot"].Height*Game.Scale.Y),
								},
								new RingSlot(gd, player, Textures["InventorySlot"], Font, Game.Scale)
								{
									Position = new Vector2(0.125f * Game.Width, 0.07f * Game.Height + 2*Textures["InventorySlot"].Height*Game.Scale.Y)
								}
							}
			};
			AddCommonPlayerUiComponents();
			AddStaminaBarUi();
			//CreateMessage("Test message");
			//Starting location -> create speedrun timer
			SpeedrunTimer = new SpeedrunTimer(Font);
			uiComponents.Add(SpeedrunTimer);
		}

        private void NextState()
        {
			GameState newState = new DungeonLobby(Game, Game.GraphicsDevice, Game.Content, player);
			Game.ChangeState(newState);
			player.gameState = newState;
        }

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
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
	}
}
