using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Items;
using GameProject.Sprites;

namespace GameProject.Inventory
{
	public class SwordSlot : InventorySlot
	{
		public new Sword Item { get; set; }
		public SwordSlot(GraphicsDevice gd, Player p, Texture2D t, SpriteFont f, float scale) : base(gd, p, t, f, scale)
		{
		}
	}
}
