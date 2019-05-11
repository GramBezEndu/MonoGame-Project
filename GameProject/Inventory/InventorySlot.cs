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
    public class InventorySlot : Slot
    {
        public InventorySlot(GraphicsDevice gd, Player p, Texture2D t, SpriteFont f, float scale) : base(gd, p, t, f, scale)
        {
			//Message to display if you can't use/equip item
			invalidUse = "You can't use this item";
			SetInvalidUsageBackgroundSprite();
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
			base.Update(gameTime);
			var mouseRectangle = new Rectangle(currentState.X, currentState.Y, 1, 1);
			if (mouseRectangle.Intersects(Rectangle))
            {
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
                            --Quantity;
                        else
                            invalidUseTime = 2f;
                    }
                    else if (Item is Equippable)
                    {
                        bool result = (Item as Equippable).Equip(player);
                        if (result == true)
                            --Quantity;
                        else
                            invalidUseTime = 2f;
                    }
                }
            }
            //Item could be used or equipped
            if (Quantity <= 0)
            {
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
