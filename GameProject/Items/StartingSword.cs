using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Items
{
	public class StartingSword : Sword
	{
		public StartingSword(Texture2D t, float scale) : base(t, scale)
		{
		}
		public override void Equip()
		{
			throw new NotImplementedException();
		}
	}
}
