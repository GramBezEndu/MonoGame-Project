using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.States;

namespace GameProject.Tutorials
{
	public class WarriorAttackingTutorial : Tutorial
	{
		public WarriorAttackingTutorial(GameState gs) : base(gs, new List<string>()
		{
			"As a warrior you can use two types of attacks: Fast Attack (Left Mouse Button) and Normal Attack (Right Mouse Button)",
			"Fast Attack consumes more Stamina than Normal Attack and has no chance for Critical Strike",
			"Plan your attacks wisely to gain advantage in fights"
		})
		{

		}

		public override bool ShouldActivate()
		{
			if (base.ShouldActivate() == false)
				return false;
			else if (GameState.ShouldActivateAttackingTut() == true)
				return true;
			else
				return false;
		}
	}
}
