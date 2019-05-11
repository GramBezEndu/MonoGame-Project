﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Sprites;
using GameProject.Inventory;

namespace GameProject.Items
{
	public abstract class Boots : Equippable
	{
		public Boots(Texture2D t, float scale) : base(t, scale)
		{
		}
		public override bool Equip(Player p)
		{
			foreach (InventorySlot i in p.InventoryManager.EquipmentManager.EquipmentSlots)
			{
				//We found the BootsSlot
				if (i is BootsSlot)
				{
					//BootsSlot is empty
					if (i.Item == null)
					{
						i.Item = (Item)this.Clone();
						return true;
					}
					//BootsSlot is not empty (add old boots to inventory (if inventory is full return false) then equip new boots)
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
			//We did not find BootsSlot -> return false
			return false;
		}
	}
}
