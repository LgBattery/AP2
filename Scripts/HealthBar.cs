using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Part part;
    private Slider healthBar;
    public Transform partTransform;
    public GameObject healthSlider;
    [SerializeField] private Text text;
    

    private void Start()
    {
        healthBar = healthSlider.GetComponent<Slider>();
        partTransform = part.part;
        transform.GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
        if (partTransform)
        {
            text.text = partTransform.name;
        }
        else
        {
            text.text = "";
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        healthBar.value = Mathf.Lerp(healthBar.value, (float)part.health / (float)part.baseHealth, Time.deltaTime * 1);

        // Look twoards camera
        transform.LookAt(Camera.main.transform.position);
        transform.Rotate(0, 180, 0);
        if (PlayerPrefs.GetInt("ShowHealthBars") == 0)
        {
            healthSlider.SetActive(false);
        }
        else
        {
            healthSlider.SetActive(true);
        }

        if (PlayerPrefs.GetInt("ShowHealthBarText") == 0)
        {
            text.gameObject.SetActive(false);
        }
        else
        {
            text.gameObject.SetActive(true);
        }

        // Set scale
        transform.localScale = new Vector3(PlayerPrefs.GetFloat("HealthBarScale"), PlayerPrefs.GetFloat("HealthBarScale"), PlayerPrefs.GetFloat("HealthBarScale"));
    }
}
