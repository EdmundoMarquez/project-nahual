using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;
using System.Collections;

namespace ProjectNahual.PCG
{
    public class PCGAsset : MonoBehaviour
    {
        [SerializeField] private float timeoutDelay = 1f;
        [SerializeField] private List<GameObject> assetVariants = new List<GameObject>();
        private IObjectPool<PCGAsset> objectPool;
        public IObjectPool<PCGAsset> ObjectPool { set => objectPool = value; }

        private void Awake()
        {
            for (int i = 0; i < assetVariants.Count; i++)
                assetVariants[i].SetActive(false);
        }
        
        private void OnEnable()
        {
            GetRandomAsset().SetActive(true);
        }

        private void OnDisable()
        {
            for (int i = 0; i < assetVariants.Count; i++)
                assetVariants[i].SetActive(false);
        }

        public GameObject GetRandomAsset()
        {
            return assetVariants[Random.Range(0, assetVariants.Count)];
        }

        public void Deactivate() => StartCoroutine(DeactivateRoutine(timeoutDelay));

        IEnumerator DeactivateRoutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            objectPool.Release(this);
        }
    }
}

