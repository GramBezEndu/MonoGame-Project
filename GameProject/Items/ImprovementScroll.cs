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
            ImprovementPercent = g.RandomNumber(20, 30);
            Name = "Improvement Scroll";
            Description = "Improves bonuses in item.\n" +
                "Visit Blacksmith to use it.\n" +
                "Scroll power: " + ImprovementPercent.ToString() + "%";
        }
    }
}
