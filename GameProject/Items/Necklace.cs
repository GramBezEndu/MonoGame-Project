using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Items;
using GameProject.Sprites;
using GameProject.Inventory;

namespace GameProject.Items
{
	public abstract class Necklace : Equippable
	{
		public Necklace(Texture2D t, float scale) : base(t, scale)
		{
		}
		public override bool Equip(Player p)
		{
			foreach (InventorySlot i in p.InventoryManager.EquipmentManager.EquipmentSlots)
			{
				if (i is NecklaceSlot)
				{
					if (i.Item == null)
					{
						i.Item = this;
						i.Quantity = 1;
						return true;
					}
					else
					{
						if (p.InventoryManager.IsFull())
							return false;
						else
						{
							var x = i.Item;
							p.InventoryManager.AddItem(x);
							i.Item = this;
							i.Quantity = 1;
							return true;
						}
					}
				}
			}
			return false;
		}
	}
}
