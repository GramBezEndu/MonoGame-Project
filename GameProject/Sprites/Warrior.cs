using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject.Animations;
using GameProject.Inventory;
using System.Diagnostics;
using GameProject.Items;
using GameProject.States;

namespace GameProject.Sprites
{
	public class Warrior : StaminaUser
	{
		private bool blockingRight = false;
		private bool blockingLeft = false;
		private bool blockingUp = false;
		private bool normalAttacking = false;
		private bool fastAttacking = false;
		private GameTimer attackTypeTimer = new GameTimer(0.2f);
		/// <summary>
		/// If last attack was normal, we do not want to perform fast attack before activating timer again (clicking again left button in game)
		/// </summary>
		private bool lastAttackWasNormal;
		private SpriteFont debuggerFont;
		private int fastAttackCounter = 0;
		private int normalAttackCounter = 0;
		public Warrior(GameState currentGameState, Dictionary<string, Animation> a, Input i, float scale, SpriteFont debugFont) : base(currentGameState, a, i, scale)
		{
			animations = a;
			animationManager = new AnimationManager(a.First().Value);
			debuggerFont = debugFont;
			animations["FastAttack"].OnAnimationEnd = OnFastAttackEnd;
			animations["NormalAttack"].OnAnimationEnd = OnNormalAttackEnd;
			attackRange = 40f * scale;
		}

		private void OnNormalAttackEnd(object sender, EventArgs e)
		{
			normalAttacking = false;
			//animations["NormalAttack"].CurrentFrame = 0;
			gameState.AttackEnemiesWithCritChance();
		}

		private void OnFastAttackEnd(object sender, EventArgs e)
		{
			fastAttacking = false;
			//animations["FastAttack"].CurrentFrame = 0;
			gameState.AttackEnemiesWithoutCrit();
		}

		public override void Update(GameTime gameTime)
		{
			ShieldBlocking();
			ManageAttacks(gameTime);
			BlockMovementWhileAttackingAndShielding();
			base.Update(gameTime);
		}

		private void ShieldBlocking()
		{
			if (Keyboard.GetState().IsKeyDown(input.KeyBindings["DodgeBlock"]))
			{
				//Check if warrior has shield equipped (with more than 0 durability), we need to check it every time
				bool equippedShield = false;
				foreach (InventorySlot i in InventoryManager.EquipmentManager.EquipmentSlots)
				{
					if (i is ShieldSlot)
					{
						if (i.Item != null)
						{
							if ((i.Item as Shield).CurrentDurability > 0)
								equippedShield = true;
						}
						break;
					}
				}
				//Allow blocking if shield is equipped and it has more than 0 durability
				if (equippedShield)
				{
					//Right Block
					if (Keyboard.GetState().IsKeyDown(input.KeyBindings["MoveRight"]))
					{
						blockingRight = true;
						blockingLeft = false;
						blockingUp = false;
					}
					//Left Block
					else if (Keyboard.GetState().IsKeyDown(input.KeyBindings["MoveLeft"]))
					{
						blockingLeft = true;
						blockingRight = false;
						blockingUp = false;
					}
					//Up Block
					else if (Keyboard.GetState().IsKeyDown(input.KeyBindings["MoveUp"]))
					{
						blockingUp = true;
						blockingLeft = false;
						blockingRight = false;
					}
				}
				//Shield is not equipped - don't allow any blocking
				else
				{
					blockingLeft = false;
					blockingRight = false;
					blockingUp = false;
				}
			}
			else
			{
				blockingRight = false;
				blockingLeft = false;
				blockingUp = false;
			}
		}

		private void BlockMovementWhileAttackingAndShielding()
		{
			if (normalAttacking || fastAttacking || blockingLeft || blockingRight || blockingUp)
				canMove = false;
			else
				canMove = true;
		}

