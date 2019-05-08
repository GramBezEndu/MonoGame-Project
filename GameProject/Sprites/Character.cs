using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.Animations;
using Microsoft.Xna.Framework;

namespace GameProject.Sprites
{
    //Class disabled - character is just an Interactable Sprite now
	public abstract class Character : Sprite
	{
		public Character(Dictionary<string, Animation> a) : base(a)
        {
            throw new Exception("Character class is not used now. Use interactable sprite\n");
        }
		//public abstract void Interact();
		//public override void Update(GameTime gameTime)
		//{
		//	animationManager.Play(animations["Idle"]);
		//	animationManager.Update(gameTime);
		//}
	}
}
