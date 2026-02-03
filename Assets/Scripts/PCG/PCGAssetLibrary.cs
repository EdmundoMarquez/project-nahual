using UnityEngine;
using System.Collections.Generic;

namespace ProjectNahual.PCG
{
    public class PCGAssetLibrary
    {
        private List<GameObject> assetLibrary = new List<GameObject>();

        public PCGAssetLibrary(GameObject[] assets)
        {
            for (int i = 0; i < assets.Length; i++)
                assetLibrary.Add(assets[i]);
        }

        public GameObject GetRandomAsset()
        {
            return assetLibrary[Random.Range(0, assetLibrary.Count)];
        }
    }
}