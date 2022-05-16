using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailableNeighbours2 : MonoBehaviour {



    public List<int> NorthInt;
    public List<int> EastInt;
    public List<int> SouthInt;
    public List<int> WestInt;


    private Dictionary<Direction, List<int>> Dict_GetList = new Dictionary<Direction, List<int>>();
    public static Dictionary<int, Dictionary<Direction, List<int>>> Dict_ID_Direction = new Dictionary<int, Dictionary<Direction, List<int>>>();

    public void Setup() {

        Debug.Log(gameObject.name);
        NorthInt = new List<int>();
        EastInt = new List<int>();
        SouthInt = new List<int>();
        WestInt = new List<int>();

        AvailableNeighbours an = gameObject.GetComponent<AvailableNeighbours>();
        
        
        foreach (GameObject neighbour in an.north) {
            NorthInt.Add(neighbour.GetComponent<AvailableNeighbours>().id);
        }
        foreach (GameObject neighbour in an.east) {
            EastInt.Add(neighbour.GetComponent<AvailableNeighbours>().id);
        }
        foreach (GameObject neighbour in an.south) {
            SouthInt.Add(neighbour.GetComponent<AvailableNeighbours>().id);
        }
        foreach (GameObject neighbour in an.west) {
            WestInt.Add(neighbour.GetComponent<AvailableNeighbours>().id);
        }

        Dict_GetList.Add(Direction.North, NorthInt);
        Dict_GetList.Add(Direction.East, EastInt);
        Dict_GetList.Add(Direction.South, SouthInt);
        Dict_GetList.Add(Direction.West, WestInt);

        Dict_ID_Direction.Add(gameObject.GetComponent<AvailableNeighbours>().id, Dict_GetList);

    }


    public static List<int> GetValidTiles(int id, Direction direction) {

        if (Dict_ID_Direction.ContainsKey(id)) {
            return Dict_ID_Direction[id][direction];
        } else {
            Debug.LogError("Target does not exist");
            return null;
        }


    }

}
