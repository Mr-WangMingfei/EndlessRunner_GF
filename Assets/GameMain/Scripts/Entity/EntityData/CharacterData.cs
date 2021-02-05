using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EndlessRunner
{
	public class CharacterData : EntityData {


		/// <summary>
		/// �ٶ�
		/// </summary>
		public float Speed { get; private set; }

		/// <summary>
		/// �ƶ���ʼ��
		/// </summary>
		public Vector3 StartPostion { get; private set; }


		public CharacterData(int entityId, int typeId,float speed,Vector3 startPostion) : base(entityId, typeId)
		{
			Speed = speed;
			StartPostion = startPostion;
		}
	}
	
}