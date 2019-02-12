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
		public Item Item { get; set; }
		public int Quantity { get; set; }
		public InventorySlot(Texture2D t, float scale) : base(t, scale) { }
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			throw new NotImplementedException();
			//Item?.Draw(gameTime, spriteBatch, scale);
		}

		public override void Update(GameTime gameTime)
		{
			throw new NotImplementedException();
		}
		public bool IsFull()
		{
			throw new NotImplementedException();
		}
	}
}
