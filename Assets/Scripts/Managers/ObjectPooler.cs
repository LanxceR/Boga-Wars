using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    // Singleton instance
    private static ObjectPooler instance;

    [Header("Main Setting")]
    [SerializeField] private Transform Parent;
    [SerializeField] private int Size; // Amount to spawn
    [SerializeField] private PoolObject[] Prefabs; // Array of prefabs to be made as pooled objects

    [Header("Stored Objects")]
    [SerializeField] private List<PoolObject> poolObjects;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate bunch of pool objects at start
        InstantiateObjects();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Instantiate pool objects
    private void InstantiateObjects()
    {
        poolObjects = new List<PoolObject>();
        for (int i = 0; i < Size; i++)
        {
            // Spawn all objects in Prefabs[] array * Size
            foreach (PoolObject obj in Prefabs)
            {
                poolObjects.Add(Instantiate(obj.gameObject, Parent).GetComponent<PoolObject>());
            }
        }
    }

    // Request an inactive pooled object
    public PoolObject RequestObject(PoolObjectType type)
    {
        foreach (PoolObject obj in poolObjects)
        {
            // Look for an inactive object in the pool array, then fetch it
            if (obj.ObjectType == type && !obj.IsActive())
            {
                return obj;
            }
        }
        // Otherwise fetch nothing
        return null;
    }

    public static ObjectPooler GetInstance()
    {
        return instance;
    }

    // Deactivate all pooled objects
    public void DeactivateAllPoolObjects()
    {
        foreach (PoolObject obj in poolObjects)
        {
            obj.Deactivate();
        }
    }
}
