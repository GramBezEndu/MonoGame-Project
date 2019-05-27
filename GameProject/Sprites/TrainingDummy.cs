using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Animations;
using GameProject.States;

namespace GameProject.Sprites
{
	public class TrainingDummy : Enemy
	{
		public TrainingDummy(Game1 g, GameState gs, SpriteFont f, Dictionary<string, Animation> a, Player p) : base(g, gs, f, a, p){ }
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			Health = Int32.MaxValue;
		}
		protected override void PlayAnimations()
		{
			//We do not play any animations in Dummy (yet)
		}
	}
}
