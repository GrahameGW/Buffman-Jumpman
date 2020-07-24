using UnityEngine;

[CreateAssetMenu]
public class EntityFactory : ScriptableObject
{
    [SerializeField] GameObject[] entityPrefabs = default;

    private void OnValidate() // runs on editor changes
    {
        for (int i = 0; i < entityPrefabs.Length; i++) {
            if (entityPrefabs[i] != null) {
                if (entityPrefabs[i].GetComponent<IEntity>() == null) {
                    entityPrefabs[i] = null;
                    Debug.LogError("Cannot add prefabs to factory without IEntity interface");
                }
            }
        }
    }

    public GameObject Spawn(int index)
    {
        if (index >= 0 && index < entityPrefabs.Length) {
            return Instantiate(entityPrefabs[index]);
        }
        else return null;
    }
}
