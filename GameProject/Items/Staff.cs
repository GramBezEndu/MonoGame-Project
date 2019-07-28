﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Sprites;
using GameProject.Inventory;
using Microsoft.Xna.Framework;

namespace GameProject.Items
{
	public abstract class Staff : Weapon
	{
		public Staff(GraphicsDevice gd, Player p, Texture2D slotTexture, SpriteFont f, Texture2D t, Vector2 scale) : base(gd, p, slotTexture, f, t, scale)
		{
		}
		public override bool Equip(Player p)
		{
			throw new NotImplementedException();
		}
	}
}
