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
using GameProject.Controls;

namespace GameProject.Sprites
{
	public class Warrior : StaminaUser
	{
		private bool blockingRight = false;
		private bool blockingLeft = false;
		private bool blockingUp = false;
		private bool normalAttacking = false;
		private bool fastAttacking = false;

		private SpriteFont debuggerFont;
		private int fastAttackCounter = 0;
		private int normalAttackCounter = 0;
		public Warrior(GameState currentGameState, Dictionary<string, Animation> a, Input i, float scale, SpriteFont debugFont) : base(currentGameState, a, i, scale)
		{
			animations = a;
			animationManager = new AnimationManager(this, a.First().Value);
			debuggerFont = debugFont;
			animations["FastAttack"].OnAnimationEnd = OnFastAttackEnd;
			animations["NormalAttack"].OnAnimationEnd = OnNormalAttackEnd;
			animations["Die"].OnAnimationEnd = Dead;
			attackRange = 40f * scale;
		}

		private void OnNormalAttackEnd(object sender, EventArgs e)
		{
			normalAttacking = false;
			//animations["NormalAttack"].CurrentFrame = 0;
			gameState.MeleeAttackWithCrit();
		}

		private void OnFastAttackEnd(object sender, EventArgs e)
		{
			fastAttacking = false;
			//animations["FastAttack"].CurrentFrame = 0;
			gameState.MeleeAttackWithoutCrit();
		}

		public override void Update(GameTime gameTime)
		{
			if(!IsDead)
			{
				ShieldBlocking();
				ManageAttacks(gameTime);
				BlockMovementWhileAttackingAndShielding();
			}
			base.Update(gameTime);
		}

		private void ShieldBlocking()
		{
			if (Keyboard.GetState().IsKeyDown(input.KeyBindings["DodgeBlock"].GetValueOrDefault()))
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
					if (Keyboard.GetState().IsKeyDown(input.KeyBindings["MoveRight"].GetValueOrDefault()))
					{
						blockingRight = true;
						blockingLeft = false;
						blockingUp = false;
					}
					//Left Block
					else if (Keyboard.GetState().IsKeyDown(input.KeyBindings["MoveLeft"].GetValueOrDefault()))
					{
						blockingLeft = true;
						blockingRight = false;
						blockingUp = false;
					}
					//Up Block
					else if (Keyboard.GetState().IsKeyDown(input.KeyBindings["MoveUp"].GetValueOrDefault()))
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
				CanMove = false;
			else
				CanMove = true;
		}

		/// <summary>
		/// New attacking - left mouse button -> fast attack, right mouse button -> normal attack
		/// </summary>
		/// <param name="gameTime"></param>
		private void ManageAttacks(GameTime gameTime)
		{
			//Left mouse is in pressed state
			if (input.CurrentMouseState.LeftButton == ButtonState.Pressed)
			{
				SetFastAttack();
			}
			//Right mouse is in pressed state
			else if (input.CurrentMouseState.RightButton == ButtonState.Pressed)
			{
				SetNormalAttack();
			}
		}
		/// <summary>
		/// Common attack requirements (sword equipped, not using shield, inventory not opened, any extra window not opened)
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
            //if has any window opened (blacsmith/shopkeeper etc.)
            if(UsingWindow)
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
					StaminaBar.Stamina.CurrentStamina -= 10;
					fastAttacking = true;
					fastAttackCounter++;
				}
			}
        }
		protected override void PlayAnimations()
		{
			if (IsDead && DyingAnimationFinished)
				animationManager.Play(animations["Dead"]);
			else if (IsDead)
				animationManager.Play(animations["Die"]);
			else if (Velocity.X > 0)
			{
				FlipHorizontally = false;
				animationManager.Play(animations["Walk"]);
			}
			else if (Velocity.X < 0)
			{
				FlipHorizontally = true;
				animationManager.Play(animations["Walk"]);
			}
			else if (blockingRight)
			{
				FlipHorizontally = false;
				animationManager.Play(animations["Block"]);
			}
			else if (blockingLeft)
			{
				FlipHorizontally = true;
				animationManager.Play(animations["Block"]);
			}
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
				spriteBatch.DrawString(debuggerFont, "Fast Attacking: " + fastAttacking.ToString(), new Vector2(0, 30), Color.Red);
				spriteBatch.DrawString(debuggerFont, "Normal Attacking: " + normalAttacking.ToString(), new Vector2(0, 60), Color.Red);
				spriteBatch.DrawString(debuggerFont, "Fast counter: " + fastAttackCounter.ToString(), new Vector2(0, 90), Color.Red);
				spriteBatch.DrawString(debuggerFont, "Normal counter: " + normalAttackCounter.ToString(), new Vector2(0, 120), Color.Red);
				spriteBatch.DrawString(debuggerFont, "Pos: " + Position.ToString(), new Vector2(0, 150), Color.Red);
			}
		}
	}
}