		private void ManageAttacks(GameTime gameTime)
		{
			//Mouse was just clicked - enable timer
			if (input.PreviousMouseState.LeftButton == ButtonState.Released && input.CurrentMouseState.LeftButton == ButtonState.Pressed)
			{
				attackTypeTimer.Start();
			}
			//If left mouse was clicked - check type of attack
			if(attackTypeTimer.Enabled == true)
			{
				//Update timer
				attackTypeTimer.Update(gameTime);
				//Mouse was released
				if (input.CurrentMouseState.LeftButton == ButtonState.Released)
				{
					//Check if it was up short time - FastAttack
					//if (attackTypeTimer.CurrentTime < attackTypeTimer.Interval)
					if (attackTypeTimer.CurrentTime <  attackTypeTimer.Interval)
					{
						attackTypeTimer.Reset();
						SetFastAttack();
					}
				}
				//Mouse was being held for more than Normal attack time - Normal Attack
				//else if(attackTypeTimer.CurrentTime >= attackTypeTimer.Interval)
				else if (attackTypeTimer.CurrentTime <= 0.0f)
				{
					SetNormalAttack();
					attackTypeTimer.Start();
				}
				////Reset normal attack if button is not being held
				//if(input.CurrentMouseState.LeftButton == ButtonState.Released)
				//{
				//	normalAttacking = false;
				//}
			}
			//Reset attacks when animations ends
			/*
			if (animations["NormalAttack"].CurrentFrame == animations["NormalAttack"].FrameCount - 1)
			{
				normalAttacking = false;
				animations["NormalAttack"].CurrentFrame = 0;
			}
			if (animations["FastAttack"].CurrentFrame == animations["FastAttack"].FrameCount - 1)
			{
				fastAttacking = false;
				animations["FastAttack"].CurrentFrame = 0;
			}*/
		}
		/// <summary>
		/// Common attack requirements (sword equipped, not using shield, inventory not opened)
		/// </summary>
		/// <returns></returns>
		private bool AttackRequirements()
		{
			bool swordEquipped = IsSwordEquipped();
			//If no sword equipped
			if (!swordEquipped)
			{
				return false;
			}
			//If Inventory is Opened - attack should not start
			if (InventoryManager.Hidden == false)
			{
				return false;
			}
			//If using shield
			if (blockingLeft || blockingRight || blockingUp)
			{
				return false;
			}
			return true;
		}

		private void SetNormalAttack()
		{
			if (!AttackRequirements())
				return;
			//If not enough stamina
			if (StaminaBar.Stamina.CurrentStamina < 5)
			{
				return;
			}
			//Basic requirements fulfilled
			else
			{
				//If is already attacking - return
				if (fastAttacking == true || normalAttacking == true)
				{
					return;
				}
				else
				{
					StaminaBar.Stamina.CurrentStamina -= 5;
					normalAttacking = true;
					lastAttackWasNormal = true;
					normalAttackCounter++;
				}
			}
		}

		/// <summary>
		/// Check if sword is equipped
		/// </summary>
		/// <returns></returns>
		private bool IsSwordEquipped()
		{
			foreach (InventorySlot i in InventoryManager.EquipmentManager.EquipmentSlots)
			{
				if (i is SwordSlot)
				{
					if (i.Item != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		private void SetFastAttack()
        {
			if (!AttackRequirements())
				return;
			//If not enough stamina
			if (StaminaBar.Stamina.CurrentStamina < 10)
			{
				return;
			}
			//Basic requirements fulfilled
			else
			{
				//If is already attacking - return
				if (fastAttacking == true || normalAttacking == true)
				{
					return;
				}
				else
				{
					//If last was normal and current timer is less then Interval (if current timer equals interval it is a new click)
					if(lastAttackWasNormal && attackTypeTimer.CurrentTime < attackTypeTimer.Interval)
					{
						lastAttackWasNormal = false;
					}
					//Else perform attack normally
					else
					{
						StaminaBar.Stamina.CurrentStamina -= 10;
						fastAttacking = true;
						fastAttackCounter++;
					}
				}
			}
        }
		protected override void PlayAnimations()
		{
			if (Velocity.X > 0)
				animationManager.Play(animations["WalkRight"]);
			else if (Velocity.X < 0)
				animationManager.Play(animations["WalkLeft"]);
			else if (blockingRight)
				animationManager.Play(animations["BlockRight"]);
			else if (blockingLeft)
				animationManager.Play(animations["BlockLeft"]);
			else if (blockingUp)
				animationManager.Play(animations["BlockUp"]);
			else if (fastAttacking)
				animationManager.Play(animations["FastAttack"]);
			else if (normalAttacking)
				animationManager.Play(animations["NormalAttack"]);
			else
				animationManager.Play(animations["Idle"]);
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);
			if(Debugger.IsAttached)
			{
				spriteBatch.DrawString(debuggerFont, "Enabled: " + attackTypeTimer.Enabled.ToString() + " currentTime: " + attackTypeTimer.CurrentTime.ToString(), new Vector2(0, 0), Color.Red);
				spriteBatch.DrawString(debuggerFont, "Fast Attacking: " + fastAttacking.ToString(), new Vector2(0, 30), Color.Red);
				spriteBatch.DrawString(debuggerFont, "Normal Attacking: " + normalAttacking.ToString(), new Vector2(0, 60), Color.Red);
				spriteBatch.DrawString(debuggerFont, "Fast counter: " + fastAttackCounter.ToString(), new Vector2(0, 90), Color.Red);
				spriteBatch.DrawString(debuggerFont, "Normal counter " + normalAttackCounter.ToString(), new Vector2(0, 120), Color.Red);
			}
		}
	}
}