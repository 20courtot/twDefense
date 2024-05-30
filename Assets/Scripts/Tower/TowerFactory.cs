using UnityEngine;

public static class TowerFactory
{
    public static ITower CreateTower(GameObject towerPrefab)
    {
        GameObject newTowerObject = Object.Instantiate(towerPrefab);
        ITower newTower = newTowerObject.GetComponent<ITower>();
        if (newTower == null)
        {
            Debug.LogError("The tower prefab does not contain a component that implements ITower.");
        }
        return newTower;
    }
}
