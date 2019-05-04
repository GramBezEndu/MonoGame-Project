using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Sprites;

namespace GameProject.Items
{
	public class StartingBoots : Boots
	{
		public StartingBoots(Texture2D t, float scale) : base(t, scale)
		{
			DamageReduction = 0.05f;
			MovementSpeed = 0.2f;
			Description = "Damage Reduction: " + DamageReduction.ToString() + "\nMovement speed: " + MovementSpeed.ToString();
		}
	}
}
