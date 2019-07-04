﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject.Sprites;
using GameProject.Items;

namespace GameProject.Inventory
{
	public class EquipmentSlot : InventorySlot
	{
		public EquipmentSlot(GraphicsDevice gd, Player p, Texture2D t, SpriteFont f, float scale) : base(gd, p, t, f, scale)
		{

		}
	}
}
