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
		public Dictionary<string, float> Attributes = new Dictionary<string, float>();
		public StatisticsManager StatisticsManager {get; set;}
		public List<InventorySlot> EquipmentSlots { get; set; }
		public EquipmentManager(Texture2D t, Texture2D slotTexture, SpriteFont font, Vector2 position, Vector2 scale) : base(t, scale)
		{
			Position = position;
			StatisticsManager = new StatisticsManager(this, t, font, scale)
			{
				Position = new Vector2(this.Position.X, this.Position.Y+this.Height)
			};

			//Attributes copy from equippable
			Attributes.Add("DamageReduction", 0f);
			//Movement speed bonus
			Attributes.Add("MovementSpeed", 0f);
			Attributes.Add("CriticalStrikeChance", 0f);
			//Attack value from <min;max> range
			Attributes.Add("DamageMin", 0f);
			Attributes.Add("DamageMax", 0f);
			//Damage increase in %
			Attributes.Add("BonusDamage", 0f);

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
			var TempDictionary = Attributes.ToDictionary(entry => entry.Key, entry => 0f);
			foreach (var eq in EquipmentSlots)
			{
				eq.Update(gameTime);
				if(eq.Item != null && (eq.Item is Equippable))
				{
					////No dictionary yet - copy it from any equippable item
					//if(Attributes == null)
					//{
					//	Attributes = (eq.Item as Equippable).Attributes.ToDictionary(entry => entry.Key,
					//						   entry => entry.Value);
					//}
					eq.Item.Update(gameTime);
					TempDictionary["DamageReduction"] += (eq.Item as Equippable).Attributes["DamageReduction"];
					TempDictionary["MovementSpeed"] += (eq.Item as Equippable).Attributes["MovementSpeed"];
					TempDictionary["DamageMin"] += (eq.Item as Equippable).Attributes["DamageMin"];
					TempDictionary["DamageMax"] += (eq.Item as Equippable).Attributes["DamageMax"];
					TempDictionary["CriticalStrikeChance"] += (eq.Item as Equippable).Attributes["CriticalStrikeChance"];
					TempDictionary["BonusDamage"] += (eq.Item as Equippable).Attributes["BonusDamage"];
				}
			}
			foreach(var v in TempDictionary)
			{
				Attributes[v.Key.ToString()] = TempDictionary[v.Key.ToString()];
			}
			StatisticsManager.Update(gameTime);
		}
	}
}
