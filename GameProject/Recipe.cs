using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.Controls;
using GameProject.Sprites;
using GameProject.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject
{
	public class Recipe : Component
	{
		public Vector2 Position;
		protected List<Component> components = new List<Component>();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="g"></param>
		/// <param name="gs"></param>
		/// <param name="pos"></param>
		/// <param name="textures">list of textures that will be put into slots</param>
		/// <param name="cost"></param>
		public Recipe(Game1 g, GameState gs, Vector2 pos, Texture2D[] textures, int cost)
		{
			Position = pos;

			Sprite[] slots =
			{
				new Sprite(gs.Textures["InventorySlot"], g.Scale),
				new Sprite(gs.Textures["InventorySlot"], g.Scale),
				new Sprite(gs.Textures["InventorySlot"], g.Scale),
				new Sprite(gs.Textures["InventorySlot"], g.Scale),
			};

			var arrow = new Sprite(gs.Textures["Arrow"], g.Scale);

			slots[0].Position = Position;
			slots[1].Position = new Vector2(slots[0].Rectangle.Right, slots[0].Position.Y);
			arrow.Position = new Vector2(slots[1].Rectangle.Right, slots[1].Position.Y);
			slots[2].Position = new Vector2(arrow.Rectangle.Right, arrow.Position.Y);
			slots[3].Position = new Vector2(slots[2].Rectangle.Right, slots[2].Position.Y);

			Sprite[] items = new Sprite[textures.Count()];

			for(int i=0;i<textures.Count();i++)
			{
				items[i] = new Sprite(textures[i], g.Scale);
				items[i].Position = slots[i].Position;
			}

			components.AddRange(slots);
			components.AddRange(items);
			components.Add(arrow);

			if (cost != 0)
			{
				components.Add(new Text(gs.Font, cost.ToString())
				{
					Position = new Vector2(slots[0].Position.X, slots[0].Rectangle.Bottom)
				});
			}
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if(!Hidden)
			{
				foreach (var c in components)
					c.Draw(gameTime, spriteBatch);
			}
		}

		public override void Update(GameTime gameTime)
		{
			if (!Hidden)
			{
				foreach (var c in components)
					c.Update(gameTime);
			}
		}
	}
}
