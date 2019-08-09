using GameProject.Sprites;
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
	public class ScrollUpgradeRecipe : Recipe
	{
		public ScrollUpgradeRecipe(Game1 g, GameState gs, Vector2 pos) :
			base(g, gs, pos, "Improvement Scroll Upgrade", new Texture2D[3] { gs.Textures["ImprovementScroll"], gs.Textures["ImprovementScroll"], gs.Textures["ImprovementScroll"] }, 0) 
		{

		}
	}
}
