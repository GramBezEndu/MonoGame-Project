using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameProject.Items
{
	public abstract class Shield : Equippable
	{
		public Shield(Texture2D t, float scale) : base(t, scale)
		{
		}
	}
}
