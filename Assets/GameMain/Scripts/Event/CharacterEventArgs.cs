using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EndlessRunner
{
    /// <summary>
    /// CharacterEventArgs����¼�
    /// </summary>
    public class CharacterEventArgs : GameEventArgs
	{

        public static readonly int EventId = typeof(CharacterEventArgs).GetHashCode();

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