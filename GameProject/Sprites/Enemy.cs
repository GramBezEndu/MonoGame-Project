using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.Animations;
using GameProject.Sprites;
using GameProject.Items;
using GameProject.Inventory;
using GameProject.Controls;
using GameProject.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameProject.Sprites
{
	public abstract class Enemy : Sprite
	{
		protected float runStepDistance;
		protected float agroRange;
		protected bool isAttacking;
		/// <summary>
		/// Determines if enemy is dead (note: enemy is dead equals True before corpses has fallen)
		/// </summary>
		protected bool IsDead;
		private SpriteFont font;
		/// <summary>
		/// For how long we will be displaying how much damage enemy received
		/// </summary>
		private const float DamageReceivedTime = 1f;
        /// <summary>
        /// Timer to display how much damage enemy received
        /// </summary>
		private GameTimer DamageReceiveTimer = new GameTimer(1f);
		private int lastDamageReceived = 0;
		private bool lastDamageCriticalHit;
		public bool AgroActivated { get; private set; }
		public int Health { get; protected set; }
        /// <summary>
        /// Game reference for Random numbers
        /// </summary>
        protected Game1 game;
        /// <summary>
        /// Game state reference to drop items
        /// </summary>
        protected GameState gameState;
		protected bool Melee;
		public Enemy(Game1 g, GameState gs, SpriteFont f, Dictionary<string, Animation> a) : base(a)
		{
			Health = Int32.MaxValue;
			font = f;
            game = g;
            gameState = gs;
			agroRange = 600f * g.Scale;
			runStepDistance = 4.5f * g.Scale;
		}
		/// <summary>
		///Check if player is close (in agro range [we check for X axis only]) then perform action (probably run enemy to player)
		/// </summary>
		public void IsPlayerClose(Player p)
		{
			if (IsDead)
				return;
			if (p.Position.X > this.Position.X - agroRange && p.Position.X < this.Position.X + agroRange)
			{
				AgroActivated = true;
				RunToPlayer(p);
			}
		}
		/// <summary>
		/// Run to player if aggro was activated
		/// </summary>
		public void RunToPlayer(Player player)
		{
			if(Melee && AgroActivated && !IsDead)
			{
				//We can attack
				if(this.IsTouching(player))
				{
					return;
				}
				//We need to run to player
				else if(player.Position.X < Position.X)
				{
					Position -= new Vector2(runStepDistance, 0);
				}
				else if(player.Position.X > Position.X)
				{
					Position += new Vector2(runStepDistance, 0);
				}
			}
		}

		public override void Update(GameTime gameTime)
		{
			if (Health <= 0)
			{
				IsDead = true;
			}
			DamageReceiveTimer.Update(gameTime);
			base.Update(gameTime);
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);
			//Draw damage recieved from player

			//If there is time left - draw
			if(DamageReceiveTimer.Enabled && DamageReceiveTimer.CurrentTime > 0)
			{
				string dmgReceived = "-" + lastDamageReceived.ToString();
				Vector2 size = font.MeasureString(dmgReceived);
				if(lastDamageCriticalHit)
					spriteBatch.DrawString(font, "-" + lastDamageReceived.ToString(), new Vector2(this.Position.X + this.Width/2 - size.X/2, this.Position.Y), Color.Gold);
				else
					spriteBatch.DrawString(font, "-" + lastDamageReceived.ToString(), new Vector2(this.Position.X + this.Width / 2 - size.X/2, this.Position.Y), Color.DarkRed);
			}
			//There is no time left - turn off timer and set lastDamageDealt to 0
			else if(DamageReceiveTimer.Enabled && DamageReceiveTimer.CurrentTime <= 0)
			{
				DamageReceiveTimer.Reset();
			}
		}

		public void DealDmg(int dmg, bool criticalHit)
		{
			//We do not want to attack a dead enemy
			if (IsDead)
				return;
			Health -= dmg;
			lastDamageReceived = dmg;
			lastDamageCriticalHit = criticalHit;
			DamageReceiveTimer.Start();
		}
	}
}
