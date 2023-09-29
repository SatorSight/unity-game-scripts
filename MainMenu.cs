using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // public static bool gamePaused = false;
    // public static bool settingsActive = false;

    // public GameObject escMenuUI;
    // public GameObject escSettingsMenuUI;
    //public GameObject playerObject;
    // public FollowTarget lookPos;
    //private GameObject sliderObj;
    // public Slider slider;

    void Awake()
    {
        //GirlMover girl = (FollowTarget)playerObject.GetComponent(typeof(GirlMover));
        //MainMenuItems.transform.Find("PlayButton").gameObject
        //sliderObj = transform.Find("SensitivitySlider").gameObject;


        // .GetComponent <Slider> ().value;
        //slider = (Slider)sliderObj.GetComponent(typeof(Slider));
        //slider = sliderObj.GetComponent<Slider>();
    }

    public void play()
    {
        Debug.Log("play");
        SceneManager.LoadScene("Scene_AAAA", LoadSceneMode.Single);
    }
    //
    // public void exit()
    // {
    //     
    // }
    //
    // public void pause()
    // {
    //     Debug.Log("pause");
    //     escMenuUI.SetActive(true);
    //     Time.timeScale = 0f;
    //     gamePaused = true;
    // }
    //
    // public void resume()
    // {
    //     Debug.Log("resume");
    //     escMenuUI.SetActive(false);
    //     Time.timeScale = 1f;
    //     gamePaused = false;
    // }
    //
    // public void goToSettings()
    // {
    //     escMenuUI.SetActive(false);
    //     escSettingsMenuUI.SetActive(true);
    // }
    //
    // public void backFromSettings()
    // {
    //     escMenuUI.SetActive(true);
    //     escSettingsMenuUI.SetActive(false);
    // }
    //
    // public void OnSliderValueChanged(float val)
    // {
    //     lookPos.setTurnSpeed(slider.value);
    //     Debug.Log("changing sensitivity");
    //     //Debug.Log(slider.value);
    // }

    public void quit()
    {
        Debug.Log("quit");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }





}
