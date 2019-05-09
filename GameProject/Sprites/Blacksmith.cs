using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.Animations;
using Microsoft.Xna.Framework;
using GameProject.States;
using GameProject.Controls;

namespace GameProject.Sprites
{
	public class Blacksmith : Character
	{
        public Blacksmith(Game1 g, GameState gs, Sprite mainSprite, Sprite interactButton, Player p) : base(g, gs, mainSprite, interactButton, p)
        {
            //Add special elements to window
            var background = UiElements.First();
            Vector2 pos = new Vector2(0,0);
            if(background is Sprite)
            {
                pos = (background as Sprite).Position;
            }
            var addRemoveScroll = new Button(gs.Textures["Button"], gs.Font, g.Scale)
            {
                Position = pos,
                Text = "Add/Remove Scroll",
                Hidden = true
            };
            UiElements.Add(addRemoveScroll);
            pos += new Vector2(0, addRemoveScroll.Height);
            var scrollUpgrade = new Button(gs.Textures["Button"], gs.Font, g.Scale)
            {
                Position = pos,
                //It could be "Joining scrolls" or sth like that
                Text = "Scroll upgrade",
                Hidden = true
            };
            UiElements.Add(scrollUpgrade);
            pos += new Vector2(0, scrollUpgrade.Height);
            var shieldRepair = new Button(gs.Textures["Button"], gs.Font, g.Scale)
            {
                Position = pos,
                Text = "Shield repair",
                Hidden = true
            };
            UiElements.Add(shieldRepair);
            //Apply changes to state
            gs.AddUiElements(UiElements);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            MainSprite.animationManager.Update(gameTime);
            PlayAnimations();
        }

        private void PlayAnimations()
        {
            MainSprite.animationManager.Play(MainSprite.animations["Idle"]);
        }
    }
}
