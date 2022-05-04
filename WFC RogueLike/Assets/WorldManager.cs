using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {
    [SerializeField] private List<GameObject> tiles;


    public static GameObject[,] grid;
    public static List<GameObject>[,] gridEntropy;
    public int width;
    public int height;


    private void Awake() {
        width = 30;
        height = 30;


        grid = new GameObject[width, height];
        gridEntropy = new List<GameObject>[width, height];
        for (int i = 0; i < gridEntropy.GetLength(0); i++) {
            for (int j = 0; j < gridEntropy.GetLength(1); j++) {
                gridEntropy[i, j] = new List<GameObject>();
            }
        }
        ResetGridEntropy();

        foreach (GameObject tile in tiles) {
            GameObject tileInstance = Instantiate(tile, new Vector2(10000, 10000), Quaternion.identity);
            Destroy(tileInstance);
            
        }
        

    }

    private void Start() {

        GameObject start = tiles[2]; // NEED TO INSERT THE INDEX OF GROUND
        
        
        // TESTING THE DICTIONARY VALUES
        
        /*
        foreach (GameObject tile in tiles) {
            Debug.Log("--" + tile.name + "--");
            AvailableNeighbours.Direction direction = AvailableNeighbours.Direction.North;
            for (int i = 0; i < 4; i++) {
                switch (i) {
                    case 0:
                        direction = AvailableNeighbours.Direction.North;
                        Debug.Log("North");
                        break;
                    case 1:
                        direction = AvailableNeighbours.Direction.East;
                        Debug.Log("East");
                        break;
                    case 2:
                        direction = AvailableNeighbours.Direction.South;
                        Debug.Log("South");
                        break;
                    case 3:
                        direction = AvailableNeighbours.Direction.West;
                        Debug.Log("West");
                        break;
                }

                foreach (GameObject possTile in AvailableNeighbours.GetAvailableTiles(tile, direction)) {
                    Debug.Log(possTile.name);
                }


            }
        }

        */

        WaveFunctionCollapse.Initialise(gridEntropy, start);

        grid = WaveFunctionCollapse.Complete(gridEntropy);
        if (grid != null) {
            Debug.Log("Successful Grid - Supposedly");
        }
        

        InstatiateWorld();
    }

    public List<GameObject> GetTilePrefabs() {
        return tiles;
    }

    private void InstatiateWorld() {

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





    private void ResetGridEntropy() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                gridEntropy[i, j].Clear();
                gridEntropy[i, j].AddRange(tiles);
            }
        }
    }



}
