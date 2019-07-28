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
	public abstract class Usable : Item
	{
		public Usable(Texture2D t, Vector2 scale) : base(t, scale) { }
		public abstract bool Use(Player p);
	}
}
