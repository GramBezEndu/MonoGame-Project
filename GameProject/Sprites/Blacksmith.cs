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
using GameProject.Tutorials;

namespace GameProject.Sprites
{
	public class Blacksmith : InteractableSpriteWithWindows
	{
		//UiElements list -> window components

		List<Component> RecipeWindow = new List<Component>();
		
		/// <summary>
		/// List of improvements slots (used in upgrade) method
		/// Note: They are added in scrollUpgradedComponents, they're used just for object reference
		/// </summary>
		List<DraggableSlot> upgradeSlots = new List<DraggableSlot>();

		/// <summary>
		/// Contains list of all recepies (without text and buttons
		/// Keys:
		/// scrollUpgrade
		/// legendaryScrollUpgrade
		/// </summary>
		Dictionary<string, Recipe> recipes = new Dictionary<string, Recipe>();

		public int PreviousUpgradeCost { get; set; }
		public int UpgradeCost { get; set; } = 0;
		public Text UpgradeCostText { get; set; }

		public Blacksmith(Game1 g, GameState gs, Sprite mainSprite, Sprite interactButton, Player p) : base(g, gs, mainSprite, interactButton, p)
		{
			MainWindowAddElements(g, gs);

			RecipeWindowAddElements();

			//Add components to state
			gs.AddUiElements(UiElements);
			gs.AddUiElements(RecipeWindow);
			foreach(var recipe in recipes.Values)
			{
				gs.AddUiElement(recipe);
			}
		}

		private void RecipeWindowAddElements()
		{
			//Background
			var background = new Sprite(GameState.Textures["BackgroundBig"], game.Scale)
			{
				Hidden = true,
			};

			background.Position = new Vector2(game.Width /2 - background.Width/2, game.Height/2 - background.Height/2);

			RecipeWindow.Add(background);

			//Back button
			var back = new Button(GameState.Textures["Button"], GameState.Font, game.Scale)
			{
				Hidden = true,
				Click = HideRecipeWindow,
				Text = "Back"
			};
			back.Position = new Vector2(background.Position.X, background.Position.Y + background.Height - back.Height);
			RecipeWindow.Add(back);

			//Improvement scroll upgrade
			Recipe scrollUpgradeRecipe = new ScrollUpgradeRecipe(game, GameState, background.Position)
			{
				Hidden = true,
			};

			recipes.Add("scrollUpgrade", scrollUpgradeRecipe);

			//Legendary improvement scroll upgrade
			Recipe legendaryScrollUpgradeRecipe = new LegendaryScrollUpgradeRecipe(game, GameState, new Vector2(scrollUpgradeRecipe.Position.X, scrollUpgradeRecipe.Position.Y + scrollUpgradeRecipe.Height))
			{
				Hidden = true,
			};

			recipes.Add("legendaryScrollUpgrade", legendaryScrollUpgradeRecipe);
		}

		private void HideRecipeWindow(object sender, EventArgs e)
		{
			foreach (var c in RecipeWindow)
				c.Hidden = true;
			foreach (var c in recipes.Values)
				c.Hidden = true;
		}

