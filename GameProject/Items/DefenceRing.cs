using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Items
{
    public class DefenceRing : Ring
    {
        public DefenceRing(Texture2D t, Vector2 scale) : base(t, scale)
        {
			Name = "Defence Ring";
			Attributes["DamageReduction"] = 0.25f;
			UpdateDescription();
		}
    }
}
