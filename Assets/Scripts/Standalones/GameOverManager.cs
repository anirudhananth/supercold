using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    public void Update() {
        string t = StaticData.gameOverText;
        text.text = t;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Level 1 New");
    }
}
