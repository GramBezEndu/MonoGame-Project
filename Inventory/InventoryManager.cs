using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject
{
	public class InventoryManager : Component
	{
		private List<InventorySlot> slots;
		/// <summary>
		/// Initializes new inventory
		/// </summary>
		/// <param name="quantitySlots">How many slots will inventory have</param>
		public InventoryManager(int quantitySlots)
		{
			slots = new List<InventorySlot>();
			for(int i=0;i<quantitySlots;i++)
			{
				slots.Add(new InventorySlot(null, 1.0f));
			}
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			foreach (var s in slots)
				s.Draw(gameTime, spriteBatch);
		}
		public override void Update(GameTime gameTime)
		{
			throw new NotImplementedException();
		}
		public void AddItem(Item i, int quantity)
		{
			throw new NotImplementedException();
		}
	}
}
