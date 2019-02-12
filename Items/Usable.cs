using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject
{
	public abstract class Usable : Item
	{
		public Usable(Texture2D t, float scale) : base(t, scale)
		{

		}
		public abstract void Use();
	}
}
