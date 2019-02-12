using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Items
{
	public class ManaPotion : Usable
	{
		public ManaPotion(Texture2D t, float scale) : base(t, scale)
		{
		}

		public override void Use()
		{
			throw new NotImplementedException();
		}
	}
}
