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
	public class Slot : Sprite
	{
		protected Player player;
		protected SpriteFont font;
		public Item Item
		{
			get => _item;
			set
			{
				_item = value;
				GenerateBackgroundSprite();
			}
		}
		public int Quantity { get; set; }
		/// <summary>
		/// For how long text will be displayed after invalid item usage (in seconds)
		/// </summary>
		protected float invalidUseTime;
		protected MouseState currentState;
		protected MouseState previousState;
		//If is hovering then 1. Gray out the slot 2. print item name and description if inventory slot contains item (it is done in Item drawing)
		protected bool isHovering;
		//GraphicsDevice reference to generate texture
		protected GraphicsDevice graphicsDevice;
		//Generate background texture for displaying description
		protected Texture2D descriptionBackground;
		protected Sprite descriptionAndNameBackground;
		protected Texture2D invalidUseTexture;


		protected Sprite _inavalidUseBackground;
		protected string invalidUse;
		protected Item _item;

		public Slot(GraphicsDevice gd, Player p, Texture2D t, SpriteFont f, float scale) : base(t, scale)
		{
			font = f;
			player = p;
			graphicsDevice = gd;
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (!Hidden)
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
					//throw new NotImplementedException();
				}
			}
		}

		protected void GenerateBackgroundSprite()
		{
			if (Item != null)
			{
				//Note: Every item should have name (not named items will have name: "Not assigned name")
				Vector2 size = font.MeasureString(Item.Name);
				//Item description is not required
				if (Item.Description != null)
				{
					size.X = Math.Max(font.MeasureString(Item.Description).X, font.MeasureString(Item.Name).X);
					size.Y += font.MeasureString(Item.Description).Y;
				}
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
				Vector2 pos = new Vector2(Position.X, Position.Y + Height);
				descriptionAndNameBackground = new Sprite(descriptionBackground, 1f)
				{
					Position = pos
				};
			}
		}

		protected void SetInvalidUsageBackgroundSprite()
		{
			Vector2 size = font.MeasureString(invalidUse);
			invalidUseTexture = new Texture2D(graphicsDevice, (int)size.X, (int)size.Y);
			Color[] data = new Color[(int)size.X * (int)size.Y];
			//Paint every pixel
			for (int i = 0; i < data.Length; i++)
			{
				data[i] = Color.LemonChiffon;
			}
			//try
			//{
			invalidUseTexture.SetData(data);
			//}
			//catch
			//{
			//    throw new NotImplementedException();
			//}
			//Set sprite
			Vector2 pos = this.Position;
			_inavalidUseBackground = new Sprite(invalidUseTexture, 1f)
			{
				Position = pos
			};
		}

		/// <summary>
		/// Draw item description, item count and invalid usage message
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		public void DrawMessages(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (Item != null)
			{
				if (isHovering)
				{
					descriptionAndNameBackground.Draw(gameTime, spriteBatch);
					spriteBatch.DrawString(font, Item.Name, descriptionAndNameBackground.Position, Color.Gold);
					if (Item.Description != null)
						spriteBatch.DrawString(font, Item.Description, new Vector2(descriptionAndNameBackground.Position.X, descriptionAndNameBackground.Position.Y + font.MeasureString(Item.Name).Y), Color.Black);
				}
				if (Item.IsStackable)
					spriteBatch.DrawString(font, Quantity.ToString(), Position, Color.Black);
			}
			if (invalidUseTime > 0)
			{
				//It should be moved from here
				_inavalidUseBackground.Position = this.Position;
				_inavalidUseBackground.Draw(gameTime, spriteBatch);
				spriteBatch.DrawString(font, invalidUse, Position, Color.Black);
			}
		}

		public override void Update(GameTime gameTime)
		{
			if(!Hidden)
			{
				previousState = currentState;
				currentState = Mouse.GetState();

				var mouseRectangle = new Rectangle(currentState.X, currentState.Y, 1, 1);

				isHovering = false;

				if (mouseRectangle.Intersects(Rectangle))
				{
					isHovering = true;
				}
				//Invalid use timer decrease if >0
				if (invalidUseTime > 0)
					invalidUseTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
			}
		}
	}
}
