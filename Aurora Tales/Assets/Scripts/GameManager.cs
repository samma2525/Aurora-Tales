using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public string nextSceneName = "Particels"; 
    
    // Set the time delay (in seconds) in the Inspector
    public float delayDuration = 5f; 

    void Start()
    {
        // Start the coroutine to wait and then load the scene
        StartCoroutine(LoadNextSceneAfterDelay(delayDuration));
    }

    IEnumerator LoadNextSceneAfterDelay(float delay)
    {
        // Wait for the specified amount of time
        yield return new WaitForSeconds(delay);

        // Load the next scene
        LoadSpecificScene(nextSceneName);
    }
    
    // A reusable function to handle the actual loading
    public void LoadSpecificScene(string sceneName)
    {
        // Check if the scene name is valid (optional but good practice)
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene " + sceneName + " not found or not in Build Settings!");
        }
    }
}
