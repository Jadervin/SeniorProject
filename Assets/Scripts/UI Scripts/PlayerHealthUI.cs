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
        playerHealth.OnPlayerHit += PlayerHealth_OnPlayerHit;

        currentHealthText.text = playerHealth.GetCurrentHealth().ToString();
        maxHealthText.text = "/ " + playerHealth.GetMaxHealth().ToString();
    }

    private void PlayerHealth_OnPlayerHit(object sender, System.EventArgs e)
    {
        currentHealthText.text = playerHealth.GetCurrentHealth().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
