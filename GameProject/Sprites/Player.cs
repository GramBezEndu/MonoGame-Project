using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject.Animations;

namespace GameProject.Sprites
{
	public abstract class Player : Sprite
	{
		//public Player(Texture2D t, float scale) : base(t, scale)
		//{
		//}
		public Player(Dictionary<string, Animation> a, float scale) : base(a)
		{
			//Scale player distance per step
			moveDistance = 1f * scale;
			sprintDistance = 2.5f * scale;
		}
		protected Input input = new Input();
		protected float moveDistance;
		protected float sprintDistance;
		protected bool canSprint = true;
		protected bool canMove = true;
		protected int healthRegen = 1;
		protected GameTimer HealthRegenTimer = new GameTimer(1f);

		public InventoryManager InventoryManager;
		public HealthBar HealthBar { get; set; }
		public override void Update(GameTime gameTime)
		{
			//Health regen
			HealthRegen(gameTime);
			//Update keyboard states
			input.Update(gameTime);
			//Hide/Show inventory (keyboard input)
			HideShowInventory();
			//Player movement (keyboard input)
			Move();
			//Play animations
			PlayAnimations();
			animationManager.Update(gameTime);
			Position += Velocity;
			Velocity = Vector2.Zero;
			HealthBar.Update(gameTime);
			InventoryManager.Update(gameTime);
		}
		private void HealthRegen(GameTime gameTime)
		{
			HealthRegenTimer.Update(gameTime);
			if (HealthRegenTimer.CurrentTime <= 0)
			{
				if (HealthBar.Health.CurrentHealth + healthRegen < HealthBar.Health.MaxHealth)
				{
					HealthBar.Health.CurrentHealth += healthRegen;
				}
				else
				{
					HealthBar.Health.CurrentHealth = HealthBar.Health.MaxHealth;
				}
				HealthRegenTimer.Restart();
			}
		}

		private void HideShowInventory()
		{
			if (input.CurrentState.IsKeyDown(input.ShowInventory) && input.PreviousState.IsKeyUp(input.ShowInventory))
			{
				if (InventoryManager.Hidden == true)
					InventoryManager.Hidden = false;
				else
					InventoryManager.Hidden = true;
			}
		}

		private void Move()
		{
			if (canMove)
			{
				if (Keyboard.GetState().IsKeyDown(input.MoveRight))
				{
					if (Keyboard.GetState().IsKeyDown(input.Sprint) && canSprint)
						Velocity.X += sprintDistance;
					else
						Velocity.X += moveDistance;
				}
				if (Keyboard.GetState().IsKeyDown(input.MoveLeft))
				{
					if (Keyboard.GetState().IsKeyDown(input.Sprint) && canSprint)
						Velocity.X -= sprintDistance;
					else
						Velocity.X -= moveDistance;
				}
			}
		}

		private void PlayAnimations()
		{
			if (Velocity.X > 0)
				animationManager.Play(animations["WalkRight"]);
			else if (Velocity.X < 0)
				animationManager.Play(animations["WalkLeft"]);
			else
				animationManager.Play(animations["Idle"]);
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			animationManager.Draw(spriteBatch);
		}
	}
}
