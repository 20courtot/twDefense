using UnityEngine;
using System.Collections.Generic;

public class Tower : MonoBehaviour, ITower
{
    [SerializeField]
    private float timeBetweenAttacks;
    [SerializeField]
    private float attackRadius;
    [SerializeField]
    private Projectile projectile;
    private bool isAttack = false;
    private Enemy targetEnemy = null;
    private float attackCounter;
    private AudioSource audioSource;

    private IAttackStrategy attackStrategy;

    public Enemy TargetEnemy => targetEnemy;
    public Projectile Projectile => projectile;
    public AudioSource AudioSource => audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SetAttackStrategy(IAttackStrategy strategy)
    {
        attackStrategy = strategy;
    }

    void Update()
    {
        attackCounter -= Time.deltaTime;

        if (targetEnemy == null || targetEnemy.IsDead)
        {
            targetEnemy = GetNearestEnemyInRange();
        }

        if (targetEnemy != null && Vector2.Distance(transform.position, targetEnemy.transform.position) <= attackRadius)
        {
            if (attackCounter <= 0f)
            {
                Attack();
                attackCounter = timeBetweenAttacks;
            }
        }
        else
        {
            isAttack = false;
            targetEnemy = null;
        }
    }

    void FixedUpdate()
    {
        if (isAttack)
        {
            Attack();
        }
    }

    public void Attack()
    {
        isAttack = false;
        attackStrategy?.Attack(this);
    }

    private List<Enemy> GetEnemiesInRange()
    {
        List<Enemy> enemiesInRange = new List<Enemy>();
        foreach (Enemy enemy in GameManager.Instance.EnemyList)
        {
            if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) <= attackRadius && !enemy.IsDead)
            {
                enemiesInRange.Add(enemy);
            }
        }
        return enemiesInRange;
    }

    private Enemy GetNearestEnemyInRange()
    {
        Enemy nearestEnemy = null;
        float smallestDistance = float.PositiveInfinity;
        foreach (Enemy enemy in GetEnemiesInRange())
        {
            if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) < smallestDistance)
            {
                smallestDistance = Vector2.Distance(transform.localPosition, enemy.transform.localPosition);
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy;
    }
}
