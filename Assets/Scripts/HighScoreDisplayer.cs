using UnityEngine;
using TMPro;
using System.Globalization;

public class HighScoreDisplayer : MonoBehaviour
{
    public TMP_Text highScoreText;

    void Start()
    {
        highScoreText.text = PlayerPrefs.GetFloat("HighScore").ToString("C", CultureInfo.CurrentCulture);
    }
}
