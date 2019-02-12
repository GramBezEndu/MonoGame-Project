using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject.Sprites
{
	public class Archer : Player
	{
		public Archer(Texture2D t, float scale) :base(t, scale)
		{
			texture = t;
			Scale = scale;
		}
		public override void Update(GameTime gameTime)
		{
			///Health regen -> make a method
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
			///Player movement -> make a method
			if (canMove)
			{
				if (Keyboard.GetState().IsKeyDown(input.MoveRight))
				{
					if (Keyboard.GetState().IsKeyDown(input.Sprint) && canSprint)
						Position.X += sprintDistance;
					else
						Position.X += moveDistance;
				}
				if (Keyboard.GetState().IsKeyDown(input.MoveLeft))
				{
					if (Keyboard.GetState().IsKeyDown(input.Sprint) && canSprint)
						Position.X -= sprintDistance;
					else
						Position.X -= moveDistance;
				}
			}
		}
	}
}
