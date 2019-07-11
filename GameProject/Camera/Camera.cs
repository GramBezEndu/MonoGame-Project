﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject.Sprites;

namespace GameProject
{
	public class Camera
	{
		public Matrix Transform { get; private set; }
		/// <summary>
		/// Sets the camera on Width: Middle (1/2 Width) & Height: 2/3 Height
		/// </summary>
		/// <param name="game"></param>
		/// <param name="target"></param>
		public void Follow(Game1 game, Sprite target)
		{
			var position = Matrix.CreateTranslation(-target.Position.X - target.Rectangle.Width / 2, -target.Position.Y - target.Rectangle.Height / 2, 0);
			var offset = Matrix.CreateTranslation(game.Width/2, game.Height * (2 / 3f), 0);
			//var offset = Matrix.CreateTranslation(game.Width / 2, game.Height / 2, 0);
			Transform = position * offset;
		}
	}
}
