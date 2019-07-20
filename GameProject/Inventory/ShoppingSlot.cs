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
using GameProject.States;

namespace GameProject.Inventory
{
	public class ShoppingSlot : Slot
	{
		public int Prize { get; set; }
		private Sprite _goldIcon;
		GameState GameState;
		public ShoppingSlot(GameState gs, GraphicsDevice gd, Player p, Texture2D t, Texture2D goldIcon, SpriteFont f, float scale) : base(gd, p, t, f, scale)
		{
			GameState = gs;
			Prize = Int32.MaxValue;
			//Rescaling goldIcon
			_goldIcon = new Sprite(goldIcon, scale * 0.5f);
		}

		protected override void GenerateBackgroundSprite()
		{
			if (Item != null)
			{
				//Note: Every item should have name (not named items will have name: "Not assigned name")
				Vector2 size = font.MeasureString(Item.Name);
				//Item description is not required
				//width = max(name, desc, prize + goldIcon)
				//height = name + desc + max(prize,goldIcon)
				if (Item.Description != null)
				{
					size.X = Math.Max(font.MeasureString(Item.Description).X, font.MeasureString(Item.Name).X);
					size.Y += font.MeasureString(Item.Description).Y;
				}
				size.X = Math.Max(size.X, (font.MeasureString(Prize.ToString()).X + _goldIcon.Width));
				//pos = position of background (if mouse is hovering)
				Vector2 pos = new Vector2(Position.X, Position.Y + Height);
				_goldIcon.Position = new Vector2(pos.X, pos.Y + size.Y);
				size.Y += Math.Max(font.MeasureString(Prize.ToString()).Y, _goldIcon.Width);
				//Make texture
				descriptionBackground = new Texture2D(graphicsDevice, (int)size.X, (int)size.Y);
				Color[] data = new Color[(int)size.X * (int)size.Y];
				//Paint every pixel
				for (int i = 0; i < data.Length; i++)
				{
					data[i] = Color.LemonChiffon;
				}
				descriptionBackground.SetData(data);
				//Set sprite
				descriptionAndNameBackground = new Sprite(descriptionBackground, 1f)
				{
					Position = pos
				};
			}
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
					else
					{
						GameState.CreateMessage("You can't buy this item");
					}
				}
			}
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);
			DrawMessages(gameTime, spriteBatch);
		}

		public override void DrawMessages(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (!Hidden)
			{
				if (Item != null)
				{
					if (isHovering)
					{
						descriptionAndNameBackground.Draw(gameTime, spriteBatch);
						spriteBatch.DrawString(font, Item.Name, descriptionAndNameBackground.Position, Color.Green);
						if (Item.Description != null)
						{
							spriteBatch.DrawString(font, Item.Description, new Vector2(descriptionAndNameBackground.Position.X, descriptionAndNameBackground.Position.Y + font.MeasureString(Item.Name).Y), Color.Black);
							_goldIcon.Draw(gameTime, spriteBatch);
							spriteBatch.DrawString(font, Prize.ToString(), new Vector2(descriptionAndNameBackground.Position.X + _goldIcon.Width, descriptionAndNameBackground.Position.Y + font.MeasureString(Item.Name).Y + font.MeasureString(Item.Description).Y), Color.Black);
						}
						else
						{
							_goldIcon.Draw(gameTime, spriteBatch);
							spriteBatch.DrawString(font, Prize.ToString(), new Vector2(descriptionAndNameBackground.Position.X + _goldIcon.Width, descriptionAndNameBackground.Position.Y + font.MeasureString(Item.Name).Y), Color.Black);
						}
					}
					if (Item.IsStackable)
						spriteBatch.DrawString(font, Item.Quantity.ToString(), Position, Color.Black);
				}
			}
		}
	}
}
