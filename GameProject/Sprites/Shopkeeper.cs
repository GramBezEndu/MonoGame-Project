using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.Animations;
using Microsoft.Xna.Framework;
using GameProject.States;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using GameProject.Controls;
using GameProject.Inventory;
using GameProject.Items;

namespace GameProject.Sprites
{
    public class Shopkeeper : InteractableSpriteWithWindows
    {
        public Shopkeeper(Game1 g, GameState gs, Sprite mainSprite, Sprite interactButton, Player p) : base(g, gs, mainSprite, interactButton, p)
        {
			var slotOne = new ShoppingSlot(gs, gs.graphicsDevice, p, gs.Textures["InventorySlot"], gs.Textures["Gold"], gs.Font, g.Scale)
			{
				Prize = 30,
				Hidden = true,
				//Position should be related to window (that pop up while shopping)
				Position = background.Position
			};
			//Set item to slot 1
			slotOne.Item = new HealthPotion(gs.Textures["HealthPotion"], g.Scale);
			var slotTwo = new ShoppingSlot(gs, gs.graphicsDevice, p, gs.Textures["InventorySlot"], gs.Textures["Gold"], gs.Font, g.Scale)
			{
				Prize = 5000,
				Hidden = true,
				//Position should be related to window (that pop up while shopping)
				Position = new Vector2(background.Position.X + slotOne.Width, background.Position.Y)
			};
			slotTwo.Item = new PurificationStone(gs.Textures["PurificationStone"], g.Scale);
			//Add special elements to window here
			UiElements.Add(slotOne);
			UiElements.Add(slotTwo);
            //Apply changes to state
            gs.AddUiElements(UiElements);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            MainSprite.animationManager.Update(gameTime);
            PlayAnimations();
        }

        private void PlayAnimations()
        {
            MainSprite.animationManager.Play(MainSprite.animations["Idle"]);
        }

	}
}
