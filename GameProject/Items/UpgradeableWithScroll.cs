using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.Sprites;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Inventory;

namespace GameProject.Items
{
	public abstract class UpgradeableWithScroll : Equippable
	{
		/// <summary>
		/// Rings, Necklaces, shields are not upgradeable
		/// </summary>
		/// <param name="gd"></param>
		/// <param name="p"></param>
		/// <param name="slotTexture"></param>
		/// <param name="f"></param>
		/// <param name="t"></param>
		/// <param name="scale"></param>
		public UpgradeableWithScroll(GraphicsDevice gd, Player p, Texture2D slotTexture, SpriteFont f, Texture2D t, float scale) : base(t, scale)
		{
			ImprovementScrollSlot = new ImprovementScrollSlot(gd, p, slotTexture, f, scale);
		}

		public void Upgrade(ImprovementScroll improvementScroll)
		{
			ImprovementScrollSlot.Item = improvementScroll;

		}
	}
}
