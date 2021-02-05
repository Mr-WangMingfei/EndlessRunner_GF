using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EndlessRunner
{
	public class TrackData : EntityData
	{


        /// <summary>
        /// 到达此目标时产生新的背景实体
        /// </summary>
        public float SpawnTarget { get; private set; }

        /// <summary>
        /// 到达此目标时隐藏自身
        /// </summary>
        public float HideTarget { get; private set; }

        /// <summary>
        /// 移动起始点
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