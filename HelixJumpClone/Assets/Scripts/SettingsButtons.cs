using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButtons : MonoBehaviour
{
    [SerializeField] private Text _muteButtonText;
    public void Quit()
    {
        Application.Quit();
    }

    public void Store()
    {

    }

    public void ToggleMute()
    {
        if (GameManager.Instance.mute == false)
        {
            GameManager.Instance.mute = true;
            _muteButtonText.text = "/";
        }
        else
        {
            GameManager.Instance.mute = false;
            _muteButtonText.text = "";
        }

    }
}
