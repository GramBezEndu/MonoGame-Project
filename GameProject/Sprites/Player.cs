using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject.Animations;
using GameProject.States;
using GameProject.Inventory;
using GameProject.Controls;

namespace GameProject.Sprites
{
	public abstract class Player : Sprite
	{
		public Player(GameState currentGameState, Dictionary<string, Animation> a, Input i, Vector2 scale) : base(a)
		{
			input = i;
			//Scale player distance per step
			baseMoveDistance = 1.5f * scale.X;
			baseSprintDistance = 3.75f * scale.X;
			moveDistance = baseMoveDistance;
			sprintDistance = baseSprintDistance;
			gameState = currentGameState;
			TutorialManager = new TutorialManager(this);
		}

		protected virtual void Dead(object sender, EventArgs e)
		{
			DyingAnimationFinished = true;
			List<Component> deathComponents = new List<Component>();
			var text = new Text(gameState.Font, "YOU DIED")
			{
				Color = Color.Red,
			};
			text.Position = new Vector2(gameState.Game.Width / 2 - text.Width / 2, gameState.Game.Height / 2 - text.Height / 2);

			deathComponents.Add(text);
			gameState.AddUiElements(deathComponents);
		}

		protected Input input;
		protected float baseMoveDistance;
		protected float baseSprintDistance;
		protected float moveDistance;
		protected float sprintDistance;
		//Game state reference to attack enemies etc.
		public GameState gameState;
		protected bool canSprint = true;
		protected bool CanMove = true;
		public bool IsDead { get; protected set; }
		protected bool DyingAnimationFinished;
		/// <summary>
		/// Attack range for diffrent classes
		/// </summary>
		public float attackRange { get; protected set; }
		public int Gold { get; set; }
		/// <summary>
		/// How many health will be restored in 1 timer tick
		/// </summary>
		protected int healthRegen = 1;
		/// <summary>
		/// How many health has to be restored
		/// </summary>
		public int HealthToRestore { get; private set; }
		public GameTimer HealthRegenTimer = new GameTimer(1f);
        /// <summary>
        /// Determines if player has opened any window (blacksmith/shopkeeper etc.)
        /// </summary>
        public bool UsingWindow { get; set; }
		/// <summary>
		/// List of current active slots (while using window)
		/// </summary>
		public List<DraggableSlot> activeSlots { get; set; }

		public InventoryManager InventoryManager { get; set; }
		public HealthBar HealthBar { get; set; }

		protected TutorialManager TutorialManager;

		public override void Update(GameTime gameTime)
		{
			if (!IsDead)
			{
				base.Update(gameTime);
				//Health regen
				if (HealthToRestore > 0)
					HealthRegenTimer.Enabled = true;
				else
					HealthRegenTimer.Enabled = false;
				HealthRegen(gameTime);
				//Tutorial manager
				TutorialManager.Update(gameTime);
				//Hide/Show inventory (keyboard input)
				HideShowInventory();
				//Player movement (keyboard input)
				Move();
				//Check collision with walls etc.
				CheckCollision();
			}
			//Play animations
			animationManager.Update(gameTime);
			PlayAnimations();
			if(!IsDead)
			{
				Position += Velocity;
				//Reset velocity after updating position
				Velocity = Vector2.Zero;

				////Note: InventoryManager and Healthbar are updated in uiComponents in State (need to rethink this concept)

				//HealthBar.Update(gameTime);
				//InventoryManager.Update(gameTime);

				//Update player movement speed bonus
				moveDistance = (1 + InventoryManager.EquipmentManager.Attributes["MovementSpeed"]) * baseMoveDistance;
				sprintDistance = (1 + InventoryManager.EquipmentManager.Attributes["MovementSpeed"]) * baseSprintDistance;
				//Die (should be after updating health bar)
				if (HealthBar.Health.CurrentHealth <= 0)
					Die();
			}
		}

		protected void Die()
		{
			IsDead = true;
			gameState.Game.ChangeBackgroundSong(gameState.Songs["Death"]);
		}

		private void HealthRegen(GameTime gameTime)
		{
			HealthRegenTimer.Update(gameTime);
			//Event activated
			if (HealthRegenTimer.CurrentTime <= 0)
			{
				HealthToRestore -= healthRegen;
				if (HealthBar.Health.CurrentHealth + healthRegen < HealthBar.Health.MaxHealth)
				{
					HealthBar.Health.CurrentHealth += healthRegen;
				}
				else
				{
					HealthBar.Health.CurrentHealth = HealthBar.Health.MaxHealth;
				}
				HealthRegenTimer.Start();
			}
		}

		/// <summary>
		/// Restores X health over time
		/// </summary>
		/// <param name="x"></param>
		public void RestoreHealth(int x)
		{
			HealthToRestore += x;
		}
		/// <summary>
		/// Get damage (for example from enemy), it recaltulates the damage based on player's damage reduction
		/// </summary>
		/// <param name="x"></param>
		public void GetDamage(int x)
		{
			if(!IsDead)
			{
				int dmg = (int)(x * (1f - this.InventoryManager.EquipmentManager.Attributes["DamageReduction"]));
				this.HealthBar.Health.CurrentHealth -= dmg;
			}
		}

		private void HideShowInventory()
		{
			if (input.CurrentState.IsKeyDown(input.KeyBindings["ShowInventory"].GetValueOrDefault()) && input.PreviousState.IsKeyUp(input.KeyBindings["ShowInventory"].GetValueOrDefault()))
			{
				if (InventoryManager.Hidden == true)
					InventoryManager.Hidden = false;
				else
					InventoryManager.Hidden = true;
			}
		}
		/// <summary>
		/// Sets the player velocity
		/// </summary>
		private void Move()
		{
			if (CanMove)
			{
				if (Keyboard.GetState().IsKeyDown(input.KeyBindings["MoveRight"].GetValueOrDefault()))
				{
					if (Keyboard.GetState().IsKeyDown(input.KeyBindings["Sprint"].GetValueOrDefault()) && canSprint)
						Velocity.X += sprintDistance;
					else
						Velocity.X += moveDistance;
				}
				if (Keyboard.GetState().IsKeyDown(input.KeyBindings["MoveLeft"].GetValueOrDefault()))
				{
					if (Keyboard.GetState().IsKeyDown(input.KeyBindings["Sprint"].GetValueOrDefault()) && canSprint)
						Velocity.X -= sprintDistance;
					else
						Velocity.X -= moveDistance;
				}
			}
		}
		/// <summary>
		/// Checks for collision with walls - if player is about to dump into wall - velocity is reset
		/// </summary>
		private void CheckCollision()
		{
			foreach(var sprite in gameState.collisionSprites)
			{
				if (Velocity.X > 0 && IsTouchingLeft(sprite))
					Velocity.X = 0;
				if (Velocity.X < 0 && IsTouchingRight(sprite))
					Velocity.X = 0;
				//if ((this.Velocity.Y > 0 && this.IsTouchingTop(sprite)) || (this.Velocity.Y < 0 & this.IsTouchingBottom(sprite)))
				//	this.Velocity.Y = 0;
			}
		}

		protected abstract void PlayAnimations();

		//protected virtual void PlayAnimations()
		//{
		//	if (Velocity.X > 0)
		//		animationManager.Play(animations["WalkRight"]);
		//	else if (Velocity.X < 0)
		//		animationManager.Play(animations["WalkLeft"]);
		//	else
		//		animationManager.Play(animations["Idle"]);
		//}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			animationManager.Draw(spriteBatch);
		}
		//public void CreateEquipmentSlots()
		//{
		//	this.InventoryManager.EquipmentManager.S
		//}
	}
}
