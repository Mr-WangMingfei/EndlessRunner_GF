using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EndlessRunner
{
	/// <summary>
	/// 倒计时事件
	/// </summary>
	public class CountDownEventArgs : GameEventArgs
    {

        public static readonly int EventId = typeof(CountDownEventArgs).GetHashCode();

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

}