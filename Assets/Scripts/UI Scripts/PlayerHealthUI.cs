using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image barImage;
    [SerializeField] private TextMeshProUGUI currentHealthText;
    [SerializeField] private TextMeshProUGUI maxHealthText;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth.OnPlayerHealthChanged += PlayerHealth_OnPlayerHealthChanged;

        playerHealth.OnPlayerMaxHealthChanged += PlayerHealth_OnPlayerMaxHealthChanged;

        currentHealthText.text = playerHealth.GetCurrentHealth().ToString();
        maxHealthText.text = "/ " + playerHealth.GetMaxHealth().ToString();
        barImage.fillAmount = (float)playerHealth.GetCurrentHealth() / playerHealth.GetMaxHealth();
    }

    private void PlayerHealth_OnPlayerMaxHealthChanged(object sender, System.EventArgs e)
    {
        maxHealthText.text = "/ " + playerHealth.GetMaxHealth().ToString();
        currentHealthText.text = playerHealth.GetCurrentHealth().ToString();

        barImage.fillAmount = playerHealth.GetCurrentHealth() / playerHealth.GetMaxHealth();
    }

    private void PlayerHealth_OnPlayerHealthChanged(object sender, System.EventArgs e)
    {
        currentHealthText.text = playerHealth.GetCurrentHealth().ToString();

        barImage.fillAmount = (float) playerHealth.GetCurrentHealth() / playerHealth.GetMaxHealth();
    }

    /*
    void Update()
    {
        
    }*/
}
