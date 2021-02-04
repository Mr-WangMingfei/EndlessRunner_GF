using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EndlessRunner
{
	public class TrackData : EntityData
	{


        /// <summary>
        /// �����Ŀ��ʱ�����µı���ʵ��
        /// </summary>
        public float SpawnTarget { get; private set; }

        /// <summary>
        /// �����Ŀ��ʱ��������
        /// </summary>
        public float HideTarget { get; private set; }

        /// <summary>
        /// �ƶ���ʼ��
        /// </summary>
        public Vector3 StartPostion { get; private set; }

        public TrackData(int entityId, int typeId, Vector3 startPostion) : base(entityId, typeId)
		{
       
            SpawnTarget = -8.66f;
            HideTarget = -26.4f;
            StartPostion = startPostion;

        }
	}
	
}