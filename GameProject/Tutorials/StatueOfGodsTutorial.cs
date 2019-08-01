using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.States;
using GameProject.Sprites;

namespace GameProject.Tutorials
{
	public class StatueOfGodsTutorial : Tutorial
	{
		public StatueOfGodsTutorial(GameState gs, Player p) : 
			base(gs)
		{
			if (p is StaminaUser)
			{
				InitializeMessages(new List<string>()
				{ "This statue will restore your health and stamina when there's no enemies nearby",
					"You can also use it so save your progress" }
				);
			}
			else if (p is ManaUser)
			{
				InitializeMessages(new List<string>()
				{ "This statue will restore your health and mana when there's no enemies nearby",
					"You can also use it so save your progress" }
				);
			}
			else
				throw new Exception("Invalid character type");
		}

		public override bool ShouldActivate()
		{
			if (base.ShouldActivate() == false)
				return false;
			else if(GameState.ShouldActivateStatueTut())
				return true;
			else
				return false;
		}
	}
}
