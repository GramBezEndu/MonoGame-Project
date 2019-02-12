using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject.Sprites
{
	public abstract class Player : Sprite
	{
		public Player(Texture2D t, float scale) : base(t, scale) { }
		protected Input input = new Input();
		protected float moveDistance = 1f;
		protected float sprintDistance = 2f;
		protected bool canSprint = true;
		protected bool canMove = true;
		protected int maxHealth = 20;
		protected int currentHealth = 5;
		protected int healthRegen = 1;
		protected GameTimer HealthRegenTimer = new GameTimer(2f);

		//protected InventoryManager InventoryManager = new InventoryManager(24);

		public int CurrentHealth { get => currentHealth; }
	}
}
