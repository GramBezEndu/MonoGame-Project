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
	public abstract class Sword : Weapon
	{
		public float CriticalStrikeChance { get; protected set; }
		public Sword(Texture2D t, float scale) : base(t, scale)
		{
		}
		public override bool Equip(Player p)
		{
			foreach (InventorySlot i in p.InventoryManager.EquipmentManager.EquipmentSlots)
			{
				//We found the SwordSlot
				if (i is SwordSlot)
				{
					//BootsSlot it is empty
					if (i.Item == null)
					{
						i.Item = (Item)this.Clone();
						return true;
					}
					//SwordSlot is not empty (add old sword to inventory (if inventory is full return false) then equip new sword)
					else
					{
						if (p.InventoryManager.IsFull())
							return false;
						else
						{
							var x = i.Item;
							p.InventoryManager.AddItem(x);
							i.Item = this;
							return true;
						}
					}
				}
			}
			//We did not find SwordSlot -> return false
			return false;
		}
	}
}
