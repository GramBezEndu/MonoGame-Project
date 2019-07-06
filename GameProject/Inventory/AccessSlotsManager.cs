using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Sprites;
using GameProject.Items;
using GameProject.Inventory;

namespace GameProject.Inventory
{
	public class AccessSlotsManager : Component
	{
		public List<FastAccessSlot> fastAccessSlots { get; set; }

		public AccessSlotsManager(GraphicsDevice gd, Player p, Texture2D slotTexture, SpriteFont f, float scale, Vector2 position)
		{
			//Make fast access slots
			var slotOne = new FastAccessSlot(gd, p, slotTexture, f, scale)
			{
				Position = position
			};
			var slotTwo = new FastAccessSlot(gd, p, slotTexture, f, scale)
			{
				Position = new Vector2(position.X + slotOne.Width, position.Y)
			};
			var slotThree = new FastAccessSlot(gd, p, slotTexture, f, scale)
			{
				Position = new Vector2(slotTwo.Position.X + slotTwo.Width, slotTwo.Position.Y)
			};
			fastAccessSlots = new List<FastAccessSlot>()
			{
				slotOne,
				slotTwo,
				slotThree
			};
		}

		public override void Update(GameTime gameTime)
		{
			foreach (var s in fastAccessSlots)
				s.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			foreach (var s in fastAccessSlots)
				s.Draw(gameTime, spriteBatch);
		}
	}
}
