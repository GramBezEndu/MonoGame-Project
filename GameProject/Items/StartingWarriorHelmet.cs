using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Sprites;

namespace GameProject.Items
{
	public class StartingWarriorHelmet : WarriorHelmet
	{
		public StartingWarriorHelmet(Texture2D t, float scale) : base(t, scale)
		{
			DamageReduction = 0.08f;
			Description = "Damage Reduction " + DamageReduction.ToString();
		}
	}
}
