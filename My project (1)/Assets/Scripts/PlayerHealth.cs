using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int maxLives = 3;
    public int currentLives;

    // Update is called once per frame
    void Start()
    {
        currentLives = maxLives;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Missile"))
        {
            currentLives--;
            Destroy(other.gameObject);
            if (currentLives <= 0)
            {
                GameOver();
            }

        }
    }

    void GameOver()
    {
        gameObject.SetActive(false);
        Invoke("RestarGame", 3.0f);
    }

    void RestarGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
