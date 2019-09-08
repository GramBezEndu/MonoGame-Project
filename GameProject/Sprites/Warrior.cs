﻿using System;
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

        public Rectangle? ShieldRectangle;
        private Texture2D ShieldRectangleTexture;
        public Vector2 ShieldRectanglePosition;

        public Warrior(GameState currentGameState, Dictionary<string, Animation> a, Input i, Vector2 scale, SpriteFont debugFont) : base(currentGameState, a, i, scale)
		{
			animations = a;
			animationManager = new AnimationManager(this, a.First().Value);
			debuggerFont = debugFont;
			animations["FastAttack"].OnAnimationEnd = OnFastAttackEnd;
			animations["NormalAttack"].OnAnimationEnd = OnNormalAttackEnd;
			animations["Die"].OnAnimationEnd = Dead;
			attackRange = 40f * scale.X;
		}

        private void ApplyShieldRectangle()
        {
            if(blockingUp)
            {
                ShieldRectanglePosition = new Vector2(this.Position.X, this.Position.Y + 0.07f * this.Height);
                ShieldRectangle = new Rectangle((int)ShieldRectanglePosition.X, (int)ShieldRectanglePosition.Y, (int)(animations["BlockUp"].FrameWidth * animations["BlockUp"].Scale.X), 1);
                ShieldRectangleTexture = SetRectangleTextureForTexture(ShieldRectangle.GetValueOrDefault());
            }
            else if(blockingLeft)
            {
                ShieldRectanglePosition = new Vector2(this.Rectangle.Left + 0.08f * this.Width, this.Position.Y);
                ShieldRectangle = new Rectangle((int)ShieldRectanglePosition.X, (int)ShieldRectanglePosition.Y, 1, (int)(animations["Block"].FrameHeight * animations["Block"].Scale.Y));
                ShieldRectangleTexture = SetRectangleTextureForTexture(ShieldRectangle.GetValueOrDefault());
            }
            else if(blockingRight)
            {
                ShieldRectanglePosition = new Vector2(this.Rectangle.Right - 0.08f * this.Width, this.Position.Y);
                ShieldRectangle = new Rectangle((int)ShieldRectanglePosition.X, (int)ShieldRectanglePosition.Y, 1, (int)(animations["Block"].FrameHeight * animations["Block"].Scale.Y));
                ShieldRectangleTexture = SetRectangleTextureForTexture(ShieldRectangle.GetValueOrDefault());
            }
            //Not blocking = rectangle = null
            else
            {
                ShieldRectangle = null;
            }
        }

		private void OnNormalAttackEnd(object sender, EventArgs e)
		{
			normalAttacking = false;
            //Reset melee attack rectangle
            MeleeAttackRectangle = null;
            //Reset attacked enemies
            EnemiesAttacked = new List<Enemy>();
        }

		private void OnFastAttackEnd(object sender, EventArgs e)
		{
			fastAttacking = false;
            //Reset melee attack rectangle
            MeleeAttackRectangle = null;
            //Reset attacked enemies
            EnemiesAttacked = new List<Enemy>();
        }

		public override void Update(GameTime gameTime)
		{
			if(!IsDead)
			{
				ShieldBlocking();
                ApplyShieldRectangle();
				ManageAttacks(gameTime);
				BlockMovementWhileAttackingAndShielding();
			}
			base.Update(gameTime);
		}

		private void ShieldBlocking()
		{
			if (Keyboard.GetState().IsKeyDown(input.KeyBindings["DodgeBlock"].GetValueOrDefault()))
			{
				//Check if warrior has shield equipped, we need to check it every time
				bool equippedShield = false;
				foreach (InventorySlot i in InventoryManager.EquipmentManager.EquipmentSlots)
				{
					if (i is ShieldSlot)
					{
						if (i.Item != null)
						{
							equippedShield = true;
						}
						break;
					}
				}
				//Allow blocking if shield is equipped
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

            if (normalAttacking)
            {
                var enemiesAttackedInThisFrame = gameState.MeleeAttackWithCrit(EnemiesAttacked);
                EnemiesAttacked.AddRange(enemiesAttackedInThisFrame);
            }
            else if (fastAttacking)
            {
                var enemiesAttackedInThisFrame = gameState.MeleeAttackWithoutCrit(EnemiesAttacked);
                EnemiesAttacked.AddRange(enemiesAttackedInThisFrame);
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
                    Vector2 size = new Vector2((int)(animations["NormalAttack"].FrameWidth * 0.3f), (int)(animations["NormalAttack"].FrameHeight * 0.3f));
                    if(!FlipHorizontally)
                        MeleeRectanglePosition = new Vector2((int)(this.Rectangle.Right - size.X), (int)(this.Position.Y + this.Width * 0.66f));
                    else
                        MeleeRectanglePosition = new Vector2((int)(this.Rectangle.Left), (int)(this.Position.Y + this.Width * 0.66f));
                    MeleeAttackRectangle = new Rectangle((int)MeleeRectanglePosition.X, (int)MeleeRectanglePosition.Y, (int)(size.X), (int)(size.Y));
                    SetMeleeAttackTexture();
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
					gameState.SoundEffects["SwordSlashFast"].Play();
                    Vector2 size = new Vector2((int)(animations["FastAttack"].FrameWidth * 0.3f), (int)(animations["FastAttack"].FrameHeight * 0.3f));
                    if (!FlipHorizontally)
                        MeleeRectanglePosition = new Vector2((int)(this.Rectangle.Right - size.X), (int)(this.Position.Y + this.Width * 0.66f));
                    else
                        MeleeRectanglePosition = new Vector2((int)(this.Rectangle.Left), (int)(this.Position.Y + this.Width * 0.66f));
                    MeleeAttackRectangle = new Rectangle((int)MeleeRectanglePosition.X, (int)MeleeRectanglePosition.Y, (int)(size.X), (int)(size.Y));
                    SetMeleeAttackTexture();
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
                if(ShieldRectangle != null && ShieldRectangleTexture != null)
                    spriteBatch.Draw(ShieldRectangleTexture, ShieldRectanglePosition, null, Color.Blue, rotation, Origin, new Vector2(1f, 1f), SpriteEffects.None, 0f);
            }
		}
	}
}