using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameProject.Sprites
{
	public class EquipmentManager : Sprite
	{
		public List<InventorySlot> EquipmentSlots { get; set; }
		public EquipmentManager(Texture2D t, Texture2D slotTexture, SpriteFont font, Vector2 position, float scale) : base(t, scale)
		{
			Position = position;
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);
			foreach (var es in EquipmentSlots)
				es.Draw(gameTime, spriteBatch);
		}
	}
}
