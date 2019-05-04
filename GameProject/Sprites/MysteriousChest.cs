﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameProject.Items;
using GameProject.States;

namespace GameProject.Sprites
{
    public class MysteriousChest : InteractableSprite
    {
        public bool Opened { get; private set; }
        public MysteriousChest(Game1 g, GameState gameState, Sprite mainSprite, Sprite interactButton, Player p) : base(g, gameState, mainSprite, interactButton, p)
        {
        }
        protected override void OnActivate()
        {
            if(!Opened)
            {
                int value = game.RandomPercent();
                //TODO: spawn scrolls
                //Roll an item
                //Actual drop chance:
                //50% improvement scroll
                //35% defence ring
                //15% legendary improvement scroll
                if(value <= 50)
                {
                    this.GameState.SpawnItem(
                    new ImprovementScroll(game, GameState.Textures["ImprovementScroll"], MainSprite.Scale)
                    {
                        Position = MainSprite.Position
                    });
                }
                else if(value <= 85)
                {
                    this.GameState.SpawnItem(
                    new DefenceRing(GameState.Textures["DefenceRing"], MainSprite.Scale)
                    {
                        Position = MainSprite.Position
                    });
                }
                else
                {
                    this.GameState.SpawnItem(
                    new LegendaryImprovementScroll(game, GameState.Textures["LegendaryImprovementScroll"], MainSprite.Scale)
                    {
                        Position = MainSprite.Position
                    });
                }
                Opened = true;
            }
        }
    }
}
