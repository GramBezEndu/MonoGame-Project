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
	public class InventorySlot : DraggableSlot
	{
		/// <summary>
		/// Trashcan sprite reference to delete items
		/// </summary>
		public Sprite Trashcan { get; set; }
		GameState GameState;
		public InventorySlot(GameState gs, GraphicsDevice gd, Player p, Texture2D t, SpriteFont f, float scale) : base(gd, p, t, f, scale)
		{
			GameState = gs;
			//Every inventory slot (and classes inherited from this class) are draggable
			Draggable = true;
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
							GameState.CreateMessage("You can't use this item");
					}
					else if (Item is Equippable)
					{
						bool result = (Item as Equippable).Equip(player);
						if (result == true)
							Item.Quantity--;
						else
							GameState.CreateMessage("You can't equip this item");
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
								//NOTE: We should delete all references to item (it can be added to fast access slot)
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
        }
	}
}
