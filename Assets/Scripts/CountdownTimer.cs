using System.Collections;
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

        TriggerCountdown(3);
    }

    public void TriggerCountdown(float countdownTime) 
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
        manager.BeginGame();

        yield return new WaitForSeconds(0.5f);

        countdownText.gameObject.SetActive(false);
    }
}
