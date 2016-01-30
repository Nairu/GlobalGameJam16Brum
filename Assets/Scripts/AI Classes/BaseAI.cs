using UnityEngine;
using System.Collections;

public class BaseAI : MonoBehaviour {

    private int _health;
    public int Health
    {
        get { return Health; }
    }

    public BaseJob myJob;
    public Tile myCareer;
    public BaseAI myEnemy;

    public MapManager myMap;

    private int targetX = 0;
    private int targetY = 0;

    private int _damage;
    public int Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    float baseSpeed = 1f;
    float speed = 1f;

    bool move = false;

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

    void OnMouseDown()
    {
        Camera.main.GetComponent<CameraMoveController>().currentWorker = this;
    }

    void Update()
    {
        if (move)
        {
            if (transform.position.y != targetY && transform.position.x != targetX)
            {

            }
        }
    }

    void FaceToMovement(Vector3 dir)
    {
        if (dir.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if(dir.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private Tile GetClosestLadder()
    {
        Tile currentTile = myMap.GetTileAt(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));
        int dir = currentTile.IsLadderReachable();

        if (dir == -1)
            return null;
        else
        {
            int checkX = (int)currentTile.Pos.x + dir;
            Tile examine = currentTile;
            while (examine.isWalkable && !examine.AllowsVerticalMove)
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
        targetX = x;

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
    }

    public void MoveToFloor(float y)
    {
        Tile ladder = GetClosestLadder();
        if (transform.position.x == (ladder.Pos.x + 1.5f))
        {
            //we are lined up with the stairs
            Vector3 dir = new Vector3(0, y - transform.position.y, 0);
            dir = Vector3.ClampMagnitude(dir, speed * Time.deltaTime);
            FaceToMovement(dir);
            transform.Translate(dir);
        }
        else
        {
            MoveToX(ladder.Pos.x);
        }
    }
}
