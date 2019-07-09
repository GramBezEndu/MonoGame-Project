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
		/// Current time, if current time is equal or less than 0, action should be performed
		/// </summary>
		public float CurrentTime { get; set; }
		public bool Enabled { get; set; }
		/// <summary>
		/// Initialize a new timer
		/// </summary>
		/// <param name="seconds">Interval in seconds (after this time action should be performed)</param>
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
		/// <summary>
		/// Starts timer
		/// Note: can be also used as restart method
		/// </summary>
		public void Start()
		{
			Enabled = true;
			CurrentTime = Interval;
		}
		/// <summary>
		/// Disables timer and resets interval
		/// </summary>
		public void Reset()
		{
			Enabled = false;
			CurrentTime = Interval;
		}
	}
}
