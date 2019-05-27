﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject.Animations;
using GameProject.States;

namespace GameProject.Sprites
{
	public abstract class Player : Sprite
	{
		//public Player(Texture2D t, float scale) : base(t, scale)
		//{
		//}
		public Player(GameState currentGameState, Dictionary<string, Animation> a, Input i, float scale) : base(a)
		{
			input = i;
			//Scale player distance per step
			baseMoveDistance = 1.5f * scale;
			baseSprintDistance = 3.75f * scale;
			moveDistance = baseMoveDistance;
			sprintDistance = baseSprintDistance;
			gameState = currentGameState;
		}

		protected Input input;
		protected float baseMoveDistance;
		protected float baseSprintDistance;
		protected float moveDistance;
		protected float sprintDistance;
		//Game state reference to attack enemies
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

		public InventoryManager InventoryManager { get; set; }
		public HealthBar HealthBar { get; set; }
		public override void Update(GameTime gameTime)
		{
			if (!IsDead)
			{
				//Health regen
				if (HealthToRestore > 0)
					HealthRegenTimer.Enabled = true;
				else
					HealthRegenTimer.Enabled = false;
				HealthRegen(gameTime);
				//Update keyboard states
				input.Update(gameTime);
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

				HealthBar.Update(gameTime);

				InventoryManager.Update(gameTime);
				//Update player movement speed bonus
				moveDistance = (1 + InventoryManager.EquipmentManager.MovementSpeedBonus) * baseMoveDistance;
				sprintDistance = (1 + InventoryManager.EquipmentManager.MovementSpeedBonus) * baseSprintDistance;
				//Die (should be after updating health bar)
				if (HealthBar.Health.CurrentHealth <= 0)
					Die();
			}
		}

		protected void Die()
		{
			IsDead = true;
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
				int dmg = (int)(x * (1f - this.InventoryManager.EquipmentManager.DamageReduction));
				this.HealthBar.Health.CurrentHealth -= dmg;
			}
		}

		private void HideShowInventory()
		{
			if (input.CurrentState.IsKeyDown(input.KeyBindings["ShowInventory"]) && input.PreviousState.IsKeyUp(input.KeyBindings["ShowInventory"]))
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
				if (Keyboard.GetState().IsKeyDown(input.KeyBindings["MoveRight"]))
				{
					if (Keyboard.GetState().IsKeyDown(input.KeyBindings["Sprint"]) && canSprint)
						Velocity.X += sprintDistance;
					else
						Velocity.X += moveDistance;
				}
				if (Keyboard.GetState().IsKeyDown(input.KeyBindings["MoveLeft"]))
				{
					if (Keyboard.GetState().IsKeyDown(input.KeyBindings["Sprint"]) && canSprint)
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
