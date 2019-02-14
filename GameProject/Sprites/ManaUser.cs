using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.Animations;

namespace GameProject.Sprites
{
	public abstract class ManaUser : Player
	{
		public ManaUser(Dictionary<string, Animation> a, float scale) : base(a, scale)
		{
		}
	}
}
