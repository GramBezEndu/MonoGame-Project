using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Sprites
{
	public abstract class Projectile : Sprite
	{
		protected bool RotatedLeft;
		/// <summary>
		/// How much projectile moves in one frame
		/// </summary>
		protected float distancePerFrame;
		protected int DamageMin;
		protected int DamageMax;
		protected Game1 Game;
		public Projectile(Game1 game, Texture2D t, float scale, bool rotatedLeft) : base(t, scale)
		{
			RotatedLeft = rotatedLeft;
			distancePerFrame = 1f * game.Scale;
			DamageMin = 1;
			DamageMax = 1;
			Game = game;
		}

		public override void Update(GameTime gameTime)
		{
			if (!Hidden)
			{
				if (RotatedLeft)
				{
					Velocity = new Vector2(-distancePerFrame, 0);
				}
				else
				{
					Velocity = new Vector2(-distancePerFrame, 0);
				}
				Position += Velocity;
			}
		}
	}
}
