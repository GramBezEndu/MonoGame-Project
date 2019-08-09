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
	public class LegendaryScrollUpgradeRecipe : Recipe
	{
		public LegendaryScrollUpgradeRecipe(Game1 g, GameState gs, Vector2 pos) : 
			base(g, gs, pos, new Texture2D[3] { gs.Textures["LegendaryImprovementScroll"], gs.Textures["LegendaryImprovementScroll"], gs.Textures["LegendaryImprovementScroll"] }, 0)
		{

		}
	}
}