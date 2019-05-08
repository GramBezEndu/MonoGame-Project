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

namespace GameProject.Sprites
{
    public class Shopkeeper : InteractableSprite
    {
        //Components that are displayed when shop is opened
        List<Component> shop = new List<Component>();
        public Shopkeeper(Game1 g, GameState gs, Sprite mainSprite, Sprite interactButton, Player p) : base(g, gs, mainSprite, interactButton, p)
        {
            //Hide shop on default
            Hidden = true;
            shop.Add(new Sprite(gs.Textures["Inventory"], g.Scale)
            {
                Position = this.MainSprite.Position,
                Hidden = true
            }
);
            shop.Add(new Button(gs.Textures["Button"], gs.Font, g.Scale)
            {
                Position = this.MainSprite.Position,
                Text = "Exit shop",
                Hidden = true,
                Click = CloseShop
            }
);
            gs.AddUiElements(shop);
        }

        private void CloseShop(object sender, EventArgs e)
        {
            Hide();
        }

        public override void Update(GameTime gameTime)
        {
            //We do not display (and do not allow clicks) on Interact button if shop is opened
            if (Hidden)
                base.Update(gameTime);
            //Close automatically shop if player is too far away (not touching main sprite now)
            if (!Hidden && !player.IsTouching(MainSprite))
                Hide();
            MainSprite.animationManager.Update(gameTime);
            PlayAnimations();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            //display a window where u can buy items
            ////if(!Hidden)
            ////{
            ////    foreach (var s in shop)
            ////        s.Draw(gameTime, spriteBatch);
            ////}
        }
        /// <summary>
        ///  Call this method to correctly hide shop and all its components
        /// </summary>
        private void Hide()
        {
            Hidden = true;
            foreach (var s in shop)
                s.Hidden = Hidden;
        }

        private void PlayAnimations()
        {
            MainSprite.animationManager.Play(MainSprite.animations["Idle"]);
        }
        protected override void OnActivate()
        {
            base.OnActivate();
            //Change hidden flag (also for each element in shop)
            //Those are the same objects that are in uiElements in current gameState
            Hidden = false;
            foreach (var s in shop)
                s.Hidden = Hidden;
        }
    }
}
