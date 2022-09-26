using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;
using KanKikuchi.AudioManager;

public class FunctionTile : MonoBehaviour
{

    public Sprite onlyRight;
    public Sprite onlyLeft;
    public Sprite onlyUp;
    public Sprite onlyDown;
    public Sprite reverseObject;
    public TileBase[] springAnimations;

    private bool reverseFinished = false;
    private Tilemap map;
    private TilemapCollider2D tileCol;

    private enum direction
    {
        None, Right, Left, Up, Down
    };

    private void Start()
    {
        map = gameObject.GetComponent<Tilemap>();
        tileCol = gameObject.GetComponent<TilemapCollider2D>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name.Contains("player"))
        {
            //Vector3 hitPos = Vector3.zero;
            //foreach (ContactPoint2D point in contacts)
            //{
            //    hitPos = point.point;
            //}
            Vector2 playerMax = collision.GetComponent<Collider2D>().bounds.max;
            Vector2 playerMin = collision.GetComponent<Collider2D>().bounds.min;

            //BoundsInt.PositionEnumerator position = gameObject.GetComponent<Tilemap>().cellBounds.allPositionsWithin;
            Vector2 gridSize = transform.parent.GetComponent<Grid>().cellSize;

            Vector2 gridPosition = transform.parent.transform.position;

            var allPosition = new List<Vector3>();

            //foreach (var variable in position)
            //{
            //    if (gameObject.GetComponent<Tilemap>().GetTile(variable) != null)
            //    {
            //        allPosition.Add(variable);
            //    }
            //}
            player player = collision.GetComponent<player>();

            int x = Mathf.FloorToInt(playerMin.x) - 1;
            while (x < Mathf.CeilToInt(playerMax.x) + 1)
            {
                int y = Mathf.FloorToInt(playerMin.y) - 1;
                while (y < Mathf.CeilToInt(playerMax.y) + 1)
                {
                    allPosition.Add(new Vector3(x - gridPosition.x, y - gridPosition.y, 0));
                    y++;
                }
                x++;
            }

            for (int i = 0; i < allPosition.Count; i++)
            {
                Vector3Int localTilePositon = Vector3Int.RoundToInt(allPosition[i]);
                if (gameObject.GetComponent<Tilemap>().GetTile(localTilePositon) != null)
                {
                    Vector2 tilePosition = (Vector2Int)localTilePositon + gridPosition;
                    Vector2 tileMax = tilePosition + gridSize;
                    Vector2 tileMin = tilePosition;
                    if (collisionAABB(tileMin, tileMax, playerMin, playerMax))//playerと接触しているか
                    {
                        Sprite tileSprite = GetComponent<Tilemap>().GetSprite(localTilePositon);
                        direction tileDirection = direction.None;
                        if (tileSprite == onlyRight) tileDirection = direction.Right;
                        if (tileSprite == onlyLeft) tileDirection = direction.Left;
                        if (tileSprite == onlyUp) tileDirection = direction.Up;
                        if (tileSprite == onlyDown) tileDirection = direction.Down;
                        if (tileDirection != direction.None)
                        {
                            //指定された方向に移動しているか
                            if ((tileDirection == direction.Right && player.velocity.x > 0) ||
                                (tileDirection == direction.Left && player.velocity.x <= 0) ||
                                (tileDirection == direction.Up && player.velocity.y > 0) ||
                                (tileDirection == direction.Down && player.velocity.y <= 0))
                            {
                                //SetNewTile(localTilePositon, null);  //当たったら消える処理今はオフ
                            }
                            else
                            {
                                player.Dead();
                            }

                        }
                        if (tileSprite == reverseObject && !reverseFinished)
                        {
                            player.ReverseDirection();
                            reverseFinished = true;
                            DOVirtual.DelayedCall(
                                    0.2f,   // 遅延させる（待機する）時間
                                    () =>
                                    {
                                        reverseFinished = false;
                                    }
                                );//連続で反転判定されるのを防ぐ
                            IEnumerator enumerator = springAnimation(localTilePositon);
                            StartCoroutine(enumerator);
                            SEManager.Instance.Play(SEPath.SPRING_SOUND, volumeRate: 0.3f);
                        }
                    }
                }
            }




            //TileBase tiletmp = gameObject.GetComponent<Tilemap>().GetTile(finalPosition);

            //if (tiletmp != null)
            //{
            //    Sprite tilemapSprite = gameObject.GetComponent<Tilemap>().GetSprite(finalPosition);

            //    Tilemap map = gameObject.GetComponent<Tilemap>();
            //    TilemapCollider2D tileCol = gameObject.GetComponent<TilemapCollider2D>();

            //    map.SetTile(finalPosition, null);
            //    tileCol.enabled = false;
            //    tileCol.enabled = true;
            //}
        }
    }

    private bool collisionAABB(Vector2 aMin, Vector2 aMax, Vector2 bMin, Vector2 bMax)
    {
        return ((aMax.x > bMin.x) &&
                (aMin.x < bMax.x) &&
                (aMax.y > bMin.y) &&
                (aMin.y < bMax.y));

    }

    private bool oneWayTile(direction tileDirection, player player)
    {   //指定された方向に移動しているか
        if ((tileDirection == direction.Right && player.velocity.x > 0) && (tileDirection == direction.Left && player.velocity.x <= 0) && (tileDirection == direction.Up && player.velocity.y > 0) && (tileDirection == direction.Down && player.velocity.y <= 0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator springAnimation(Vector3Int tilePos)
    {

        TileBase originalTile = map.GetTile(tilePos);
        for (int i = 0; i < springAnimations.Length; i++)
        {

            SetNewTile(tilePos, springAnimations[i]);

            yield return new WaitForSeconds(0.3f);
        }

        SetNewTile(tilePos, originalTile);
        yield break;
    }

    private void SetNewTile(Vector3Int newTilePos, TileBase newTile)
    {
        map.SetTile(newTilePos, newTile);
        tileCol.enabled = false;
        tileCol.enabled = true;
    }

}

