using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.Animations;
using Microsoft.Xna.Framework;

namespace GameProject.Sprites
{
	public abstract class Character : Sprite
	{
		public Character(Dictionary<string, Animation> a) : base(a){ }
		public abstract void Interact();
		public override void Update(GameTime gameTime)
		{
			animationManager.Play(animations["Idle"]);
			animationManager.Update(gameTime);
		}
	}
}
