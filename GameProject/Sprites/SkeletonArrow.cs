using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Sprites
{
	public class SkeletonArrow : EnemyProjectile
	{
		public SkeletonArrow(Game1 game, Player p, Texture2D t, float scale, bool rotatedLeft = true) : base(game, p, t, scale, rotatedLeft)
		{
			DamageMin = 2;
			DamageMax = 4;
			distancePerFrame = 9f * game.Scale;
			if (rotatedLeft == false)
				this.FlipHorizontally = true;
		}
	}
}
