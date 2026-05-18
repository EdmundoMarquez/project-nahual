using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace ProjectNahual.PCG
{
    public class PCGAssetLibrary : MonoBehaviour
    {
        [SerializeField] private PCGAsset pcgAssetPrefab = null;
        private IObjectPool<PCGAsset> objectPool;
        [SerializeField] private bool collectionCheck = true;
        [SerializeField] private int defaultCapacity = 20;
        [SerializeField] private int maxSize = 100;
        private List<PCGAsset> assetInstances = new List<PCGAsset>();
        
        private void Awake()
        {
            objectPool = new ObjectPool<PCGAsset>(CreatePCGAsset, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
            collectionCheck, defaultCapacity, maxSize);
        }

        public PCGAsset GetAsset() => objectPool.Get();

        private PCGAsset CreatePCGAsset()
        {
            PCGAsset asset = Instantiate(pcgAssetPrefab);
            asset.ObjectPool = objectPool;
            return asset;
        }

        private void OnGetFromPool(PCGAsset pooledAsset)
        {
            pooledAsset.gameObject.SetActive(true);
        }

        private void OnReleaseToPool(PCGAsset pooledAsset)
        {
            pooledAsset.gameObject.SetActive(false);
        }

        private void OnDestroyPooledObject(PCGAsset pooledAsset)
        {
            Destroy(pooledAsset.gameObject);
        }

        public void OnAssetPlaced(PCGAsset asset)
        {
            assetInstances.Add(asset);
        }

        public void ReleasePool()
        {
            foreach (var instance in assetInstances)
                instance.Deactivate();
            assetInstances.Clear();
        }
    }
}