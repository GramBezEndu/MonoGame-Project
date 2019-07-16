using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Sprites;
using Microsoft.Xna.Framework.Audio;

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
		SoundEffect notificationSound;
		Input Input;
		public Message(Game1 g, GraphicsDevice gd, Input i, SpriteFont font, string message, SoundEffect notification) : base(font, message)
		{
			Color = Color.White;
			displayTimer = new GameTimer(4f);
			notificationSound = notification;
			Input = i;
			//Background size = whole width, 10% height of the screen size for each line
			int[] size = new int[2];
			size[0] = g.Width;
			size[1] = (int)(g.Height * 0.1f);
			backgroundTexture = new Texture2D(gd, size[0], size[1]);
			Color[] data = new Color[size[0] * size[1]];
			//Paint every pixel
			for (int j = 0; j < data.Length; j++)
			{
				//Multiply by less than 1 to make it a bit transparent
				data[j] = Color.Black * 0.9f;
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
				if (displayTimer.Enabled && displayTimer.CurrentTime >= 0)
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
				//Enable timer on first update and play sound notification
				if (displayTimer.Enabled == false)
				{
					displayTimer.Start();
					notificationSound.Play();
				}
				//Check if user skipped message
				if (Input.CurrentState.IsKeyDown(Input.KeyBindings["SkipMessage"].GetValueOrDefault()) && Input.PreviousState.IsKeyUp(Input.KeyBindings["SkipMessage"].GetValueOrDefault()))
				{
					//Setting hidden to true has the same effect like deleting message
					Hidden = true;
				}
				displayTimer.Update(gameTime);
			}
		}
	}
}
