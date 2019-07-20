using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Controls
{
    public class Text : Component
    {
        private string _message;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public Vector2 Position { get; set; }
		/// <summary>
		/// You can apply any extra effect to text component using this method
		/// </summary>
		public EventHandler ExtraUpdate { get; set; }
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                //Update width and height
                Width = (int)(Font.MeasureString(_message).X);
                Height = (int)(Font.MeasureString(_message).Y);
            }
        }
        public Color Color { get; set; }
        protected SpriteFont Font { get; set; }
        public Text(SpriteFont font, string message)
        {
            Color = Color.Black;
            Font = font;
            Message = message;
        }
        public override void Update(GameTime gameTime)
        {
			ExtraUpdate?.Invoke(this, new EventArgs());
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Hidden)
                spriteBatch.DrawString(Font, Message, Position, Color);
        }
    }
}
