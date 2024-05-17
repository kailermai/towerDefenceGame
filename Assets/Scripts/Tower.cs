using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public enum TowerTargetPriority
    {
        First,
        Close,
        Strong
    }

    [Header("Info")]
    public float range;
    private List<Enemy> curEnemiesInRange = new List<Enemy>();
    private Enemy curEnemy;

    public TowerTargetPriority targetPriority;
    public bool rotateTowardsTarget;

    [Header("Attacking")]
    public float attackRate;
    private float lastAttackTime;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPos;

    public int projectileDamage;
    public float projectileSpeed;


    void Update()
    {
        if (Time.time - lastAttackTime > attackRate)
        {
            lastAttackTime = Time.time;

            curEnemy = GetEnemy();

            if (curEnemy != null)
            {
                Attack();
            }
        }
    }

    Enemy GetEnemy()
    {
        curEnemiesInRange.RemoveAll(x => x == null);

        if (curEnemiesInRange.Count == 0)
        {
            return null;
        }

        if (curEnemiesInRange.Count == 1)
        {
            return curEnemiesInRange[0];
        }

        switch (targetPriority)
        {
            case TowerTargetPriority.First:
                {
                    return curEnemiesInRange[0];
                }
            case TowerTargetPriority.Close:
                {
                    Enemy closest = null;
                    float dist = 99;
                    for (int x = 0; x < curEnemiesInRange.Count; x++)
                    {
                        // Can use Vector3 if want the exact distance, but squareroot is more performance friendly since we only want the closest object
                        float d = (transform.position - curEnemiesInRange[x].transform.position).sqrMagnitude;
                        if (d < dist)
                        {
                            closest = curEnemiesInRange[x];
                            dist = d;
                        }
                    }
                    return closest;
                }
            case TowerTargetPriority.Strong:
                {
                    Enemy strongest = null;
                    int strongestHealth = 0;
                    foreach (Enemy enemy in curEnemiesInRange)
                    {
                        if (enemy.health > strongestHealth)
                        {
                            strongest = enemy;
                            strongestHealth = enemy.health;
                        }
                    }
                    return strongest;
                }
        }


        return null;
    }


    void Attack()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        // If object in range has the tag enemy
        if (other.CompareTag("Enemy"))
        {
            // Adds to the list
            curEnemiesInRange.Add(other.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //Removes from the list
            curEnemiesInRange.Remove(other.GetComponent<Enemy>());
        }
    }
}
