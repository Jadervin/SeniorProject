using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private TextMeshProUGUI currentHealthText;
    [SerializeField] private TextMeshProUGUI maxHealthText;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth.OnPlayerHealthChanged += PlayerHealth_OnPlayerHealthChanged;

        playerHealth.OnPlayerMaxHealthChanged += PlayerHealth_OnPlayerMaxHealthChanged;

        currentHealthText.text = playerHealth.GetCurrentHealth().ToString();
        maxHealthText.text = "/ " + playerHealth.GetMaxHealth().ToString();
    }

    private void PlayerHealth_OnPlayerMaxHealthChanged(object sender, System.EventArgs e)
    {
        maxHealthText.text = "/ " + playerHealth.GetMaxHealth().ToString();
        currentHealthText.text = playerHealth.GetCurrentHealth().ToString();
    }

    private void PlayerHealth_OnPlayerHealthChanged(object sender, System.EventArgs e)
    {
        currentHealthText.text = playerHealth.GetCurrentHealth().ToString();
    }

    /*
    void Update()
    {
        
    }*/
}
