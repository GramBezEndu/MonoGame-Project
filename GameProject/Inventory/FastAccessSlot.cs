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
	public class FastAccessSlot : DraggableSlot
	{
		private Slot actualSlot;
		public FastAccessSlot(GraphicsDevice gd, Player p, Texture2D t, SpriteFont f, Vector2 scale) : base(gd, p, t, f, scale)
		{

		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			//Update item
			if(actualSlot != null)
			{
				if(actualSlot.Item != null)
				{
					if(!(actualSlot.Item is Usable))
					{
						actualSlot = null;
						this.Item = null;
						return;
					}
				}
				this.Item = actualSlot.Item;
			}
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
						actualSlot = player.InventoryManager.WhichSlotIsDragging();
						bool usable = actualSlot.Item is Usable;
						//If dragged item is not usable then you can't assign it to fast access slot
						if (!usable)
						{
							//End dragging and reset actual slot
							actualSlot.IsDragging = false;
							actualSlot = null;
							return;
						}
						//Assign normally
						this.Item = actualSlot.Item;
						actualSlot.IsDragging = false;
					}
					////Try to start dragging
					////You can not start dragging fast access slot now
					////It should be possible to remove item from fast inventory slot (need to be implemented later)
					//else if (!IsDragging)
					//{
					//	//You can't start dragging two items -> extra check
					//	if (player.InventoryManager.IsAlreadyDragging())
					//		return;
					//	else
					//		IsDragging = true;
					//}
				}
			}
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);
			DrawQuantity(spriteBatch);
		}

		public override void DrawQuantity(SpriteBatch spriteBatch)
		{
			if (Item == null)
				return;
			if (Item.IsStackable)
				spriteBatch.DrawString(font, Item.Quantity.ToString(), this.Position, Color.Black);
		}
	}
}
