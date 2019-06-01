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
			ImprovementPercent = g.RandomNumber(min, max);
            //Update description
            Name = "Legendary Improvement Scroll";
			Description = "Improves bonuses in item by " + min.ToString() + "-" + max.ToString() + "%.\n" +
				"Visit Blacksmith to use it.\n" +
				"Scroll power: " + ImprovementPercent.ToString() + "%";
		}
    }
}
