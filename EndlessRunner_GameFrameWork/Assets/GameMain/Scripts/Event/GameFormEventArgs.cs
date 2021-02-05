using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EndlessRunner
{
    /// <summary>
    /// ��Ϸ��������¼�
    /// </summary>
	public class GameFormEventArgs : GameEventArgs
    {

        public static readonly int EventId = typeof(GameFormEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public override void Clear()
        {

        }
    }

    public class GameFormData
    {

        public GameFormEnum gameFormEnum;

        //����
        public object Data;

    }

    public enum GameFormEnum 
    {
        Init,//��ʼ��

        BloodLoss,//��Ѫ
        Coin,//���
        FishBone, // ���

    }
	
}