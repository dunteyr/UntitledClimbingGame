using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadsUpDisplay : MonoBehaviour
{
    public Slider healthBar;
    private PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponentInChildren<Slider>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();


    }

    public void SetHealthBar(float healthValue)
    {
        healthBar.minValue = 0;
        healthBar.maxValue = playerHealth.playerMaxHealth;
        healthBar.value = healthValue;
    }
}
