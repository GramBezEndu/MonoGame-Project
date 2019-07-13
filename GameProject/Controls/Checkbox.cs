using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject.Sprites;

namespace GameProject.Controls
{
	/// <summary>
	/// Should be written using animations
	/// </summary>
    public class Checkbox : Sprite
    {
        MouseState currentState;
        MouseState previousState;
        Texture2D _checkbox;
        Texture2D _checkboxChecked;
        public bool Checked = false;
        bool isHovering;
		public EventHandler Click { get; set; }
        public Checkbox(Texture2D checkbox, Texture2D checkboxChecked, float scale) : base(checkbox, scale)
        {
            _checkbox = checkbox;
            _checkboxChecked = checkboxChecked;

        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var color = Color.White;

            if (isHovering)
                color = Color.Gray;

            spriteBatch.Draw(texture, Position, null, color, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
        }
        public override void Update(GameTime gameTime)
        {
            previousState = currentState;
            currentState = Mouse.GetState();

            var mouseRectangle = new Rectangle(currentState.X, currentState.Y, 1, 1);

            isHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                isHovering = true;

                if (currentState.LeftButton == ButtonState.Released && previousState.LeftButton == ButtonState.Pressed)
                {
                    //Set correct texture
                    if (Checked)
                    {
                        Checked = false;
                        texture = _checkbox;
                    }
                    else
                    {
                        Checked = true;
                        texture = _checkboxChecked;
                    }
					Click?.Invoke(this, new EventArgs());
                }
            }
        }
    }
}
