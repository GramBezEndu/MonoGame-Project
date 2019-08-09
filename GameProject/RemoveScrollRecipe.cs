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
	public class RemoveScrollRecipe : Recipe
	{
		public RemoveScrollRecipe(Game1 g, GameState gs, Vector2 pos) :
			base(g, gs, pos, "Remove scroll", new Texture2D[4] { gs.Textures["StartingSword"], gs.Textures["PurificationStone"], gs.Textures["StartingSword"], gs.Textures["ImprovementScroll"] }, 0)
		{

		}
	}
}
