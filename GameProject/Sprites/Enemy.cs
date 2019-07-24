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
		/// <summary>
		/// Distance at which enemy is seeing player
		/// </summary>
		protected float agroRange;
		/// <summary>
		/// Distance at which enemy can hit player (ranged only)
		/// </summary>
		protected float attackRange
		{
			get => _attackRange;
			set
			{
				if (!Melee)
					_attackRange = value;
				else
					throw new Exception("This unit is not ranged, you can not assign attackRange value");
			}
		}
		List<MovingText> DamageReceived = new List<MovingText>();
		protected bool isAttacking;
		/// <summary>
		/// Enemy is able to attack every N seconds (stated in this timer)
		/// </summary>
		protected GameTimer AttackTimer;
		/// <summary>
		/// Determines if enemy is dead (note: enemy is dead equals True before corpses has fallen)
		/// </summary>
		public bool IsDead { get; protected set; }
		protected bool DyingAnimationFinished;
		private SpriteFont font;
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
		private float _attackRange;

		public Enemy(Game1 g, GameState gs, SpriteFont f, Dictionary<string, Animation> a, Player p) : base(a)
		{
			font = f;
			game = g;
			gameState = gs;
			player = p;
			agroRange = 800f * g.Scale;
			runStepDistance = 2.5f * g.Scale;
		}
		/// <summary>
		///Check if player is close (in agro range [we check for X axis only]) then perform action (probably run to player)
		/// </summary>
		public virtual void IsPlayerClose(Player p)
		{
			if (IsDead)
				return;
			//If player is close activate the aggro
			if (p.Position.X > this.Position.X - agroRange && p.Position.X < this.Position.X + agroRange)
			{
				AgroActivated = true;
			}
			if (AgroActivated && Melee)
				MeleeRunAndAttack(p);
			else if (AgroActivated)
				RangedRunAndAttack(p);
		}

		private void RangedRunAndAttack(Player p)
		{
			//Calculate if in attack range
			Vector2 fixedEnemyPosition = new Vector2(Position.X + Width / 2, Position.Y);
			bool inAttackRange = false;
			if (Math.Abs(fixedEnemyPosition.X - p.Position.X) < attackRange)
			{
				inAttackRange = true;
			}

			//We are already attacking -> take no action (return)
			if (isAttacking)
				return;
			//player is dead -> take no action (return)
			else if (player.IsDead)
				return;
			//Ranged - We can start attacking because we're in attack range
			else if (inAttackRange)
			{
				//Attack is not on cooldown
				if(AttackTimer.CurrentTime <= 0)
				{
					isAttacking = true;
					AttackTimer.Start();
				}
				return;
			}
			//We need to run to player
			else if (player.Position.X < Position.X)
			{
				Velocity = new Vector2(-runStepDistance, 0);
			}
			else if (player.Position.X > Position.X)
			{
				Velocity = new Vector2(runStepDistance, 0);
			}
		}

		/// <summary>
		/// Run to player if aggro was activated and start attacking
		/// </summary>
		public void MeleeRunAndAttack(Player player)
		{
			//We are already attacking -> take no action (return)
			if (isAttacking)
				return;
			//player is dead -> take no action (return)
			else if (player.IsDead)
				return;
			//Melee - We can start attacking because we're touching player
			else if (this.IsTouching(player))
			{
				//Attack is not on cooldown
				if (AttackTimer.CurrentTime <= 0)
				{
					isAttacking = true;
					AttackTimer.Start();
				}
				return;
			}
			//We need to run to player
			else if (player.Position.X < Position.X)
			{
				Velocity = new Vector2(-runStepDistance, 0);
			}
			else if (player.Position.X > Position.X)
			{
				Velocity = new Vector2(runStepDistance, 0);
			}
		}

		protected abstract void PlayAnimations();

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			foreach (var x in DamageReceived)
				x.Update(gameTime);
			AttackTimer?.Update(gameTime);
			IsPlayerClose(player);
			if (Health <= 0)
			{
				IsDead = true;
			}
			animationManager.Update(gameTime);
			PlayAnimations();
			Position += Velocity;
			Velocity = Vector2.Zero;
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);
			foreach (var x in DamageReceived)
				x.Draw(gameTime, spriteBatch);
		}

		protected virtual void Attack(object sender, EventArgs e)
		{
			isAttacking = false;
			player.GetPhysicalDamage(game.RandomNumber(damageMin, damageMax));
		}

		/// <summary>
		/// Get damage, Note: getting damage activates aggro
		/// </summary>
		/// <param name="dmg"></param>
		/// <param name="criticalHit"></param>
		public void GetPhysicalDamage(int dmg, bool criticalHit)
		{
			//We do not want to attack a dead enemy
			if (IsDead)
				return;
			Health -= dmg;
			AgroActivated = true;
			string dmgReceived = "-" + dmg.ToString(); 
			Vector2 size = font.MeasureString(dmgReceived);
			var text = new MovingText(game, font, dmgReceived)
			{
				Position = new Vector2(this.Position.X + this.Width / 2 - size.X / 2, this.Position.Y)
			};
			if(criticalHit)
			{
				text.Color = Color.Gold;
			}
			else
			{
				text.Color = Color.DarkRed;
			}
			DamageReceived.Add(text);
		}
	}
}
