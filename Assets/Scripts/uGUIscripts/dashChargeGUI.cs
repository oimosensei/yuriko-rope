using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dashChargeGUI : MonoBehaviour
{
    private Image dashChargeImage;
    private Image dashChargeBack;
    private player player;
    private Text timerText;

    private float time = 0f;
    void Start()
    {
        dashChargeImage = transform.Find("fadeImage").GetComponent<Image>();
        dashChargeBack = transform.Find("backImage").GetComponent<Image>();
        timerText = transform.Find("timerText").GetComponent<Text>();
        timerText.gameObject.SetActive(false);
        player = GameObject.Find("player").GetComponent<player>();
    }

    void Update()
    {
        time = player.dashCooldownTime;
        if (time > 0)
        {
            if (time <= player.dashCooldownDuration)
            {
                timerText.gameObject.SetActive(true);
                dashChargeBack.gameObject.SetActive(true);
                dashChargeImage.gameObject.SetActive(true);
                float leftTIme = player.dashCooldownDuration - time;
                dashChargeImage.fillAmount = leftTIme / player.dashCooldownDuration;
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
            dashChargeImage.fillAmount = 0;
            timerText.gameObject.SetActive(false);
            dashChargeBack.gameObject.SetActive(false);
            dashChargeImage.gameObject.SetActive(false);
        }
    }
}
