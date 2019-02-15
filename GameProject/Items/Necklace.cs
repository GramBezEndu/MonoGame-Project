using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Items
{
	public abstract class Necklace : Equippable
	{
		public Necklace(Texture2D t, float scale) : base(t, scale)
		{
		}
	}
}
