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
using GameProject.Sprites;
using GameProject.Animations;
using GameProject.Items;

namespace GameProject.States
{
	public abstract class GameState : State
	{
		public GameState(Game1 g, GraphicsDevice gd, ContentManager c) : base(g, gd, c)
		{
			camera = new Camera();
			PickUpPrompt = new Sprite(Keys[Input.KeyBindings["PickUp"].ToString()], g.Scale)
			{
				Hidden = true
			};
        }
		//public static bool SpeedrunTimerEnabled = false;
		//public static GameTimer SpeedrunTimer = new GameTimer(0f, true);
		protected bool Paused { get; set; }
		protected KeyboardState PreviousState { get; set; }
		protected KeyboardState CurrentState { get; set; }
		protected Camera camera;
		//protected EnemyManager enemyManager;
		protected Player player;
		protected List<Component> movingComponents;
		protected List<Component> uiComponents = new List<Component>();
		protected List<Component> pausedComponents;
		public List<Sprite> collisionSprites { get; protected set; } = new List<Sprite>();
		protected List<Enemy> enemies = new List<Enemy>();
        protected List<MysteriousChest> mysteriousChests = new List<MysteriousChest>();

		/// <summary>
		/// List of moving components that are waiting to be spawned on state (Items, projectiles etc.)
		/// </summary>
        protected List<Component> movingComponentsToSpawn = new List<Component>();
		/// <summary>
		/// List of moving components that are waiting to be removed from state (Items, projectiles etc.)
		/// </summary>
		protected List<Component> movingComponentsToRemove = new List<Component>();

		/// <summary>
		/// Displays a sprite with key to pick up item if player is touching item
		/// </summary>
		protected Sprite PickUpPrompt;

        //TODO: Update input in state, not in player
        public override void Update(GameTime gameTime)
        {
			base.Update(gameTime);
            //Manage pause on button press
            if (Input.CurrentState.IsKeyDown(Input.KeyBindings["Pause"]) && Input.PreviousState.IsKeyUp(Input.KeyBindings["Pause"]))
            {
                if (Paused)
                    Paused = false;
                else
                    Paused = true;
            }
            //Note: If the window has no focus, we should automaticly pause the game
			if(!Paused)
			{
				//Update enemies
				foreach (var e in enemies)
				{
					e.Update(gameTime);
				}
				//Update walls (static sprites don't need to be updated but we might add moving walls or sth later)
				foreach(var cs in collisionSprites)
				{
					cs.Update(gameTime);
				}
			}
            //Add every item, projectile etc. that is waiting to be spawned
            SpawnMovingComponents();
            //Remove items, projectiles from state
            RemoveMovingComponents();
            //Pick up items
            PickUpItems();
        }

        private void SpawnMovingComponents()
        {
            foreach (Component i in movingComponentsToSpawn)
                movingComponents.Add(i);
            movingComponentsToSpawn.Clear();
        }

        /// <summary>
        /// Spawn 3 skeleton warriors in the beginning of the level
        /// </summary>
        /// <param name="animations"></param>
        protected void SpawnWarriorsGroup()
        {
			//skeleton warrior animations
			var animations = new Dictionary<string, Animation>()
			{
				{"Idle", new Animation(content.Load<Texture2D>("Skeleton/Warrior/Idle"), 1, game.Scale) },
				{"Attack", new Animation(content.Load<Texture2D>("Skeleton/Warrior/Attack"), 3, game.Scale, 0.2f) },
				{"Run", new Animation(content.Load<Texture2D>("Skeleton/Warrior/Run"), 3, game.Scale, 0.5f) },
				{"Die", new Animation(content.Load<Texture2D>("Skeleton/Warrior/Die"), 3, game.Scale) },
				{"Dead", new Animation(content.Load<Texture2D>("Skeleton/Warrior/Dead"), 1, game.Scale) }
			};
			for (int i = 0; i < 3; i++)
            {
                enemies.Add(new SkeletonWarrior(game, this, Font, (animations), player)
                {
                    Position = new Vector2((0.7f + i * 0.1f) * game.Width, 0.6f * game.Height),
                });
            }
        }

