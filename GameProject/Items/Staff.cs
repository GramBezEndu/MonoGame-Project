using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Sprites;
using GameProject.Inventory;

namespace GameProject.Items
{
	public abstract class Staff : Weapon
	{
		public Staff(Texture2D t, float scale) : base(t, scale)
		{
		}
		public override bool Equip(Player p)
		{
			throw new NotImplementedException();
		}
	}
}
