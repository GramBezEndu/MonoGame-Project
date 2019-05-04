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
		public bool AgroActivated { get; private set; }
		public int Health { get; private set; }
		public Enemy(Dictionary<string, Animation> a) : base(a)
		{
			Health = 1;
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
			base.Update(gameTime);
		}

		public void DealDmg(int dmg)
		{
			Health -= dmg;
		}
	}
}
