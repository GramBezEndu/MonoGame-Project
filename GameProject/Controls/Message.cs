using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Sprites;

namespace GameProject.Controls
{
	/// <summary>
	/// Displays text with background for a limited time
	/// </summary>
	public class Message : Text
	{
		/// <summary>
		/// Determines how long message will be displayed
		/// </summary>
		GameTimer displayTimer;
		Texture2D backgroundTexture;
		Sprite backgroundSprite;
		public Message(Game1 g, GraphicsDevice gd, SpriteFont font, string message) : base(font, message)
		{
			Color = Color.White;
			displayTimer = new GameTimer(5f);
			//Background size = whole width, 20% height of the screen size
			int[] size = new int[2];
			size[0] = g.Width;
			size[1] = (int)(g.Height * 0.1f);
			backgroundTexture = new Texture2D(gd, size[0], size[1]);
			Color[] data = new Color[size[0] * size[1]];
			//Paint every pixel
			for (int i = 0; i < data.Length; i++)
			{
				//Multiply by less than 1 to make it a bit transparent
				data[i] = Color.Black * 0.9f;
			}
			backgroundTexture.SetData(data);
			backgroundSprite = new Sprite(backgroundTexture, 1f);
			//Set the position of background sprite
			backgroundSprite.Position = new Vector2(0, 0.8f * g.Height);
			Vector2 textSize = font.MeasureString(message);
			//Set the position of text
			this.Position = new Vector2(backgroundSprite.Position.X + backgroundSprite.Width / 2 - textSize.X / 2,
				backgroundSprite.Position.Y + backgroundSprite.Height/2 - textSize.Y / 2);
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if(!Hidden)
			{
				if (displayTimer.CurrentTime >= 0)
				{
					backgroundSprite.Draw(gameTime, spriteBatch);
					base.Draw(gameTime, spriteBatch);
				}
			}
		}

		public override void Update(GameTime gameTime)
		{
			if(!Hidden)
			{
				//Enable timer on first update
				if (displayTimer.Enabled == false)
					displayTimer.Start();
				displayTimer.Update(gameTime);
			}
		}
	}
}
