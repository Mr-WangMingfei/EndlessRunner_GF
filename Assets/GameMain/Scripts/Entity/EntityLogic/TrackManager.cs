using GameFramework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EndlessRunner
{
	public class TrackManager : Entity
	{

		protected static Queue<Track> AllTrack = new Queue<Track>();

		/// <summary>
		/// ��һ������е�Track
		/// </summary>
		protected static Track ProTrack;

		//�����ߵ�Track
		protected static Track CurrentGoneTrack=null;
		//����
		Transform Character;

		protected bool TrackInitFinished = false;

		protected override void OnShow(object userData)
		{
			base.OnShow(userData);
	
			//����Charactor
			GameEntry.Event.Subscribe(CharacterEventArgs.EventId, TrackEvent);

		}

		private void TrackEvent(object sender, GameEventArgs e)
		{
	
			Character = (Transform)sender;
			
		}

		protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate(elapseSeconds, realElapseSeconds);
			if (Character!=null && CurrentGoneTrack!=null) {
				if (Character.position.z> CurrentGoneTrack.GetComponent<TrackSegment>().pathParent.GetChild(1).position.z+5) 
				{
					CurrentGoneTrack.transform.SetLocalPosition(new Vector3(0, -2.2f, ProTrack.GetComponent<TrackSegment>().pathParent.GetChild(1).transform.position.z + 4f));
					ProTrack = CurrentGoneTrack;
					AllTrack.Enqueue(CurrentGoneTrack);
					CurrentGoneTrack = AllTrack.Dequeue();
					
				}

			}

		}

		protected override void OnHide(object userData)
		{
			base.OnHide(userData);
		}

	}

}