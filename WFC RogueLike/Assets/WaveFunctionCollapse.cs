using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WaveFunctionCollapse {

    public static GameObject defaultTile;
    private static bool canGo = true;

    public static List<GameObject>[,] Initialise(List<GameObject>[,] world, GameObject tile) {





        defaultTile = tile;
        Vector2Int position = new Vector2Int(world.GetLength(0) / 2, world.GetLength(1) / 2);
        //Vector2Int position = new Vector2Int(Random.Range(0, world.GetLength(0)), Random.Range(0, world.GetLength(1)));
        world[position.x, position.y] = new List<GameObject> { tile };
        world = Propagate(world, position);
        return world;
    }

    public static GameObject[,] Complete(List<GameObject>[,] potentialWorld) {

        List<GameObject>[,] collapsedWorld = null;

        bool complete = false;
        int count = 0;

        collapsedWorld = Recursive_Complete(potentialWorld);
        int max = potentialWorld.GetLength(0) * potentialWorld.GetLength(1);
        for (int i = 0; i < max; i++) {
            if (WorldComplete(collapsedWorld)) {
                Debug.Log("WORLD COMPLETED");
                break;
            }
            /*
            if (WorldNotPossible(collapsedWorld)) {
                Debug.Log("WORLD NOT POSSIBLE, STILL RETURNING");                
                return ConvertToSingleItemArray(collapsedWorld);
            }
            */
            collapsedWorld = Recursive_Complete(collapsedWorld);
        }
        
        Debug.Log("START OF WHILE");


        /*

        while (!complete) {
            collapsedWorld = Recursive_Complete(collapsedWorld);
            if (WorldComplete(collapsedWorld)) {
                complete = true;
            }

            count++;
            Debug.Log("Iteration: " + count);
        }

        */

        return ConvertToSingleItemArray(collapsedWorld);
    }

    private static List<GameObject>[,] Recursive_Complete(List<GameObject>[,] world) {

        if (WorldComplete(world)) {
            return world;
        }
        /*
        if (WorldNotPossible(world)) {
            Debug.Log("World Not Possible");
            return world;
        }*/

        Vector2Int position = FindLowestEntropy(world);
        Debug.Log("Lowest Entropy: " + position);
        world[position.x, position.y] = CollapseWave(world[position.x, position.y]);
        

        world = Propagate(world, position);

        //world = Recursive_Complete(world);    //THIS IS THE RECURSIVE PART OF THIS METHOD
        return world;

    }

    private static GameObject[,] ConvertToSingleItemArray(List<GameObject>[,] world) {

        Debug.Log("ConvertToSingleItemArray");
        
        int width = world.GetLength(0);
        int height = world.GetLength(1);
        GameObject[,] completeList = new GameObject[width,height];

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                completeList[i, j] = world[i, j][0];
            }
        }

        return completeList;

    }


    // Recursive algorithm that propagates updates of the world each time a new tiles entropy is changed

    // PROBLEMS: SEEMS TO RECURSE INFINITELY - Sorted... I think
    private static List<GameObject>[,] Propagate(List<GameObject>[,] world, Vector2Int center, int iteration = 0) {

        //changedFlags[center.x, center.y] = true;
        iteration++;
        //Debug.Log("Iteration: " + iteration);
        //Debug.Log(center);


        List<Vector2Int> neighbours = FindValidNeighbours(world, center);
        //bool[] neighbourFlags = new bool[neighbours.Count];   // Flags true if any of the neighbours are changed in this pass
        bool hasChanged = false;




        List<Vector2Int> temp = new List<Vector2Int>();     //Remove neighbours that have already been checked
        
        
        foreach (Vector2Int neighbour in neighbours) {
            if (world[neighbour.x, neighbour.y].Count >= world[center.x, center.y].Count) {
                temp.Add(neighbour);
            }
        }

        neighbours = temp;
        
        //temp = null;
        


        // Base Case
        if (neighbours.Count == 0) {
            //Debug.Log("Position: "+ center + "Neighbours are 0");
            return world;
        }

        List<GameObject> centerList = world[center.x, center.y];
        List<GameObject> target = new List<GameObject>();
        AvailableNeighbours.Direction directionName = AvailableNeighbours.Direction.North;


        int counter = -1;
        foreach (Vector2Int neighbour in neighbours) {

            counter++;

            Vector2Int directionVector = neighbour - center;
            target = world[center.x + directionVector.x, center.y + directionVector.y];
            List<GameObject> newTargetList = new List<GameObject>();


            if (directionVector.Equals(Vector2Int.down)) {
                directionName = AvailableNeighbours.Direction.South;
            }
            if (directionVector.Equals(Vector2Int.up)) {
                directionName = AvailableNeighbours.Direction.North;
            }
            if (directionVector.Equals(Vector2Int.left)) {
                directionName = AvailableNeighbours.Direction.West;
            }
            if (directionVector.Equals(Vector2Int.right)) {
                directionName = AvailableNeighbours.Direction.East;
            }


            List<GameObject> neighbourPossibles = new List<GameObject>();
            foreach (GameObject centerTile in centerList) {
                List<GameObject> viables = AvailableNeighbours.GetAvailableTiles(centerTile, directionName);
                foreach (GameObject viable in viables) {
                    if (!neighbourPossibles.Contains(viable)) {
                        neighbourPossibles.Add(viable);
                    }
                }

            }

            

            if (!AreListsEqual(neighbourPossibles, target)) {
                hasChanged = true;
                
            }

            world[neighbour.x, neighbour.y] = neighbourPossibles;

        }
        

        if (hasChanged) {
            foreach (Vector2Int neighbour in neighbours) {
                world = Propagate(world, neighbour, iteration);
            }        
        }
        

        return world;
        
    }

    private static bool AreListsEqual(List<GameObject> listOne, List<GameObject> listTwo) {
        if (listOne.Count != listTwo.Count) {
            return false;
        }

        for (int i = 0; i < listOne.Count; i++) {
            if (!listOne[i].Equals(listTwo[i])) {
                return false;
            }
        }

        return true;
    }



    private static List<Vector2Int> FindValidNeighbours(List<GameObject>[,] world, Vector2Int center, bool diagonalsIncluded = false) {

        List<Vector2Int> list = new List<Vector2Int>();
        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {

                if ((i == 0 && j == 0)                   ||
                    (center.x + i < 0)                   ||
                    (center.x + i >= world.GetLength(0)) ||
                    (center.y + j < 0)                   ||
                    (center.y + j >= world.GetLength(1))) {

                    continue;

                } else if(!diagonalsIncluded) {

                    if (Mathf.Abs(i) == Mathf.Abs(j)) {
                        continue;
                    }
                }

                list.Add(new Vector2Int(center.x + i, center.y + j));
                
            }
        }
        //Debug.Log(list.Count);
        return list;

    }

    

    private static List<GameObject> CollapseWave(List<GameObject> cell) {

        //Debug.Log("Before: " + cell.Count);


        Debug.Log("Collapse Wave");
        foreach (GameObject item in cell) {
            Debug.Log(item.name);
        }
        
        int index = Random.Range(0, cell.Count);

        Debug.Log("Random Number: " + index + ", Tile: " + cell[index].name);
        Debug.Log("---------------------------------");


        List<GameObject> singleItem = new List<GameObject>();
        singleItem.Add(cell[index]);
        //Debug.Log("After: " + singleItem.Count);
        return singleItem;

    }

    private static Vector2Int FindLowestEntropy(List<GameObject>[,] world) {
        Vector2Int position = new Vector2Int(Random.Range(0, world.GetLength(0)),Random.Range(0, world.GetLength(1)));
        int lowest = int.MaxValue;

        for (int i = 0; i < world.GetLength(0); i++) {
            for (int j = 0; j < world.GetLength(1); j++) {
                if (world[i, j].Count == 0) {
                    Debug.Log("0 Entropy Found");
                }


                if (world[i, j].Count < lowest && world[i, j].Count > 1) {
                    position = new Vector2Int(i, j);
                    lowest = world[i, j].Count;
                }
            }
        }
        return position;

    }

    private static bool WorldComplete(List<GameObject>[,] world) {

        for (int i = 0; i < world.GetLength(0); i++) {
            for (int j = 0; j < world.GetLength(1); j++) {

                if (world[i,j].Count > 1 || world[i,j].Count == 0) {
                    return false;
                }
            }
        }

        return true;

    }

    private static bool WorldNotPossible(List<GameObject>[,] world) {
        for (int i = 0; i < world.GetLength(0); i++) {
            for (int j = 0; j < world.GetLength(1); j++) {

                if (world[i, j].Count == 0) {
                    return true;
                }
            }
        }

        return false;

    }

}