		/// <summary>
		/// Spawn skeleton group (2 warriors, 1 archer) in the beginning of the level
		/// </summary>
		protected void SpawnSkeletonGroup()
		{
			//skeleton warrior animations
			var warriorAnimations = new Dictionary<string, Animation>()
			{
				{"Idle", new Animation(content.Load<Texture2D>("Skeleton/Warrior/Idle"), 1, game.Scale) },
				{"Attack", new Animation(content.Load<Texture2D>("Skeleton/Warrior/Attack"), 3, game.Scale, 0.2f) },
				{"Run", new Animation(content.Load<Texture2D>("Skeleton/Warrior/Run"), 3, game.Scale, 0.5f) },
				{"Die", new Animation(content.Load<Texture2D>("Skeleton/Warrior/Die"), 3, game.Scale) },
				{"Dead", new Animation(content.Load<Texture2D>("Skeleton/Warrior/Dead"), 1, game.Scale) }
			};
			var archerAnimations = new Dictionary<string, Animation>()
			{
				{"Idle", new Animation(content.Load<Texture2D>("Skeleton/Archer/Idle"), 1, game.Scale) },
				{"Attack", new Animation(content.Load<Texture2D>("Skeleton/Archer/Attack"), 3, game.Scale, 0.2f) },
				{"Run", new Animation(content.Load<Texture2D>("Skeleton/Archer/Run"), 3, game.Scale, 0.5f) },
				{"Die", new Animation(content.Load<Texture2D>("Skeleton/Archer/Die"), 3, game.Scale) },
				{"Dead", new Animation(content.Load<Texture2D>("Skeleton/Archer/Dead"), 1, game.Scale) }
			};

			for (int i = 0; i < 3; i++)
			{
				//Middle - archer
				if(i == 1)
				{
					enemies.Add(new SkeletonArcher(game, this, Font, (archerAnimations), player)
					{
						Position = new Vector2((0.7f + i * 0.1f) * game.Width, 0.6f * game.Height),
					});
				}
				//Warrior
				else
				{
					enemies.Add(new SkeletonWarrior(game, this, Font, (warriorAnimations), player)
					{
						Position = new Vector2((0.7f + i * 0.1f) * game.Width, 0.6f * game.Height),
					});
				}
			}
		}

		protected void SpawnShopkeeper(Vector2 position)
		{
			var shopkeeperAnimations = new Dictionary<string, Animation>()
			{
				{"Idle", new Animation(content.Load<Texture2D>("Shopkeeper"), 1, game.Scale) },
			};
			var shopkeeperSprite = new Sprite(shopkeeperAnimations)
			{
				Position = position
			};
			var shopkeeperButton = new Sprite(Keys[Input.KeyBindings["Interact"].ToString()], game.Scale);
			movingComponents.Add(new Shopkeeper(game, this, shopkeeperSprite, shopkeeperButton, player));
		}

		protected void SpawnBlacksmith(Vector2 posiition)
		{
			var blackSmithAnimations = new Dictionary<string, Animation>()
			{
				{"Idle", new Animation(content.Load<Texture2D>("Blacksmith"), 3, game.Scale) },
			};
			var blacksmithSprite = new Sprite(blackSmithAnimations)
			{
				Position = posiition
			};
			var blacksmithButton = new Sprite(Keys[Input.KeyBindings["Interact"].ToString()], game.Scale);
			movingComponents.Add(new Blacksmith(game, this, blacksmithSprite, blacksmithButton, player));
			movingComponents.Add(new Sprite(Textures["box"], game.Scale)
			{
				Position = new Vector2(posiition.X, posiition.Y + 0.21f*game.Height)
			});
			movingComponents.Add(new Sprite(Textures["Anvil"], game.Scale)
			{
				Position = new Vector2(posiition.X, posiition.Y + 0.11f * game.Height)
			});
		}

		protected void SpawnStatueOfGods(Vector2 position)
		{
			var Statue = new Sprite(Textures["StatueOfGods"], game.Scale)
			{
				Position = position
			};
			var button = new Sprite(Keys[Input.KeyBindings["Interact"].ToString()], game.Scale);
			movingComponents.Add(new StatueOfGods(game, this, Statue, button, player));
		}
		protected void SpawnTrainingDummy(Vector2 position)
		{
			var trainingDummyAnimations = new Dictionary<string, Animation>()
			{
				{"Idle", new Animation(Textures["TrainingDummy"], 1, game.Scale)}
			};

			enemies.Add(new TrainingDummy(game, this, Font, trainingDummyAnimations, player)
			{
				Position = position
			}
			);
		}

        protected void SpawnMysteriousChest(Vector2 position)
        {
            var animations = new Dictionary<string, Animation>()
            {
                {"MysteriousChest", new Animation(content.Load<Texture2D>("MysteriousChest"), 1, game.Scale) },
                {"MysteriousChestOpen", new Animation(content.Load<Texture2D>("MysteriousChestOpen"), 2, game.Scale) },
                {"MysteriousChestOpened", new Animation(content.Load<Texture2D>("MysteriousChestOpened"), 1, game.Scale) }
            };
            mysteriousChests.Add(new MysteriousChest(game,
                this,
                new Sprite(animations)
                {
                    Position = position
                },
                //new Sprite(Textures["MysteriousChest"], game.Scale)
                //{
                //    Position = position
                //},
                new Sprite(Keys[Input.KeyBindings["Interact"].ToString()], game.Scale),
                player));
        }
        /// <summary>
        /// Spawn any item
        /// </summary>
        /// <param name="item"></param>
        public void SpawnItem(Item item)
        {
            movingComponentsToSpawn.Add(item);
        }
        /// <summary>
        /// Remove item from scene
        /// </summary>
        /// <param name="i"></param>
        protected void RemoveItem(Item i)
        {
            movingComponentsToRemove.Add(i);
        }

