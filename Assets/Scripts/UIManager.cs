using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;

    [SerializeField] private Image liveImage;

    [SerializeField] private Sprite[] liveSprites;

    [SerializeField] private Text gameOverText;

    [SerializeField] private Text restartGameText;

    [SerializeField] private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameOverText.gameObject.SetActive(false);
        restartGameText.gameObject.SetActive(false);
        scoreText.text = "Score: " + 0;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int playerScore)
    {
        scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        liveImage.sprite = liveSprites[currentLives];
        if (currentLives == 0) GameOverSequence();
        
    }

    private void GameOverSequence()
    {
        gameOverText.gameObject.SetActive(true);
        restartGameText.gameObject.SetActive(true);
        gameManager.GameOver();
        StartCoroutine(GameOverTextRoutine());
    }

    IEnumerator GameOverTextRoutine()
    {
        while (true)
        {
            gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.2f);
            gameOverText.text = "";
            yield return new WaitForSeconds(0.2f);
        }
    }
}
