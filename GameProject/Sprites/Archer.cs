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
	public class Archer : Player
	{
		public Archer(Dictionary<string, Animation> a, float scale) : base(a, scale)
		{
			animations = a;
			animationManager = new AnimationManager(a.First().Value);
			//Scale = scale;
		}
		//public Archer(Texture2D t, float scale) :base(t, scale)
		//{
		//	texture = t;
		//	Scale = scale;
		//}
		public override void Update(GameTime gameTime)
		{
			///Health regen
			HealthRegen(gameTime);
			///Player movement
			Move();
			//Play animations
			PlayAnimations();
			animationManager.Update(gameTime);
			Position += Velocity;
			Velocity = Vector2.Zero;
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

		private void HealthRegen(GameTime gameTime)
		{
			HealthRegenTimer.Update(gameTime);
			if (HealthRegenTimer.CurrentTime <= 0)
			{
				if (currentHealth + healthRegen < maxHealth)
				{
					currentHealth += healthRegen;
				}
				else
				{
					currentHealth = maxHealth;
				}
				HealthRegenTimer.Restart();
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
	}
}
