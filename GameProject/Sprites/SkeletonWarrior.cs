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
	public class SkeletonWarrior : Enemy
	{
        /// <summary>
        /// Player reference
        /// </summary>
        private Player player { get; set; }
		private bool DyingAnimationFinished;
		public SkeletonWarrior(Game1 g, GameState gs, SpriteFont f, Dictionary<string, Animation> a, Player p) : base(g, gs, f, a)
		{
			Health = 15;
            player = p;
			animations["Die"].OnAnimationEnd = Die;
			Melee = true;
		}

		private void Die(object sender, EventArgs e)
		{
			DyingAnimationFinished = true;
            //base.OnDeath();
            //Drop gold and items here
            //30% chance for 30-50 gold
            int drop = game.RandomPercent();
            if(drop <= 30)
            {
                int quantity = game.RandomNumber(30, 50);
                GoldCoin goldCoin = new GoldCoin(gameState.Textures["Gold"], game.Scale)
                {
                    Position = new Vector2(this.Position.X+this.Width/2, this.Position.Y+this.Height/2),
                    Quantity = quantity
                };
                gameState.SpawnItem(goldCoin);
            }
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
