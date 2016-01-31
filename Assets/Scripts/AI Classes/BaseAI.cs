using UnityEngine;
using System.Collections;

public class BaseAI : MonoBehaviour
{

    private int _health;
    public int Health
    {
        get { return Health; }
    }

    public BaseJob myJob;
    public Tile myCareer;
    public BaseAI myEnemy;

    public MapManager myMap;

    private float targetX = 0;
    private float endX = 0;
    private float targetY = 0;
    private float endY = 0;

    private int _damage;
    public int Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    float baseSpeed = 1f;
    float speed = 1f;

    bool move = false;
    bool initEndPos = false;

    public void DealDamage(int damageToTake)
    {
        _health -= damageToTake;
        if (_health < 0)
            Die();
    }

    public void HealDamage(int amountToHeal)
    {
        _health += amountToHeal;
    }

    protected virtual void Attack()
    {
        myEnemy.DealDamage(Damage);
    }

    void Start()
    {
        myMap = GameObject.Find("Map").GetComponent<MapManager>();
    }

    protected virtual void Die()
    {
        //lower the number of cultists

        if (myCareer != null)
        {

        }
        if (myJob != null)
        {
            myJob.Done();
            myJob = null;
        }
    }

    void FaceToMovement(Vector3 dir)
    {
        if (dir.x > 0)
        {
            Vector3 lScale = transform.localScale;
            if (lScale.x > 0)
                lScale.x = -lScale.x;
            
            transform.localScale = lScale;
        }
        else if (dir.x < 0)
        {
            Vector3 lScale = transform.localScale;
            transform.localScale = lScale;
        }
    }

    private Tile GetClosestLadder()
    {
        Tile currentTile = myMap.GetTileAt((int)Mathf.Round(transform.position.x / 3) * 3, (int)Mathf.Round(transform.position.y / 2) * 2);
        if (currentTile.TileType == Enumerations.GetEnumDescription(TileTypes.Tunnel)
            || currentTile.TileType == Enumerations.GetEnumDescription(TileTypes.TunnelStart))
            return currentTile;

        int dir = currentTile.IsLadderReachable();

        if (dir == -1)
            return null;
        else
        {
            int checkX = (int)currentTile.Pos.x + dir;
            Tile examine = currentTile;
            while (examine.isWalkable)
            {
                if (!examine.isWalkable)
                    return null;

                if (examine.AllowsVerticalMove)
                    return examine;

                checkX += dir;
                examine = myMap.GetTileAt(checkX, (int)examine.Pos.y);
            }
        }

        return null;
    }

    public IEnumerator MoveToX(float x)
    {
        while (transform.position.x != x)
        {
            Vector3 dir = Vector3.zero;
            if (transform.position.x > targetX)
            {
                dir = Vector3.left * speed * Time.deltaTime;
                dir = Vector3.ClampMagnitude(dir, Mathf.Abs(targetX - transform.position.x));
            }
            else if (transform.position.x < targetX)
            {
                dir = Vector3.right * speed * Time.deltaTime;
                dir = Vector3.ClampMagnitude(dir, Mathf.Abs(targetX - transform.position.x));
            }

            FaceToMovement(dir);
            transform.Translate(dir);
            yield return null;
        }
    }

    public IEnumerator MoveToFloor(float y)
    {
        while (transform.position.y != y)
        {
            //we are lined up with the stairs
            Vector3 dir = new Vector3(0, y - transform.position.y, 0);
            dir = Vector3.ClampMagnitude(dir, speed * Time.deltaTime);

            FaceToMovement(dir);
            transform.Translate(dir, Space.Self);
            yield return null;
        }
    }
}
