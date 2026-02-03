using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;

namespace ProjectNahual.PCG
{
    [RequireComponent(typeof(NavMeshSurface))]
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject gridCellPrefab;
        [SerializeField] private GameObject groundPlane;
        [Header("Grid Settings")]
        [SerializeField] [Range(0.1f,1)] private float groundScaleFactor = 0.5f;
        [SerializeField] private Vector2Int worldSize;
        [SerializeField] private float cellOffset = 1.1f;
        [SerializeField] [Range(1, 4)] private int borderWidth = 1;
        [Header("Asset Libraries")]
        [SerializeField] private GameObject[] borderPrefabs;
        [SerializeField] private GameObject[] environmentPrefabs;
        [SerializeField] private GameObject[] enemyPrefabs;
        [SerializeField] private GameObject levelExitPrefab;
        private PCGAlgorithm algorithm;
        private NavMeshSurface navMeshSurface;

        private void Awake() => navMeshSurface = GetComponent<NavMeshSurface>();

        private void Start()
        {
            algorithm = new PCGAlgorithm();

            //Generate borders
            PCGAssetLibrary borderLibrary = new PCGAssetLibrary(borderPrefabs);
            algorithm.GenerateWithBorder(
                gridCellPrefab, 
                borderLibrary, 
                borderWidth, 
                transform, 
                worldSize.x, 
                worldSize.y, 
                cellOffset);
            algorithm.SpawnLevelExit(levelExitPrefab);

            GameObject ground = Instantiate(groundPlane);

            ground.transform.localScale = new Vector3(
                worldSize.x * cellOffset * groundScaleFactor,
                0,
                worldSize.y * cellOffset * groundScaleFactor
            );

            ground.transform.position = new Vector3(
                (worldSize.x * cellOffset) / 2, 
                ground.transform.position.y, 
                (worldSize.y * cellOffset) / 2);
            
            PCGAssetLibrary environmentLibrary = new PCGAssetLibrary(environmentPrefabs);
            algorithm.ScatterAssetLibrary(environmentLibrary, 50);

            // navMeshSurface.BuildNavMesh();
            // PCGAssetLibrary enemyLibrary = new PCGAssetLibrary(enemyPrefabs);
            // algorithm.ScatterAssetLibrary(enemyLibrary, 20);
        }
    }
}
