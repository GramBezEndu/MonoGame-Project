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
	public abstract class Equippable : Item
	{
		public float DamageReduction { get; set; }
		public float MagicDamageReduction { get; set; }
		public float MovementSpeed { get; set; }
		public Equippable(Texture2D t, float scale) : base(t, scale)
		{
		}
		public abstract bool Equip(Player p);
	}
}
