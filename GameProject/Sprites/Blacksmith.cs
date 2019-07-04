using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.Animations;
using Microsoft.Xna.Framework;
using GameProject.States;
using GameProject.Controls;
using GameProject.Inventory;
using GameProject.Items;

namespace GameProject.Sprites
{
	public class Blacksmith : Character
	{
		//UiElements list -> base components

		List<Component> addingScrollComponents = new List<Component>();

		List<Component> scrollUpgradeComponents = new List<Component>();

        /// <summary>
        /// List of improvements slots (used in upgrade) method
        /// Note: They are added in scrollUpgradedComponents, they're used just for object reference
        /// </summary>
        List<Slot> upgradeSlots = new List<Slot>();

        List<Component> shieldRepairComponents = new List<Component>();
        public Blacksmith(Game1 g, GameState gs, Sprite mainSprite, Sprite interactButton, Player p) : base(g, gs, mainSprite, interactButton, p)
		{
			MainWindowAddElements(g, gs);

			//Note: Fused scroll adding and scroll upgrade to one window
			//Shield Repair window is now disabled because repair feature might get removed
			////Extra window 1
			//ScrollAddingWindow(g, gs);
			////Extra window 2
			//ShieldRepairWindow(g, gs);
			//Extra window 3
			ScrollUpgradeWindow(g, gs);

			//Apply changes to state
			gs.AddUiElements(UiElements);
			//gs.AddUiElements(addingScrollComponents);
			gs.AddUiElements(scrollUpgradeComponents);
			//gs.AddUiElements(shieldRepairComponents);
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
			//var addScroll = new Button(gs.Textures["Button"], gs.Font, g.Scale)
			//{
			//	Position = pos,
			//	Text = "Add Scroll",
			//	Hidden = true,
			//	Click = ActivateScrollAddingWindow
			//};
			//UiElements.Add(addScroll);
			//pos += new Vector2(0, addScroll.Height);
			var scrollUpgrade = new Button(gs.Textures["Button"], gs.Font, g.Scale)
			{
				Position = pos,
				//It could be "Joining scrolls" or sth like that
				Text = "Upgrade",
				Click = ActivateScrollUpgradeWindow,
				Hidden = true
			};
			UiElements.Add(scrollUpgrade);

			//pos += new Vector2(0, scrollUpgrade.Height);
			//var shieldRepair = new Button(gs.Textures["Button"], gs.Font, g.Scale)
			//{
			//	Position = pos,
			//	Text = "Shield repair",
			//	Click = ActivateRepairShieldWindow,
			//	Hidden = true
			//};
			//UiElements.Add(shieldRepair);
		}

		private void ScrollAddingWindow(Game1 g, GameState gs)
		{
			var backgroundWindow = new Sprite(gs.Textures["Inventory"], g.Scale)
			{
				Hidden = true
			};
			backgroundWindow.Position = new Vector2(g.Width / 2 - backgroundWindow.Width / 2, g.Height / 2 - backgroundWindow.Height / 2);
			addingScrollComponents.Add(backgroundWindow);
            var exitButton = new Button(gs.Textures["Button"], gs.Font, g.Scale)
            {
                Text = "Exit",
                Hidden = true,
                Click = HideWindow
            };
            exitButton.Position = new Vector2(backgroundWindow.Position.X, backgroundWindow.Rectangle.Bottom - exitButton.Height);
            addingScrollComponents.Add(exitButton);

            var upgradeCostDisplay = new Text(gs.Font, "Upgrade cost: 0")
            {
                Hidden = true,
            };
            upgradeCostDisplay.Position = new Vector2(exitButton.Position.X, exitButton.Position.Y - upgradeCostDisplay.Height);
            addingScrollComponents.Add(upgradeCostDisplay);
        }

		private void ScrollUpgradeWindow(Game1 g, GameState gs)
		{
			var scrollUpgradeBackground = new Sprite(gs.Textures["Inventory"], g.Scale)
			{
				Hidden = true
			};
			scrollUpgradeBackground.Position = new Vector2(g.Width / 2 - scrollUpgradeBackground.Width / 2, g.Height / 2 - scrollUpgradeBackground.Height / 2);
			scrollUpgradeComponents.Add(scrollUpgradeBackground);
            var exitButton = new Button(gs.Textures["Button"], gs.Font, g.Scale)
            {
                Text = "Exit",
                Hidden = true,
                Click = HideWindow
            };
            exitButton.Position = new Vector2(scrollUpgradeBackground.Position.X, scrollUpgradeBackground.Rectangle.Bottom - exitButton.Height);
            scrollUpgradeComponents.Add(exitButton);

            var improvementSlotOne = new ImprovementScrollSlot(gs.graphicsDevice, player, gs.Textures["InventorySlot"], gs.Font, g.Scale)
            {
                Position = scrollUpgradeBackground.Position,
                Hidden = true
            };
            scrollUpgradeComponents.Add(improvementSlotOne);
            upgradeSlots.Add(improvementSlotOne);

            var improvementSlotTwo = new ImprovementScrollSlot(gs.graphicsDevice, player, gs.Textures["InventorySlot"], gs.Font, g.Scale)
            {
                Position = new Vector2(scrollUpgradeBackground.Position.X + improvementSlotOne.Width, scrollUpgradeBackground.Position.Y),
                Hidden = true
            };
            scrollUpgradeComponents.Add(improvementSlotTwo);
            upgradeSlots.Add(improvementSlotTwo);

			var arrow = new Sprite(gs.Textures["Arrow"], g.Scale)
			{
				Position = new Vector2(scrollUpgradeBackground.Position.X + improvementSlotOne.Width + improvementSlotTwo.Width, scrollUpgradeBackground.Position.Y),
				Hidden = true
			};
			scrollUpgradeComponents.Add(arrow);

			var improvementSlotThree = new ImprovementScrollSlot(gs.graphicsDevice, player, gs.Textures["InventorySlot"], gs.Font, g.Scale)
            {
                Position = new Vector2(arrow.Position.X + arrow.Width, arrow.Position.Y),
                Hidden = true
            };
            scrollUpgradeComponents.Add(improvementSlotThree);
            upgradeSlots.Add(improvementSlotThree);

			var upgradeCostDisplay = new Text(gs.Font, "Upgrade cost: 0")
			{
				Hidden = true,
			};
			upgradeCostDisplay.Position = new Vector2(improvementSlotOne.Position.X, improvementSlotOne.Position.Y + improvementSlotOne.Height);
			scrollUpgradeComponents.Add(upgradeCostDisplay);

			var upgradeButton = new Button(gs.Textures["Button"], gs.Font, g.Scale)
            {
                Position = new Vector2(upgradeCostDisplay.Position.X, upgradeCostDisplay.Position.Y + upgradeCostDisplay.Height),
                Text = "Upgrade",
                Hidden = true,
                Click = Upgrade
            };

            scrollUpgradeComponents.Add(upgradeButton);
        }

