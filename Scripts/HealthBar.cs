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
    

    private void Start()
    {
        healthBar = GetComponent<Slider>();
        partTransform = part.part;
        transform.parent.GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        healthBar.value = Mathf.Lerp(healthBar.value, (float)part.health / (float)part.baseHealth, Time.deltaTime * 1);
        transform.LookAt(Camera.main.transform.position);
        if (PlayerPrefs.GetInt("ShowHealthBars") < 1)
        {
            gameObject.SetActive(false);
        }
    }
}
