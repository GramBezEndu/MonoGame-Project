using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.States;
using Microsoft.Xna.Framework;

namespace GameProject.Sprites
{
	public class StatueOfGods : InteractableSprite
	{
		public StatueOfGods(Game1 g, GameState gameState, Sprite mainSprite, Sprite interactButton, Player p) : base(g,gameState,mainSprite,interactButton,p)
		{

		}
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			//Restore players HP, mana and stamina if player is near && all enemies are killed
			if(GameState.AllEnemiesAreKilled())
			{
				if (player.IsTouching(this.MainSprite))
				{
					if (player is StaminaUser)
					{
						//Something has to be restored -> we can display a message here
						if(player.HealthBar.Health.CurrentHealth != player.HealthBar.Health.MaxHealth ||
							(player as StaminaUser).StaminaBar.Stamina.CurrentStamina != (player as StaminaUser).StaminaBar.Stamina.MaxStamina)
						{
							player.HealthBar.Health.CurrentHealth = player.HealthBar.Health.MaxHealth;
							(player as StaminaUser).StaminaBar.Stamina.CurrentStamina = (player as StaminaUser).StaminaBar.Stamina.MaxStamina;
						}
					}
					else if (player is ManaUser)
					{
						throw new NotImplementedException();
						Console.WriteLine("o");
					}
				}
			}
		}
	}
}
