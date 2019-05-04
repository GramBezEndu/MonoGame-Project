using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.States;

namespace GameProject.Sprites
{
    public class Door : InteractableSprite
    {
        public Door(Game1 g, GameState gameState, Sprite mainSprite, Sprite interactButton, Player p) : base(g, gameState, mainSprite, interactButton, p)
        {
        }
    }
}
