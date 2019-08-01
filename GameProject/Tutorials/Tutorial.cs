using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Controls;
using GameProject.States;

namespace GameProject.Tutorials
{
	public abstract class Tutorial : Component
	{
		public GameState GameState;
		protected List<string> MessagesStrings;
		protected List<Message> Messages = new List<Message>();
		protected List<bool> MessageWasShown;
		protected int actualMessageIndex = 0;
		/// <summary>
		/// Determines if tutorial was completed
		/// </summary>
		public bool Completed;

		public bool Activated;

		public Tutorial(GameState gs, List<string> messages)
		{
			GameState = gs;
			MessagesStrings = messages;
			MessageWasShown = new List<bool>(new bool[messages.Count]);
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			//throw new NotImplementedException();
		}

		public override void Update(GameTime gameTime)
		{
			CheckForActivation();
			if(Activated && !Completed)
			{
				GameState.IsDisplayingTutorial = true;
				//If message was not shown yet
				if (MessageWasShown[actualMessageIndex] == false)
				{
					var actualMsg = GameState.CreateMessage(MessagesStrings[actualMessageIndex], true);
					//Change the text from "Skip " to "Next " if there are more incoming messages
					if(actualMessageIndex + 1 < MessagesStrings.Count)
						actualMsg.SkipTextString = "Next ";
					Messages.Add(actualMsg);
					MessageWasShown[actualMessageIndex] = true;
				}
				//Switch to next message (when message timer ended or message was skipped)
				if (Messages[actualMessageIndex].Hidden == true)
				{
					actualMessageIndex += 1;
				}
				//End of messages -> every message ENDED
				if (actualMessageIndex >= MessagesStrings.Count)
				{
					Completed = true;
					GameState.IsDisplayingTutorial = false;
				}
			}
		}

		public virtual void CheckForActivation()
		{
			//Player is doing different tutorial right now, we do not allow to activate another tutorial
			if (GameState.IsDisplayingTutorial)
				return;
		}
	}
}
