using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject.Sprites;
using GameProject.Items;
using GameProject.States;

namespace GameProject.Inventory
{
	public class EquipmentSlot : InventorySlot
	{
		public EquipmentSlot(GraphicsDevice gd, Player p, Texture2D t, SpriteFont f, float scale) : base(gd, p, t, f, scale)
		{

		}

		/// <summary>
		/// You can not equip/unequip items through item dragging now
		/// Current implementation only ends dragging when trying to swap items using equipment slots
		/// </summary>
		public override void DragAndDrop()
		{
			//Item dragging
			if (this.Draggable)
			{
				if (currentState.LeftButton == ButtonState.Released && previousState.LeftButton == ButtonState.Pressed)
				{
					//End dragging within the same slot
					if (IsDragging)
						IsDragging = false;
					//End dragging within different slot
					else if (player.InventoryManager.IsAlreadyDragging())
					{
						Slot slotDragging = player.InventoryManager.WhichSlotIsDragging();
						slotDragging.IsDragging = false;
						////Swap normally
						//var item = slotDragging.Item;
						//slotDragging.Item = this.Item;
						//this.Item = item;
						//slotDragging.IsDragging = false;
						////Swap equipment Slot
						///If atleast one of two slots is equipment slot we have to handle it differently
						//if (slotDragging is EquipmentSlot)
						//{
						//	if(this.Item is Equippable)
						//	{
						//		Item temp = slotDragging.Item;
						//		bool result = (this.Item as Equippable).Equip(player);
						//		if(result)
						//		{
						//			if(this is EquipmentSlot)
						//			{
						//				//It will never be correct
						//			}
						//			else
						//			{
						//				this.Item = temp;
						//			}
						//		}
						//	}
						//	//Any errors - end dragging
						//	else
						//	{
						//		slotDragging.IsDragging = false;
						//		return;
						//	}
						//}
					}
					//Try to start dragging
					else if (!IsDragging)
					{
						//You can't start dragging two items -> extra check
						if (player.InventoryManager.IsAlreadyDragging())
							return;
						else
						{
							IsDragging = true;
							IsDragging = false;
						}
					}
				}
			}
		}
	}
}
