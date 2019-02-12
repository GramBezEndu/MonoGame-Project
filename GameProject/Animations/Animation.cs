using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject.Animations
{
	public class Animation
	{
		public int CurrentFrame { get; set; }
		public int FrameCount { get; private set; }
		public int FrameHeight { get { return (int)(Texture.Height); } }
		public float FrameTime { get; set; }
		public int FrameWidth { get { return (int)(Texture.Width/FrameCount); } }
		public Texture2D Texture { get; set; }
		public bool IsLooping { get; set; }
		public float Scale { get; set; }
		public Animation(Texture2D texture, int frameCount, float scale)
		{
			Texture = texture;
			FrameCount = frameCount;

			IsLooping = true;

			FrameTime = 0.5f;
			Scale = scale;
		}
	}
}
