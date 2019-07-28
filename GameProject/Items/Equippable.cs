using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Sprites;
using GameProject.Inventory;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;

namespace GameProject.Items
{
	public abstract class Equippable : Item
	{
		/// <summary>
		/// Contains every possible equippable item attribute
		/// </summary>
		public Dictionary<string, float> Attributes = new Dictionary<string, float>();
		//public float MovementSpeed { get; set; }
		public Equippable(Texture2D t, Vector2 scale) : base(t, scale)
		{
			//All equippable items can not be deleted unless stated different in class (now: rings and necklaces can be deleted)
			CanBeDeleted = false;
			//Attack value from <min;max> range
			Attributes.Add("DamageMin", 0);
			Attributes.Add("DamageMax", 0);
			Attributes.Add("DamageReduction", 0f);
			//Movement speed bonus
			Attributes.Add("MovementSpeed", 0f);
			Attributes.Add("CriticalStrikeChance", 0f);
			//Damage increase in %
			Attributes.Add("BonusDamage", 0f);
		}

		/// <summary>
		/// Update description after modyfing attributes (it should be probably called in constructor)
		/// </summary>
		public virtual void UpdateDescription()
		{
			//We need to clear previous description (used when item is being upgraded with scroll)
			Description = "";
			//new Dictionary containing only attributes that have real values (not 0)
			Dictionary<string, float> temp = Attributes.Where(p => p.Value != 0).ToDictionary(p => p.Key, p => p.Value);
			foreach(var a in temp)
			{
				//NOTE: It will only work if DamageMin and DamageMax are next to each other in dictionary
				if(a.Key == "DamageMin")
				{
					Description += "Damage: " + a.Value + "-";
				}
				else if(a.Key == "DamageMax")
				{
					Description += a.Value + "\n";
				}
				else
				{
					//No \n character at last item
					if(a.Equals(temp.Last()))
					{
						Description += Regex.Replace(a.Key, "(\\B[A-Z])", " $1") + ": " + a.Value * 100 + "%";
					}
					else
					{
						Description += Regex.Replace(a.Key, "(\\B[A-Z])", " $1") + ": " + a.Value * 100 + "%\n";
					}
				}
			}
		}

		public abstract bool Equip(Player p);

		//public virtual bool Equip(Player p)
		//{
		//	string realType = this.GetType().ToString();
		//	switch(realType)
		//	{
		//		case "Sword":
		//			int x;
		//			return true;
		//		default:
		//			throw new Exception("Item type not handled\n");
		//	}
		//}
	}
}
