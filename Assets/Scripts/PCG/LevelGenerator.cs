using System.Collections;
using System.Linq;
using PlasticGui.Diff;
using Unity.AI.Navigation;
using UnityEngine;

namespace ProjectNahual.PCG
{
    [RequireComponent(typeof(NavMeshSurface))]
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject groundPlane;
        [SerializeField] private GameObject player;
        [Header("Grid Settings")]
        [SerializeField] [Range(0.1f,1)] private float groundScaleFactor = 0.5f;
        [SerializeField] private Vector2Int worldSize;
        [SerializeField] private float cellOffset = 1.1f;
        [SerializeField] [Range(1, 4)] private int borderWidth = 1;
        [Header("Asset Libraries")]
        [SerializeField] private PCGAssetLibrary cellAssetLibrary;
        [SerializeField] private PCGAssetLibrary borderAssetLibrary;
        [SerializeField] private PCGAssetLibrary environmentAssetLibrary;
        [SerializeField] [Range(20, 50)] private int environmentPopulation = 20;
        [SerializeField] private PCGAssetLibrary enemyAssetLibrary;
        [SerializeField] [Range(5, 50)] private int enemyPopulation = 20;
        [SerializeField] private GameObject levelExit;
        private PCGAlgorithm algorithm;
        private NavMeshSurface navMeshSurface;
        private Coroutine GenerateLevelCoroutine;
        private void Awake() => navMeshSurface = GetComponent<NavMeshSurface>();


        public void GenerateLevel()
        {
            borderAssetLibrary.ReleasePool();
            environmentAssetLibrary.ReleasePool();
            enemyAssetLibrary.ReleasePool();

            if(GenerateLevelCoroutine != null)
                StopCoroutine(GenerateLevelCoroutine);
            GenerateLevelCoroutine = StartCoroutine(GenerateLevel_Timer(3));
        }

        IEnumerator GenerateLevel_Timer(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            algorithm = new PCGAlgorithm();

            //Generate borders
            algorithm.GenerateWithBorder(
                cellAssetLibrary, 
                borderAssetLibrary, 
                borderWidth, 
                transform, 
                worldSize.x, 
                worldSize.y, 
                cellOffset);
            algorithm.SpawnLevelExit(levelExit);

            groundPlane.transform.localScale = new Vector3(
                worldSize.x * cellOffset * groundScaleFactor,
                0,
                worldSize.y * cellOffset * groundScaleFactor
            );

            groundPlane.transform.position = new Vector3(
                (worldSize.x * cellOffset) / 2, 
                groundPlane.transform.position.y, 
                (worldSize.y * cellOffset) / 2);

            algorithm.ScatterAssetLibrary(environmentAssetLibrary, environmentPopulation, transform);

            
            navMeshSurface.BuildNavMesh();
            algorithm.ScatterAssetLibrary(enemyAssetLibrary, enemyPopulation, transform);

            algorithm.PlacePlayerInCell(player);
        }
    }
}
