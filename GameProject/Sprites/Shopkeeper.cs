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

namespace GameProject.Sprites
{
	public class Shopkeeper : InteractableSprite
	{
		public Shopkeeper(Game1 g, GameState gs, Sprite mainSprite, Sprite interactButton, Player p) : base(g, gs, mainSprite, interactButton, p)
        {

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
        protected override void OnActivate()
        {
            base.OnActivate();
            //Make a window where you can buy items
        }
    }
}
