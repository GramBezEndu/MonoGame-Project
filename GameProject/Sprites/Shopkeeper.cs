using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.Animations;

namespace GameProject.Sprites
{
	public class Shopkeeper : Character
	{
		public Shopkeeper(Dictionary<string, Animation> a) : base(a) { }
		public override void Interact()
		{
			throw new NotImplementedException();
		}
	}
}
