using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class linerender : MonoBehaviour
{
    public player p;

    LineRenderer line;
   
    void Start()
    {
        //コンポーネントを取得する
        this.line = GetComponent<LineRenderer>();

        //線の幅を決める
        this.line.startWidth = 0.1f;
        this.line.endWidth = 0.1f;

        //頂点の数を決める
        this.line.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        //Updateに書いたのは作者が動的に変化させたかったため
        //0や1は頂点の順番(多分)
        float x = 0.3f * Mathf.Cos(Mathf.Atan2(p.startPos.y - p.transform.position.y, p.startPos.x - p.transform.position.x)) + p.transform.position.x;
        float y = (p.startPos.y - p.transform.position.y) / (p.startPos.x - p.transform.position.x) * (x - p.transform.position.x) + p.transform.position.y;
        line.SetPosition(0, new Vector3(x, y, 10));
        line.SetPosition(1, new Vector3(p.startPos.x, p.startPos.y, 10));
        //line.sortingLayerName = "temae";
    }
}