		private void MainWindowAddElements(Game1 g, GameState gs)
		{
			var improvementSlotOne = new DraggableBlacksmithSlot(this, gs.graphicsDevice, player, gs.Textures["InventorySlot"], gs.Font, g.Scale)
			{
				Position = background.Position,
				Hidden = true
			};
			UiElements.Add(improvementSlotOne);
			upgradeSlots.Add(improvementSlotOne);

			var improvementSlotTwo = new DraggableBlacksmithSlot(this, gs.graphicsDevice, player, gs.Textures["InventorySlot"], gs.Font, g.Scale)
			{
				Position = new Vector2(background.Position.X + improvementSlotOne.Width, background.Position.Y),
				Hidden = true
			};
			UiElements.Add(improvementSlotTwo);
			upgradeSlots.Add(improvementSlotTwo);

			var arrow = new Button(gs.Textures["Arrow"], gs.Font, g.Scale)
			{
				Position = new Vector2(background.Position.X + improvementSlotOne.Width + improvementSlotTwo.Width, background.Position.Y),
				Hidden = true,
				Click = Upgrade
			};
			UiElements.Add(arrow);

			var improvementSlotThree = new DraggableBlacksmithSlot(this, gs.graphicsDevice, player, gs.Textures["InventorySlot"], gs.Font, g.Scale)
			{
				Position = new Vector2(arrow.Position.X + arrow.Width, arrow.Position.Y),
				Hidden = true
			};
			UiElements.Add(improvementSlotThree);
			upgradeSlots.Add(improvementSlotThree);

			var improvementSlotFour = new DraggableBlacksmithSlot(this, gs.graphicsDevice, player, gs.Textures["InventorySlot"], gs.Font, g.Scale)
			{
				Position = new Vector2(improvementSlotThree.Rectangle.Right, improvementSlotThree.Position.Y),
				Hidden = true
			};
			UiElements.Add(improvementSlotFour);
			upgradeSlots.Add(improvementSlotFour);

			UpgradeCostText = new Text(gs.Font, String.Format("Upgrade cost: {0}", UpgradeCost))
			{
				Hidden = true,
			};
			UpgradeCostText.Position = new Vector2(improvementSlotOne.Position.X, improvementSlotOne.Position.Y + improvementSlotOne.Height);
			UiElements.Add(UpgradeCostText);

			var recipeButton = new Button(gs.Textures["RecipeBook"], gs.Font, g.Scale)
			{
				Hidden = true,
				Position = new Vector2(UpgradeCostText.Position.X, UpgradeCostText.Position.Y + UpgradeCostText.Height),
				Click = ShowRecipeWindow
			};
			UiElements.Add(recipeButton);

			var recipeText = new Text(gs.Font, "Show recipes")
			{
				Hidden = true,
			};
			recipeText.Position = new Vector2(recipeButton.Position.X + recipeButton.Width, recipeButton.Position.Y + recipeButton.Height/2 - recipeText.Height/2);
			UiElements.Add(recipeText);

			var questionmark = new Button(gs.Textures["Questionmark"], gs.Font, g.Scale)
			{
				Hidden = true,
				Position = new Vector2(recipeButton.Position.X, recipeButton.Position.Y + recipeButton.Height),
				Click = RepeatTutorial
			};
			UiElements.Add(questionmark);

			var showTutorialText = new Text(gs.Font, "Show tutorial")
			{
				Hidden = true,
			};
			showTutorialText.Position = new Vector2(questionmark.Position.X + questionmark.Width, questionmark.Position.Y + questionmark.Height / 2 - showTutorialText.Height / 2);
			UiElements.Add(showTutorialText);
		}

		private void ShowRecipeWindow(object sender, EventArgs e)
		{
			foreach (var c in RecipeWindow)
				c.Hidden = false;
			//Hide context recipes when opening the window (but not the whole recipe)
			foreach (var c in recipes.Values)
			{
				c.Hidden = false;
				c.HideRecipe();
			}
		}

		private void RepeatTutorial(object sender, EventArgs e)
		{
			var blacksmithTutorial =  player.TutorialManager.Tutorials.First(s => s is BlacksmithTutorial);
			if(blacksmithTutorial != null)
			{
				blacksmithTutorial.Reset();
			}
		}

