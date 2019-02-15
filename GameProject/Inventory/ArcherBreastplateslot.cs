using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Items;

namespace GameProject.Inventory
{
	public class ArcherBreastplateslot : InventorySlot
	{
		public new ArcherBreastplateslot Item { get; set; }
		public ArcherBreastplateslot(Texture2D t, SpriteFont f, float scale) : base(t, f, scale)
		{
		}
	}
}
