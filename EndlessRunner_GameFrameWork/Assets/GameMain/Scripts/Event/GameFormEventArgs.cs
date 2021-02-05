using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EndlessRunner
{
    /// <summary>
    /// 游戏界面相关事件
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

        //数据
        public object Data;

    }

    public enum GameFormEnum 
    {
        Init,//初始化

        BloodLoss,//掉血
        Coin,//金币
        FishBone, // 鱼骨

    }
	
}