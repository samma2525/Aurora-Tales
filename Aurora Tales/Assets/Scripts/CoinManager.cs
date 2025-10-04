using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int coinCount;
    public TextMeshProUGUI coinText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        coinText.text = "Coin Count: " + coinCount.ToString();
    }
}
