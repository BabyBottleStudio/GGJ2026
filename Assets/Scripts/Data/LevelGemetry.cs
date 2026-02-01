using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu]
public class LevelGemetry : ScriptableObject
{
    [Header("Maze 3D Geometry")]
    [SerializeField] private List<GameObject> roomGeometry;
    [SerializeField] private List<GameObject> npcScenesGeometry;
    [Space(10)]
    [SerializeField] private List<GameObject> horizontalWallGeometry;
    [SerializeField] private List<GameObject> verticalWallGeometry;
    [Space(10)]
    [SerializeField] private List<GameObject> horizontalPassageGeometry;
    [SerializeField] private List<GameObject> verticalPassageGeometry;
    [Space(10)]
    [SerializeField] private List<GameObject> pillarGeometry;
    [Space(10)]
    [SerializeField] private List<GameObject> exitKeyGeometry;
    [Space(10)]
    [SerializeField] private List<GameObject> pickupGeometry;
    [Space(10)]
    [SerializeField] private List<GameObject> firstRoomGeometry;
    [SerializeField] private List<GameObject> exitRoomGeometry;


    //HashSet<GameObject> npcRandomSet;

    //void RandomizeNPCList()
    //{
    //    npcRandomSet = new HashSet<GameObject>();

    //    for (int i = 0; i <= 10; i++)
    //    {
    //        var elementToAdd = npcScenesGeometry.ElementAt(Random.Range(0, npcScenesGeometry.Count));
    //        var test = npcRandomSet.Add(elementToAdd);

    //        Debug.Log($"{test} Game Object added to hashlist");
    //    }

    //}

    public Stack<GameObject> ShuffleToStack()
    {
        // napravi kopiju da ne diraš original
        List<GameObject> temp = new List<GameObject>(npcScenesGeometry);

        // Fisher–Yates shuffle
        for (int i = temp.Count - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);
            (temp[i], temp[rnd]) = (temp[rnd], temp[i]);
        }

        return new Stack<GameObject>(temp);
    }


    public GameObject GetRandomElement(string inputKey)
    {
        switch (inputKey)
        {
            case "room":
                if (roomGeometry.Count == 1)
                    return roomGeometry.FirstOrDefault();

                if (roomGeometry.Count > 1)
                    return roomGeometry.ElementAtOrDefault(Random.Range(0, npcScenesGeometry.Count));

                return null;

            case "npc":
                if (npcScenesGeometry.Count == 1)
                    return npcScenesGeometry.FirstOrDefault();

                if (npcScenesGeometry.Count > 1)
                    return npcScenesGeometry.ElementAtOrDefault(Random.Range(0, npcScenesGeometry.Count));

                return null;

            case "firstRoom":
                if (firstRoomGeometry.Count == 1)
                    return firstRoomGeometry.FirstOrDefault();

                if (firstRoomGeometry.Count > 1)
                    return firstRoomGeometry.ElementAtOrDefault(Random.Range(0, roomGeometry.Count));

                return null;

            case "exitRoom":
                if (exitRoomGeometry.Count == 1)
                    return exitRoomGeometry.FirstOrDefault();

                if (exitRoomGeometry.Count > 1)
                    return exitRoomGeometry.ElementAtOrDefault(Random.Range(0, roomGeometry.Count));

                return null;
            case "horizontalWall":
                if (horizontalWallGeometry.Count == 1)
                    return horizontalWallGeometry.FirstOrDefault();

                if (horizontalWallGeometry.Count > 1)
                    return horizontalWallGeometry.ElementAtOrDefault(Random.Range(0, roomGeometry.Count));

                return null;

            case "verticalWall":
                if (verticalWallGeometry.Count == 1)
                    return verticalWallGeometry.FirstOrDefault();

                if (verticalWallGeometry.Count > 1)
                    return verticalWallGeometry.ElementAtOrDefault(Random.Range(0, roomGeometry.Count));

                return null;
                
            case "verticalPassage":
                if (verticalPassageGeometry.Count == 1)
                    return verticalPassageGeometry.FirstOrDefault();

                if (verticalPassageGeometry.Count > 1)
                    return verticalPassageGeometry.ElementAtOrDefault(Random.Range(0, roomGeometry.Count));

                return null;

            case "horizontalPassage":

                if (horizontalPassageGeometry.Count == 1)
                    return horizontalPassageGeometry.FirstOrDefault();

                if (horizontalPassageGeometry.Count > 1)
                    return horizontalPassageGeometry.ElementAtOrDefault(Random.Range(0, roomGeometry.Count));

                return null;

            case "pillar":
                if (pillarGeometry.Count == 1)
                    return pillarGeometry.FirstOrDefault();

                if (pillarGeometry.Count > 1)
                    return pillarGeometry.ElementAtOrDefault(Random.Range(0, roomGeometry.Count));

                return null;

            case "exitKey":
                if (exitKeyGeometry.Count == 1)
                    return exitKeyGeometry.FirstOrDefault();

                if (exitKeyGeometry.Count > 1)
                    return exitKeyGeometry.ElementAtOrDefault(Random.Range(0, roomGeometry.Count));

                return null;

            case "pickup":
                if (pickupGeometry.Count == 1)
                    return pickupGeometry.FirstOrDefault();

                if (pickupGeometry.Count > 1)
                    return pickupGeometry.ElementAtOrDefault(Random.Range(0, roomGeometry.Count));

                return null;

            default:
                Debug.LogWarning("Invalid input key for getting the random element!");
                return null;
                
        }
                

    }

    

}
