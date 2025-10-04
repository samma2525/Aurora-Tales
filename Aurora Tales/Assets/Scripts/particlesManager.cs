using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager2D : MonoBehaviour
{
    [Header("References")]
    public GameObject[] particlePrefabs; // 0 = useful, 1 & 2 = harmful
    public Transform sun;
    public Transform earth;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI resultText;

    [Header("Game Settings")]
    public int maxScore = 30;
    public float timeLimit = 30f;
    public int totalParticles = 40;
    public float spawnInterval = 0.5f;
    public float speed = 5f;

    private int score = 0;
    private float timer;
    private bool gameEnded = false;
    private int usefulHits = 0;

    void Start()
    {
        timer = timeLimit;
        resultText.text = "";
        UpdateUI();
        StartCoroutine(SpawnParticles());
    }

    void Update()
    {
        if (gameEnded) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 0;
            EndGame(score >= maxScore, "Time's up!");
        }

        UpdateUI();
    }

    IEnumerator SpawnParticles()
    {
        for (int i = 0; i < totalParticles; i++)
        {
            GameObject prefab = particlePrefabs[Random.Range(0, particlePrefabs.Length)];
            GameObject p = Instantiate(prefab, sun.position, Quaternion.identity);

            ParticleMover2D mover = p.AddComponent<ParticleMover2D>();
            mover.target = earth;
            mover.speed = speed;

            // Determine type
            mover.particleType = (prefab == particlePrefabs[0]) ? 
                ParticleMover2D.ParticleType.Useful : ParticleMover2D.ParticleType.Harmful;

            // When particle reaches Earth
            mover.onReachedEarth = () =>
            {
                if (mover.particleType == ParticleMover2D.ParticleType.Useful)
                    score++; // Useful reaches Earth → reward
                else
                    score = Mathf.Max(score - 1, 0); // Harmful hits Earth → penalty
            };

            // When particle is clicked
            mover.onClicked = () =>
            {
                if (mover.particleType == ParticleMover2D.ParticleType.Useful)
                {
                    usefulHits++;
                    score = Mathf.Max(score - 1, 0); // penalty
                    if (usefulHits > 5)
                        EndGame(false, "Too many useful particles clicked!");
                }
                else
                    score++; // Clicking harmful → reward
            };

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void EndGame(bool won, string message = "")
    {
        if (gameEnded) return;

        gameEnded = true;
        resultText.text = message != "" ? message : (won ? "You Win!" : "You Lose!");
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        timerText.text = "Time: " + Mathf.CeilToInt(timer);
    }
}

// -------------------------------
// 2D Particle Movement (Straight Line Only)
// -------------------------------
public class ParticleMover2D : MonoBehaviour
{
    public enum ParticleType { Useful, Harmful }
    public ParticleType particleType;

    public Transform target;
    public float speed;

    public System.Action onClicked;
    public System.Action onReachedEarth;

    void Update()
    {
        if (target == null) return;

        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            onReachedEarth?.Invoke();
            Destroy(gameObject);
        }
    }

    void OnMouseDown()
    {
        onClicked?.Invoke();
        Destroy(gameObject);
    }
}