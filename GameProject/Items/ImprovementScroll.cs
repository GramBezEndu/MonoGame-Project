using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Items
{
    public class ImprovementScroll : Item
    {
        public float ImprovementPercent { get; protected set; }
        public ImprovementScroll(Game1 g, Texture2D t, float scale) : base(t, scale)
        {
			int min = 10;
			int max = 20;
            ImprovementPercent = g.RandomNumber(min, max);
            Name = "Improvement Scroll";
            Description = "Improves bonuses in item by " + min.ToString() + "-" + max.ToString() + "%.\n" +
                "Visit Blacksmith to use it.\n" +
                "Scroll power: " + ImprovementPercent.ToString() + "%";
        }
    }
}
