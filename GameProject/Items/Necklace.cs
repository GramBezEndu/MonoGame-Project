using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Items;
using GameProject.Sprites;
using GameProject.Inventory;
using Microsoft.Xna.Framework;

namespace GameProject.Items
{
	public abstract class Necklace : Equippable
	{
		public Necklace(GraphicsDevice gd, Player p, Texture2D slotTexture, SpriteFont f, Texture2D t, Vector2 scale) : base(t, scale)
		{
			CanBeDeleted = true;
		}
		public override bool Equip(Player p)
		{
			foreach (InventorySlot i in p.InventoryManager.EquipmentManager.EquipmentSlots)
			{
				if (i is NecklaceSlot)
				{
					if (i.Item == null)
					{
						i.Item = (Item)this.Clone();
                        i.Item.Quantity = 1;
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
							i.Item = (Item)this.Clone();
                            i.Item.Quantity = 1;
							return true;
						}
					}
				}
			}
			return false;
		}
	}
}
