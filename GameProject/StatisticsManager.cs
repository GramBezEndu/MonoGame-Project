using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject
{
	/// <summary>
	/// Prints current statistic to window
	/// </summary>
	public class StatisticsManager : Sprite
	{
		private SpriteFont font;
		private EquipmentManager equipmentManager;
		public StatisticsManager(EquipmentManager eq, Texture2D t, SpriteFont f, float scale) : base(t, scale)
		{
			equipmentManager = eq;
			font = f;
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);
			string damageStr = "Damage: " + equipmentManager.DamageMin.ToString() + '-' + equipmentManager.DamageMax.ToString();
			string criticalStrikeChanceStr = "Critical Strike Chance: " + equipmentManager.CriticalStrikeChance.ToString();
			string damageReductionStr = "Damage reduction: " + equipmentManager.DamageReduction.ToString();
			string magicDamangeReductionStr = "Magic damage reduction: " + equipmentManager.MagicDamangeReduction.ToString();
			string movementSpeedBonusStr = "Movement speed bonus: " + equipmentManager.MovementSpeedBonus.ToString();

			spriteBatch.DrawString(font, damageStr, this.Position, Color.Black);
			Vector2 size = font.MeasureString(damageStr);

			spriteBatch.DrawString(font, criticalStrikeChanceStr, new Vector2(this.Position.X, this.Position.Y + size.Y), Color.Black);
			size += font.MeasureString(criticalStrikeChanceStr);

			spriteBatch.DrawString(font, damageReductionStr, new Vector2(this.Position.X, this.Position.Y + size.Y), Color.Black);
			size += font.MeasureString(damageReductionStr);

			spriteBatch.DrawString(font, magicDamangeReductionStr, new Vector2(this.Position.X, this.Position.Y + size.Y), Color.Black);
			size += font.MeasureString(magicDamangeReductionStr);

			spriteBatch.DrawString(font, movementSpeedBonusStr, new Vector2(this.Position.X, this.Position.Y + size.Y), Color.Black);
		}
	}
}
