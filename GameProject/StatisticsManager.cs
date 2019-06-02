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
			//Will contain window size
			Vector2 windowSize = Vector2.Zero;
			foreach(var a in equipmentManager.Attributes)
			{
				//We should handle damage min/max differently
				string temp = a.Key + ": " + a.Value.ToString();
				Vector2 tempSize = font.MeasureString(temp);
				windowSize.X = Math.Max(windowSize.X, tempSize.X);
				windowSize.Y += tempSize.Y;
			}
			string damageStr = "Damage: " + equipmentManager.Attributes["DamageMin"].ToString() + '-' + equipmentManager.Attributes["DamageMax"].ToString();
			string criticalStrikeChanceStr = "Critical Strike Chance: " + equipmentManager.Attributes["CriticalStrikeChance"] * 100 + "%";
			string damageReductionStr = "Damage reduction: " + equipmentManager.Attributes["DamageReduction"] * 100 + "%";
			string magicDamangeReductionStr = "Magic damage reduction: " + equipmentManager.Attributes["MagicDamageReduction"] * 100 + "%";
			string movementSpeedBonusStr = "Movement speed bonus: " + equipmentManager.Attributes["MovementSpeed"] * 100 + "%";
			string bonusDamage = "Bonus damage: " + equipmentManager.Attributes["BonusDamage"] * 100 + "%";

			spriteBatch.DrawString(font, damageStr, this.Position, Color.Black);
			Vector2 size = font.MeasureString(damageStr);

			spriteBatch.DrawString(font, criticalStrikeChanceStr, new Vector2(this.Position.X, this.Position.Y + size.Y), Color.Black);
			size += font.MeasureString(criticalStrikeChanceStr);

			spriteBatch.DrawString(font, damageReductionStr, new Vector2(this.Position.X, this.Position.Y + size.Y), Color.Black);
			size += font.MeasureString(damageReductionStr);

			spriteBatch.DrawString(font, magicDamangeReductionStr, new Vector2(this.Position.X, this.Position.Y + size.Y), Color.Black);
			size += font.MeasureString(magicDamangeReductionStr);

			spriteBatch.DrawString(font, movementSpeedBonusStr, new Vector2(this.Position.X, this.Position.Y + size.Y), Color.Black);
			size += font.MeasureString(movementSpeedBonusStr);

			spriteBatch.DrawString(font, bonusDamage, new Vector2(this.Position.X, this.Position.Y + size.Y), Color.Black);
		}
	}
}
