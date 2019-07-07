using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.Animations;
using GameProject.States;
using GameProject.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Sprites
{
	public class SkeletonArcher : Enemy
	{
		public SkeletonArcher(Game1 g, GameState gs, SpriteFont f, Dictionary<string, Animation> a, Player p) : base(g, gs, f, a, p)
		{
			Health = 10;
			animations["Attack"].OnAnimationEnd = Attack;
			animations["Die"].OnAnimationEnd = Die;
			Melee = false;
			damageMin = 2;
			damageMax = 4;
		}

		protected override void Attack(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		private void Die(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		protected override void PlayAnimations()
		{
			throw new NotImplementedException();
		}
	}
}
