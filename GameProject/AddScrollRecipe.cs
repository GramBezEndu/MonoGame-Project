using GameProject.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject
{
	public class AddScrollRecipe : Recipe
	{
		public AddScrollRecipe(Game1 g, GameState gs, Vector2 pos) :
			base(g, gs, pos, "Add scroll to item", new Texture2D[3] { gs.Textures["StartingSword"], gs.Textures["ImprovementScroll"], gs.Textures["StartingSword"] }, 500)
		{

		}
	}
}
