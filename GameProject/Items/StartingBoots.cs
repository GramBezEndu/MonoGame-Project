using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Sprites;

namespace GameProject.Items
{
	public class StartingBoots : Boots
	{
		public StartingBoots(GraphicsDevice gd, Player p, Texture2D slotTexture, SpriteFont f, Texture2D t, float scale) : base(gd, p, slotTexture, f, t, scale)
		{
			Attributes["DamageReduction"] = 0.05f;
			Attributes["MovementSpeed"] = 0.2f;
			UpdateDescription();
		}
	}
}
