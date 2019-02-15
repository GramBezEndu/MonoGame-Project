﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Items;

namespace GameProject.Inventory
{
	public class BowSlot : InventorySlot
	{
		public new Bow Item { get; set; }
		public BowSlot(Texture2D t, SpriteFont f, float scale) : base(t, f, scale)
		{
		}
	}
}
