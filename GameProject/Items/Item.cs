using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject.Sprites;
using GameProject.Items;
using GameProject.Inventory;

namespace GameProject
{
	public abstract class Item : Sprite, ICloneable
	{
		private int _quantity;

		public Item(Texture2D t, float scale) : base(t, scale)
		{
			Name = "Not assigned name";
			_quantity = 1;
		}
		public bool IsStackable { get; set; }
		public int Quantity
		{
			get => _quantity;
			set
			{
				////If quantity is set to 0 or below, we should delete the item (in InventoryManger or somewhere where is belongs)
				//if (value <= 0)
				//    throw new Exception("Invalid quantity in Item (below 0)\n");
				if (value < 1)
					_quantity = value;
				else
				{
					if (IsStackable)
						_quantity = value;
					else
						throw new Exception("Invalud quantity (grater than 1): Item is not stackable\n");
				}
			}
		}
		public string Name { get; set; }
		public string Description { get; set; }

		public object Clone()
		{
			return MemberwiseClone();
		}
	}
}
