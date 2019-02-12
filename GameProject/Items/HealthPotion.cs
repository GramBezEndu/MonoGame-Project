using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject.Sprites;

namespace GameProject
{
	public class HealthPotion : Usable 
	{
		private int maxCharges;
		private int leftCharges;
		public HealthPotion(Texture2D t, float scale) : base(t, scale)
		{
			texture = t;
			//rectangle = new Rectangle ((int)position.X, (int)position.Y, t.Width, t.Height);
			maxCharges = 10;
			leftCharges = maxCharges;
			IsStackable = true;
			Name = "Health potion";
			Description = "Restores 10 health over time";
		}
		public override void Update(GameTime gameTime)
		{
			throw new NotImplementedException();
		}
		public override void Use()
		{
			throw new NotImplementedException();
		}
	}
}
