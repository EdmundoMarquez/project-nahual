using System.Collections.Generic;
using UnityEngine;

namespace ProjectNahual.PCG
{

    public class PCGAlgorithm
    {
        private Transform gridParent;
        private List<Vector3> cellPositions = new List<Vector3>();
        private List<Vector3> positionsForLevelExit = new List<Vector3>();

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

        public void GenerateWithBorder(GameObject cellPrefab, PCGAssetLibrary borderAssetLibrary, int borderWidth = 1, Transform parent = null, int worldSizeX = 10, int worldSizeZ = 10, float gridOffset = 3)
        {
            gridParent = parent;

            int excludeColumn = Random.Range((worldSizeX / 2) + borderWidth, (worldSizeX /2) - borderWidth);

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

                    //Exclude column used to spawn level exit
                    if(excludeColumn == x && z <= borderWidth)
                    {
                        positionsForLevelExit.Add(position);
                        continue;
                    }

                    //Handle border logic
                    if(x <= borderWidth || z <= borderWidth || x >= worldSizeX - borderWidth || z >= worldSizeZ - borderWidth)
                    {
                        GameObject randomizedBorder = borderAssetLibrary.GetRandomAsset();
                        Vector3 borderPosition = cellPositions[cellPositions.Count - 1];
                        SpawnObject(randomizedBorder, borderPosition, true);
                        cellPositions.RemoveAt(cellPositions.Count - 1);
                    }
                }
            }
        }

        public void SpawnObject(GameObject objectToSpawn, Vector3 position, bool randomizeRotation = false)
        {
            GameObject toPlaceObject = Object.Instantiate(objectToSpawn,
                position,
                randomizeRotation ? RandomizeRotation() : Quaternion.identity);
        }

        public void ScatterAssetLibrary(PCGAssetLibrary assetLibary, int population = 20)
        {
            for (int i = 0; i < population; i++)
            {
                ScatterObject(assetLibary.GetRandomAsset());
            }
        }

        public void ScatterObject(GameObject objectToSpawn, int population = 1)
        {
            for (int i = 0; i < population; i++)
            {
                if(cellPositions.Count < 1)
                {
                    Debug.Log("Too crowded for object generation. Make a bigger grid!");
                    return;
                }

                GameObject toPlaceObject = Object.Instantiate(objectToSpawn,
                    ObjectSpawnLocation(),
                    Quaternion.identity);

                toPlaceObject.transform.rotation = RandomizeRotation();
            }
        }

        public void SpawnLevelExit(GameObject levelExit)
        {
            if(positionsForLevelExit.Count < 1)
            {
                Debug.LogError("No positions to spawn exit!");
                return;
            }
            Vector3 position = positionsForLevelExit[positionsForLevelExit.Count - 1];
            SpawnObject(levelExit, position);
        }

        private Quaternion RandomizeRotation()
        {
            return Quaternion.Euler(
                0,
                Random.rotation.eulerAngles.y,
                0
            );
        }

        public Vector3 ObjectSpawnLocation()
        {
            int rndIndex = Random.Range(0, cellPositions.Count);

            Vector3 newPos = new Vector3(
                cellPositions[rndIndex].x,
                cellPositions[rndIndex].y + 0.5f,
                cellPositions[rndIndex].z
            );
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