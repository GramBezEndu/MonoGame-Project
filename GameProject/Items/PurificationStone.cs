using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Items
{
	public class PurificationStone : Item
	{
		public PurificationStone(Texture2D t, float scale) : base(t, scale)
		{
			IsStackable = true;
			Name = "Purification Stone";
			Description = "Removes scroll from item.\n" +
				"Scroll is regained.";
		}
	}
}
