using GameProject.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject.Tutorials
{
	public class BlacksmithTutorial : Tutorial
	{
		public BlacksmithTutorial(GameState gs) : base(gs, new List<string>
		{
			"Blacksmith can make new equipment for you",
			"He can also fuse improvements scrolls to make them more powerful",
			"He can also attach scrolls to {Green} items and remove them if you have Purification Stone",
			"Some people say his prices are high, but he's very talented - he can do any order in few seconds!"
		}) 
		{

		}

		public override bool ShouldActivate()
		{
			if (base.ShouldActivate() == false)
				return false;
			else if (GameState.ShouldActivateBlacksmithTut())
				return true;
			else
				return false;
		}
	}
}
