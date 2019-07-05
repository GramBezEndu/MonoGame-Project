using GameProject.Sprites;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GameProject.States;
using GameProject.Items;
using GameProject.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject.Inventory
{
	public class DraggableBlacksmithsSlot : DraggableSlot
	{
		Blacksmith blacksmith;
		public DraggableBlacksmithsSlot(Blacksmith b, GraphicsDevice gd, Player p, Texture2D t, SpriteFont f, float scale) : base(gd, p, t, f, scale)
		{
			blacksmith = b;
		}

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
						//Swap normally
						var item = slotDragging.Item;
						slotDragging.Item = this.Item;
						this.Item = item;
						slotDragging.IsDragging = false;
						//Upgrade cost
						if(this.Item is UpgradeableWithScroll)
						{
							var temp = (this.Item as UpgradeableWithScroll);
							blacksmith.UpgradeCost = temp.UpgradeCost;
						}
						else
						{
							blacksmith.UpgradeCost = 0;
						}
					}
					//Try to start dragging
					else if (!IsDragging)
					{
						//You can't start dragging two items -> extra check
						if (player.InventoryManager.IsAlreadyDragging())
							return;
						else
							IsDragging = true;
					}
				}
			}
		}
	}
}
