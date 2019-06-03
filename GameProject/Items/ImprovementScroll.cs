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
			//Make sure it is float
			ImprovementPercent = g.RandomNumber(min, max)/100f;
            Name = "Improvement Scroll";
			//We create special description for this item
			Description = "Improves bonuses in item by " + min + "-" + max + "%.\n" +
                "Visit Blacksmith to use it.\n" +
                "Scroll power: " + ImprovementPercent * 100 + "%";
        }
    }
}
