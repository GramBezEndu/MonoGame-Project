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
	public class Wizard : Player
	{
		public Wizard(GameState currentGameState, Dictionary<string, Animation> a, Input i, Vector2 scale) : base(currentGameState, a, i, scale)
		{
			animations = a;
			animationManager = new AnimationManager(this, a.First().Value);
			//Scale = scale;
		}
		protected override void PlayAnimations()
		{
			throw new NotImplementedException();
		}
	}
}
