using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Items;
using GameProject.Sprites;
using GameProject.States;

namespace GameProject.Inventory
{
	public class ArcherBreastplateSlot : EquipmentSlot
	{
		public new ArcherBreastplateSlot Item { get; set; }
		public ArcherBreastplateSlot(GraphicsDevice gd, Player p, Texture2D t, SpriteFont f, Vector2 scale) : base(gd, p, t, f, scale)
		{
		}
	}
}
