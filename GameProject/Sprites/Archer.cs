using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject.Animations;

namespace GameProject.Sprites
{
	public class Archer : StaminaUser
	{
		public Archer(Dictionary<string, Animation> a, float scale) : base(a, scale)
		{
			animations = a;
			animationManager = new AnimationManager(a.First().Value);
		}
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}
	}
}
