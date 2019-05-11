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
        }
    }
}
