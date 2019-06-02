using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Items
{
	public class StartingShield : Shield
	{
		public StartingShield(Texture2D t, float scale) : base(t, scale)
		{
			BlockingDamageReduction = 0.4f;
			MaxDurability = 100;
			CurrentDurability = MaxDurability;
			Description = "Damage Reduction while blocking: " + BlockingDamageReduction * 100 + "%" + "\nDurability: " + CurrentDurability.ToString() + "/" + MaxDurability.ToString();
		}
		public override void Update(GameTime gameTime)
		{
			Description = "Damage Reduction while blocking: " + BlockingDamageReduction * 100 + "%" + "\nDurability: " + CurrentDurability.ToString() + "/" + MaxDurability.ToString();
		}
	}
}
