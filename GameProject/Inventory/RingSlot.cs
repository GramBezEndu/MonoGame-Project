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
	public class RingSlot : InventorySlot
	{
		public new Ring Item { get; set; }
		public RingSlot(GraphicsDevice gd,Player p,Texture2D t, SpriteFont f, float scale) : base(gd, p, t, f, scale)
		{
		}
	}
}
