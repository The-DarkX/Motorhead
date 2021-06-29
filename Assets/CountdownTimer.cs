using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public TMP_Text countdownText;

    float countdown;

    GameManager manager;

	private void Start()
	{
        manager = GameManager.instance;
    }

    public void TriggerCountdown(float countdownTime) 
    {
        countdown = countdownTime;

        StartCoroutine(CountdownToStart());

        manager.BeginGame();
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
