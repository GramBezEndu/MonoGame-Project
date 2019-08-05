using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.States;

namespace GameProject.Tutorials
{
	public class InventoryTutorial : Tutorial
	{
		public InventoryTutorial(GameState gs) : base(gs, new List<string>()
		{
			"If you want to equip or use item, you can do it by clicking Right Mouse Button on item",
			"You can drag items using Left Mouse Button. If you ever want to delete an item, drag it to Trash icon",
			"You can also assign usable items (Health Potions etc.) to Fast Access Slots by dragging them",
			"Fast Access Slots are visible on the bottom left corner"
		})
		{

		}

		public override bool ShouldActivate()
		{
			if (base.ShouldActivate() == false)
				return false;
			else if (GameState.ShouldActivateInventoryTut())
				return true;
			else
				return false;
		}
	}
}
