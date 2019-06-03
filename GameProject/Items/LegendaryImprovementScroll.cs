using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Items
{
    public class LegendaryImprovementScroll : ImprovementScroll
    {
        public LegendaryImprovementScroll(Game1 g, Texture2D t, float scale) : base(g, t, scale)
        {
			int min = 21;
			int max = 30;
			//Make sure it is float
			ImprovementPercent = g.RandomNumber(min, max)/100f;
            Name = "Legendary Improvement Scroll";
			//We create special description for this item
			Description = "Improves bonuses in item by " + min + "-" + max + "%.\n" +
				"Visit Blacksmith to use it.\n" +
				"Scroll power: " + ImprovementPercent * 100 + "%";
		}
    }
}
