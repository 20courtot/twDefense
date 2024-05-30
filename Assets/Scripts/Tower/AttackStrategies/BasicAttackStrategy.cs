using System.Collections;
using UnityEngine;

public class BasicAttackStrategy : IAttackStrategy
{
    public void Attack(Tower tower)
    {
        if (tower.TargetEnemy != null)
        {
            Projectile newProjectile = Object.Instantiate(tower.Projectile) as Projectile;
            newProjectile.transform.localPosition = tower.transform.localPosition;

            switch (newProjectile.ProjectileType)
            {
                case proType.arrow:
                    tower.AudioSource.PlayOneShot(SoundManager.Instance.Arrow);
                    break;
                case proType.fireball:
                    tower.AudioSource.PlayOneShot(SoundManager.Instance.Fireball);
                    break;
                case proType.rock:
                    tower.AudioSource.PlayOneShot(SoundManager.Instance.Rock);
                    break;
            }

            tower.StartCoroutine(MoveProjectile(newProjectile, tower.TargetEnemy));
        }
    }

    private IEnumerator MoveProjectile(Projectile projectile, Enemy targetEnemy)
    {
        while (projectile != null && targetEnemy != null && Vector2.Distance(projectile.transform.position, targetEnemy.transform.position) > 0.2f)
        {
            projectile.transform.position = Vector2.MoveTowards(projectile.transform.position, targetEnemy.transform.position, 5f * Time.deltaTime);
            yield return null;
        }

        if (projectile != null)
        {
            Object.Destroy(projectile.gameObject);
        }
    }
}