		private void Upgrade(object sender, EventArgs e)
		{
			//Missing item
			if (upgradeSlots[0].Item == null || upgradeSlots[1].Item == null)
			{
				//Message: one of required slots is missing
				return;
			}
			//Need to take upgraded item first
			if (upgradeSlots[2].Item != null)
			{
				//Message: you need to take upgraded item first
				return;
			}
			//Scroll checking
			var type1 = upgradeSlots[0].Item.GetType();
			var type2 = upgradeSlots[1].Item.GetType();
			//Check if it is normal scroll upgrade
			if (type1 == typeof(ImprovementScroll))
			{
				if(type2 != type1)
				{
					//Message: You can't fuse Legendary Scroll with Normal Scroll
					return;
				}
				UpgradeNormalScrolls();
			}
			//Check if it is legendary scroll upgrade
			else if(type1 == typeof(LegendaryImprovementScroll))
			{
				if (type2 != type1)
				{
					//Message: You can't fuse Legendary Scroll with Normal Scroll
					return;
				}
				UpgradeLegendaryScrolls();
			}
			//Check if it is equipment upgrade (not done yet)
			else
			{
				//Any upgradeable item in slot one
				if(upgradeSlots[0].Item is UpgradeableWithScroll)
				{
					if(upgradeSlots[1].Item is ImprovementScroll)
					{
						//It is ok
						UpgradeEquipment();
					}
					else
					{
						//Message: Item can only be upgraded with scroll
						return;
					}
				}
				//Any upgradeable item in slot two
				else if (upgradeSlots[1].Item is UpgradeableWithScroll)
				{
					if (upgradeSlots[0].Item is ImprovementScroll)
					{
						//It is ok
						UpgradeEquipment();
					}
					else
					{
						//Message: Item can only be upgraded with scroll
						return;
					}
				}
				//No upgradeable item
				else
				{
					//Message: You can't upgrade this item
					return;
				}
			}
		}

		private void UpgradeEquipment()
		{
			throw new NotImplementedException();
		}

		private void UpgradeNormalScrolls()
		{
			ImprovementScroll scrollOne = (ImprovementScroll)upgradeSlots[0].Item;
			ImprovementScroll scrollTwo = (ImprovementScroll)upgradeSlots[1].Item;
			upgradeSlots[2].Item = new ImprovementScroll(game, GameState.Textures["ImprovementScroll"], game.Scale)
			{
				ImprovementPower = game.RandomNumber(
					   (int)Math.Round((scrollOne.ImprovementPower + scrollTwo.ImprovementPower) * 100f / 2f),
					   (int)Math.Round(scrollOne.MaxPower * 100f)) / 100f,
			};
			//Destroy previous scrolls
			upgradeSlots[0].Item = null;
            upgradeSlots[1].Item = null;
		}

		private void UpgradeLegendaryScrolls()
        {
			LegendaryImprovementScroll scrollOne = (LegendaryImprovementScroll)upgradeSlots[0].Item;
			LegendaryImprovementScroll scrollTwo = (LegendaryImprovementScroll)upgradeSlots[1].Item;
			upgradeSlots[2].Item = new LegendaryImprovementScroll(game, GameState.Textures["ImprovementScroll"], game.Scale)
			{
				ImprovementPower = game.RandomNumber(
					   (int)Math.Round((scrollOne.ImprovementPower + scrollTwo.ImprovementPower) * 100f / 2f),
					   (int)Math.Round(scrollOne.MaxPower * 100f)) / 100f,
			};
			//Destroy previous scrolls
			upgradeSlots[0].Item = null;
			upgradeSlots[1].Item = null;
		}

        private void ShieldRepairWindow(Game1 g, GameState gs)
		{
			var shieldRepairWindow = new Sprite(gs.Textures["Inventory"], g.Scale)
			{
				Hidden = true
			};
			shieldRepairWindow.Position = new Vector2(g.Width / 2 - shieldRepairWindow.Width / 2, g.Height / 2 - shieldRepairWindow.Height / 2);
			shieldRepairComponents.Add(shieldRepairWindow);
            var exitButton = new Button(gs.Textures["Button"], gs.Font, g.Scale)
            {
                Text = "Exit",
                Hidden = true,
                Click = HideWindow
            };
            exitButton.Position = new Vector2(shieldRepairWindow.Position.X, shieldRepairWindow.Rectangle.Bottom - exitButton.Height);
            shieldRepairComponents.Add(exitButton);
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