		/// <summary>
		/// Remove every moving component: items that were picked up and old projectiles
		/// </summary>
        protected void RemoveMovingComponents()
        {
            foreach(var x in movingComponentsToRemove)
            {
                if (movingComponents.Contains(x))
                {
                    movingComponents.Remove(x);
                }
                else
                {
                    throw new Exception("Moving component to remove was not found\n");
                }
            }
            movingComponentsToRemove.Clear();
			//Remove old projectiles -> if projectile is Hidden then it is not needed anymore
			movingComponents.RemoveAll(i => i is Projectile && i.Hidden == true);
        }
        protected void PickUpItems()
        {
			bool promptVisible = false;
            foreach(var i in movingComponents)
            {
                if(i is Item)
                {
                    //Gold is pick up automaticly
                    if(i is GoldCoin)
                    {
                        if(player.IsTouching(i as Item))
                        {
                            RemoveItem(i as Item);
                            player.InventoryManager.AddItem(i as Item);
                        }
                    }
					//We could display pick up button to make it more clear (default: Z)
                    //Else you have to use pick up key
                    else if(player.IsTouching(i as Item))
					{
						promptVisible = true;
						PickUpPrompt.Position = new Vector2(
							(i as Item).Position.X + (i as Item).Width / 2 - PickUpPrompt.Width/2,
							(i as Item).Position.Y - PickUpPrompt.Height);
						PickUpPrompt.Hidden = false;
						if (Input.PreviousState.IsKeyDown(Input.KeyBindings["PickUp"]) && Input.CurrentState.IsKeyUp(Input.KeyBindings["PickUp"]))
						{
							RemoveItem(i as Item);
							player.InventoryManager.AddItem(i as Item);
						}
					}
                }
            }
			if (!promptVisible)
				PickUpPrompt.Hidden = true;
        }

		/// <summary>
		/// Note: Training dummy is not treated like an enemy in this method
		/// </summary>
		/// <returns></returns>
		public bool AllEnemiesAreKilled()
		{
			foreach(var e in enemies)
			{
				//Skip training dummy
				if (e is TrainingDummy)
					continue;
				if (!e.IsDead)
					return false;
			}
			return true;
		}

        /// <summary>
        /// Add UI elements (they are displayed in UI spritebatch)
        /// </summary>
        /// <param name="components"></param>
        public void AddUiElements(List<Component> components)
        {
            foreach(var c in components)
            {
                uiComponents.Add(c);
            }
        }
		/// <summary>
		/// Call this function WHEN PLAYER IS PERFORMING ATTACK (we check for X axis only now)
		/// </summary>
		public void MeleeAttackWithCrit()
		{
			foreach(var e in enemies)
			{
				//Enemy should be hit
				//We should use player attack range here
				//Now we use just IsTouching function
				//if (player.Position.X > e.Position.X - player.attackRange && player.Position.X < e.Position.X + player.attackRange)
				if(player.IsTouching(e))
				{
					int dmg = game.RandomNumber((int)(player.InventoryManager.EquipmentManager.Attributes["DamageMin"] * (1+ player.InventoryManager.EquipmentManager.Attributes["BonusDamage"])),
						(int)(player.InventoryManager.EquipmentManager.Attributes["DamageMax"] * (1 + player.InventoryManager.EquipmentManager.Attributes["BonusDamage"])));
					//Check if critical strike was drawn
					int criticalStrike = game.RandomPercent();
					//Was critical
					if (criticalStrike <= player.InventoryManager.EquipmentManager.Attributes["CriticalStrikeChance"] * 100)
					{
						float multiplier = 1f;
						multiplier = game.RandomCriticalMultiplier();
						dmg = (int)(dmg * multiplier);
						//Deal damage to enemy
						e.GetDamage(dmg, true);
					}
					//Was not critical
					else
					{
						e.GetDamage(dmg, false);
					}
				}
			}
		}
		/// <summary>
		/// Call this function WHEN PLAYER IS PERFORMING ATTACK (we check for X axis only now)
		/// </summary>
		public void MeleeAttackWithoutCrit()
		{
			foreach (var e in enemies)
			{
				//Enemy should be hit
				//We should use player attack range here
				//Now we use just IsTouching function
				//if (player.Position.X > e.Position.X - player.attackRange && player.Position.X < e.Position.X + player.attackRange)
				if (player.IsTouching(e))
				{
					int dmg = game.RandomNumber((int)(player.InventoryManager.EquipmentManager.Attributes["DamageMin"] * (1 + player.InventoryManager.EquipmentManager.Attributes["BonusDamage"])),
						(int)(player.InventoryManager.EquipmentManager.Attributes["DamageMax"] * (1 + player.InventoryManager.EquipmentManager.Attributes["BonusDamage"])));
					e.GetDamage(dmg, false);
				}
			}
		}

		public void SpawnProjectile(Projectile projectile)
		{
			movingComponentsToSpawn.Add(projectile);
		}
	}
}
