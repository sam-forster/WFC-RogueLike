using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour {
    [SerializeField] public List<GameObject> tiles;
    public GameObject problemCell;
    public GameObject undefinedCell;



    public GameObject[,] grid;
    public GameObject[,] previousGrid;
    public int[,] gridIds;
    //public static List<GameObject>[,] gridEntropy;
    public List<int>[,] gridWithEntropy;


    public int width;
    public int height;
    public bool foundWorld = false;
    private bool canRun = true;
    private int iteration = 0;


    private void Awake() {
        width = 35;
        height = 35;


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
        previousGrid = new GameObject[width, height];
        gridIds = new int[width, height];

        
        foreach (GameObject tile in tiles) {
            tile.GetComponent<AvailableNeighbours2>().Setup();
        }
        

    }

    
    private void Update() {

        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(0);
        }

        if (!canRun) {
            return;
        }

        if (!foundWorld) {
            iteration++;
            gridWithEntropy = WaveFunctionCollapse.FinishWorld(gridWithEntropy);
            InstantiateWorld2(gridWithEntropy);
            if (!CompleteWorld(gridWithEntropy)) {
                StartCoroutine(Delay());
            } else {
                foundWorld = true;
                StartCoroutine(Delay());
                
            }
            
            
        }
        

    }

    private void InstantiateWorld2(List<int>[,] world) {

        previousGrid = grid;
        grid = new GameObject[width, height];
        
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {

                if (world[i, j].Count == 1) {
                    GameObject go = Instantiate(tiles[world[i, j][0]], new Vector2(i, j), Quaternion.identity);
                    grid[i, j] = go;
                }else if (world[i, j].Count > 0) {
                    GameObject go = Instantiate(undefinedCell, new Vector2(i, j), Quaternion.identity);
                    grid[i, j] = go;
                }

            }
        }
    }
   

    
    private void InstantiateWorld(List<int>[,] world, int iteration) {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {

                if (world[i,j].Count == 1) {
                    GameObject go = Instantiate(tiles[world[i, j][0]], new Vector2(width * iteration + i, j), Quaternion.identity);
                    grid[i, j] = go;
                } else if (world[i,j].Count > 1) {
                    GameObject go = Instantiate(tiles[4], new Vector2(width * iteration + i, j), Quaternion.identity);
                    grid[i, j] = go;
                } else {
                    GameObject go = Instantiate(problemCell, new Vector2(width * iteration + i, j), Quaternion.identity);
                    
                    grid[i, j] = go;
                }
                
            }
        }
    }

    public IEnumerator Delay() {
        canRun = false;
        yield return new WaitForSeconds(Time.deltaTime);

        foreach (GameObject item in previousGrid) {
            
            Destroy(item);
        }
        canRun = true;
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
