using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameProject.Items;
using GameProject.Inventory;

namespace GameProject.Sprites
{
	public class EquipmentManager : Sprite
	{
		public float DamageReduction { get; set; }
		public float MagicDamangeReduction { get; set; }
		public float MovementSpeedBonus { get; set; }
		public float CriticalStrikeChance { get; set; }
		public int DamageMin { get; set; }
		public int DamageMax { get; set; }
		public StatisticsManager StatisticsManager {get; set;}
		public List<InventorySlot> EquipmentSlots { get; set; }
		public EquipmentManager(Texture2D t, Texture2D slotTexture, SpriteFont font, Vector2 position, float scale) : base(t, scale)
		{
			Position = position;
			StatisticsManager = new StatisticsManager(this, t, font, scale)
			{
				Position = new Vector2(this.Position.X, this.Position.Y+this.Height)
			};
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);
			StatisticsManager.Draw(gameTime, spriteBatch);
			//Iterate twice over slots (first time draw items, second time messages [this way messages are always on top of items])
			foreach (var es in EquipmentSlots)
				es.Draw(gameTime, spriteBatch);
			foreach (var es in EquipmentSlots)
				es.DrawMessages(gameTime, spriteBatch);
		}
		/// <summary>
		/// Recalculate bonuses in equipped items
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime)
		{
			float tempReduction = 0f;
			float tempMovementSpeed = 0f;
			float tempMagicReduction = 0f;
			int tempDamageMin = 0;
			int tempDamageMax = 0;
			float tempCriticalStrikeChance = 0f;
			foreach (var eq in EquipmentSlots)
			{
				eq.Update(gameTime);
				if(eq.Item != null && (eq.Item is Equippable))
				{
					eq.Item.Update(gameTime);
					tempReduction += (eq.Item as Equippable).DamageReduction;
					tempMovementSpeed += (eq.Item as Equippable).MovementSpeed;
					tempMagicReduction += (eq.Item as Equippable).MagicDamageReduction;
				}
				if(eq.Item is Weapon)
				{
					tempDamageMin = (eq.Item as Weapon).DamageMin;
					tempDamageMax = (eq.Item as Weapon).DamageMax;
				}
				//Critical strike chance - only sword
				if (eq.Item is Sword)
				{
					tempCriticalStrikeChance += (eq.Item as Sword).CriticalStrikeChance;
				}
			}
			DamageReduction = tempReduction;
			MovementSpeedBonus = tempMovementSpeed;
			MagicDamangeReduction = tempMagicReduction;
			DamageMin = tempDamageMin;
			DamageMax = tempDamageMax;
			CriticalStrikeChance = tempCriticalStrikeChance;
			StatisticsManager.Update(gameTime);
		}
	}
}
