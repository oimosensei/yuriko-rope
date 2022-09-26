using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//背景のスクロールをコントロールする
public class BackgroundController : MonoBehaviour
{
    public float m_speedX = 0.02f;
    public float m_speedY = 0f;

    private GameObject cameraObject;
    private Vector3 previousPos;
    private Material material;

    private void Start()
    {
        cameraObject = GameObject.Find("Main Camera");
        previousPos = cameraObject.transform.position;
        transform.position = new Vector3(previousPos.x, previousPos.y, transform.position.z);
        material = GetComponent<SpriteRenderer>().material;

    }

    private void LateUpdate()
    {
        Vector2 amountOfMovement = cameraObject.transform.position - previousPos;
        previousPos = cameraObject.transform.position;
        transform.position += (Vector3)amountOfMovement;
        // 背 景 の テ ク ス チ ャ を ス ク ロ ー ル す る
        material.mainTextureOffset += new Vector2(m_speedX, m_speedY) * amountOfMovement;
    }

    public void TextureOffsetAdjust(Vector2 change)
    {
        material.mainTextureOffset += change * new Vector2(m_speedX, m_speedY);
    }

}
