using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public int damageToPlayer;
    public int moneyOnDeath;
    public float moveSpeed;

    // Path
    private Transform[] path;
    private int curPathWaypoint;

    void Start()
    {
        path = GameManager.instance.enemyPath.waypoints;
    }

    void Update()
    {
        MoveAlongPath();
    }

    void MoveAlongPath()
    {
        if (curPathWaypoint < path.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position, path[curPathWaypoint].position, moveSpeed * Time.deltaTime);

            if (transform.position == path[curPathWaypoint].position)
            {
                curPathWaypoint++;
            }

            else
            {
                GameManager.instance.TakeDamage(damageToPlayer);
                GameManager.instance.onEnemyDestroyed.Invoke();
                Destroy(gameObject);
            }
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            GameManager.instance.AddMoney(moneyOnDeath);
            GameManager.instance.onEnemyDestroyed.Invoke();
            Destroy(gameObject);
        }
    }
}
