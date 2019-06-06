using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Items
{
    public class LifeFruit : Usable
    {
        public LifeFruit(Texture2D t, float scale) : base(t, scale)
        {
            IsStackable = true;
            Name = "Life fruit";
            Description = "Instantly restores all missing health";
        }
        public override bool Use(Player p)
        {
            if (p.HealthBar.Health.CurrentHealth == p.HealthBar.Health.MaxHealth)
                return false;
            else
            {
                p.HealthBar.Health.CurrentHealth = p.HealthBar.Health.MaxHealth;
                return true;
            }
        }
    }
}
