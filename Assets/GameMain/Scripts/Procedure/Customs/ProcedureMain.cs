using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace EndlessRunner
{
	public class ProcedureMain : ProcedureBase
	{

		private GameForm m_GameForm = null;

		protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
		{
			base.OnEnter(procedureOwner);
			GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

			//打开UI界面
			GameEntry.UI.OpenUIForm(UIFormId.Game, this);
		
			//实例化track
			for (int i=14;i<=34;i++)
			{
				GameEntry.Entity.ShowTrack(new TrackData(GameEntry.Entity.GenerateSerialId(), i, new Vector3(0, -2.2f, 0)));
			}

			GameEntry.Entity.ShowCharacter(new CharacterData(GameEntry.Entity.GenerateSerialId(),2,6f,new Vector3(0,-2f,-3)));
		}

		protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
		}

		private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
		{
			OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
			if (ne.UserData != this)
			{
				return;
			}

			m_GameForm = (GameForm)ne.UIForm.Logic;
		}
	}
	
}