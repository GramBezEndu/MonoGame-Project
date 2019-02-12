using System;
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
	public abstract class Item : Sprite
	{
		public Item(Texture2D t, float scale) : base(t, scale) { }
		public bool IsStackable { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
	}
}
