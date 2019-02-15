using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject.Animations;
using GameProject.Sprites;

namespace GameProject
{
	public class StaminaUser : Player
	{
		GameTimer StaminaRegenTimer = new GameTimer(1f, true);
		//Restore x stamina every tick
		int staminaRegen = 5;
		public StaminaBar StaminaBar { get; set; }
		public StaminaUser(Dictionary<string, Animation> a, float scale) : base(a, scale) { }
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			//Stamina regen
			StaminaRegenTimer.Update(gameTime);
			if (StaminaRegenTimer.CurrentTime <= 0)
			{
				if (StaminaBar.Stamina.CurrentStamina + staminaRegen < StaminaBar.Stamina.MaxStamina)
				{
					StaminaBar.Stamina.CurrentStamina += staminaRegen;
				}
				else
				{
					StaminaBar.Stamina.CurrentStamina = StaminaBar.Stamina.MaxStamina;
				}
				StaminaRegenTimer.Restart();
			}
		}
	}
}
