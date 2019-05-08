using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Controls;
using GameProject.States;

namespace GameProject.Sprites
{
	public abstract class Character : InteractableSprite
    {
        //Components that are displayed when interaction window is opened
        protected List<Component> UiElements = new List<Component>();
        public Character(Game1 g, GameState gs, Sprite mainSprite, Sprite interactButton, Player p) : base(g, gs, mainSprite, interactButton, p)
        {
            //Hide window on default
            //Note: if Hidden equals true only ui componenets are hidden
            //It is not possible to hide character
            Hidden = true;
            Sprite background = new Sprite(gs.Textures["Inventory"], g.Scale);
            Vector2 pos = new Vector2(g.Width / 2 - background.Width / 2, g.Height / 2 - background.Width / 2);
            background.Position = pos;
            background.Hidden = true;

            UiElements.Add(background);
            UiElements.Add(new Button(gs.Textures["Button"], gs.Font, g.Scale)
            {
                Position = pos,
                Text = "Exit",
                Hidden = true,
                Click = OnClose
            }
);
            gs.AddUiElements(UiElements);
        }
        /// <summary>
        ///  Call this method to correctly hide window and all its components
        /// </summary>
        protected void Hide()
        {
            Hidden = true;
            foreach (var ui in UiElements)
                ui.Hidden = Hidden;
        }
        public override void Update(GameTime gameTime)
        {
            //We do not display (and do not allow clicks) on Interact button if window is opened
            if (Hidden)
                base.Update(gameTime);
            //Close automatically window if player is too far away (too far away -> not touching main sprite now)
            if (!Hidden && !player.IsTouching(MainSprite))
                Hide();
        }
        /// <summary>
        /// When closing window with ui elements
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnClose(object sender, EventArgs e)
        {
            Hidden = true;
            foreach (var ui in UiElements)
                ui.Hidden = Hidden;
        }
        protected override void OnActivate()
        {
            base.OnActivate();
            //Change hidden flag (also for each element in uiElements [window])
            //Those are the same objects that are in uiElements in current gameState
            Hidden = false;
            foreach (var ui in UiElements)
                ui.Hidden = Hidden;
        }
    }
}
