using GameFramework;
using GameFramework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace EndlessRunner
{
	public class GameForm : UGuiForm
	{

		public Text CountDown;

		public GameObject PauseMenu;
		int CountDownCount = 5;
		float intervalTime = 1;
		private Transform m_Character;
		bool IsStartCountDown = false;

		[SerializeField]
		private Text Socre;

		[SerializeField]
		private Text Distance;

		[SerializeField]
		private Text PremiumCollectibleCount;

		[SerializeField]
		private Text PickupCount;



		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			//监听Charactor
			GameEntry.Event.Subscribe(CharacterEventArgs.EventId, TrackEvent);
		}

		private void TrackEvent(object sender, GameEventArgs e)
		{
			m_Character = (Transform)sender;
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			CountDown.gameObject.SetActive(true);
			CountDown.text = "5";
			IsStartCountDown = true;
			intervalTime =1;

		}

		protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate(elapseSeconds, realElapseSeconds);

			Distance.text = "Distance: "+((int)m_Character.position.z+3).ToString()+" m";
			if (IsStartCountDown) {

				intervalTime -= Time.deltaTime;
				if (intervalTime<=0) {
					intervalTime += 1;
					CountDownClick();
				}
				
			}
		}
	     void CountDownClick() 
		{		
			CountDownCount--;
			CountDown.text = CountDownCount.ToString();
		
			if (CountDownCount<=0) {
				CountDown.gameObject.SetActive(false);
				//倒计时完成 
				GameEntry.Event.Fire(this, ReferencePool.Acquire<CountDownEventArgs>());
				IsStartCountDown = false;
			}
		}


	}
	
}