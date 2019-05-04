using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.Animations;
using GameProject.States;

namespace GameProject.Sprites
{
	public abstract class ManaUser : Player
	{
		public ManaUser(GameState currentGameState, Dictionary<string, Animation> a, Input i, float scale) : base(currentGameState, a, i, scale)
		{
		}
	}
}
