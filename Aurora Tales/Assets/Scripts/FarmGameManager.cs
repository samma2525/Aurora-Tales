using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class FarmGameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI badCounterText;
    public TextMeshProUGUI goodCounterText;
    public TextMeshProUGUI resultText;

    [Header("Game Rules")]
    public float gameTime = 90f;   // 1 minute 30 seconds
    public int minBadToWin = 15;
    public int maxGoodMistakes = 5;

    private float timer;
    private int badRemoved = 0;
    private int goodRemoved = 0;
    private bool gameOver = false;

    private List<GameObject> crops = new List<GameObject>();

    void Start()
    {
        timer = gameTime;
        resultText.gameObject.SetActive(false);

        RegisterCrops(); // <-- collect the ones placed manually
        UpdateUI();
    }

    void Update()
    {
        if (gameOver) return;

        // Timer countdown
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            EndGame(false);
            return;
        }

        UpdateUI();
    }

    void RegisterCrops()
    {
        // Find all crops manually placed in scene by tag
        GameObject[] goodCrops = GameObject.FindGameObjectsWithTag("GoodCrop");
        GameObject[] badCrops = GameObject.FindGameObjectsWithTag("BadCrop");

        foreach (GameObject crop in goodCrops)
        {
            Crop cropScript = crop.GetComponent<Crop>();
            if (cropScript == null) cropScript = crop.AddComponent<Crop>();

            cropScript.onClicked = () =>
            {
                goodRemoved++; // penalty
                if (goodRemoved > maxGoodMistakes)
                    EndGame(false);
            };

            crops.Add(crop);
        }

        foreach (GameObject crop in badCrops)
        {
            Crop cropScript = crop.GetComponent<Crop>();
            if (cropScript == null) cropScript = crop.AddComponent<Crop>();

            cropScript.onClicked = () =>
            {
                badRemoved++; // reward
                if (badRemoved >= minBadToWin)
                    EndGame(true);
            };

            crops.Add(crop);
        }
    }

    void UpdateUI()
    {
        timerText.text = "Time: " + Mathf.Ceil(timer).ToString();
        badCounterText.text = "Bad Removed: " + badRemoved;
        goodCounterText.text = "Good Mistakes: " + goodRemoved;
    }

    void EndGame(bool win)
    {
        gameOver = true;
        resultText.gameObject.SetActive(true);

        if (win)
            resultText.text = "🎉 You Win!";
        else
            resultText.text = "❌ You Lose!";
    }
}

public class Crop : MonoBehaviour
{
    public System.Action onClicked;

    void OnMouseDown()
    {
        Debug.Log("Clicked on " + gameObject.name);
        onClicked?.Invoke();
        Destroy(gameObject); // Optional: remove the crop when clicked
    }
}
