using UnityEngine;
using TMPro;

public class CoinCollector : MonoBehaviour
{
    public TextMeshProUGUI scoreTmp;

    private int score = 0;
    private int coinScore = 50;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            score += coinScore;
            Destroy(collision.gameObject);
            scoreTmp.text = string.Format("Score: {0}", score);
        }
    }
}
