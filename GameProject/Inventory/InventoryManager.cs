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
using GameProject.Inventory;
using GameProject.States;

namespace GameProject
{
	public class InventoryManager : Sprite
	{
		private Player player;
		private Texture2D goldTexture;
		private Vector2 goldTexturePos;
		private Vector2 goldCountTextPos;
		private SpriteFont font;
		public EquipmentManager EquipmentManager { get; set; }
		public AccessSlotsManager AccessSlotsManager { get; set; }
		public bool Hidden { get; set; }
		private List<InventorySlot> slots;
		private Sprite Trashcan;
		private GameState GameState;
		/// <summary>
		/// Initializes new inventory
		/// </summary>
		/// <param name="quantitySlots">How many slots will inventory have</param>
		public InventoryManager(GameState gs, GraphicsDevice gd, Player p, Texture2D inventoryTexture, Texture2D slotTexture, Texture2D gold, Texture2D trashcan, SpriteFont f, int quantitySlots, Vector2 position, float scale) : base(inventoryTexture, scale)
		{
			GameState = gs;
			Hidden = true;
			Position = position;
			player = p;
			goldTexture = gold;
			font = f;
			Trashcan = new Sprite(trashcan, scale)
			{
				Position = new Vector2(Position.X + 0.01f * inventoryTexture.Width * scale, Position.Y + 0.75f * inventoryTexture.Height * scale)
			};
			goldTexturePos = new Vector2(Position.X + 0.6f * inventoryTexture.Width * scale, Position.Y + 0.8f * inventoryTexture.Height * scale);
			goldCountTextPos = new Vector2(goldTexturePos.X * 1.035f, goldTexturePos.Y * 1.025f);
			slots = new List<InventorySlot>();
			//How many slots we want to have in a row
			const int quanitySlotsInRow = 4;
			//How many rows there will be
			int rowsCount = quantitySlots / quanitySlotsInRow;
			//Check ramainder slots - if slot quantity can not be divided by quantitySlotsInRow throw an exception
			int ramainderSlots = quantitySlots % quanitySlotsInRow;
			if (ramainderSlots != 0)
			{
				throw new Exception("Invalid slot count in inventory, make sure slot count can be divided by " + quanitySlotsInRow.ToString());
			}
			for (int i=0;i<rowsCount;i++)
			{
				for(int j=0;j<quanitySlotsInRow;j++)
				{
					slots.Add(
						new InventorySlot(gs, gd, player, slotTexture, f, scale)
						{
							Position = new Vector2(this.Position.X + (slotTexture.Width * Scale * j), this.Position.Y + (slotTexture.Height * Scale * i)),
							Trashcan = this.Trashcan
						}
					);
				}
			}
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			//Access slots are always visible
			AccessSlotsManager.Draw(gameTime, spriteBatch);

			if (!Hidden)
			{
				spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
				spriteBatch.Draw(goldTexture, goldTexturePos, null, Color.White, 0f, Vector2.Zero, Scale*0.5f, SpriteEffects.None, 0f);
				spriteBatch.DrawString(font, player.Gold.ToString(), goldCountTextPos, Color.Black);
				EquipmentManager.Draw(gameTime, spriteBatch);
				Trashcan.Draw(gameTime, spriteBatch);
				//Iterate twice over slots (first time draw items, second time messages [this way messages are always on top of items])
				foreach (var s in slots)
					s.Draw(gameTime, spriteBatch);
				foreach (var s in slots)
					s.DrawMessages(gameTime, spriteBatch);
			}
		}
		public override void Update(GameTime gameTime)
		{
			//AccessSlots should always be updated (they are also always visable
			AccessSlotsManager.Update(gameTime);

			//It might be incorrect later, watch out! (InventorySlot Update() now checks only for player input and item Use() or Equip() so it's correct, but if you want to update here Item it won't be correct)
			if (!Hidden)
			{
				EquipmentManager.Update(gameTime);
				foreach (var s in slots)
					s.Update(gameTime);
			}
		}

		/// <summary>
		/// Item is simply not added to inventory if it is full
		/// </summary>
		/// <param name="i"></param>
		/// <param name="quantity"></param>
		public void AddItem(Item i)
		{
            //We handle gold differently -> redirect to correct fuunction
            if(i is GoldCoin)
            {
                player.Gold += i.Quantity;
                return;
            }

			if(i.IsStackable == true)
			{
				foreach(var s in slots)
				{
					//Check if the item already exists in inventory and just increase it's amount
					//We have to check runtime type since '==' will also check Item quantity
					if(s.Item == null)
					{
						continue;
					}
					if(s.Item.GetType() == i.GetType())
					{
						s.Item.Quantity += i.Quantity;
						return;
					}
				}
                //Look for empty slot
				foreach (var s in slots)
				{
					if (s.Item == null)
					{
						s.Item = (Item)i.Clone();
						//s.Item.Quantity = i.Quantity;
						//s.Item.Quantity = i.Quantity;
						return;
					}
				}
			}
            //Not stackable
			else
			{
				foreach(var s in slots)
				{
					if(s.Item == null)
					{
						s.Item = (Item)i.Clone();
						//s.Quantity = i.Quantity;
						return;
					}
				}
			}
		}

		public bool IsAlreadyDragging()
		{
			foreach (var s in slots)
			{
				if (s.Draggable && s.IsDragging)
					return true;
			}
			foreach (var s in EquipmentManager.EquipmentSlots)
			{
				if (s.IsDragging)
					return true;
			}
			if (player.activeSlots != null)
			{
				foreach (var s in player.activeSlots)
				{
					if (s.IsDragging)
						return true;
				}
			}
			return false;
		}

		public Slot WhichSlotIsDragging()
		{
			foreach (var s in slots)
			{
				if (s.Draggable && s.IsDragging)
					return s;
			}
			foreach (var s in EquipmentManager.EquipmentSlots)
			{
				if (s.IsDragging)
					return s;
			}
			if(player.activeSlots != null)
			{
				foreach (var s in player.activeSlots)
				{
					if (s.IsDragging)
						return s;
				}
			}
			throw new Exception("WhichSlotIsDragging(): Invalid inventory slot\n");
		}

		public bool IsFull()
		{
			foreach(var s in slots)
			{
				if (s.Item == null)
					return false;
			}
			return true;
		}
	}
}
