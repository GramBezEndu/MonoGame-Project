using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;


namespace GameProject
{
    public class Input
	{
		public KeyboardState CurrentState;
		public KeyboardState PreviousState;
		public MouseState CurrentMouseState;
		public MouseState PreviousMouseState;
		public Dictionary<string, Keys> KeyBindings { get; private set; }
		/// <summary>
		/// Sets default key binds
		/// </summary>
		public Input()
		{
			KeyBindings = new Dictionary<string, Keys>
			{
				//Used for warrior shield blocking
				{"MoveUp", Keys.W },
				{"MoveRight", Keys.D },
				{"MoveLeft",Keys.A },
				{"Sprint",Keys.LeftShift },
				{"DodgeBlock",Keys.Space },
				{"PickUp",Keys.Z },
				{"ShowInventory",Keys.Tab },
				{"Pause",Keys.Escape },
				{"Interact",Keys.E }
			};
		}
		public void Update(GameTime gameTime)
		{
			PreviousState = CurrentState;
			CurrentState = Keyboard.GetState();
			PreviousMouseState = CurrentMouseState;
			CurrentMouseState = Mouse.GetState();
		}
		public void RestoreToDefault()
		{
			throw new NotImplementedException();
		}
		public void Save()
		{
			throw new NotImplementedException();
		}
		public void Load()
		{
			throw new NotImplementedException();
		}
	}
}
