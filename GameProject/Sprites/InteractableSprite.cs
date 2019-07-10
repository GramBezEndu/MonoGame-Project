using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameProject.Sprites;
using GameProject.Animations;
using GameProject.Items;
using GameProject.Inventory;
using GameProject.Controls;
using GameProject.States;

namespace GameProject.Sprites
{
    /// <summary>
    /// Represents openable/interactable sprite
    /// </summary>
    public class InteractableSprite : Component
    {
        /// <summary>
        /// Game reference for changing state on door interaction etc.
        /// </summary>
        protected Game1 game { get; set; }
        /// <summary>
        /// Player reference for collision checking
        /// </summary>
        protected Player player { get; set; }
        /// <summary>
        /// Input reference for checking "Interact" button press
        /// </summary>
        protected Input input { get; set; }
        /// <summary>
        /// GameState reference for spawning items
        /// </summary>
        protected GameState GameState { get; private set; }
        public Sprite MainSprite { get; set; }
        public Sprite InteractButton { get; set; }
        public bool Activated { get; set; }
        public InteractableSprite(Game1 g, GameState gameState, Sprite mainSprite, Sprite interactButton, Player p)
        {
            game = g;
            input = gameState.Input;
            GameState = gameState;
            MainSprite = mainSprite;
            InteractButton = interactButton;
            player = p;
            //Set interaction button position based on sprite position
            //Width: middle of the button is in the middle of the sprite
            //Height: right above the sprite 
            var middlePositionEntrance = new Vector2(
                MainSprite.Position.X + MainSprite.Width / 2,
                MainSprite.Position.Y - interactButton.Height);
            //Hald button texture width (height = 0, we don't use it)
            var halfButtonWidth = new Vector2(interactButton.Width / 2, 0);
            //Calculate start vector
            var startVector = middlePositionEntrance - halfButtonWidth;
            InteractButton.Position = startVector;
        }
        public override void Update(GameTime gameTime)
        {
            Activated = false;
            if (player.IsTouching(MainSprite))
            {
                InteractButton.Hidden = false;
                if (input.CurrentState.IsKeyDown(input.KeyBindings["Interact"]) && input.PreviousState.IsKeyUp(input.KeyBindings["Interact"]))
                {
                    Activated = true;
                }
            }
            else
            {
                InteractButton.Hidden = true;
            }
            if (Activated)
                OnActivate();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            MainSprite.Draw(gameTime, spriteBatch);
            InteractButton.Draw(gameTime, spriteBatch);
        }

        protected virtual void OnActivate()
        {
            InteractButton.Hidden = true;
        }
    }
}
