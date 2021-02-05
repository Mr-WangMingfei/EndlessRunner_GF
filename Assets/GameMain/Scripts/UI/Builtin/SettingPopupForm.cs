using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace EndlessRunner
{
	public class SettingPopupForm : UGuiForm
	{
        public void OnCloseButtonClick()
        {
            Close();
        }

        public void OnMusicSettingValueChange(float value)
        {
            GameEntry.Sound.SetVolume("Music", value);
        }

        public void OnSoundSettingValueChange(float value)
        {
            GameEntry.Sound.SetVolume("Sound", value);
            GameEntry.Sound.SetVolume("UISound", value);
        }


    }

}