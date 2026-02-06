using System.Collections;
using System.Linq;
using ProjectNahual.FPCharacter;
using ProjectNahual.Utils;
using Unity.AI.Navigation;
using UnityEngine;

namespace ProjectNahual.PCG
{
    [RequireComponent(typeof(NavMeshSurface))]
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject groundPlane;
        [SerializeField] private GameObject levelEntrance;
        [SerializeField] private GameObject levelExit;
        [Header("Grid Settings")]
        [Tooltip("Ground scale used to scale ground according to world size")]
        [SerializeField] [Range(0.1f,1)] private float groundScaleFactor = 0.5f;
        [SerializeField] private Vector2Int worldSize;
        [SerializeField] private float cellOffset = 1.1f;
        [Tooltip("Border width measured in cells")]
        [SerializeField] [Range(1, 4)] private int borderWidth = 1;
        [Header("Asset Libraries")]
        [SerializeField] private PCGAssetLibrary cellAssetLibrary;
        [SerializeField] private PCGAssetLibrary borderAssetLibrary;
        [SerializeField] private PCGAssetLibrary environmentAssetLibrary;
        [Tooltip("Population represented in percentage")]
        [SerializeField] [Range(20, 50)] private int environmentPopulation = 20;
        [SerializeField] private PCGAssetLibrary enemyAssetLibrary;
        [Tooltip("Population represented in percentage")]
        [SerializeField] [Range(5, 50)] private int enemyPopulation = 20;
        private PCGAlgorithm algorithm;
        private NavMeshSurface navMeshSurface;
        private Coroutine GenerateLevelCoroutine;
        
        private void Awake()
        {
            Registry<LevelGenerator>.TryAdd(this);
            navMeshSurface = GetComponent<NavMeshSurface>();
        }


        public Coroutine GenerateLevel()
        {
            borderAssetLibrary.ReleasePool();
            environmentAssetLibrary.ReleasePool();
            enemyAssetLibrary.ReleasePool();

            if(GenerateLevelCoroutine != null)
                StopCoroutine(GenerateLevelCoroutine);
            GenerateLevelCoroutine = StartCoroutine(GenerateLevel_Timer(1));

            return GenerateLevelCoroutine;
        }

        IEnumerator GenerateLevel_Timer(float waitTime)
        {
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

            
            //Spawn gateways in limits of map
            algorithm.SpawnLevelEntrance(levelEntrance);
            algorithm.SpawnLevelExit(levelExit);
            
            //Adjust player and floor position
            SetPlayerPosition();
            SetGroundPlane();

            //Place environment assets and enemies
            algorithm.ScatterAssetLibrary(environmentAssetLibrary, environmentPopulation, transform);
            
            navMeshSurface.BuildNavMesh();
            algorithm.ScatterAssetLibrary(enemyAssetLibrary, enemyPopulation, transform, true, 25f);
            yield return null;
        }

        private void SetGroundPlane()
        {
            groundPlane.transform.localScale = new Vector3(
                worldSize.x * cellOffset * groundScaleFactor,
                0,
                worldSize.y * cellOffset * groundScaleFactor
            );

            groundPlane.transform.position = new Vector3(
                (worldSize.x * cellOffset) / 2, 
                groundPlane.transform.position.y, 
                (worldSize.y * cellOffset) / 2);
        }

        private void SetPlayerPosition()
        {
            IPlayerCharacter player = Registry<IPlayerCharacter>.GetFirst();
            Vector3 playerPosition = levelEntrance.transform.position + levelEntrance.transform.forward * 2f;
            playerPosition.y += 1f;
            Quaternion playerRotation = Quaternion.LookRotation(levelEntrance.transform.forward);
            player.SetPosition(playerPosition, playerRotation);
        }
    }
}