		private void Upgrade(object sender, EventArgs e)
		{
			//Missing item
			if (upgradeSlots[0].Item == null || upgradeSlots[1].Item == null)
			{
				//Message:
				GameState.CreateMessage("There is no item in atleast one of the slots");
				return;
			}
			//Need to take upgraded item first
			if (upgradeSlots[2].Item != null)
			{
				//Message: you need to take upgraded item first
				GameState.CreateMessage("You need to take upgraded item before starting another upgrade");
				return;
			}
			//Scroll checking
			var type1 = upgradeSlots[0].Item.GetType();
			var type2 = upgradeSlots[1].Item.GetType();
			//Check if it is normal scroll upgrade
			if (type1 == typeof(ImprovementScroll))
			{
				if (type2 != type1)
				{
					//Message: You can't fuse Legendary Scroll with Normal Scroll
					GameState.CreateMessage("It is not possible to fuse Legendary Scroll with Normal Scroll");
					return;
				}
				UpgradeNormalScrolls();
			}
			//Check if it is legendary scroll upgrade
			else if (type1 == typeof(LegendaryImprovementScroll))
			{
				if (type2 != type1)
				{
					//Message: You can't fuse Legendary Scroll with Normal Scroll
					GameState.CreateMessage("It is not possible to fuse Legendary Scroll with Normal Scroll");
					return;
				}
				UpgradeLegendaryScrolls();
			}
			//Check if it is equipment upgrade (adding scrolls)
			else
			{
				//Any upgradeable item in slot one
				if (upgradeSlots[0].Item is UpgradeableWithScroll)
				{
					if (upgradeSlots[1].Item is ImprovementScroll)
					{
						//It is ok
						UpgradeEquipmentSlotOne();
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
						UpgradeEquipmentSlotTwo();
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
				//TODO: We should check if it is a equipment upgrade with resources (bones etc.)
			}
		}

		/// <summary>
		/// Upgraded equipment is in slot one
		/// </summary>
		private void UpgradeEquipmentSlotOne()
		{
			if(player.Gold < UpgradeCost)
			{
				//Message: Not enough gold
				GameState.CreateMessage("You don't have enough gold for upgrade");
				return;
			}
			player.Gold -= UpgradeCost;
			//There should be a cost of upgrading
			var upgradingItem = upgradeSlots[0].Item as UpgradeableWithScroll;
			upgradingItem.ImprovementScrollSlot.Item = upgradeSlots[1].Item as ImprovementScroll;

			//Clear previous slots
			upgradeSlots[0].Item = null;
			upgradeSlots[1].Item = null;

			//Add new item to third slot
			upgradeSlots[2].Item = upgradingItem;
		}

		/// <summary>
		/// Upgraded equipment is in slot two
		/// </summary>
		private void UpgradeEquipmentSlotTwo()
		{
			if (player.Gold < UpgradeCost)
			{
				//Message: Not enough gold
				GameState.CreateMessage("You don't have enough gold for upgrade");
				return;
			}
			player.Gold -= UpgradeCost;
			//There should be a cost of upgrading
			var upgradingItem = upgradeSlots[1].Item as UpgradeableWithScroll;
			upgradingItem.ImprovementScrollSlot.Item = upgradeSlots[0].Item as ImprovementScroll;

			//Clear previous slots
			upgradeSlots[0].Item = null;
			upgradeSlots[1].Item = null;

			//Add new item to third slot
			upgradeSlots[2].Item = upgradingItem;
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
			upgradeSlots[2].Item = new LegendaryImprovementScroll(game, GameState.Textures["LegendaryImprovementScroll"], game.Scale)
			{
				ImprovementPower = game.RandomNumber(
					   (int)Math.Round((scrollOne.ImprovementPower + scrollTwo.ImprovementPower) * 100f / 2f),
					   (int)Math.Round(scrollOne.MaxPower * 100f)) / 100f,
			};
			//Destroy previous scrolls
			upgradeSlots[0].Item = null;
			upgradeSlots[1].Item = null;
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

		/// <summary>
		/// Hides components and adds items that are left in slots to player's inventory
		/// </summary>
		protected override void Hide()
		{
			base.Hide();
			foreach(var i in upgradeSlots)
			{
				if(i.Item != null)
				{
					player.InventoryManager.AddItem((Item)i.Item.Clone());
					i.Item = null;
				}
			}
			player.activeSlots = null;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			MainSprite.animationManager.Update(gameTime);
			PlayAnimations();

			//Update Upgrade cost
			PreviousUpgradeCost = UpgradeCost;
			if (upgradeSlots[0].Item != null && upgradeSlots[0].Item is UpgradeableWithScroll)
			{
				var temp = upgradeSlots[0].Item as UpgradeableWithScroll;
				UpgradeCost = temp.UpgradeCost;
			}
			else if (upgradeSlots[1].Item != null && upgradeSlots[1].Item is UpgradeableWithScroll)
			{
				var temp = upgradeSlots[1].Item as UpgradeableWithScroll;
				UpgradeCost = Math.Max(UpgradeCost, temp.UpgradeCost);
			}
			else
				UpgradeCost = 0;
			if (PreviousUpgradeCost != UpgradeCost)
			{
				UpgradeCostText.Message = String.Format("Upgrade cost: {0}", UpgradeCost);
			}
		}

		private void PlayAnimations()
		{
			MainSprite.animationManager.Play(MainSprite.animations["Idle"]);
		}

		protected override void OnActivate()
		{
			base.OnActivate();
			player.activeSlots = upgradeSlots;
		}
	}
}
