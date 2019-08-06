using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject.Sprites;
using GameProject.Animations;

namespace GameProject.Controls
{
    public class Checkbox : Sprite
    {
		Input Input;

        public bool Checked = false;
        bool isHovering;
		public EventHandler Click { get; set; }
        public Checkbox(Input i, Dictionary<string, Animation> a) : base(a)
        {
			Input = i;
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
			if(!Hidden)
			{
				var color = Color.White;

				if (isHovering)
					color = Color.Gray;

				base.Draw(gameTime, spriteBatch);
			}
        }
        public override void Update(GameTime gameTime)
        {
			if(!Hidden)
			{
				var mouseRectangle = new Rectangle(Input.CurrentMouseState.Position.X, Input.CurrentMouseState.Position.Y, 1, 1);

				isHovering = false;

				if (mouseRectangle.Intersects(Rectangle))
				{
					isHovering = true;

					if (Input.CurrentMouseState.LeftButton == ButtonState.Released && Input.PreviousMouseState.LeftButton == ButtonState.Pressed)
					{
						//Change flag
						if (Checked)
						{
							Checked = false;
						}
						else
						{
							Checked = true;
						}
						Click?.Invoke(this, new EventArgs());
					}
				}
				PlayAnimations();
			}
        }

		private void PlayAnimations()
		{
			if (Checked)
				animationManager.Play(animations["CheckboxChecked"]);
			else
				animationManager.Play(animations["Checkbox"]);
		}
    }
}
