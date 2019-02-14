using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Sprites;

namespace GameProject
{
	public class InventoryManager : Sprite
	{
		public EquipmentManager EquipmentManager { get; set; }
		public bool Hidden { get; set; }
		private List<InventorySlot> slots;
		/// <summary>
		/// Initializes new inventory
		/// </summary>
		/// <param name="quantitySlots">How many slots will inventory have</param>
		public InventoryManager(Texture2D inventoryTexture, Texture2D slotTexture, SpriteFont font, int quantitySlots, Vector2 position, float scale) : base(inventoryTexture, scale)
		{
			Hidden = true;
			Position = position;
			slots = new List<InventorySlot>();
			for(int i=0;i<quantitySlots;i++)
			{
				slots.Add(
					new InventorySlot(slotTexture, font, scale)
					{
						Position = new Vector2(this.Position.X+(slotTexture.Width*Scale*i), this.Position.Y)
					}
					);
			}
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if(!Hidden)
			{
				spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
				EquipmentManager.Draw(gameTime, spriteBatch);
				foreach (var s in slots)
					s.Draw(gameTime, spriteBatch);
			}
		}
		public override void Update(GameTime gameTime)
		{
			//It might be incorrect later, watch out! (InventorySlot Update() now checks only for player input and item Use() or Equip() so it's correct, but if you want to update here Item it won't be correct)
			if (!Hidden)
			{
				foreach (var s in slots)
					s.Update(gameTime);
			}
		}
		/// <summary>
		/// Item is simply not added to inventory if it is full
		/// </summary>
		/// <param name="i"></param>
		/// <param name="quantity"></param>
		public void AddItem(Item i, int quantity = 1)
		{
			if(i.IsStackable == true)
			{
				foreach(var s in slots)
				{
					if(s.Item == i)
					{
						s.Quantity += quantity;
						return;
					}
				}
				foreach (var s in slots)
				{
					if (s.Item == null)
					{
						s.Item = i;
						s.Quantity = quantity;
						return;
					}
				}
			}
			else
			{
				foreach(var s in slots)
				{
					if(s.Item == null)
					{
						s.Item = i;
						if(quantity != 1)
						{
							throw new Exception("Item is not stackable, can't add other value than 1");
						}
						s.Quantity = quantity;
						return;
					}
				}
			}
		}
	}
}
