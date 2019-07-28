using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.Sprites;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Inventory;
using Microsoft.Xna.Framework;

namespace GameProject.Items
{
	public abstract class UpgradeableWithScroll : Equippable
	{
        public int UpgradeCost { get; set; } = 500;
		public ImprovementScrollSlot ImprovementScrollSlot { get; set; }
		/// <summary>
		/// Rings, Necklaces, shields are not upgradeable
		/// </summary>
		/// <param name="gd"></param>
		/// <param name="p"></param>
		/// <param name="slotTexture"></param>
		/// <param name="f"></param>
		/// <param name="t"></param>
		/// <param name="scale"></param>
		public UpgradeableWithScroll(GraphicsDevice gd, Player p, Texture2D slotTexture, SpriteFont f, Texture2D t, Vector2 scale) : base(t, scale)
		{
			ImprovementScrollSlot = new ImprovementScrollSlot(gd, p, slotTexture, f, scale);
		}

		public void Upgrade(ImprovementScroll improvementScroll)
		{
			ImprovementScrollSlot.Item = improvementScroll;
			//Update attributes
			//Remember to correctly round numbers
			foreach(var a in Attributes.Keys.ToList())
			{
				if(a == "DamageMin" || a == "DamageMax")
				{
					//Round to int
					Attributes[a] = (float)(Math.Round((1 + improvementScroll.ImprovementPower) * Attributes[a]));
				}
				//Round to 2 decimal places
				else
				{
					Attributes[a] = (float)(Math.Round((1 + improvementScroll.ImprovementPower) * Attributes[a], 2));
				}
			}
			UpdateDescription();
		}
	}
}
