using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Items
{
	public class DamageRing : Ring
	{
		public DamageRing(Texture2D t, Vector2 scale) : base(t, scale)
		{
			Name = "Damage Ring";
			Attributes["BonusDamage"] = 0.15f;
			UpdateDescription();
		}
	}
}
