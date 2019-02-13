using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject.Sprites;

namespace GameProject
{
	public class InventorySlot : Sprite
	{
		private SpriteFont font;
		public Item Item { get; set; }
		public int Quantity { get; set; }
		public InventorySlot(Texture2D t, SpriteFont f, float scale) : base(t, scale)
		{
			font = f;
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{

			//throw new NotImplementedException();
			//Item?.Draw(gameTime, spriteBatch, scale);
			spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
			//Drop actual item and quantity (string)
			if (Item != null)
			{
				Item.Position = this.Position;
				Item.Draw(gameTime, spriteBatch);
				spriteBatch.DrawString(font, Quantity.ToString(), Position, Color.Black);
				//throw new NotImplementedException();
			}
		}

		public override void Update(GameTime gameTime)
		{
			//throw new NotImplementedException();
		}
		public bool IsFull()
		{
			throw new NotImplementedException();
		}
	}
}
