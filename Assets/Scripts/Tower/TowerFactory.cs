using UnityEngine;

public static class TowerFactory
{
    public static ITower CreateTower(GameObject towerPrefab, IAttackStrategy attackStrategy)
    {
        GameObject newTowerObject = Object.Instantiate(towerPrefab);
        Tower newTower = newTowerObject.GetComponent<Tower>();
        if (newTower == null)
        {
            Debug.LogError("The tower prefab does not contain a component that implements ITower.");
        }
        else
        {
            newTower.SetAttackStrategy(attackStrategy);
        }
        return newTower;
    }
}
