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

namespace GameProject.Inventory
{
	public class DraggableBlacksmithSlot : DraggableSlot
	{
		Blacksmith blacksmith;
		public DraggableBlacksmithSlot(Blacksmith b, GraphicsDevice gd, Player p, Texture2D t, SpriteFont f, Vector2 scale) : base(gd, p, t, f, scale)
		{
			blacksmith = b;
		}

		//public override void DragAndDrop()
		//{
		//	//Item dragging
		//	if (this.Draggable)
		//	{
		//		if (currentState.LeftButton == ButtonState.Released && previousState.LeftButton == ButtonState.Pressed)
		//		{
		//			//End dragging within the same slot
		//			if (IsDragging)
		//				IsDragging = false;
		//			//End dragging within different slot
		//			else if (player.InventoryManager.IsAlreadyDragging())
		//			{
		//				Slot slotDragging = player.InventoryManager.WhichSlotIsDragging();
		//				//Swap normally
		//				var item = slotDragging.Item;
		//				slotDragging.Item = this.Item;
		//				this.Item = item;
		//				slotDragging.IsDragging = false;
		//				////Update description (upgrade cost)
		//				//if(Item is UpgradeableWithScroll)
		//				//{
		//				//	blacksmith.UpgradeCost = (Item as UpgradeableWithScroll).UpgradeCost;
		//				//}
		//				//else
		//				//{
		//				//	blacksmith.UpgradeCost = 0;
		//				//}
		//			}
		//			//Try to start dragging
		//			else if (!IsDragging)
		//			{
		//				//You can't start dragging two items -> extra check
		//				if (player.InventoryManager.IsAlreadyDragging())
		//					return;
		//				else
		//					IsDragging = true;
		//			}
		//		}
		//	}
		//}
	}
}
