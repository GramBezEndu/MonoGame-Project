using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.Animations;
using Microsoft.Xna.Framework;

namespace GameProject.Sprites
{
	public class Blacksmith : Character
	{
		public Blacksmith(Dictionary<string, Animation> a) : base(a) { }
		public override void Interact()
		{
			throw new NotImplementedException();
		}
	}
}
