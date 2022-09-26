using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ropeChargeGUI : MonoBehaviour
{
    private Image ropeChargeImage;
    private Image ropeChargeBack;
    private player player;
    private Text timerText;

    private float time = 0f;
    void Start()
    {
        ropeChargeImage = transform.Find("fadeImage").GetComponent<Image>();
        ropeChargeBack = transform.Find("backImage").GetComponent<Image>();
        timerText = transform.Find("timerText").GetComponent<Text>();
        timerText.gameObject.SetActive(false);
        player = GameObject.Find("player").GetComponent<player>();
    }

    void Update()
    {
        time = player.connectedCooldownTime;
        if (time > 0)
        {
            if (time <= player.connectedCooldownDuration)
            {
                timerText.gameObject.SetActive(true);
                ropeChargeBack.gameObject.SetActive(true);
                ropeChargeImage.gameObject.SetActive(true);
                float leftTIme = player.connectedCooldownDuration - time;
                ropeChargeImage.fillAmount = leftTIme / player.connectedCooldownDuration;
                timerText.gameObject.SetActive(true);
                if (time < 1)
                {
                    timerText.text = ((float)Mathf.FloorToInt(time * 10) / 10).ToString();
                }
                else
                {
                    timerText.text = Mathf.FloorToInt(time).ToString();
                }
                
            }
        }
        else
        {
            ropeChargeImage.fillAmount = 0;
            timerText.gameObject.SetActive(false);
            ropeChargeBack.gameObject.SetActive(false);
            ropeChargeImage.gameObject.SetActive(false);
        }
    }
}
