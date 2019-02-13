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
	public abstract class Player : Sprite
	{
		//public Player(Texture2D t, float scale) : base(t, scale)
		//{
		//}
		public Player(Dictionary<string, Animation> a, float scale) : base(a)
		{
			//Scale player distance per step
			moveDistance = 1f * scale;
			sprintDistance = 2f * scale;
		}
		protected Input input = new Input();
		protected float moveDistance;
		protected float sprintDistance;
		protected bool canSprint = true;
		protected bool canMove = true;
		protected int healthRegen = 1;
		protected GameTimer HealthRegenTimer = new GameTimer(2f, true);

		public InventoryManager InventoryManager;
		public HealthBar HealthBar { get; set; }
	}
}
