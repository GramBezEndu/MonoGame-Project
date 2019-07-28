using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Sprites
{
	public abstract class EnemyProjectile : Projectile
	{
		protected Player player;
		public EnemyProjectile(Game1 game, Player p, Texture2D t, Vector2 scale, bool rotatedLeft) : base(game, t, scale, rotatedLeft)
		{
			player = p;
		}

		public override void Update(GameTime gameTime)
		{
			if(!Hidden)
			{
				base.Update(gameTime);
				//We hit the player
				if (this.IsTouching(player))
				{
					int dmg = Game.RandomNumber(DamageMin, DamageMax);
					player.GetPhysicalDamage(dmg);
					Hidden = true;
				}
			}
		}
	}
}
