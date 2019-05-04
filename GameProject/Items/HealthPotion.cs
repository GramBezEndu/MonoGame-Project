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
		private int maxRestore;
		//private int leftRestore;
		public HealthPotion(Texture2D t, float scale) : base(t, scale)
		{
			texture = t;
			//Rectangle = new Rectangle ((int)position.X, (int)position.Y, t.Width, t.Height);
			maxRestore = 10;
			//leftRestore = maxRestore;
			IsStackable = true;
			Name = "Health potion";
			Description = "Restores " + maxRestore.ToString() + " health over time";
		}
		public override void Update(GameTime gameTime)
		{
			//throw new NotImplementedException();
		}
		/// <summary>
		/// Uses health potion (it can not be used if any health will not be restored)
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public override bool Use(Player p)
		{
			if (p.HealthBar.Health.CurrentHealth == p.HealthBar.Health.MaxHealth || p.HealthBar.Health.CurrentHealth+p.HealthToRestore >= p.HealthBar.Health.MaxHealth)
				return false;
			p.RestoreHealth(maxRestore);
			return true;
		}
	}
}
