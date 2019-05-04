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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameProject.Sprites
{
	public abstract class Enemy : Sprite
	{
        /// <summary>
        /// Need to add scaling based on screen resolution (or make map tile-based)
        /// </summary>
		protected float agroRange = 600f;
		/// <summary>
		/// Determines if enemy is dead (note: enemy is dead equals True before corpses has fallen)
		/// </summary>
		protected bool IsDead;
		private SpriteFont font;
		/// <summary>
		/// For how long we will be displaying how much damage enemy recieved
		/// </summary>
		private const float DamageReceivedTime = 1f;
		private GameTimer DamageReceiveTimer = new GameTimer(1f);
		private int lastDamageReceived = 0;
		public bool AgroActivated { get; private set; }
		public int Health { get; protected set; }
		public Enemy(SpriteFont f, Dictionary<string, Animation> a) : base(a)
		{
			Health = Int32.MaxValue;
			font = f;
		}
		/// <summary>
		///Check if player is close (in agro range [we check for X axis only]) then perform action (probably run enemy to player)
		/// </summary>
		public void IsPlayerClose(Player p)
		{
			if (p.Position.X > this.Position.X - agroRange && p.Position.X < this.Position.X + agroRange)
				AgroActivated = true;
			else
				AgroActivated = false;
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

			//Start the timer if there was a damage and timer is not already started (that way we do not restart the timer over and over again)
			if(lastDamageReceived > 0 && !DamageReceiveTimer.Enabled)
			{
				DamageReceiveTimer.Start();
			}
			//If there is time left - draw
			if(DamageReceiveTimer.Enabled && DamageReceiveTimer.CurrentTime > 0)
			{
				spriteBatch.DrawString(font, "-" + lastDamageReceived.ToString(), new Vector2(this.Position.X + this.Width/2, this.Position.Y), Color.Red);
			}
			//There is no time left - turn off timer and set lastDamageDealt to 0
			else if(DamageReceiveTimer.Enabled && DamageReceiveTimer.CurrentTime <= 0)
			{
				DamageReceiveTimer.Reset();
				lastDamageReceived = 0;
			}
		}

		public void DealDmg(int dmg)
		{
			//We do not want to attack a dead enemy
			if (IsDead)
				return;
			Health -= dmg;
			lastDamageReceived = dmg;
		}
	}
}
