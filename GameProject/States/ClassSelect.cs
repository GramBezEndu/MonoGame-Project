using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameProject.Controls;
using GameProject.Sprites;

namespace GameProject.States
{
	public class ClassSelect : State
	{
		public ClassSelect(Game1 g, GraphicsDevice gd, ContentManager c) : base(g, gd, c)
		{
			var background = content.Load<Texture2D>("Background");
			var classCard = content.Load<Texture2D>("ClassCard");
			var buttonFont = content.Load<SpriteFont>("Font");
			components = new List<Component>
			{
				new Sprite(background, g.Scale)
				{
					Position = new Vector2(0,0)
				},
				new Button(classCard, buttonFont, g.Scale)
				{
					Position = new Vector2(0.01f*g.Width, 0.2f*g.Height),
					Text = "Warrior",
					Click = WarriorClick
				},
				new Button(classCard, buttonFont, g.Scale)
				{
					Position = new Vector2(0.37f*g.Width, 0.2f*g.Height),
					Text = "Archer",
					Click = ArcherClick
				},
				new Button(classCard, buttonFont, g.Scale)
				{
					Position = new Vector2(0.73f*g.Width, 0.2f*g.Height),
					Text = "Wizard (coming soon)"
				}
			};
		}

		private void ArcherClick(object sender, EventArgs e)
		{
			game.ChangeState(new Village(game, graphicsDevice, content, PlayerClasses.Archer));
		}

		private void WarriorClick(object sender, EventArgs e)
		{
			game.ChangeState(new Village(game, graphicsDevice, content, PlayerClasses.Warrior));
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, float scale)
		{
			spriteBatch.Begin();
			foreach (var c in components)
				c.Draw(gameTime, spriteBatch);
			spriteBatch.End();
		}

		public override void PostUpdate()
		{
			//throw new NotImplementedException();
		}

		public override void Update(GameTime gameTime)
		{
			foreach (var c in components)
				c.Update(gameTime);
		}
	}
}
