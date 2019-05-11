﻿using System;
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

namespace GameProject.Sprites
{
    public class Shopkeeper : Character
    {
		private List<Component> slots = new List<Component>();
        public Shopkeeper(Game1 g, GameState gs, Sprite mainSprite, Sprite interactButton, Player p) : base(g, gs, mainSprite, interactButton, p)
        {
			//Add special elements to window here
			slots.Add(new ShoppingSlot(gs.graphicsDevice, p, gs.Textures["InventorySlot"], gs.Font, g.Scale)
			{
				Item = new HealthPotion(gs.Textures["HealthPotion"], g.Scale),
				Hidden = true,
				Position = mainSprite.Position
			}
			);
            //Apply changes to state
            gs.AddUiElements(UiElements);
			gs.AddUiElements(slots);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
			if(!Hidden)
			{
				foreach (var s in slots)
					s.Update(gameTime);
			}
            MainSprite.animationManager.Update(gameTime);
            PlayAnimations();
        }

		/// <summary>
		///  Call this method to correctly hide window and all its components
		/// </summary>
		protected override void Hide()
		{
			Hidden = true;
			foreach (var ui in UiElements)
				ui.Hidden = Hidden;
			foreach (var s in slots)
				s.Hidden = Hidden;
			player.UsingWindow = false;
		}

        private void PlayAnimations()
        {
            MainSprite.animationManager.Play(MainSprite.animations["Idle"]);
        }

		protected override void OnActivate()
		{
			base.OnActivate();
			//Change hidden flag (also for each element in uiElements [window])
			//Those are the same objects that are in uiElements in current gameState
			player.UsingWindow = true;
			Hidden = false;
			foreach (var s in slots)
				s.Hidden = Hidden;
		}

		/// <summary>
		/// When closing window with ui elements
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void OnClose(object sender, EventArgs e)
		{
			Hidden = true;
			foreach (var ui in UiElements)
				ui.Hidden = Hidden;
			foreach (var s in slots)
				s.Hidden = Hidden;
			//Allow attacking!
			player.UsingWindow = false;
		}
	}
}
