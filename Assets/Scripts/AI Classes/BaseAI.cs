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
    protected float speed = 1f;

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

    protected void FaceToMovement(Vector3 dir)
    {
        if (dir.x > 0)
        {
            transform.localScale = new Vector3(-0.3f, 0.3f, 0.3f);
        }
        else if (dir.x < 0)
        {            
            transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
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

    public void MoveToX(float x)
    {
        Vector3 dir = Vector3.zero;
        if (transform.position.x > x)
        {
            dir = Vector3.left * speed * Time.deltaTime;
            dir = Vector3.ClampMagnitude(dir, Mathf.Abs(x - transform.position.x));
        }
        else if (transform.position.x < x)
        {
            dir = Vector3.right * speed * Time.deltaTime;
            dir = Vector3.ClampMagnitude(dir, Mathf.Abs(x - transform.position.x));
        }

        FaceToMovement(dir);
        transform.Translate(dir);
    }

    public void MoveToFloor(float y)
    {
        if (transform.position.x == 1.5f)
        {
            Vector3 dir = Vector3.zero;
            //we are lined up with the stairs
            if (transform.position.y > y)
            {
                dir = Vector3.down * speed * Time.deltaTime;
                dir = Vector3.ClampMagnitude(dir, Mathf.Abs(y - transform.position.y));
            }
            else if (transform.position.y < y)
            {
                dir = Vector3.up * speed * Time.deltaTime;
                dir = Vector3.ClampMagnitude(dir, Mathf.Abs(y - transform.position.y));
            }

            FaceToMovement(dir);
            transform.Translate(dir);
        }
        else
        {
            MoveToX(1.5f);
        }         
    }
}
