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
    private Toggle rotateCameraWithVehicle;
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

    [SerializeField]
    private Transform gameMenu;
    [SerializeField] private Vector3 gameMenuTo;
    [SerializeField] private Vector3 gameMenuStart;


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
        rotateCameraWithVehicle.isOn = PlayerPrefs.GetInt("RotateCameraWithVehicle") == 0 ? false : true;
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
        PlayerPrefs.SetInt("ShowHealthBars", showVehicleHealthBars.isOn ? 1 : 0);
        // Toggle showing part names
        PlayerPrefs.SetInt("ShowHealthBarText", showVehiclePartNames.isOn ? 1 : 0);
        // Toggle rotating camera with vheicle
        PlayerPrefs.SetInt("RotateCameraWithVehicle", rotateCameraWithVehicle.isOn ? 1 : 0);
        // Set health bar size
        PlayerPrefs.SetFloat("HealthBarScale", healthBarSize.value);
    }
    public void OpenVisibilityMenu()
    {
        // Set visibility menu active or inactive and tween it into position
        if (visibilityMenu.gameObject.activeInHierarchy)
        {
            LeanTween.moveLocal(visibilityMenu.gameObject, visibilityMenuStart, TweenTime).setOnComplete(SetVisibilityMenuActive).setEase(TweenCurve);
        }
        else
        {
            SetVisibilityMenuActive();
            LeanTween.moveLocal(visibilityMenu.gameObject, visibilityMenuTo, TweenTime).setEase(TweenCurve);
        }
        
    }
    public void OpenGameMenu()
    {
        // Set game menu active or inactive and tween it into position
        if (gameMenu.gameObject.activeInHierarchy)
        {
            LeanTween.moveLocal(gameMenu.gameObject, gameMenuStart, TweenTime).setOnComplete(SetGameMenuActive).setEase(TweenCurve);
        }
        else
        {
            SetGameMenuActive();
            LeanTween.moveLocal(gameMenu.gameObject, gameMenuTo, TweenTime).setEase(TweenCurve);
        }

    }
    public void OpenControlMenu()
    {

    }
    void SetVisibilityMenuActive()
    {
        visibilityMenu.gameObject.SetActive(visibilityMenu.gameObject.activeInHierarchy == true ? false : true);
    }
    void SetGameMenuActive()
    {
        gameMenu.gameObject.SetActive(gameMenu.gameObject.activeInHierarchy == true ? false : true);
    }
}
