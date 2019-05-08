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

namespace GameProject.States
{
	public abstract class GameState : State
	{
		public GameState(Game1 g, GraphicsDevice gd, ContentManager c) : base(g, gd, c)
		{
			camera = new Camera();
            LoadTextures();
			Font = content.Load<SpriteFont>("Font");
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
		protected List<Component> uiComponents;
		protected List<Component> pausedComponents;
		public List<Sprite> collisionSprites { get; protected set; } = new List<Sprite>();
		protected List<Enemy> enemies = new List<Enemy>();
        protected List<MysteriousChest> mysteriousChests = new List<MysteriousChest>();
        protected List<Item> itemsToSpawn = new List<Item>();
        protected List<Item> itemsToRemove = new List<Item>();
		protected SpriteFont Font;
        //Should add all textures to one dictionary
        public Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();

        private void LoadTextures()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(content.RootDirectory);
            if (!directoryInfo.Exists)
                throw new DirectoryNotFoundException();
            FileInfo[] files = directoryInfo.GetFiles("*.*");
            foreach (FileInfo file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file.Name);
                if (key == "Font")
                    continue;
                //Keys[key] = content.Load<Texture2D>(directoryInfo.ToString() + key);
                Textures[key] = content.Load<Texture2D>(Directory.GetCurrentDirectory() + "/Content/" + key);
            }
        }

        //TODO: Update input in state, not in player
        public override void Update(GameTime gameTime)
        {
            PreviousState = CurrentState;
            CurrentState = Keyboard.GetState();
            if (CurrentState.IsKeyDown(Input.KeyBindings["Pause"]) && PreviousState.IsKeyUp(Input.KeyBindings["Pause"]))
            {
                if (Paused)
                    Paused = false;
                else
                    Paused = true;
            }
			if(!Paused)
			{
				//Check for enemies aggro and update enemies
				foreach (var e in enemies)
				{
					e.IsPlayerClose(this.player);
					e.Update(gameTime);
				}
				//Update walls (static sprites don't need to be updated but we might add moving walls or sth later)
				foreach(var cs in collisionSprites)
				{
					cs.Update(gameTime);
				}
			}
            //Add every item that is waiting to be spawned
            SpawnItems();
            //Remove items
            RemoveItems();
            //Pick up items
            PickUpItems();
        }

        private void SpawnItems()
        {
            foreach (Item i in itemsToSpawn)
                movingComponents.Add(i);
            itemsToSpawn.Clear();
        }

        /// <summary>
        /// Spawn 3 skeleton warriors (in the beginning of the level)
        /// </summary>
        /// <param name="animations"></param>
        protected void SpawnWarriorsGroup(Dictionary<string, Animation> animations)
        {
            for (int i = 0; i < 3; i++)
            {
                enemies.Add(new SkeletonWarrior(Font, (animations), player)
                {
                    Position = new Vector2((0.7f + i * 0.1f) * game.Width, 0.6f * game.Height),
                });
            }
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
            itemsToSpawn.Add(item);
        }
        /// <summary>
        /// Remove item from scene
        /// </summary>
        /// <param name="i"></param>
        protected void RemoveItem(Item i)
        {
            itemsToRemove.Add(i);
        }
        protected void RemoveItems()
        {
            foreach(var x in itemsToRemove)
            {
                if (movingComponents.Contains(x))
                {
                    movingComponents.Remove(x);
                }
                else
                {
                    throw new Exception("Item to remove was not found\n");
                }
            }
            itemsToRemove.Clear();
        }
        protected void PickUpItems()
        {
            foreach(var i in movingComponents)
            {
                if(i is Item)
                {
                    if(player.IsTouching(i as Item) 
                        && Input.PreviousState.IsKeyDown(Input.KeyBindings["PickUp"])
                        && Input.CurrentState.IsKeyUp(Input.KeyBindings["PickUp"]))
                    {
                        RemoveItem(i as Item);
                        player.InventoryManager.AddItem(i as Item);
                    }
                }
            }
        }
		/// <summary>
		/// Call this function WHEN PLAYER IS PERFORMING ATTACK (we check for X axis only now)
		/// </summary>
		public void AttackEnemiesWithCritChance()
		{
			foreach(var e in enemies)
			{
				//Enemy should be hit
				//We should use player attack range here
				//Now we use just IsTouching function
				//if (player.Position.X > e.Position.X - player.attackRange && player.Position.X < e.Position.X + player.attackRange)
				if(player.IsTouching(e))
				{
					int dmg = game.RandomNumber(player.InventoryManager.EquipmentManager.DamageMin, player.InventoryManager.EquipmentManager.DamageMax);
					//Check if critical strike was drawn
					int criticalStrike = game.RandomPercent();
					//Was critical
					if (criticalStrike <= player.InventoryManager.EquipmentManager.CriticalStrikeChance * 100)
					{
						float multiplier = 1f;
						multiplier = game.RandomCriticalMultiplier();
						dmg = (int)(dmg * multiplier);
						//Deal damage to enemy
						e.DealDmg(dmg, true);
					}
					//Was not critical
					else
					{
						e.DealDmg(dmg, false);
					}
				}
			}
		}
		/// <summary>
		/// Call this function WHEN PLAYER IS PERFORMING ATTACK (we check for X axis only now)
		/// </summary>
		public void AttackEnemiesWithoutCrit()
		{
			foreach (var e in enemies)
			{
				//Enemy should be hit
				//We should use player attack range here
				//Now we use just IsTouching function
				//if (player.Position.X > e.Position.X - player.attackRange && player.Position.X < e.Position.X + player.attackRange)
				if (player.IsTouching(e))
				{
					int dmg = game.RandomNumber(player.InventoryManager.EquipmentManager.DamageMin, player.InventoryManager.EquipmentManager.DamageMax);
					e.DealDmg(dmg, false);
				}
			}
		}
	}
}
