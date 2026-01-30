using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu]
public class Level3DGemetry : ScriptableObject
{
    [Header("Maze 3D Geometry")]
    [SerializeField] private List<GameObject> roomGeometry;
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




    public GameObject GetRandomElement(string inputKey)
    {
        switch (inputKey)
        {
            case "room":
                if (roomGeometry.Count == 1)
                    return roomGeometry.FirstOrDefault();

                if (roomGeometry.Count > 1)
                    return roomGeometry.ElementAtOrDefault(Random.Range(0, roomGeometry.Count));

                return null;
                

            case "horizontalWall":
                if (roomGeometry.Count == 1)
                    return horizontalWallGeometry.FirstOrDefault();

                if (roomGeometry.Count > 1)
                    return horizontalWallGeometry.ElementAtOrDefault(Random.Range(0, roomGeometry.Count));

                return null;

            case "verticalWall":
                if (roomGeometry.Count == 1)
                    return verticalWallGeometry.FirstOrDefault();

                if (roomGeometry.Count > 1)
                    return verticalWallGeometry.ElementAtOrDefault(Random.Range(0, roomGeometry.Count));

                return null;
                
            case "verticalPassage":
                if (roomGeometry.Count == 1)
                    return verticalPassageGeometry.FirstOrDefault();

                if (roomGeometry.Count > 1)
                    return verticalPassageGeometry.ElementAtOrDefault(Random.Range(0, roomGeometry.Count));

                return null;

            case "horizontalPassage":

                if (roomGeometry.Count == 1)
                    return horizontalPassageGeometry.FirstOrDefault();

                if (roomGeometry.Count > 1)
                    return horizontalPassageGeometry.ElementAtOrDefault(Random.Range(0, roomGeometry.Count));

                return null;

            case "pillar":
                if (roomGeometry.Count == 1)
                    return pillarGeometry.FirstOrDefault();

                if (roomGeometry.Count > 1)
                    return pillarGeometry.ElementAtOrDefault(Random.Range(0, roomGeometry.Count));

                return null;

            case "exitKey":
                if (roomGeometry.Count == 1)
                    return exitKeyGeometry.FirstOrDefault();

                if (roomGeometry.Count > 1)
                    return exitKeyGeometry.ElementAtOrDefault(Random.Range(0, roomGeometry.Count));

                return null;

            case "pickup":
                if (roomGeometry.Count == 1)
                    return pickupGeometry.FirstOrDefault();

                if (roomGeometry.Count > 1)
                    return pickupGeometry.ElementAtOrDefault(Random.Range(0, roomGeometry.Count));

                return null;

            default:
                Debug.LogWarning("Invalid input key for getting the random element!");
                return null;
                
        }
                

    }

    

}
