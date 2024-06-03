using UnityEngine;

public static class TowerFactory
{
    public static ITower CreateTower(GameObject towerPrefab, IAttackStrategy attackStrategy)
    {
        GameObject towerObject = Object.Instantiate(towerPrefab);
        ITower tower = towerObject.GetComponent<ITower>();
        if (tower != null)
        {
            tower.SetAttackStrategy(attackStrategy);
        }
        return tower;
    }

    public static IAttackStrategy GetAttackStrategy(proType projectileType)
    {
        switch (projectileType)
        {
            case proType.arrow:
                return new BasicAttackStrategy();
            case proType.fireball:
                return new FireballAttackStrategy();
            case proType.rock:
                return new RockAttackStrategy();
            default:
                return new BasicAttackStrategy();
        }
    }
}
