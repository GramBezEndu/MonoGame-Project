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
		public Village(Game1 g, GraphicsDevice gd, ContentManager c, PlayerClasses playerClass) : base(g, gd, c)
		{
			#region local variables
			var slotTexture = content.Load<Texture2D>("InventorySlot");
			int inventorySlots = 12;
			var font = content.Load<SpriteFont>("Font");
			var healthBarTexture = content.Load<Texture2D>("HealthBarBorder");
			var anivilTexture = content.Load<Texture2D>("Anvil");
			var background = content.Load<Texture2D>("VillageBackground");
			var startingSwordTexture = content.Load<Texture2D>("StartingSword");
			var startingBowTexture = content.Load<Texture2D>("StartingBow");
			var startingWarriorBreastplate = content.Load<Texture2D>("StartingWarriorBreastplate");
			var startingArcherBreastplate = content.Load<Texture2D>("StartingArcherBreastplate");
			var startingWarriorHelmet = content.Load<Texture2D>("StartingWarriorHelmet");
			var startingArcherHelmet = content.Load<Texture2D>("StartingArcherHelmet");
			var startingBoots = content.Load<Texture2D>("StartingBoots");
			var startingShieldTexture = content.Load<Texture2D>("StartingShield");
			var goldTexture = content.Load<Texture2D>("Gold");
			var buttonTexture = content.Load<Texture2D>("Button");
			var pauseBorderTexture = content.Load<Texture2D>("PauseBorder");
			var dungeonEntranceTexture = content.Load<Texture2D>("DungeonEntrance");

            Sprite dungeonEntrance;
            //"E" button
            Sprite interactionButton;

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
							{"Idle", new Animation(content.Load<Texture2D>("Warrior"), 1, game.Scale) },
							{"WalkRight", new Animation(content.Load<Texture2D>("WalkRight"), 3, game.Scale)},
							{"WalkLeft", new Animation(content.Load<Texture2D>("WalkLeft"), 3, game.Scale) },
							{"BlockRight", new Animation(content.Load<Texture2D>("BlockRight"), 1, game.Scale) },
							{"BlockLeft", new Animation(content.Load<Texture2D>("BlockLeft"), 1, game.Scale) },
							{"BlockUp", new Animation(content.Load<Texture2D>("BlockUp"), 1, game.Scale) },
							{"FastAttack", new Animation(content.Load<Texture2D>("WarriorFastAttack"), 3, game.Scale, 0.1f) },
							{"NormalAttack", new Animation(content.Load<Texture2D>("WarriorNormalAttack"), 3, game.Scale, 0.25f) }
						};
						player = new Warrior(this, animations, Input, g.Scale, font)
						{
							Position = new Vector2(0.05f * game.Width, 0.4f * game.Height)
						};
						player.InventoryManager = new InventoryManager(gd, player, Textures["Inventory"], slotTexture, goldTexture, font, inventorySlots, new Vector2(0.55f * game.Width, 0.05f * game.Height), game.Scale);
						player.HealthBar = new HealthBar(healthBarTexture, Textures["Health"], font, new Vector2(0.03f * game.Width, 0.9f * game.Height), game.Scale);
						if (player is StaminaUser)
						{
							(player as StaminaUser).StaminaBar = new StaminaBar(healthBarTexture, Textures["Stamina"], font, new Vector2(0.03f * game.Width, 0.95f * game.Height), game.Scale);
						}
						player.InventoryManager.AddItem(new StartingSword(startingSwordTexture, game.Scale));
						player.InventoryManager.AddItem(new StartingWarriorHelmet(startingWarriorHelmet, game.Scale));
						player.InventoryManager.AddItem(new StartingWarriorBreastplate(startingWarriorBreastplate, game.Scale));
						player.InventoryManager.AddItem(new StartingBoots(startingBoots, game.Scale));
						player.InventoryManager.AddItem(new StartingShield(startingShieldTexture, game.Scale));

						player.InventoryManager.EquipmentManager = new EquipmentManager(Textures["Inventory"],
							slotTexture,
							font,
							new Vector2(0.02f * game.Width, 0.05f * game.Height),
							game.Scale)
						{
							EquipmentSlots = new List<InventorySlot>()
							{
								new WarriorHelmetSlot(gd, player, slotTexture, font, game.Scale)
								{
									Position = new Vector2(0.075f * game.Width, 0.07f * game.Height)
								},
								new NecklaceSlot(gd, player, slotTexture, font, game.Scale)
								{
									Position = new Vector2(0.075f * game.Width, 0.07f * game.Height + slotTexture.Height*game.Scale)
								},
								new WarriorBreastplateSlot(gd, player, slotTexture, font, game.Scale)
								{
									Position = new Vector2(0.075f * game.Width, 0.07f * game.Height + 2*slotTexture.Height*game.Scale)
								},
								new BootsSlot(gd, player, slotTexture, font, game.Scale)
								{
									Position = new Vector2(0.075f * game.Width, 0.07f * game.Height + 3*slotTexture.Height*game.Scale)
								},
								new SwordSlot(gd, player, slotTexture, font, game.Scale)
								{
									Position = new Vector2(0.025f * game.Width, 0.07f * game.Height + 2*slotTexture.Height*game.Scale)
								},
								new ShieldSlot(gd, player, slotTexture, font, game.Scale)
								{
									Position = new Vector2(0.125f * game.Width, 0.07f * game.Height + 1*slotTexture.Height*game.Scale),
								},
								new RingSlot(gd, player, slotTexture, font, game.Scale)
								{
									Position = new Vector2(0.125f * game.Width, 0.07f * game.Height + 2*slotTexture.Height*game.Scale)
								}
							}
						};
						break;
					}
				case PlayerClasses.Archer:
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
						player.InventoryManager = new InventoryManager(gd, player, Textures["Inventory"], slotTexture, goldTexture, font, inventorySlots, new Vector2(0.55f * game.Width, 0.05f * game.Height), game.Scale);
						player.HealthBar = new HealthBar(healthBarTexture, Textures["Health"], font, new Vector2(0.03f * game.Width, 0.9f * game.Height), game.Scale);
						if (player is StaminaUser)
						{
							(player as StaminaUser).StaminaBar = new StaminaBar(healthBarTexture, Textures["Stamina"], font, new Vector2(0.03f * game.Width, 0.95f * game.Height), game.Scale);
						}
						player.InventoryManager.AddItem(new StartingBow(startingBowTexture, game.Scale));
						player.InventoryManager.AddItem(new StartingArcherHelmet(startingArcherHelmet, game.Scale));
						player.InventoryManager.AddItem(new StartingArcherBreastplate(startingArcherBreastplate, game.Scale));
						player.InventoryManager.AddItem(new StartingBoots(startingBoots, game.Scale));

						player.InventoryManager.EquipmentManager = new EquipmentManager(Textures["Inventory"],
							slotTexture,
							font,
							new Vector2(0.02f * game.Width, 0.05f * game.Height),
							game.Scale)
						{
							EquipmentSlots = new List<InventorySlot>()
							{
								new ArcherHelmetSlot(gd, player, slotTexture, font, game.Scale)
								{
									Position = new Vector2(0.075f * game.Width, 0.07f * game.Height)
								},
								new NecklaceSlot(gd, player, slotTexture, font, game.Scale)
								{
									Position = new Vector2(0.075f * game.Width, 0.075f * game.Height + slotTexture.Height*game.Scale)
								},
								new ArcherBreastplateSlot(gd, player, slotTexture, font, game.Scale)
								{
									Position = new Vector2(0.075f * game.Width, 0.07f * game.Height + 2*slotTexture.Height*game.Scale)
								},
								new BootsSlot(gd, player, slotTexture, font, game.Scale)
								{
									Position = new Vector2(0.075f * game.Width, 0.07f * game.Height + 3*slotTexture.Height*game.Scale)
								},
								new BowSlot(gd, player, slotTexture, font, game.Scale)
								{
									Position = new Vector2(0.025f * game.Width, 0.07f * game.Height + 2*slotTexture.Height*game.Scale)
								},
								new RingSlot(gd, player, slotTexture, font, game.Scale)
								{
									Position = new Vector2(0.125f * game.Width, 0.07f * game.Height + 2*slotTexture.Height*game.Scale)
								}
							}
						};
						break;
					}
				case PlayerClasses.Wizard:
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
						break;
					}
			}

            player.InventoryManager.AddItem(
                new HealthPotion(Textures["HealthPotion"], game.Scale)
                {
                    Quantity = 5
                }
            );

			//Static background
			staticComponents = new List<Component>
			{
				new Sprite(background, g.Scale)
				{
					Position = new Vector2(0,0)
				},
			};

			dungeonEntrance = new Sprite(dungeonEntranceTexture, g.Scale)
			{
				Position = new Vector2(1.113f * game.Width, 0.4f * game.Height)
			};

			interactionButton = new Sprite(Keys[Input.KeyBindings["Interact"].ToString()], g.Scale)
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

            var shopkeeperSprite = new Sprite(shopkeeperAnimations)
            {
                Position = new Vector2(0.513f * game.Width, 0.40f * game.Height)
            };
            var shopkeeperButton = new Sprite(Keys[Input.KeyBindings["Interact"].ToString()], game.Scale);

            movingComponents = new List<Component>(optionalEntrances)
			{
				new Blacksmith(game, this, blacksmithSprite, blacksmithButton, player),
				new Shopkeeper(game, this, shopkeeperSprite, shopkeeperButton, player),
				new Sprite(Textures["box"], g.Scale)
				{
					Position = new Vector2(0.813f*game.Width,0.55f*game.Height)
				},
				new Sprite(anivilTexture, g.Scale)
				{
					Position = new Vector2(0.813f*game.Width,0.45f*game.Height)
				},
				player,
			};

			var trainingDummyAnimations = new Dictionary<string, Animation>()
			{
				{"Idle", new Animation(content.Load<Texture2D>("TrainingDummy"), 1, game.Scale)}
			};

			enemies.Add(new TrainingDummy(game, this, font, trainingDummyAnimations)
			{
				Position = new Vector2(0.95f* game.Width, 0.4f* game.Height)
			}
			);

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
				//Static background batch
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
