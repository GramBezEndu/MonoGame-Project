﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Items;
using GameProject.Sprites;
using GameProject.States;
using Microsoft.Xna.Framework;

namespace GameProject.Inventory
{
	public class NecklaceSlot : EquipmentSlot
	{
		public new Necklace Item { get; set; }
		public NecklaceSlot(GraphicsDevice gd, Player p,Texture2D t, SpriteFont f, Vector2 scale) : base(gd, p, t, f, scale)
		{
		}
	}
}
