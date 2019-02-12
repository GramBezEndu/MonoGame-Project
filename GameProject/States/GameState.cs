using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameProject.Sprites;

namespace GameProject.States
{
	public abstract class GameState : State
	{
		public GameState(Game1 g, GraphicsDevice gd, ContentManager c) : base(g, gd, c)
		{
			camera = new Camera();
		}
		protected Camera camera;
		protected EnemyManager enemyManager;
		protected Player player;
		protected List<Component> movingComponents;
	}
}
