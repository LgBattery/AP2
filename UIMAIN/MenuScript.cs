using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    // Variables
    [SerializeField] private float TweenTime = 1f;
    [SerializeField] private AnimationCurve TweenCurve;

    // Buttons
    [SerializeField]
    private Toggle showVehicleHealthBars;
    [SerializeField]
    private Toggle showVehiclePartNames;
    [SerializeField]
    private Slider healthBarSize;

    // Toggles
    private bool paused = false;

    // References
    [SerializeField]
    private Transform menu;

    [SerializeField]
    private Transform visibilityMenu;
    [SerializeField] private Vector3 visibilityMenuTo;
    [SerializeField] private Vector3 visibilityMenuStart;

    [SerializeField]
    private Transform controlsMenu;
    [SerializeField] private Vector3 controlMenuTo;
    [SerializeField] private Vector3 controlMenuStart;


    // Start is called before the first frame update
    void Start()
    {
        menu.gameObject.SetActive(paused);
        LoadValues();
    }

    // Update is called once per frame
    void Update()
    {
        // Disable cursor lock when you escape to menu and toggles menu on and off
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // In game
            if (paused)
            {
                paused = false;
                Cursor.lockState = CursorLockMode.Locked;
                menu.gameObject.SetActive(paused);
            }
            // In menu
            else
            {
                paused = true;
                Cursor.lockState = CursorLockMode.None;
                menu.gameObject.SetActive(paused);
            }
        }
    }
    void LoadValues()
    {
        showVehicleHealthBars.isOn = PlayerPrefs.GetInt("ShowHealthBars") == 0 ? false : true;
        showVehiclePartNames.isOn = PlayerPrefs.GetInt("ShowHealthBarText") == 0 ? false : true;
        healthBarSize.value = PlayerPrefs.GetFloat("HealthBarScale");
    }
    // This exists because when the values are being loaded this gets called making it so only the first value gets loaded, kinda unfortunate :( DO NOT REMOVE THE DELAY
    public void UpdateUI()
    {
        Invoke("UIupdate", 0.01f);
    }
    void UIupdate()
    {
        // Toggle showing part health bars
        if (showVehicleHealthBars.isOn)
        {
            PlayerPrefs.SetInt("ShowHealthBars", 1);
        }
        else
        {
            PlayerPrefs.SetInt("ShowHealthBars", 0);
        }
        // Toggle showing part names
        if (showVehiclePartNames.isOn)
        {
            PlayerPrefs.SetInt("ShowHealthBarText", 1);
        }
        else
        {
            PlayerPrefs.SetInt("ShowHealthBarText", 0);
        }
        // Set health bar size
        PlayerPrefs.SetFloat("HealthBarScale", healthBarSize.value);
    }
    public void OpenVisibilityMenu()
    {
        // Set visibility menu active or inactive and tween it into position
        if (visibilityMenu.gameObject.activeInHierarchy)
        {
            LeanTween.moveLocal(visibilityMenu.gameObject, visibilityMenuStart, TweenTime).setOnComplete(setVisibilityMenuActive).setEase(TweenCurve);
        }
        else
        {
            setVisibilityMenuActive();
            LeanTween.moveLocal(visibilityMenu.gameObject, visibilityMenuTo, TweenTime).setEase(TweenCurve);
        }
        
    }
    public void OpenControlMenu()
    {

    }
    void setVisibilityMenuActive()
    {
        visibilityMenu.gameObject.SetActive(visibilityMenu.gameObject.activeInHierarchy == true ? false : true);
    }
}
