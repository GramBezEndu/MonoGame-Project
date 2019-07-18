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
		/// List of all options
		/// </summary>
		List<TextButton> Options = new List<TextButton>();
		/// <summary>
		/// Currently selected option
		/// </summary>
		public TextButton SelectedOption { get; private set; }
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

		/// <summary>
		/// Returns the largest width of text (so we can set correct arrow selector button position)
		/// </summary>
		/// <returns></returns>
		private int SetOptionsPosition()
		{
			SelectedOption.Position = new Vector2(this.Position.X, this.Position.Y);
			int extraHeight = SelectedOption.Height;
			int maxWidth = SelectedOption.Width;
			for (int i=0;i<Options.Count;i++)
			{
				Options[i].Position = new Vector2(this.Position.X, this.Position.Y + extraHeight);
				extraHeight += Options[i].Height;
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
			SelectedOption = (TextButton)Options[0].Clone();
			SelectedOption.Hidden = false;

			ArrowSelector = new Button(arrowSelectorTexture, font, Scale);
			ArrowSelector.Click = ExpandOrHide;
		}

		//TODO: Finish this method
		private void SetSelectedValue(object sender, EventArgs e)
		{
			//Hide list after click
			Hide();

			int index = Options.FindIndex(x => x == sender);
			//Element is not in the list -> so it should be currently selected option
			if(index == -1)
			{
				if((sender as TextButton).Message == SelectedOption.Message)
				{
					//User clicked on first element (currently selected) -> perform no further action
					return;
				}
				else
				{
					throw new Exception("SelectableList: selected value is not in the list");
				}
			}
			ChangeCurrentlySelected(index);
		}

		private void ChangeCurrentlySelected(int index)
		{
			SelectedOption = (TextButton)Options[index].Clone();
			SelectedOption.Hidden = false;
			SelectedOption.Position = new Vector2(this.Position.X, this.Position.Y);
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
			//Make every option visible
			foreach (var o in Options)
				o.Hidden = false;
			Expanded = true;
		}

		private void Hide()
		{
			foreach (var o in Options)
			{
				o.Hidden = true;
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
				SelectedOption.Draw(gameTime, spriteBatch);
			}
		}

		public override void Update(GameTime gameTime)
		{
			if (!Hidden)
			{
				ArrowSelector.Update(gameTime);
				foreach (var b in Options)
					b.Update(gameTime);
				SelectedOption.Update(gameTime);
			}
		}
	}
}
