using System.Collections.Generic;
using UnityEngine;
using ProjectNahual.FPCharacter;
using ProjectNahual.Utils;

namespace ProjectNahual.PCG
{

    public class PCGAlgorithm
    {
        private Transform gridParent;
        private List<Vector3> cellPositions = new List<Vector3>();
        private List<Vector3> positionsForLevelEntrance = new List<Vector3>();
        private List<Vector3> positionsForLevelExit = new List<Vector3>();
        private IPlayerCharacter playerCharacter;
        public PCGAlgorithm() => playerCharacter = Registry<IPlayerCharacter>.GetFirst();

        public void Generate(GameObject cellPrefab, Transform parent = null, int worldSizeX = 10, int worldSizeZ = 10, float gridOffset = 3)
        {
            gridParent = parent;

            for (int x = 0; x < worldSizeX; x++)
            {
                for (int z = 0; z < worldSizeZ; z++)
                {
                    Vector3 position = new Vector3(x * gridOffset,
                    0 //GenerateNoise(x, z, 8) * noiseHeight
                    , z * gridOffset);
                    GameObject cell = Object.Instantiate(cellPrefab, position, Quaternion.identity);
                    cellPositions.Add(cell.transform.position);
                    cell.transform.SetParent(gridParent);
                }
            }
        }

        public void GenerateWithBorder(PCGAssetLibrary cellLibrary, PCGAssetLibrary borderAssetLibrary, int borderWidth = 1, Transform parent = null, int worldSizeX = 10, int worldSizeZ = 10, float gridOffset = 3)
        {
            gridParent = parent;

            int entranceExcludeColumn = Random.Range(borderWidth, worldSizeX - borderWidth - 1);
            int exitExcludeColumn = Random.Range(borderWidth, worldSizeX - borderWidth - 1);

            for (int x = 0; x < worldSizeX; x++)
            {
                for (int z = 0; z < worldSizeZ; z++)
                {
                    Vector3 position = new Vector3(x * gridOffset,
                    0 //GenerateNoise(x, z, 8) * noiseHeight
                    , z * gridOffset);
                    PCGAsset cell = cellLibrary.GetAsset();
                    cell.transform.position = position;
                    cellPositions.Add(cell.transform.position);
                    cell.transform.SetParent(gridParent);
                    cellLibrary.OnAssetPlaced(cell);


                    //Exclude column used to spawn level entrance
                    if (entranceExcludeColumn == z && x <= borderWidth + 1)
                    {
                        positionsForLevelEntrance.Add(position);
                        cellPositions.RemoveAt(cellPositions.Count - 1);
                        continue;
                    }

                    //Exclude column used to spawn level exit
                    if (exitExcludeColumn == x && z <= borderWidth + 1)
                    {
                        positionsForLevelExit.Add(position);
                        cellPositions.RemoveAt(cellPositions.Count - 1);
                        continue;
                    }

                    //Handle border logic
                    if (x <= borderWidth || z <= borderWidth || x >= worldSizeX - borderWidth || z >= worldSizeZ - borderWidth)
                    {
                        PCGAsset border = borderAssetLibrary.GetAsset();
                        Vector3 borderPosition = cellPositions[cellPositions.Count - 1];
                        border.transform.SetPositionAndRotation(borderPosition, RandomizeRotation());
                        border.transform.SetParent(gridParent);
                        cellPositions.RemoveAt(cellPositions.Count - 1);
                        borderAssetLibrary.OnAssetPlaced(border);
                    }
                }
            }
        }

        public void SpawnObject(GameObject objectToSpawn, Vector3 position, bool randomizeRotation = false)
        {
            objectToSpawn.transform.SetPositionAndRotation(position, randomizeRotation ? RandomizeRotation() : objectToSpawn.transform.rotation);
        }

        public void ScatterAssetLibrary(PCGAssetLibrary assetLibary, int population = 20, Transform parent = null, bool avoidPlayer = false, float avoidingRange = 10f)
        {
            int totalInstances = cellPositions.Count * population / 100;

            for (int i = 0; i < totalInstances; i++)
            {
                PCGAsset objectToScatter = assetLibary.GetAsset();
                ScatterObject(objectToScatter.gameObject, 1, avoidPlayer, avoidingRange);
                objectToScatter.transform.SetParent(parent);
                assetLibary.OnAssetPlaced(objectToScatter);
            }
        }

        public void ScatterObject(GameObject objectToSpawn, int population = 1, bool avoidPlayer = false, float avoidingRange = 10f)
        {
            for (int i = 0; i < population; i++)
            {
                if (cellPositions.Count < 1)
                {
                    Debug.Log("Too crowded for object generation. Make a bigger grid!");
                    return;
                }

                objectToSpawn.transform.SetPositionAndRotation(ObjectSpawnLocation(avoidPlayer, avoidingRange), RandomizeRotation());
            }
        }

        //Spawns the level entrance at the last position available
        public void SpawnLevelEntrance(GameObject levelEntrance) => SpawnLevelGateway(levelEntrance, positionsForLevelEntrance);
        //Spawns the level exit at the second last position available
        public void SpawnLevelExit(GameObject levelExit) => SpawnLevelGateway(levelExit, positionsForLevelExit, -2);

        private void SpawnLevelGateway(GameObject gateway, List<Vector3> positionsCollection, int positionOffset = -1)
        {
            if (positionsCollection.Count < 1)
            {
                Debug.LogError("No positions to spawn gateway!");
                return;
            }
            Vector3 position = positionsCollection[positionsCollection.Count + positionOffset]; //If we use 2 we get rid of infront objects
            SpawnObject(gateway, position);
        }

        private Quaternion RandomizeRotation()
        {
            return Quaternion.Euler(
                0,
                Random.rotation.eulerAngles.y,
                0
            );
        }

        public Vector3 ObjectSpawnLocation(bool avoidPlayerLocation = false, float avoidingRange = 10f)
        {
            int rndIndex = Random.Range(0, cellPositions.Count);

            Vector3 newPos = new Vector3(
                cellPositions[rndIndex].x,
                cellPositions[rndIndex].y + 0.5f,
                cellPositions[rndIndex].z
            );

            if (avoidPlayerLocation)
            {
                int retries = 20;
                for (int i = 0; i < retries; i++)
                {
                    float distance = Vector3.Distance(newPos, playerCharacter.Position);
                    Debug.Log("Reposition PCG Asset. Try #" + i + ": " + distance + " away from player.");
                    if (distance < avoidingRange)
                    {
                        rndIndex = Random.Range(0, cellPositions.Count);
                        newPos = new Vector3(
                            cellPositions[rndIndex].x,
                            cellPositions[rndIndex].y + 0.5f,
                            cellPositions[rndIndex].z
                            );
                        continue;
                    }
                    break;
                }
            }

            cellPositions.RemoveAt(rndIndex);
            return newPos;
        }

        public float GenerateNoise(int x, int z, float detailScale)
        {
            float xNoise = (x + gridParent.position.x) / detailScale;
            float zNoise = (z + gridParent.position.z) / detailScale;
            return Mathf.PerlinNoise(xNoise, zNoise);
        }
    }
}