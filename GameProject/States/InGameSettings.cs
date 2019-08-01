using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject.States
{
	public class InGameSettings : Settings
	{
		GameState previousGameState;
		public InGameSettings(GameState gs, Game1 g, GraphicsDevice gd, ContentManager c) : base(g, gd, c)
		{
			previousGameState = gs;
		}

		protected override void ShowVideoComponents(object sender, EventArgs e)
		{
			this.CreateMessage("Video settings are available only from Main Menu");
		}

		protected override void Back(object sender, EventArgs e)
		{
			//If selected keys by player are ok we can go back to game state
			if (ValidateKeys())
			{
				Game.ChangeState(previousGameState);
			}
			//We should display a window with message that current settings are not correct
			else
			{
				CreateMessage("Selected keybindings are not correct. Make sure there is no duplicates or not assigned actions.");
			}
		}
	}
}
