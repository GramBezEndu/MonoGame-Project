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
	public class InventorySlot : DraggableSlot
	{
		/// <summary>
		/// Trashcan sprite reference to delete items
		/// </summary>
		public Sprite Trashcan { get; set; }
		public InventorySlot(GraphicsDevice gd, Player p, Texture2D t, SpriteFont f, float scale) : base(gd, p, t, f, scale)
		{
			//Message to display if you can't use/equip item

			//Every inventory slot (and classes inherited from this class) are draggable
			Draggable = true;
			invalidUse = "You can't use this item";
			SetInvalidUsageBackgroundSprite();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			var mouseRectangle = new Rectangle(currentState.X, currentState.Y, 1, 1);
			if (mouseRectangle.Intersects(Rectangle))
			{
				if (currentState.RightButton == ButtonState.Released && previousState.RightButton == ButtonState.Pressed)
				{
					//Reset dragging flag
					IsDragging = false;

					//Click?.Invoke(this, new EventArgs());
					if (Item == null)
						return;

					//Usable
					if (Item is Usable)
					{
						bool result = (Item as Usable).Use(player);
						if (result == true)
							Item.Quantity -= 1;
						else
							invalidUseTime = 2f;
					}
					else if (Item is Equippable)
					{
						bool result = (Item as Equippable).Equip(player);
						if (result == true)
							Item.Quantity--;
						else
							invalidUseTime = 2f;
					}
				}
			}
			//Check if player wants to delete current item
			//Player still might want to drop an item on the ground (not added yet)
			//This check is equal to: If real type is InventorySlot (should be rewritten later)
			else if(Trashcan != null)
			{
				if (mouseRectangle.Intersects(Trashcan.Rectangle))
				{
					if (currentState.LeftButton == ButtonState.Released && previousState.LeftButton == ButtonState.Pressed)
					{
						if (player.InventoryManager.IsAlreadyDragging())
						{
							var slot = player.InventoryManager.WhichSlotIsDragging();
							if (slot.Item == null)
							{
								slot.IsDragging = false;
								return;
							}
							else if(slot.Item.CanBeDeleted)
							{
								slot.Item = null;
								slot.IsDragging = false;
							}
							else
							{
								//Message: This item can not be deleted
								slot.IsDragging = false;
							}
						}
					}
				}
			}
			//Item could be used or equipped
			if (Item?.Quantity <= 0)
            {
                Item = null;
            }
        }
	}
}
