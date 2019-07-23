using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.States;
using Microsoft.Xna.Framework;
using GameProject.Controls;

namespace GameProject.Sprites
{
	public class StatueOfGods : InteractableSpriteWithWindows
	{
		public StatueOfGods(Game1 g, GameState gameState, Sprite mainSprite, Sprite interactButton, Player p) : base(g,gameState,mainSprite,interactButton,p)
		{
			//We want to change background sprite and exit button in this case
			UiElements.Clear();
			background = new Sprite(gameState.Textures["BackgroundBig"], g.Scale);
			Vector2 pos = new Vector2(g.Width / 2 - background.Width / 2, g.Height / 2 - background.Width / 2);
			background.Position = pos;
			background.Hidden = true;

			var exitButton = new Button(gameState.Textures["Button"], gameState.Font, g.Scale)
			{
				Text = "Exit",
				Hidden = true,
				Click = OnClose
			};
			exitButton.Position = new Vector2(background.Position.X, background.Rectangle.Bottom - exitButton.Height);
			var exitButtonPos = background.Rectangle.Bottom;
			UiElements.Add(background);
			UiElements.Add(exitButton);

			//Apply changes to state
			gameState.AddUiElements(UiElements);
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
							GameState.CreateMessage("Your health and stamina was restored");
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
