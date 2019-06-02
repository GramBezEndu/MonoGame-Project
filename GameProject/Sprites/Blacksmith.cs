using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.Animations;
using Microsoft.Xna.Framework;
using GameProject.States;
using GameProject.Controls;

namespace GameProject.Sprites
{
	public class Blacksmith : Character
	{
		//UiElements list -> base components
		List<Component> addingScrollComponents = new List<Component>();
		List<Component> scrollUpgradeComponents = new List<Component>();
		List<Component> shieldRepairComponents = new List<Component>();
        public Blacksmith(Game1 g, GameState gs, Sprite mainSprite, Sprite interactButton, Player p) : base(g, gs, mainSprite, interactButton, p)
		{
			MainWindowAddElements(g, gs);

			//Extra window 1
			ScrollAddingWindow(g, gs);
			//Extra window 2
			ShieldRepairWindow(g, gs);
			//Extra window 3
			ScrollUpgradeWindow(g, gs);

			//Apply changes to state
			gs.AddUiElements(UiElements);
			gs.AddUiElements(addingScrollComponents);
			gs.AddUiElements(scrollUpgradeComponents);
			gs.AddUiElements(shieldRepairComponents);
		}

		private void MainWindowAddElements(Game1 g, GameState gs)
		{
			//Add special elements to window
			var background = UiElements.First();
			Vector2 pos = new Vector2(0, 0);
			if (background is Sprite)
			{
				pos = (background as Sprite).Position;
			}
			else
			{
				throw new Exception("First component should be background sprite\n");
			}
			var addScroll = new Button(gs.Textures["Button"], gs.Font, g.Scale)
			{
				Position = pos,
				Text = "Add Scroll",
				Hidden = true,
				Click = ActivateScrollAddingWindow
			};
			UiElements.Add(addScroll);
			pos += new Vector2(0, addScroll.Height);
			var scrollUpgrade = new Button(gs.Textures["Button"], gs.Font, g.Scale)
			{
				Position = pos,
				//It could be "Joining scrolls" or sth like that
				Text = "Scroll upgrade",
				Click = ActivateScrollUpgradeWindow,
				Hidden = true
			};
			UiElements.Add(scrollUpgrade);

			pos += new Vector2(0, scrollUpgrade.Height);
			var shieldRepair = new Button(gs.Textures["Button"], gs.Font, g.Scale)
			{
				Position = pos,
				Text = "Shield repair",
				Click = ActivateRepairShieldWindow,
				Hidden = true
			};
			UiElements.Add(shieldRepair);
		}

		private void ScrollAddingWindow(Game1 g, GameState gs)
		{
			var backgroundWindow = new Sprite(gs.Textures["Inventory"], g.Scale)
			{
				Hidden = true
			};
			backgroundWindow.Position = new Vector2(g.Width / 2 - backgroundWindow.Width / 2, g.Height / 2 - backgroundWindow.Height / 2);
			addingScrollComponents.Add(backgroundWindow);
			addingScrollComponents.Add(new Button(gs.Textures["Button"], gs.Font, g.Scale)
			{
				Position = backgroundWindow.Position,
				Text = "Exit",
				Hidden = true,
				Click = HideWindow
			});
		}

		private void ScrollUpgradeWindow(Game1 g, GameState gs)
		{
			var scrollUpgradeBackground = new Sprite(gs.Textures["Inventory"], g.Scale)
			{
				Hidden = true
			};
			scrollUpgradeBackground.Position = new Vector2(g.Width / 2 - scrollUpgradeBackground.Width / 2, g.Height / 2 - scrollUpgradeBackground.Height / 2);
			scrollUpgradeComponents.Add(scrollUpgradeBackground);
			scrollUpgradeComponents.Add(new Button(gs.Textures["Button"], gs.Font, g.Scale)
			{
				Position = scrollUpgradeBackground.Position,
				Text = "Exit",
				Hidden = true,
				Click = HideWindow
			});
		}

		private void ShieldRepairWindow(Game1 g, GameState gs)
		{
			var shieldRepairWindow = new Sprite(gs.Textures["Inventory"], g.Scale)
			{
				Hidden = true
			};
			shieldRepairWindow.Position = new Vector2(g.Width / 2 - shieldRepairWindow.Width / 2, g.Height / 2 - shieldRepairWindow.Height / 2);
			shieldRepairComponents.Add(shieldRepairWindow);
			shieldRepairComponents.Add(new Button(gs.Textures["Button"], gs.Font, g.Scale)
			{
				Position = shieldRepairWindow.Position,
				Text = "Exit",
				Hidden = true,
				Click = HideWindow
			});
		}

		private void ActivateRepairShieldWindow(object sender, EventArgs e)
		{
			//Hide every element
			//Note: We can note call Hide() here because it enables interact button and sets player.IsUsingWindow flag to false
			HideWindows();
			//Display elements from correct component list only
			foreach (var c in shieldRepairComponents)
			{
				c.Hidden = false;
			}
		}

		private void ActivateScrollUpgradeWindow(object sender, EventArgs e)
		{
			//Hide every element
			//Note: We can note call Hide() here because it enables interact button and sets player.IsUsingWindow flag to false
			HideWindows();
			//Display elements from correct component list only
			foreach (var c in scrollUpgradeComponents)
			{
				c.Hidden = false;
			}
		}

		private void ActivateScrollAddingWindow(object sender, EventArgs e)
		{
			//Hide every element
			//Note: We can note call Hide() here because it enables interact button and sets player.IsUsingWindow flag to false
			HideWindows();
			//Display elements from correct component list only
			foreach (var c in addingScrollComponents)
			{
				c.Hidden = false;
			}
		}

		private void HideWindows()
		{
			foreach(var c in UiElements)
			{
				c.Hidden = true;
			}
			foreach (var c in addingScrollComponents)
			{
				c.Hidden = true;
			}
			foreach (var c in scrollUpgradeComponents)
			{
				c.Hidden = true;
			}
			foreach(var c in shieldRepairComponents)
			{
				c.Hidden = true;
			}
		}

		/// <summary>
		/// Hides all blacksmith's components -> calls Hide() method
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void HideWindow(object sender, EventArgs e)
		{
			Hide();
		}

		protected override void Hide()
		{
			base.Hide();
			HideWindows();
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
