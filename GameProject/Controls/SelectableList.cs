using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameProject.Sprites;

namespace GameProject.Controls
{
	public class SelectableList : Component
	{
		Button ArrowSelector;
		/// <summary>
		/// Note: First element of the list contains Selected Option
		/// </summary>
		List<TextButton> Options = new List<TextButton>();
		bool Expanded;
		private Vector2 _position;

		public Vector2 Position
		{
			get
			{
				return _position;
			}
			set
			{
				_position = value;
				int extraWidth = SetOptionsPosition();
				SetArrowSelectorPosition(extraWidth);
			}
		}

		private int SetOptionsPosition()
		{
			int totalheight = 0;
			int maxWidth = 0;
			for(int i=0;i<Options.Count;i++)
			{
				Options[i].Position = new Vector2(this.Position.X, this.Position.Y + totalheight);
				totalheight += Options[i].Height;
				if (Options[i].Width > maxWidth)
					maxWidth = Options[i].Width;
			}
			return maxWidth;
		}

		private void SetArrowSelectorPosition(int extraWidth)
		{
			if (ArrowSelector != null)
			{
				ArrowSelector.Position = new Vector2(this.Position.X + extraWidth, this.Position.Y);
			}
		}

		public SelectableList(Input input, Texture2D arrowSelectorTexture, float Scale, SpriteFont font, List<string> options)
		{
			for (int i = 0; i < options.Count; i++)
			{
				Options.Add(new TextButton(input, font, options[i])
				{
					Hidden = true,
					Click = SetSelectedValue
				});
			}

			//Selected option is always visible
			Options[0].Hidden = false;

			//Get the largest width of text (so we can set correct arrow selector button position)
			int width = 0;
			foreach (var t in Options)
			{
				if (t.Width > width)
					width = t.Width;
			}

			ArrowSelector = new Button(arrowSelectorTexture, font, Scale);
			ArrowSelector.Click = ExpandOrHide;
		}

		//TODO: Finish this method
		private void SetSelectedValue(object sender, EventArgs e)
		{
			int index = Options.FindIndex(x => x == sender);
			if(index == -1)
				throw new Exception("SelectableList: selected value is not in the list");
			if(index == 0)
			{
				//User clicked on first element (currently selected) -> perform no action
				return;
			}
			Hide();
		}

		private void ExpandOrHide(object sender, EventArgs e)
		{
			if (Expanded)
			{
				Hide();
			}
			else
			{
				Expand();
			}
		}

		private void Expand()
		{
			foreach (var o in Options)
			{
				o.Hidden = false;
			}
			Expanded = true;
		}

		private void Hide()
		{
			for(int i=1;i<Options.Count;i++)
			{
				Options[i].Hidden = true;
			}
			Expanded = false;
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (!Hidden)
			{
				foreach (var o in Options)
					o.Draw(gameTime, spriteBatch);
				ArrowSelector.Draw(gameTime, spriteBatch);
			}
		}

		public override void Update(GameTime gameTime)
		{
			if (!Hidden)
			{
				ArrowSelector.Update(gameTime);
				foreach (var b in Options)
					b.Update(gameTime);
			}
		}
	}
}
