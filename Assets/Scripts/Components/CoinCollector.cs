using UnityEngine;
using TMPro;

public class CoinCollector : MonoBehaviour
{
    public TextMeshProUGUI scoreTmp;
    
    private SoundManager soundManager;
    private int score = 0;
    private int coinScore = 50;

    void Start()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            score += coinScore;
            soundManager.PlayCoinSound();
            Destroy(collision.gameObject);
            scoreTmp.text = string.Format("{0}", score);
        }
    }
}
