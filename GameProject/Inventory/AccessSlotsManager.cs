﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Sprites;
using GameProject.Items;
using GameProject.Inventory;

namespace GameProject.Inventory
{
	public class AccessSlotsManager : Component
	{
		public List<FastAccessSlot> fastAccessSlots { get; set; }
		/// <summary>
		/// Input reference for checking if keyboard key was pressed
		/// </summary>
		private Input input;
		private Player player;
		public AccessSlotsManager(GraphicsDevice gd, Player p, Texture2D slotTexture, SpriteFont f, float scale, Vector2 position, Input i)
		{
			input = i;
			player = p;
			//Make fast access slots
			var slotOne = new FastAccessSlot(gd, p, slotTexture, f, scale)
			{
				Position = position
			};
			var slotTwo = new FastAccessSlot(gd, p, slotTexture, f, scale)
			{
				Position = new Vector2(position.X + slotOne.Width, position.Y)
			};
			var slotThree = new FastAccessSlot(gd, p, slotTexture, f, scale)
			{
				Position = new Vector2(slotTwo.Position.X + slotTwo.Width, slotTwo.Position.Y)
			};
			fastAccessSlots = new List<FastAccessSlot>()
			{
				slotOne,
				slotTwo,
				slotThree
			};
		}

		public override void Update(GameTime gameTime)
		{
			foreach (var s in fastAccessSlots)
				s.Update(gameTime);
			//Check if player pressed key to use item
			for(int i=0;i<fastAccessSlots.Count();i++)
			{
				string actualKeybind = "FastSlot" + (i + 1).ToString();
				if (input.CurrentState.IsKeyDown(input.KeyBindings[actualKeybind]) && input.PreviousState.IsKeyUp(input.KeyBindings[actualKeybind]))
				{
					if (fastAccessSlots[i].Item == null)
						return;
					var item = fastAccessSlots[i].Item as Usable;
					bool result = item.Use(player);
					if (result == true)
					{
						fastAccessSlots[i].Item.Quantity -= 1;
						var debug = gameTime;
					}
				}
			}
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			foreach (var s in fastAccessSlots)
				s.Draw(gameTime, spriteBatch);
		}
	}
}