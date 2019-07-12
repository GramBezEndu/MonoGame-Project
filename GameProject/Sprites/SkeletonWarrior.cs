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
		public SkeletonWarrior(Game1 g, GameState gs, SpriteFont f, Dictionary<string, Animation> a, Player p) : base(g, gs, f, a, p)
		{
			Health = 15;
			animations["Attack"].OnAnimationEnd = Attack;
			animations["Die"].OnAnimationEnd = Die;
			Melee = true;
			damageMin = 3;
			damageMax = 5;
			AttackTimer = new GameTimer(1.5f, true);
		}

		private void Die(object sender, EventArgs e)
		{
			DyingAnimationFinished = true;
            //base.OnDeath();
            //Drop gold and items here
            //30% chance for 30-50 gold
            //25% chance for 1 bone
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
            drop = game.RandomPercent();
            if (drop <= 25)
            {
                Bone bone = new Bone(gameState.Textures["Bone"], game.Scale)
                {
                    Position = new Vector2(this.Position.X + this.Width / 2, this.Position.Y + this.Height / 2),
                    Quantity = 1
                };
                gameState.SpawnItem(bone);
            }

        }

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}
		protected override void PlayAnimations()
		{
			if (IsDead && DyingAnimationFinished)
				animationManager.Play(animations["Dead"]);
			else if (IsDead)
				animationManager.Play(animations["Die"]);
			else if (isAttacking)
				animationManager.Play(animations["Attack"]);
			//Moving right
			else if (Velocity.X > 0)
			{
				FlipHorizontally = false;
				animationManager.Play(animations["Run"]);
			}
			//Moving left
			else if (Velocity.X < 0)
			{
				FlipHorizontally = true;
				animationManager.Play(animations["Run"]);
			}
			else
				animationManager.Play(animations["Idle"]);
		}
	}
}
