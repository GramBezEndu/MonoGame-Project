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

		public Tutorial(GameState gs)
		{
			GameState = gs;
		}

		/// <summary>
		/// Call this method in constructor to initialize tutorial messages if you don't want to use constructor with List of strings
		/// </summary>
		/// <param name="messages"></param>
		public void InitializeMessages(List<string> messages)
		{
			MessagesStrings = messages;
			MessageWasShown = new List<bool>(new bool[messages.Count]);
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			//throw new NotImplementedException();
		}

		public override void Update(GameTime gameTime)
		{
			//Set the flag to activated
			if (ShouldActivate())
				Activated = true;
			if (Activated && !Completed)
			{
				if(Settings.EnableTutorials)
				{
					GameState.IsDisplayingTutorial = true;
					//If message was not shown yet
					if (MessageWasShown[actualMessageIndex] == false)
					{
						var actualMsg = GameState.CreateMessage(MessagesStrings[actualMessageIndex], true);
						actualMsg.Dispose += OnDispose;
						//Change the text from "Skip " to "Next " if there are more incoming messages
						if (actualMessageIndex + 1 < MessagesStrings.Count)
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
		}
		/// <summary>
		/// If message was disposed (which means tutorial was interupted for example by entering another stage [includes going into InGameSettings)
		/// then reset the tutorial
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnDispose(object sender, EventArgs e)
		{
			Reset();
		}

		/// <summary>
		/// Resets the tutorial so it can be activated and shown again
		/// </summary>
		public void Reset()
		{
			GameState.IsDisplayingTutorial = false;
			actualMessageIndex = 0;
			MessageWasShown = new List<bool>(new bool[MessagesStrings.Count]);
			Messages = new List<Message>();
			Activated = false;
			Completed = false;
		}

		public virtual bool ShouldActivate()
		{
			//Player is doing different tutorial right now, we do not allow to activate another tutorial
			if (GameState.IsDisplayingTutorial)
				return false;
			else
				return true;
		}
	}
}
