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

namespace GameProject
{
	public class InventorySlot : Sprite
	{
		private SpriteFont font;
		public Item Item { get; set; }
		public int Quantity { get; set; }
		MouseState currentState;
		MouseState previousState;
		//If is hovering then 1. Gray out the slot 2. print item name and description if inventory slot contains item
		bool isHovering;
		public InventorySlot(Texture2D t, SpriteFont f, float scale) : base(t, scale)
		{
			font = f;
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			var color = Color.White;

			if (isHovering)
			{
				color = Color.Gray;
			}
			//throw new NotImplementedException();
			//Item?.Draw(gameTime, spriteBatch, scale);
			spriteBatch.Draw(texture, Position, null, color, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
			//Drop actual item and quantity (string)
			if (Item != null)
			{
				Item.Position = this.Position;
				Item.Draw(gameTime, spriteBatch);
				if(Item.IsStackable)
					spriteBatch.DrawString(font, Quantity.ToString(), Position, Color.Black);
				//throw new NotImplementedException();
			}
		}

		public override void Update(GameTime gameTime)
		{
			previousState = currentState;
			currentState = Mouse.GetState();

			var mouseRectangle = new Rectangle(currentState.X, currentState.Y, 1, 1);

			isHovering = false;

			if (mouseRectangle.Intersects(rectangle))
			{
				isHovering = true;

				if (currentState.RightButton == ButtonState.Released && previousState.RightButton == ButtonState.Pressed)
				{
					//Click?.Invoke(this, new EventArgs());
					if (Item == null)
						return;
					if (Item is Usable)
					{
						Quantity--;
						(Item as Usable).Use();
					}
					else throw new NotImplementedException();
				}
			}
			//Item could be used or equipped
			if(Quantity<=0)
			{
				Item = null;
			}
		}
		public bool IsFull()
		{
			throw new NotImplementedException();
		}
	}
}
