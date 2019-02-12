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
	public class Wizard : Player
	{
		public Wizard(Dictionary<string, Animation> a, float scale) : base(a, scale)
		{
			animations = a;
			animationManager = new AnimationManager(a.First().Value);
			//Scale = scale;
		}
	}
}
