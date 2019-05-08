﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Items
{
	public class StartingArcherHelmet : ArcherHelmet
	{
		public StartingArcherHelmet(Texture2D t, float scale) : base(t, scale)
		{
			DamageReduction = 0.08f;
			Description = "Damage Reduction: " + DamageReduction.ToString();
		}
	}
}