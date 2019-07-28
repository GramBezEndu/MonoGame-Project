using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Items
{
	public class StartingShield : Shield
	{
		public StartingShield(Texture2D t, Vector2 scale) : base(t, scale)
		{
			BlockingDamageReduction = 0.4f;
			Description = "Damage Reduction while blocking: " + BlockingDamageReduction * 100 + "%";
		}
	}
}
