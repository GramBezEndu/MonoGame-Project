using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Items
{
	public abstract class Weapon : Equippable
	{
		public int DamageMin { get; protected set; }
		public int DamageMax { get; protected set; }
		public Weapon(Texture2D t, float scale) : base(t, scale)
		{
		}
	}
}
