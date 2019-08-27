using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.Controls;
using GameProject.Sprites;
using GameProject.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject
{
	public class Recipe : Component
	{
		Game1 Game;
		GameState GameState;
		string Name;
		Texture2D[] Textures;
		int Cost;

		private Vector2 position;

		public Vector2 Position
		{
			get => position;
			set
			{
				position = value;
				//Change position of components if they were initialized (there are any)
				if(components.Count >= 1)
				{
					components.Clear();
					if(recipeComponents[0].Hidden == true)
					{
						recipeComponents.Clear();
						CreateComponents(Game, GameState, position, Name, Textures, Cost, true);
					}
					else
					{
						recipeComponents.Clear();
						CreateComponents(Game, GameState, position, Name, Textures, Cost, false);
					}
				}
			}
		}

		/// <summary>
		/// Returns the actual hight of recipe
		/// </summary>
		public int Height
		{
			get
			{
				//Last element is either text or sprite
				if (recipeComponents[recipeComponents.Count() - 1] is Text)
				{
					if (recipeComponents[0].Hidden == false)
						return (int)((recipeComponents[recipeComponents.Count() - 1] as Text).Position.Y + (recipeComponents[recipeComponents.Count() - 1] as Text).Height - this.Position.Y);
					else
						return Math.Max((components[0] as Button).Height, (components[1] as TextButton).Height);
				}
				else
				{
					if (recipeComponents[0].Hidden == false)
						return (int)((recipeComponents[recipeComponents.Count() - 1] as Sprite).Position.Y + (recipeComponents[recipeComponents.Count() - 1] as Sprite).Height - this.Position.Y);
					else
						return Math.Max((components[0] as Button).Height, (components[1] as TextButton).Height);
				}
			}
		}

		protected List<Component> components = new List<Component>();
		protected List<Component> recipeComponents = new List<Component>();

		/// <summary>
		/// Last component should be either text or the last sprite
		/// </summary>
		/// <param name="g"></param>
		/// <param name="gs"></param>
		/// <param name="pos"></param>
		/// <param name="textures">list of textures that will be put into slots</param>
		/// <param name="cost"></param>
		public Recipe(Game1 g, GameState gs, Vector2 pos, string name, Texture2D[] textures, int cost)
		{
			//Save parameteres so we can use them later
			Game = g;
			GameState = gs;
			Position = pos;
			Name = name;
			Textures = textures;
			Cost = cost;

			//Hide recipe components on default
			CreateComponents(g, gs, pos, name, textures, cost, true);
		}

		private void CreateComponents(Game1 g, GameState gs, Vector2 pos, string name, Texture2D[] textures, int cost, bool hideRecipeComponents)
		{
			var button = new Button(gs.Textures["ArrowSelector"], gs.Font, g.Scale * new Vector2(0.5f, 0.5f))
			{
				Position = pos,
				Click = HideShowRecipe
			};

			components.Add(button);

			var textButton = new TextButton(g.Input, gs.Font, name)
			{
				Position = new Vector2(button.Position.X + button.Width, button.Position.Y),
				Click = HideShowRecipe
			};

			components.Add(textButton);

			Sprite[] slots =
			{
				new Sprite(gs.Textures["InventorySlot"], g.Scale),
				new Sprite(gs.Textures["InventorySlot"], g.Scale),
				new Sprite(gs.Textures["InventorySlot"], g.Scale),
				new Sprite(gs.Textures["InventorySlot"], g.Scale),
			};

			var arrow = new Sprite(gs.Textures["Arrow"], g.Scale);

			slots[0].Position = new Vector2(textButton.Position.X, textButton.Rectangle.Bottom);
			slots[1].Position = new Vector2(slots[0].Rectangle.Right, slots[0].Position.Y);
			arrow.Position = new Vector2(slots[1].Rectangle.Right, slots[1].Position.Y);
			slots[2].Position = new Vector2(arrow.Rectangle.Right, arrow.Position.Y);
			slots[3].Position = new Vector2(slots[2].Rectangle.Right, slots[2].Position.Y);

			Sprite[] items = new Sprite[textures.Count()];

			for (int i = 0; i < textures.Count(); i++)
			{
				items[i] = new Sprite(textures[i], g.Scale);
				items[i].Position = slots[i].Position;
			}

			recipeComponents.Add(arrow);
			recipeComponents.AddRange(slots);
			recipeComponents.AddRange(items);
			if (cost != 0)
			{
				recipeComponents.Add(new Text(gs.Font, "Cost: " + cost.ToString() + "g")
				{
					Position = new Vector2(slots[0].Position.X, slots[0].Rectangle.Bottom)
				});
			}

			if (hideRecipeComponents)
			{
				foreach (var r in recipeComponents)
					r.Hidden = true;
			}
			else
			{
				foreach (var r in recipeComponents)
					r.Hidden = false;
			}

			components.AddRange(recipeComponents);
		}

		public void HideShowRecipe(object sender, EventArgs e)
		{
			foreach (var c in recipeComponents)
				c.Hidden = !c.Hidden;
		}

		public void HideRecipe()
		{
			foreach (var c in recipeComponents)
				c.Hidden = true;
		}

		public void ShowRecipe()
		{
			foreach (var c in recipeComponents)
				c.Hidden = false;
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (!Hidden)
			{
				foreach (var c in components)
					c.Draw(gameTime, spriteBatch);
			}
		}

		public override void Update(GameTime gameTime)
		{
			if (!Hidden)
			{
				foreach (var c in components)
					c.Update(gameTime);
			}
		}
	}
}
