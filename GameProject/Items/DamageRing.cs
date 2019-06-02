using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Items
{
	public class DamageRing : Ring
	{
		public DamageRing(Texture2D t, float scale) : base(t, scale)
		{
			Attributes["BonusDamage"] = 0.15f;
			UpdateDescription();
		}
	}
}
