using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Sprites;

namespace GameProject.Items
{
	public class StartingBow : Bow
	{
		public StartingBow(Texture2D t, float scale) : base(t, scale)
		{
		}
	}
}
