using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Items
{
    public class Bone : Item
    {
        public Bone(Texture2D t, float scale) : base(t, scale)
        {
            IsStackable = true;
            Name = "Bone";
            Description = "Common drop from skeleton monsters.\n"
            + "Used to upgrade gear.";
        }
    }
}
