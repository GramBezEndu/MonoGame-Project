﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Items;
using GameProject.Sprites;
using GameProject.States;

namespace GameProject.Inventory
{
	public class BootsSlot : EquipmentSlot
	{
		public new Boots Item { get; set; }
		public BootsSlot(GraphicsDevice gd, Player p, Texture2D t, SpriteFont f, float scale) : base(gd, p, t, f, scale)
		{
		}
	}
}
