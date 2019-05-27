using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject.Animations;
using GameProject.States;

namespace GameProject.Sprites
{
	public class Archer : StaminaUser
	{
		public Archer(GameState currentGameState, Dictionary<string, Animation> a, Input i,  float scale) : base(currentGameState, a, i, scale)
		{
			animations = a;
			animationManager = new AnimationManager(a.First().Value);
		}
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		protected override void PlayAnimations()
		{
			//throw new NotImplementedException();
		}
	}
}
