using Cinemachine;
using GameFramework;
using GameFramework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace EndlessRunner
{
	public class Character : Entity {

		static int s_DeadHash = Animator.StringToHash("Dead");
		static int s_RunStartHash = Animator.StringToHash("runStart");
		static int s_MovingHash = Animator.StringToHash("Moving");
		static int s_JumpingHash = Animator.StringToHash("Jumping");
		static int s_JumpingSpeedHash = Animator.StringToHash("JumpSpeed");
		static int s_SlidingHash = Animator.StringToHash("Sliding");

		private CharacterData m_CharacterData = null;

	

		[Header("Controls")]
		public float jumpLength = 2.0f;     // Distance jumped
		public float jumpHeight = 1.2f;
		public float slideLength = 2.0f;

		public bool m_jump=false;
		public bool m_Slide = false;
		public bool isMoving = false;
		public bool iSDead=false;

		float ratio = 0;
		int Currentdirection = 0;

		Animator ani;
		AnimatorStateInfo animatorInfo;
		/// <summary>
		/// 当前主角所在的地面名字
		/// </summary>
		string CurrentGroundName = "";

		//游戏界面数据
		GameFormData _GameFormData;
		int RemainingLift = 2;
		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			_GameFormData = new GameFormData();
		}

		protected override void OnShow(object userData)
		{
			base.OnShow(userData);
			m_CharacterData = (CharacterData)userData;
			CachedTransform.position = m_CharacterData.StartPostion;
			ani = CachedTransform.GetComponent<Animator>();
	
			Camera.main.GetComponent<CameraFollow>().target= CachedTransform;

			_GameFormData.gameFormEnum = GameFormEnum.Init;
			_GameFormData.Data = CachedTransform;
			GameEntry.Event.Fire(_GameFormData, ReferencePool.Acquire<GameFormEventArgs>());

			//监听倒计时事件
			GameEntry.Event.Subscribe(CountDownEventArgs.EventId, ConutDonwFinished);
	
		}

		private void ConutDonwFinished(object sender, GameEventArgs e)
		{

			StartRunning();
		}


		protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate(elapseSeconds, realElapseSeconds);

			if (iSDead) return;

#if UNITY_EDITOR || UNITY_STANDALONE
			// Use key input in editor or standalone
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				ChangeLane(-1);
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				ChangeLane(1);	
			}
			else if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				Jump();
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				Slide();
			}
#else
        // Use touch input on mobile
        if (Input.touchCount == 1)
        {
			if(m_IsSwiping)
			{
				Vector2 diff = Input.GetTouch(0).position - m_StartingTouch;

				// Put difference in Screen ratio, but using only width, so the ratio is the same on both
                // axes (otherwise we would have to swipe more vertically...)
				diff = new Vector2(diff.x/Screen.width, diff.y/Screen.width);

				if(diff.magnitude > 0.01f) //we set the swip distance to trigger movement to 1% of the screen width
				{
					if(Mathf.Abs(diff.y) > Mathf.Abs(diff.x))
					{
						if(diff.y < 0)
						{
							Slide();
						}
						else
						{
							Jump();
						}
					}
					else
					{
						if(diff.x < 0)
						{
							ChangeLane(-1);
						}
						else
						{
							ChangeLane(1);
						}
					}
						
					m_IsSwiping = false;
				}
            }

        	// Input check is AFTER the swip test, that way if TouchPhase.Ended happen a single frame after the Began Phase
			// a swipe can still be registered (otherwise, m_IsSwiping will be set to false and the test wouldn't happen for that began-Ended pair)
			if(Input.GetTouch(0).phase == TouchPhase.Began)
			{
				m_StartingTouch = Input.GetTouch(0).position;
				m_IsSwiping = true;
			}
			else if(Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				m_IsSwiping = false;
			}
        }
#endif
			if (isMoving) {
				this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, this.transform.localPosition + new Vector3(0, 0, 1000), m_CharacterData.Speed * Time.deltaTime);
			}
		

			if (m_jump)
			{
				animatorInfo = ani.GetCurrentAnimatorStateInfo(0);
				if ((animatorInfo.normalizedTime >= 0.89f) && (animatorInfo.IsName("Jump")))
				{
				
		
				}
				
			}
			if (m_Slide)
			{
				animatorInfo = ani.GetCurrentAnimatorStateInfo(0);			
				if ((animatorInfo.normalizedTime >= 0.89f) && (animatorInfo.IsName("Sliding")))
				{
					ChangeCollider(1);
					StopSliding();
					m_Slide = false;	
				}			
			}  		
		}


		void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Obstacle")
			{
				ani.SetTrigger("Hit");
				//碰到障碍物受伤掉血
				_GameFormData.gameFormEnum = GameFormEnum.BloodLoss;
				_GameFormData.Data = RemainingLift;
				GameEntry.Event.Fire(_GameFormData, ReferencePool.Acquire<GameFormEventArgs>());
				if (RemainingLift <= 0)
				{
					iSDead = true;
					//执行死亡动画
					ani.SetBool(s_DeadHash, true);
					ani.SetFloat("RandomDeath", 0);
				}
			
				RemainingLift--;

			}
		
		}


		void StartRunning()
		{
			if (this.GetComponent<Animator>())
			{
				ani.Play(s_RunStartHash);
				ani.SetBool(s_MovingHash, true);
				isMoving = true;
			}
		}

		void StopMoving()
		{		
			if (this.GetComponent<Animator>())
			{
				ani.SetBool(s_MovingHash, false);
			}
		}

		public void Jump()
		{
			if (m_jump) {

				return;
			}
			this.GetComponent<Rigidbody>().velocity = new Vector3(this.GetComponent<Rigidbody>().velocity.x,5, this.GetComponent<Rigidbody>().velocity.z);
			this.GetComponent<Rigidbody>().AddForce(Vector3.up * 50, ForceMode.Acceleration);
			//ani.SetFloat(s_JumpingSpeedHash, 1f);
			ani.SetBool(s_JumpingHash, true);

			//jump 音效 todo

			GameEntry.Sound.PlaySound(22);
			m_jump = true;
		
			
		}

		public void Slide()
		{
			if (m_Slide) {
				return;
			}
			ChangeCollider(0);
			//ani.SetFloat(s_JumpingSpeedHash, 0.5f);
			ani.SetBool(s_SlidingHash, true);
			//Slide 音效 todo
			m_Slide = true;
		}

		public void StopSliding()
		{
			ani.SetBool(s_SlidingHash, false);
		}

		public void ChangeLane(int direction)
		{
			if (Currentdirection == 1&& direction==1) {

				return;
			} else if (Currentdirection == -1 && direction == -1)
			{
				return;
			}
			Currentdirection += direction;
			this.transform.localPosition = this.transform.localPosition+ new Vector3(direction*1.3f,0 , 0);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index">0表示压缩 1表示还原 </param>
		void ChangeCollider(int index)
		{
			if (index == 0)
			{
				this.GetComponent<BoxCollider>().center = new Vector3(0,0.2f,0);
				this.GetComponent<BoxCollider>().size = new Vector3(0.52f, 0.4f, 0.8f);
			}
			else
			{
				this.GetComponent<BoxCollider>().center = new Vector3(0, 0.68f, 0);
				this.GetComponent<BoxCollider>().size = new Vector3(0.52f, 1.26f, 0.49f);
			}
		}

		void OnCollisionEnter(Collision collision)
		{
			if (collision.collider.tag == "Ground")
			{
				ani.SetBool(s_JumpingHash, false);
				m_jump = false;
			}
		
		}
	

	
	}
	
}