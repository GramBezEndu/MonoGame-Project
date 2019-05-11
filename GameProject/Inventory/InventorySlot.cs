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

namespace GameProject.Inventory
{
    public class InventorySlot : Sprite
    {
        private Player player;
        private SpriteFont font;
        public Item Item
        { get => _item;
            set
            {
                _item = value;
                GenerateBackgroundSprite();
            }
        }
		/// <summary>
		/// Determines if player wants to drag current slot
		/// </summary>
		public bool IsDragging { get; set; }
        //public int Quantity { get; set; }
        /// <summary>
        /// For how long text will be displayed after invalid item usage (in seconds)
        /// </summary>
        private float invalidUseTime;
        MouseState currentState;
        MouseState previousState;
        //If is hovering then 1. Gray out the slot 2. print item name and description if inventory slot contains item (it is done in Item drawing)
        bool isHovering;
        //GraphicsDevice reference to generate texture
        private GraphicsDevice graphicsDevice;
        //Generate background texture for displaying description
        private Texture2D descriptionBackground;
        private Sprite descriptionAndNameBackground;
        private Texture2D invalidUseTexture;

        private void SetInvalidUsageBackgroundSprite()
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

        private Sprite _inavalidUseBackground;
        private string invalidUse = "You can't use this item";
        private Item _item;

        public InventorySlot(GraphicsDevice gd, Player p, Texture2D t, SpriteFont f, float scale) : base(t, scale)
        {
            font = f;
            player = p;
            graphicsDevice = gd;
            SetInvalidUsageBackgroundSprite();
        }
        private void GenerateBackgroundSprite()
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
				if (IsDragging)
				{
					var mousePos = Mouse.GetState().Position;
					Vector2 pos = new Vector2(mousePos.X, mousePos.Y);
					Item.Position = pos;
				}
				Item.Draw(gameTime, spriteBatch);
                //throw new NotImplementedException();
            }
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
                    if(Item.Description != null)
                        spriteBatch.DrawString(font, Item.Description, new Vector2(descriptionAndNameBackground.Position.X, descriptionAndNameBackground.Position.Y + font.MeasureString(Item.Name).Y), Color.Black);
                }
                if (Item.IsStackable)
                    spriteBatch.DrawString(font, Item.Quantity.ToString(), Position, Color.Black);
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
            previousState = currentState;
            currentState = Mouse.GetState();

            var mouseRectangle = new Rectangle(currentState.X, currentState.Y, 1, 1);

            isHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                isHovering = true;

				//Use/Equip
                if (currentState.RightButton == ButtonState.Released && previousState.RightButton == ButtonState.Pressed)
                {
                    //Click?.Invoke(this, new EventArgs());
                    if (Item == null)
                        return;
                    //Usable
                    if (Item is Usable)
                    {
                        bool result = (Item as Usable).Use(player);
                        if (result == true)
                            Item.Quantity--;
                        else
                            invalidUseTime = 2f;
                    }
                    else if (Item is Equippable)
                    {
                        bool result = (Item as Equippable).Equip(player);
						if (result == true)
							Item.Quantity--;
						else
							invalidUseTime = 2f;
					}
                }
				//Drag items
				else if (currentState.LeftButton == ButtonState.Released && previousState.LeftButton == ButtonState.Pressed)
				{
					//End dragging within the same slot
					if (IsDragging)
						IsDragging = false;
					//End dragging within diffrent slot
					else if(player.InventoryManager.IsAlreadyDragging())
					{
						InventorySlot slotDragging = player.InventoryManager.WhichSlotIsDragging();
						//Type typeOfSlot = slotDragging.GetType();
						//Type t = typeof(InventorySlot);
						//if (typeOfSlot.Equals(t))
						//{
						//	var item = slotDragging.Item;
						//	slotDragging.Item = this.Item;
						//	this.Item = item;
						//	slotDragging.IsDragging = false;
						//}
						if (slotDragging is InventorySlot)
						{
							var item = slotDragging.Item;
							slotDragging.Item = this.Item;
							this.Item = item;
							slotDragging.IsDragging = false;
						}
					}
					//Try to start dragging
					else if(!IsDragging)
					{
						//You can't start dragging two items
						if (player.InventoryManager.IsAlreadyDragging())
							return;
						else
							IsDragging = true;
					}
				}
            }
            //Item could be used or equipped
            if (Item != null)
            {
				if (Item.Quantity <= 0)
					Item = null;
            }
            //Invalid use timer decrease if >0
            if (invalidUseTime > 0)
                invalidUseTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        //Po co to lol
        public bool IsFull()
        {
            throw new NotImplementedException();
        }
    }
}
