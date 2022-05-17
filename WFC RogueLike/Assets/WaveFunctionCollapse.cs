using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static  class WaveFunctionCollapse {

    public static List<int>[,] InitialiseWorld(List<int>[,] startingWorld) {

        Vector2Int position = new Vector2Int((int)Mathf.Ceil(startingWorld.GetLength(0) / 2), (int)Mathf.Ceil(startingWorld.GetLength(1) / 2));

        startingWorld[position.x, position.y] = new List<int>() { 1 };
        //PrintWorld(startingWorld);
        startingWorld = Propagate(startingWorld, position);
        //PrintWorld(startingWorld);

        return startingWorld;
    }

    public static List<int>[,] FinishWorld(List<int>[,] world) {

        List<int>[,] startingWorld = world;
        
        int width = world.GetLength(0);
        int height = world.GetLength(1);

        world = InitialiseWorld(world);

        return StepThrough(world);

        /*
        for (int i = 0; i < width * height; i++) {

            if (ZeroEntropy(world)) {
                Debug.Log("Zero Entropy Detected");
                
                //return FinishWorld(startingWorld);

                PrintWorld(world);
                return world;
            }
            
            if (WorldComplete(world)) {
                Debug.Log("Completed in " + i + " iterations");
                return world;
            }

            Vector2Int position = GetPositionLowestEntropy(world);
            world = CollapseWave(world, position);
            world = Propagate(world, position);
            //PrintWorld(world);
        }

        return world;
        */
    }

    public static List<int>[,] StepThrough(List<int>[,] world) {
        int width = world.GetLength(0);
        int height = world.GetLength(1);



        Vector2Int position = GetPositionLowestEntropy(world);
        world = CollapseWave(world, position);
        world = Propagate(world, position);

        return world;
    }

    private static void PrintWorld(List<int>[,] world) {

        int x = -1;
        int y = -1;

        Debug.Log("--------------World------------------");

        for (int i = 0; i < world.GetLength(1); i++) {

            string layer = "";
            for (int j = 0; j < world.GetLength(0); j++) {

                if (world[i, j].Count == 15) {
                    layer = layer + "[ ]   ";
                } else {
                    layer = layer + "[" + world[i, j].Count + "]   ";
                }

                if (world[i, j].Count == 0) {
                    x = i;
                    y = j;
                }
                
            }

            Debug.Log(layer);

        }

        Debug.Log("------------------------------------");
        /*
        if (x != -1) {
            Debug.Log("Left:" + world[x - 1, y][0]);
            Debug.Log("Up:" + world[x, y + 1][0]);
            Debug.Log("Right:" + world[x + 1, y][0]);
            Debug.Log("Down:" + world[x, y - 1][0]);
        }

        */

    }

    private static List<int>[,] Propagate(List<int>[,] world, Vector2Int position) {

        List<int> listAtPosition = world[position.x, position.y];

        List<Vector2Int> temp = FindValidNeighbours(world, position);
        List<Vector2Int> neighbours = new List<Vector2Int>();

        foreach (Vector2Int neighbour in temp) {
            if (world[neighbour.x, neighbour.y].Count > 1) {
                neighbours.Add(neighbour);
            }
        }

        // Base Case 1
        if (neighbours.Count == 0) {
            //Debug.Log("Base Case 1");
            return world;
        }

        List<Vector2Int> changedNeighbours = new List<Vector2Int>();

        foreach (Vector2Int neighbour in neighbours) {
            Vector2Int directionToNeighbour = neighbour - position;
            Direction directionName = CalculateDirectionFromVector(directionToNeighbour);

            List<int> compareList = new List<int>();
            for (int i = 0; i < listAtPosition.Count; i++) {
                compareList.AddRange(AvailableNeighbours2.GetValidTiles(listAtPosition[i], directionName));
            }

            compareList = RemoveDuplicates(compareList);

            List<int> currentNeighbours = world[neighbour.x, neighbour.y];
            List<int> newNeighbours = new List<int>();

            foreach (int item in currentNeighbours) {
                if (compareList.Contains(item)) {
                    newNeighbours.Add(item);
                } else {
                    changedNeighbours.Add(neighbour);
                }
            }

            world[neighbour.x, neighbour.y] = newNeighbours;
        }
        
        if (changedNeighbours.Count > 0) {
            foreach (Vector2Int neighbour in changedNeighbours) {
                world = Propagate(world, neighbour);
            }
        }
        

        return world;
    }

    private static List<int> RemoveDuplicates(List<int> list) {

        List<int> noDuplicates = new List<int>();
        foreach (int item in list) {
            if (!noDuplicates.Contains(item)) {
                noDuplicates.Add(item);
            }
        }
        return noDuplicates;

    }

    private static Direction CalculateDirectionFromVector(Vector2Int directionVector) {

        if (directionVector.Equals(Vector2Int.up)) {
            return Direction.North;
        }
        if (directionVector.Equals(Vector2Int.down)) {
            return Direction.South;
        }
        if (directionVector.Equals(Vector2Int.left)) {
            return Direction.West;
        }
        if (directionVector.Equals(Vector2Int.right)) {
            return Direction.East;
        }

        Debug.LogError("WTF have you inputted??");
        Debug.Log(directionVector);

        return Direction.North;

    }

    private static List<Vector2Int> FindValidNeighbours(List<int>[,] world, Vector2Int center, bool diagonalsIncluded = false) {

        List<Vector2Int> list = new List<Vector2Int>();
        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {

                if ((i == 0 && j == 0) ||
                    (center.x + i < 0) ||
                    (center.x + i >= world.GetLength(0)) ||
                    (center.y + j < 0) ||
                    (center.y + j >= world.GetLength(1))) {

                    continue;

                } else if (!diagonalsIncluded) {

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


    private static List<int>[,] CollapseWave(List<int>[,] world, Vector2Int position) {

        List<int> list = world[position.x, position.y];

        int bias = 0;
        
        if (list.Contains(1)) {
            bias = 10;
        }

        int index = UnityEngine.Random.Range(0, list.Count + bias);
        if (index >= list.Count) {
            list = new List<int>() { 1 };
        } else {
            list = new List<int>() { list[index] };
        }
        
        

        world[position.x, position.y] = list;
        return world;

    }

    private static bool WorldComplete(List<int>[,] world) {
        int width = world.GetLength(0);
        int height = world.GetLength(1);


        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {

                if (world[i, j].Count > 1) {
                    //Debug.Log("World Incomplete");
                    return false;
                }

            }
        }

        return true;

    }

    private static bool ZeroEntropy(List<int>[,] world) {
        int width = world.GetLength(0);
        int height = world.GetLength(1);


        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {

                if (world[i, j].Count == 0) {
                    //Debug.Log("World Incomplete");
                    return true;
                }

            }
        }

        return false;
    }

    private static int[,] ConvertTo2D(List<int>[,] world) {
        int width = world.GetLength(0);
        int height = world.GetLength(1);

        int[,] returnWorld = new int[width, height];
        
        
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {

                returnWorld[i, j] = world[i, j][0];

            }
        }

        return returnWorld;

    }

    private static Vector2Int GetPositionLowestEntropy(List<int>[,] world) {
        int width = world.GetLength(0);
        int height = world.GetLength(1);


        Vector2Int lowestPosition = new Vector2Int(0, 0);
        int lowestValue = int.MaxValue;

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {

                if (world[i,j].Count > 1 && world[i, j].Count < lowestValue) {

                    lowestValue = world[i, j].Count;
                    lowestPosition = new Vector2Int(i, j);

                }
            }
        }

        return lowestPosition;


    }



}
