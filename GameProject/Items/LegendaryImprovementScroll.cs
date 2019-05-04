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
            ImprovementPercent = g.RandomNumber(40, 50);
            //Update description
            Name = "Legendary Improvement Scroll";
            Description = "Improves bonuses in item.\n" +
            "Visit Blacksmith to use it.\n" +
            "Scroll power: " + ImprovementPercent.ToString() + "%";
        }
    }
}
