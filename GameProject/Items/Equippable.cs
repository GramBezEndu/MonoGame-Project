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
		/// <summary>
		/// Contains every possible equippable item attribute
		/// </summary>
		public Dictionary<string, float> Attributes = new Dictionary<string, float>();
		//public float DamageReduction { get; set; }
		//public float MagicDamageReduction { get; set; }
		//public float MovementSpeed { get; set; }
		public Equippable(Texture2D t, float scale) : base(t, scale)
		{
			Attributes.Add("DamageReduction", 0f);
			Attributes.Add("MagicDamageReduction", 0f);
			//Movement speed bonus
			Attributes.Add("MovementSpeed", 0f);
			Attributes.Add("CriticalStrikeChance", 0f);
			//Attack value from <min;max> range
			Attributes.Add("DamageMin", 0f);
			Attributes.Add("DamageMax", 0f);
			//Damage increase in %
			Attributes.Add("BonusDamage", 0f);
		}
		public abstract bool Equip(Player p);
	}
}
