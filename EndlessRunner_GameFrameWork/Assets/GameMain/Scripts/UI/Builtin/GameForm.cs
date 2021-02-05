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

		//生命值Image
		public List<GameObject> LiftCount;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);


			//监听GameForm相关事件
			GameEntry.Event.Subscribe(GameFormEventArgs.EventId, GameFormEvent);
		}

	    /// <summary>
		/// 界面相关的事件监听
		/// </summary>
		/// <param name="sender">事件数据</param>
		/// <param name="e"></param>
		private void GameFormEvent(object sender, GameEventArgs e)
		{
			GameFormData data=(GameFormData)sender;
			switch (data.gameFormEnum) {

				case GameFormEnum.Init:
					m_Character = (Transform)data.Data;
					break;
				case GameFormEnum.BloodLoss:
					LiftCount[int.Parse(data.Data.ToString())].SetActive(false);
					break;
				case GameFormEnum.Coin:
					break;
				case GameFormEnum.FishBone:
					break;
			}

	
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