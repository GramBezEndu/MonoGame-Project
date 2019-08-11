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
	public class Animation : ICloneable
	{
		public int CurrentFrame { get; set; }
		public int FrameCount { get; private set; }
		/// <summary>
		/// Does not include scaling
		/// </summary>
		public int FrameHeight { get { return (int)(Texture.Height); } }
		public float FrameTime { get; set; }
		/// <summary>
		/// Does not include scaling
		/// </summary>
		public int FrameWidth { get { return (int)(Texture.Width/FrameCount); } }
		public Texture2D Texture { get; set; }
		public bool IsLooping { get; set; }
		public Vector2 Scale { get; set; }
		public EventHandler OnAnimationEnd { get; set; }

		public float Rotation { get; private set; } = 0f;
		public Vector2 Origin;

		public Animation(Texture2D texture, int frameCount, Vector2 scale, float frameTime = 0.3f, bool looping = true)
		{
			Texture = texture;
			FrameCount = frameCount;

			IsLooping = looping;

			FrameTime = frameTime;
			Scale = scale;

			//Same origin for every frame
			Origin = new Vector2(FrameWidth / 2f, FrameHeight / 2f);
		}

		public object Clone()
		{
			return MemberwiseClone();
		}
	}
}
