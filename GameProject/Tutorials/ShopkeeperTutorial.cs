using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.States;
using GameProject.Controls;

namespace GameProject.Tutorials
{
	public class ShopkeeperTutorial : Tutorial
	{
		public ShopkeeperTutorial(GameState gs) : base(gs, new List<string>() { "You can buy items using Right Mouse Button" })
		{
		}

		public override bool ShouldActivate()
		{
			if (base.ShouldActivate() == false)
				return false;
			else if (GameState.ShouldActivateShopkeeperTut())
				return true;
			else
				return false;
		}
	}
}
