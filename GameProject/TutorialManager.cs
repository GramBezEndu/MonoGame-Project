using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Tutorials;
using GameProject.States;
using GameProject.Sprites;

namespace GameProject
{
	public class TutorialManager : Component
	{
		GameState GameState;
		Player Player;
		List<Tutorial> Tutorials;

		public TutorialManager(Player p)
		{
			Player = p;
			GameState = Player.gameState;
			Tutorials = new List<Tutorial>()
			{
				new ShopkeeperTutorial(GameState),
				new BlacksmithTutorial(GameState),
				new StatueOfGodsTutorial(GameState, p),
			};
			if (p is Warrior)
				Tutorials.Add(new WarriorAttackingTutorial(GameState));
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			//throw new NotImplementedException();
		}

		public override void Update(GameTime gameTime)
		{
			//Update current game state in tutorials
			GameState = Player.gameState;
			foreach (var t in Tutorials)
			{
				t.GameState = GameState;
				t.Update(gameTime);
			}
		}
	}
}
