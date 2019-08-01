using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.States;

namespace GameProject.Tutorials
{
	public class StatueOfGodsTutorial : Tutorial
	{
		public StatueOfGodsTutorial(GameState gs) : 
			base(gs, new List<string>() { "This statue will restore your health and stamina when there's no enemies on stage", "You can also use it so save your progress" })
		{
			
		}

		public override void CheckForActivation()
		{
			base.CheckForActivation();
			if (GameState.ShouldActivateStatueTut() == true)
				Activated = true;
		}
	}
}
