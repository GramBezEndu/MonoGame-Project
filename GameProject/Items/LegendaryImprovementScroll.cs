using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Items
{
    public class LegendaryImprovementScroll : ImprovementScroll
    {
        public override float MinPower { get; } = 0.21f;
        public override float MaxPower { get; } = 0.30f;
        public LegendaryImprovementScroll(Game1 g, Texture2D t, Vector2 scale) : base(g, t, scale)
        {
            Name = "Legendary Improvement Scroll";
            ////Make sure it is float
            //ImprovementPower = g.RandomNumber((int)MinPower * 100, (int)MaxPower * 100) / 100f;
            ////We create special description for this item
            //Description = "Improves bonuses in item by " + MinPower * 100 + "-" + MaxPower * 100 + "%.\n" +
            //    "Visit Blacksmith to use it.\n" +
            //    "Scroll power: " + ImprovementPower * 100 + "%";
        }
    }
}
