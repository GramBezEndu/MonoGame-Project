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
        private float _improvementPower;

        public virtual float MinPower { get; } = 0.10f;
        public virtual float MaxPower { get; } = 0.20f;

        public float ImprovementPower
        {
            get => _improvementPower;
            set
            {
                if(value < MinPower || value > MaxPower)
                {
                    int x;
                    throw new Exception("Invalid improvement power percent\n");
                }
                else
                {
                    _improvementPower = value;
                }
            }
        }
        public ImprovementScroll(Game1 g, Texture2D t, float scale) : base(t, scale)
        {
            //Make sure it is float
            ImprovementPower = g.RandomNumber((int)Math.Round(MinPower * 100), (int)Math.Round(MaxPower * 100)) / 100f;
            Name = "Improvement Scroll";
            //We create special description for this item
            Description = "Improves bonuses in item by " + MinPower * 100 + "-" + MaxPower * 100 + "%.\n" +
                "Visit Blacksmith to use it.\n" +
                "Scroll power: " + ImprovementPower * 100 + "%";
        }
    }
}
