using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Sprites;

namespace GameProject.Items
{
	public class StartingSword : Sword
	{
		public StartingSword(GraphicsDevice gd, Player p, Texture2D slotTexture, SpriteFont f, Texture2D t, float scale) : base(gd, p, slotTexture, f, t, scale)
		{
			DamageMin = 7;
			DamageMax = 10;
			CriticalStrikeChance = 0.05f;
			Description = "Damage: " + DamageMin.ToString() + "-" + DamageMax.ToString() + '\n' + "Critical Strike Chance: " + CriticalStrikeChance.ToString();
		}
	}
}
