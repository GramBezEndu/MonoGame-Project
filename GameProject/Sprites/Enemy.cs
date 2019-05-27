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
		public int Health { get; protected set; } = Int32.MaxValue;
		/// <summary>
		/// Game reference for Random numbers
		/// </summary>
		protected Game1 game;
        /// <summary>
        /// Game state reference to drop items
        /// </summary>
        protected GameState gameState;
		/// <summary>
		/// Player reference
		/// </summary>
		protected Player player { get; set; }
		protected bool Melee;
		protected int damageMin = Int32.MaxValue;
		protected int damageMax = Int32.MaxValue;
		public Enemy(Game1 g, GameState gs, SpriteFont f, Dictionary<string, Animation> a, Player p) : base(a)
		{
			font = f;
            game = g;
            gameState = gs;
			player = p;
			agroRange = 600f * g.Scale;
			runStepDistance = 2.5f * g.Scale;
		}
		/// <summary>
		///Check if player is close (in agro range [we check for X axis only]) then perform action (probably run enemy to player)
		/// </summary>
		public void IsPlayerClose(Player p)
		{
			if (IsDead)
				return;
			//If player is close activate the aggro
			if (p.Position.X > this.Position.X - agroRange && p.Position.X < this.Position.X + agroRange)
			{
				AgroActivated = true;
			}
			if (AgroActivated && Melee)
				RunToPlayer(p);
		}
		/// <summary>
		/// Run to player if aggro was activated
		/// </summary>
		public void RunToPlayer(Player player)
		{
			if(AgroActivated)
			{
				//We are already attacking -> take no action (return)
				if (isAttacking)
					return;
				//We can start attacking
				else if(this.IsTouching(player))
				{
					isAttacking = true;
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

		protected void Attack(object sender, EventArgs e)
		{
			isAttacking = false;
			player.GetDamage(game.RandomNumber(damageMin, damageMax));
		}

		/// <summary>
		/// Get damage, Note: getting damage activates aggro
		/// </summary>
		/// <param name="dmg"></param>
		/// <param name="criticalHit"></param>
		public void GetDamage(int dmg, bool criticalHit)
		{
			//We do not want to attack a dead enemy
			if (IsDead)
				return;
			Health -= dmg;
			lastDamageReceived = dmg;
			lastDamageCriticalHit = criticalHit;
			AgroActivated = true;
			DamageReceiveTimer.Start();
		}
	}
}
