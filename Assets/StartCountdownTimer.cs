using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartCountdownTimer : MonoBehaviour
{
    public TMP_Text countdownText;
    public int countdownTime = 3;

    float countdown;

	private void Start()
	{
        countdown = countdownTime;

        StartCoroutine(CountdownToStart());
    }

    IEnumerator CountdownToStart()
    {
        while (countdown > 0)
        {
            countdownText.text = countdown.ToString();

            yield return new WaitForSeconds(1f);

            countdown--;
        }

        countdownText.text = "GO!";

        GameManager.instance.BeginGame();
        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false);
    }
}
