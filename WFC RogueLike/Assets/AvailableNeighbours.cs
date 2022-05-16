using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { North, East, South, West }

public class AvailableNeighbours : MonoBehaviour {

    public int id;

    public List<GameObject> north;
    public List<GameObject> east;
    public List<GameObject> south;
    public List<GameObject> west;
















    /*
    private Dictionary<Direction, List<GameObject>> validTilesInDirection;
    public static Dictionary<string, Dictionary<Direction, List<GameObject>>> TileAvailableNeighbours;



    private void Awake() {

        if (TileAvailableNeighbours == null) {
            TileAvailableNeighbours = new Dictionary<string, Dictionary<Direction, List<GameObject>>>();
        }

        if (gameObject.name.Contains("(Clone)")) {
            gameObject.name = gameObject.name.Split('(')[0];
        }

        if (!TileAvailableNeighbours.ContainsKey(gameObject.name)) {


            validTilesInDirection = new Dictionary<Direction, List<GameObject>>();
            validTilesInDirection.Add(Direction.North, north);
            validTilesInDirection.Add(Direction.East, east);
            validTilesInDirection.Add(Direction.South, south);
            validTilesInDirection.Add(Direction.West, west);

            TileAvailableNeighbours.Add(gameObject.name, validTilesInDirection);


        }

        
    }

    



    public static List<GameObject> GetAvailableTiles(GameObject target, Direction direction) {

        //Debug.Log("TARGET NAME: " + target.name);

        if (TileAvailableNeighbours.ContainsKey(target.name)) {
            return TileAvailableNeighbours[target.name][direction];
        } else {
            
            Debug.LogError("Target does not exist");
            return null;
        }
        
    }

    */

}
