using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace GameProject
{
	public class Input
	{
		public KeyboardState CurrentState;
		public KeyboardState PreviousState;
		public Keys MoveRight { get; set; }
		public Keys MoveLeft { get; set; }
		public Keys Sprint { get; set; }
		public Keys DodgeBlock { get; set; }
		public Keys PickUp { get; set; }
		public Keys ShowInventory { get; set; }
		/// <summary>
		/// Sets default key binds
		/// </summary>
		public Input()
		{
			MoveRight = Keys.D;
			MoveLeft = Keys.A;
			Sprint = Keys.LeftShift;
			DodgeBlock = Keys.Space;
			PickUp = Keys.Z;
			ShowInventory = Keys.Tab;
		}
		public void Update(GameTime gameTime)
		{
			PreviousState = CurrentState;
			CurrentState = Keyboard.GetState();
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
