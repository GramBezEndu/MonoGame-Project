using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Items
{
	public abstract class Ring : Equippable
	{
		public Ring(Texture2D t, float scale) : base(t, scale)
		{
		}
	}
}
