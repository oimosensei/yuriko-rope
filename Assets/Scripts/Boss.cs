using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public GameObject Enemy2Prefab;
    public GameObject DieEffect;
    public float OffsetX;
    public float RotationSpeed = 2f;
    public float EnemyGenerateInterval = .5f;
    public float hp = 10f;

    private player Player;
    private float freqY = 2;//縦の振動の周波数
    private float freqX = 0.5f;//横の振動の周波数
    private float time = 0;
    private float flashCountor = 0f;
    private bool damageFlash = false;
    private bool isDied = false;
    private float enemyGenerateTimer = 0f;
    private bool isStart = false;

    private SpriteRenderer sp;
    private StageCtrl sc;

    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        sc = GameObject.Find("StageCtrl").GetComponent<StageCtrl>();
        Player = GameObject.Find("player").GetComponent<player>();
    }
    private void Update()
    {
        if (Player.transform.position.x + OffsetX >= transform.position.x) isStart = true;
        if (!sc.pause && isStart)
        {
            time += Time.deltaTime;
            enemyGenerateTimer += Time.deltaTime;
            if (enemyGenerateTimer > EnemyGenerateInterval)
            {
                enemyGenerateTimer = 0f;
                GenerateEnemy();
            }
            transform.Rotate(new Vector3(0, 0, RotationSpeed));
            transform.position = new Vector2(Player.transform.position.x + OffsetX + 2 * Mathf.Sin(freqX * time * 2 * Mathf.PI), Mathf.Sin(freqY * time * 2 * Mathf.PI));
            if (damageFlash) DamageFlash();
            if (isDied) Died();
        }
    }

    private void Died()
    {
        GameObject effect = Instantiate(DieEffect, transform.position, Quaternion.identity);
        effect.transform.localScale = new Vector3(1, 1, effect.transform.localScale.z);
        Destroy(gameObject);
    }

    private void DamageFlash()
    {
        float _flashAlpha = Mathf.Sin(Time.time * 100) / 2 + 0.5f;

        // 透明度を適用する
        Color _color = sp.color;
        _color.a = _flashAlpha;
        sp.color = _color;

        flashCountor++;

        if (flashCountor > 60)
        {
            flashCountor = 0f;
            sp.color = new Color(255, 255, 255, 255);
            damageFlash = false;
        }
    }

    public void Damaged()
    {
        hp -= 2f;
        if (hp <= 0)
        {
            isDied = true;
            return;
        }
        damageFlash = true;
        Debug.Log("damaged");
    }
    private void GenerateEnemy()
    {
        float random = Random.Range(-3.5f, 3.5f);
        GameObject enemy = Instantiate(Random.Range(0, 2) == 0 ? EnemyPrefab : Enemy2Prefab, new Vector3(-4f + Player.transform.position.x + OffsetX, random, 0), Quaternion.identity);//敵の出現位置はsinに影響しない
        enemy.transform.parent = transform.parent;
    }
}
