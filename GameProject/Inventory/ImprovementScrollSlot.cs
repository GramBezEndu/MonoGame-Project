using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.Sprites;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Items;
using Microsoft.Xna.Framework;

namespace GameProject.Inventory
{
	public class ImprovementScrollSlot : Slot
	{
		public new ImprovementScroll Item { get; set; }
		public ImprovementScrollSlot(GraphicsDevice gd, Player p, Texture2D t, SpriteFont f, float scale) : base(gd, p, t, f, scale)
		{
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);
			if(Item != null)
			{
				Item.Position = this.Position;
				Item.Draw(gameTime, spriteBatch);
			}
		}
	}
}
