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
			var warriorButton = new Button(classCard, buttonFont, g.Scale)
			{
				Position = new Vector2(0.01f * g.Width, 0.2f * g.Height),
				Text = "Warrior",
				Click = WarriorClick
			};
			var warriorFigure = content.Load<Texture2D>("Warrior");
			var warriorFigureSprite = new Sprite(warriorFigure, g.Scale);
			warriorFigureSprite.Position = new Vector2(0.01f * g.Width + warriorButton.Width / 2 - warriorFigureSprite.Width / 2, 0.2f * g.Height);
			var archerFigure = content.Load<Texture2D>("Archer");
			var archerButton = new Button(classCard, buttonFont, g.Scale)
			{
				Position = new Vector2(0.37f * g.Width, 0.2f * g.Height),
				Text = "Archer",
				Click = ArcherClick
			};
			var archerFigureSprite = new Sprite(archerFigure, g.Scale);
			archerFigureSprite.Position = new Vector2(0.37f * g.Width + archerButton.Width / 2 - archerFigureSprite.Width / 2, 0.2f * g.Height);
			var wizardButton = new Button(classCard, buttonFont, g.Scale)
			{
				Position = new Vector2(0.73f * g.Width, 0.2f * g.Height),
				Text = "Wizard (coming soon)"
			};
			staticComponents = new List<Component>
			{
				new Sprite(background, g.Scale)
				{
					Position = new Vector2(0,0)
				},
				warriorButton,
				warriorFigureSprite,
				archerButton,
				archerFigureSprite,
				wizardButton
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
			foreach (var c in staticComponents)
				c.Draw(gameTime, spriteBatch);
			spriteBatch.End();
		}

		public override void PostUpdate()
		{
			//throw new NotImplementedException();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			foreach (var c in staticComponents)
				c.Update(gameTime);
		}
	}
}
