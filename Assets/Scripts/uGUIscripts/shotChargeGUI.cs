using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shotChargeGUI : MonoBehaviour
{
    private Image shotChargeImage;
    private Image shotChargeBack;
    private player player;
    private Text timerText;

    private float time = 0f;
    void Start()
    {
        shotChargeImage = transform.Find("fadeImage").GetComponent<Image>();
        shotChargeBack = transform.Find("backImage").GetComponent<Image>();
        timerText = transform.Find("timerText").GetComponent<Text>();
        timerText.gameObject.SetActive(false);
        player = GameObject.Find("player").GetComponent<player>();
    }

    void Update()
    {
        time = player.shotCooldownTime;
        if (time > 0)
        {
            if (time <= player.shotCooldownDuration)
            {
                timerText.gameObject.SetActive(true);
                shotChargeBack.gameObject.SetActive(true);
                shotChargeImage.gameObject.SetActive(true);
                float leftTIme = player.shotCooldownDuration - time;
                shotChargeImage.fillAmount = leftTIme / player.shotCooldownDuration;
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
            shotChargeImage.fillAmount = 0;
            timerText.gameObject.SetActive(false);
            shotChargeBack.gameObject.SetActive(false);
            shotChargeImage.gameObject.SetActive(false);
        }
    }
}
