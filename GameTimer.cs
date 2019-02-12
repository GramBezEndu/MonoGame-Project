using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject
{
	public class GameTimer
	{
		/// <summary>
		/// Set the interval of action
		/// </summary>
		public float Interval { get; set; }
		/// <summary>
		/// Current time 
		/// </summary>
		public float CurrentTime { get; set; }
		public bool Enabled { get; set; }
		/// <summary>
		/// Initialize a new timer
		/// </summary>
		/// <param name="seconds">Interval in seconds</param>
		/// <param name="enabled"></param>
		public GameTimer(float seconds, bool enabled = false)
		{
			Interval = seconds;
			CurrentTime = Interval;
			Enabled = enabled;
		}
		public void Update(GameTime gameTime)
		{
			if(Enabled)
				CurrentTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
		}
		public void Restart()
		{
			CurrentTime = Interval;
		}
	}
}
