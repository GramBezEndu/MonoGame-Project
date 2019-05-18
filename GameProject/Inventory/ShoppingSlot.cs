using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject.Items;

namespace GameProject.Inventory
{
	public class ShoppingSlot : Slot
	{
		public int Prize { get; set; }
		public ShoppingSlot(GraphicsDevice gd, Player p, Texture2D t, SpriteFont f, float scale) : base(gd, p, t, f, scale)
		{
			//Message to display if you can't buy item
			invalidUse = "You can't buy this item";
			SetInvalidUsageBackgroundSprite();
			Prize = Int32.MaxValue;
		}
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			var mouseRectangle = new Rectangle(currentState.X, currentState.Y, 1, 1);
			if (mouseRectangle.Intersects(Rectangle))
			{
				if (currentState.RightButton == ButtonState.Released && previousState.RightButton == ButtonState.Pressed)
				{
					//Player has enough gold -> buy
					if(player.Gold >= Prize)
					{
						player.InventoryManager.AddItem(this.Item);
						player.Gold -= Prize;
					}
				}
			}
		}
	}
}
