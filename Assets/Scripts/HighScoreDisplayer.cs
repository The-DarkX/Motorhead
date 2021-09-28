using UnityEngine;
using TMPro;
using System.Globalization;

public class HighScoreDisplayer : MonoBehaviour
{
    public TMP_Text highScoreText;

    void LateUpdate()
    {
        highScoreText.text = PlayerPrefs.GetInt("HighScore").ToString("C0", CultureInfo.CurrentCulture);
    }
}
