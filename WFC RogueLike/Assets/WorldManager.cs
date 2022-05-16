using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {
    [SerializeField] public List<GameObject> tiles;
    public GameObject problemCell;



    public GameObject[,] grid;
    public int[,] gridIds;
    //public static List<GameObject>[,] gridEntropy;
    public List<int>[,] gridWithEntropy;


    public int width;
    public int height;
    private bool foundWorld = false;
    private int iteration = 0;


    private void Awake() {
        width = 11;
        height = 11;


        gridWithEntropy = new List<int>[width, height];
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {

                gridWithEntropy[i, j] = new List<int>();

                for (int k = 0; k < tiles.Count; k++) {
                    gridWithEntropy[i, j].Add(k);       // NEEDS TO BE CHANGED IF IDs ARE CHANGED
                }
            }
        }
        grid = new GameObject[width, height];
        gridIds = new int[width, height];

        
        foreach (GameObject tile in tiles) {
            tile.GetComponent<AvailableNeighbours2>().Setup();
        }
        

    }

    /*

    private void Start() {
        List<int>[,] finishedWorld = WaveFunctionCollapse.FinishWorld(gridWithEntropy);
        InstantiateWorld(finishedWorld);
    }
    */
    
    private void Update() {

        if (!foundWorld) {

            gridWithEntropy = new List<int>[width, height];
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {

                    gridWithEntropy[i, j] = new List<int>();

                    for (int k = 0; k < tiles.Count; k++) {
                        gridWithEntropy[i, j].Add(k);       // NEEDS TO BE CHANGED IF IDs ARE CHANGED
                    }
                }
            }


            List<int>[,] finishedWorld = WaveFunctionCollapse.FinishWorld(gridWithEntropy);
            InstantiateWorld(finishedWorld, iteration);
            if (CompleteWorld(finishedWorld)) {
                foundWorld = true;
            } else {
                iteration++;
                StartCoroutine(Delay());
            }
        }

        

    }
    
    public IEnumerator Delay() {
        foundWorld = true;
        yield return new WaitForSeconds(1f);
        foundWorld = false;
    }




    public List<GameObject> GetTilePrefabs() {
        return tiles;
    }

    private bool CompleteWorld(List<int>[,] world) {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {

                if (world[i, j].Count > 1 || world[i, j].Count == 0) {
                    return false;
                }


            }
        }

        return true;

    }
    private void InstantiateWorld(List<int>[,] world, int iteration) {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {

                if (world[i,j].Count == 1) {
                    GameObject go = Instantiate(tiles[world[i, j][0]], new Vector2(width * iteration + i, j), Quaternion.identity);
                    grid[i, j] = go;
                } else if (world[i,j].Count > 1) {
                    GameObject go = Instantiate(tiles[0], new Vector2(width * iteration + i, j), Quaternion.identity);
                    grid[i, j] = go;
                } else {
                    GameObject go = Instantiate(problemCell, new Vector2(width * iteration + i, j), Quaternion.identity);
                    
                    grid[i, j] = go;
                }


                
                
            }
        }
    }

    private void InstantiateWorld() {

        Debug.Log("Instantiate World");

        for (int i = 0; i < grid.GetLength(0); i++) {
            for (int j  = 0; j < grid.GetLength(1); j++) {
                Instantiate(grid[i, j], new Vector2(i, j), Quaternion.identity);
            }
        }
    }

    public static void InstantiateSingleCell(GameObject cell, Vector2Int position) {
        Vector3 v3Pos = new Vector3(position.x, position.y, 0);
        
        Instantiate(cell, v3Pos, Quaternion.identity);
    }



    /*

    private void ResetGridEntropy() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                gridWithEntropy[i, j].Clear();
                gridWithEntropy[i, j].AddRange(tiles);
            }
        }
    }

    */

}
