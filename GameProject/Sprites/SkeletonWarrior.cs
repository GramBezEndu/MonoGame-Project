using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.Animations;
using Microsoft.Xna.Framework;

namespace GameProject.Sprites
{
	public class SkeletonWarrior : Enemy
	{
        /// <summary>
        /// Player reference
        /// </summary>
        private Player player { get; set; }
		private bool DyingAnimationFinished;
		public SkeletonWarrior(Dictionary<string, Animation> a, Player p) : base(a)
		{
            player = p;
			animations["Die"].OnAnimationEnd = Die;
		}

		private void Die(object sender, EventArgs e)
		{
			DyingAnimationFinished = true;
			//base.OnDeath();
			//Drop gold and items here
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			animationManager.Update(gameTime);
			PlayAnimations();
		}
		private void PlayAnimations()
		{
			if (IsDead && DyingAnimationFinished)
				animationManager.Play(animations["Dead"]);
			else if (IsDead)
				animationManager.Play(animations["Die"]);
			else if (AgroActivated)
				animationManager.Play(animations["Run"]);
			else
				animationManager.Play(animations["Idle"]);
		}
	}
}
