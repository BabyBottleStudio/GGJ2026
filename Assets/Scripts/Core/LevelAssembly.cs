using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class LevelAssembly : MonoBehaviour
{
    [SerializeField] private LevelGemetry levelGeometry;

    [SerializeField] int mazeSize = 8;
    int roomSize = 5;
    public int chanceToSpawnNPC;

    MazeGenerator mazeGenerator;

    Stack<GameObject> npcPool;

    char[,] maze;

    private void Awake()
    {
        mazeGenerator = new MazeGenerator(mazeSize);
        maze = mazeGenerator.Maze;

        if (maze == null)
        {
            Debug.LogError("Maze is not initialized!");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(DrawDataMaze(maze));
        npcPool = levelGeometry.ShuffleToStack();
        DrawMaze(maze);
    }





    public string DrawDataMaze(char[,] lavirintToDisplay)
    {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < lavirintToDisplay.GetLength(0); i++)
        {
            for (int j = 0; j < lavirintToDisplay.GetLength(1); j++)
            {
                sb.Append($"{lavirintToDisplay[i, j]}");
            }
            sb.AppendLine("");
        }

        return sb.ToString().Trim();
    }

    void DrawHorizontalMazeBorder(float zCoord)
    {
        for (int i = 0; i < mazeSize; i++)
        {
            var newPosition = new Vector3(i * roomSize, 0f, zCoord);

            Instantiate(levelGeometry.GetRandomElement("pillar"), newPosition, Quaternion.identity);
            Instantiate(levelGeometry.GetRandomElement("horizontalWall"), newPosition, Quaternion.identity);
        }
        Instantiate(levelGeometry.GetRandomElement("pillar"), new Vector3(roomSize * mazeSize, 0f, zCoord), Quaternion.identity);
    }

    void DrawVerticalMazeBorder(float xCoord)
    {
        for (int i = 0; i < mazeSize; i++)
        {
            var newPosition = new Vector3(xCoord, 0f, i * roomSize * -1);

            Instantiate(levelGeometry.GetRandomElement("pillar"), newPosition, Quaternion.identity);
            Instantiate(levelGeometry.GetRandomElement("verticalWall"), newPosition, Quaternion.identity);
        }
        Instantiate(levelGeometry.GetRandomElement("pillar"), new Vector3(xCoord, 0f, mazeSize * roomSize * -1), Quaternion.identity);
    }

    void DrawPillars()
    {
        for (int i = 1; i < mazeSize; i++)
        {
            //Console.Write($" {verticallWall} ");
            for (int j = 1; j < mazeSize; j++)
            {

                Instantiate(levelGeometry.GetRandomElement("pillar"), new Vector3(i * roomSize, 0f, j * roomSize * -1), Quaternion.identity);
            }
        }
    }

    void DrawRooms()
    {

        for (int i = 0; i < maze.GetLength(0); i++)
        {
            //Console.Write($" {verticallWall} ");
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                var pos = new Vector3(j / 2 * roomSize, 0f, i / 2 * -roomSize); // kad sam invertovao koordinate sve je proradilo. Umoran da skontam zašto :)


                if (maze[i, j] == 'C')
                {
                    Instantiate(levelGeometry.GetRandomElement("room"), pos, Quaternion.identity);
                    Instantiate(levelGeometry.GetRandomElement("pickup"), pos + new Vector3(2.5f, 0f, -2.5f), Quaternion.identity);
                }
                else if (maze[i, j] == 'K')
                {
                    Instantiate(levelGeometry.GetRandomElement("room"), pos, Quaternion.identity);
                    Instantiate(levelGeometry.GetRandomElement("exitKey"), pos + new Vector3(2.5f, 0f, -2.5f), Quaternion.identity);
                    
                    Instantiate(levelGeometry.GetRandomElement("exitKeyStand"), pos + new Vector3(2.5f, 0f, -2.5f), Quaternion.identity);


                }
                else if (maze[i, j] == 'E')
                {
                    // izgenerisi izlaz
                    Instantiate(levelGeometry.GetRandomElement("exitRoom"), pos, Quaternion.identity);
                }
                else if (maze[i, j] == 'P')
                {
                    Instantiate(levelGeometry.GetRandomElement("firstRoom"), pos, Quaternion.identity);
                    // generisi plejera
                }
                else if(maze[i, j] == 'v')
                {

                    Instantiate(levelGeometry.GetRandomElement("room"), pos, Quaternion.identity);

                    int test = Random.Range(0, 100);

                    if (test < chanceToSpawnNPC && npcPool.Count > 0)
                    {
                        Instantiate(npcPool.Pop(), pos, Quaternion.identity);
                    }
                   
                }
                else
                {
                    continue;
                }

            }
        }
    }


    void DrawHorizontalMazeWalls()
    {

        for (int i = 1; i < maze.GetLength(0); i += 2)
        {
            var parent = new GameObject($"{i}_horizontalWall_parent");

            //Console.Write($" {verticallWall} ");
            for (int j = 0; j < maze.GetLength(1); j += 2)
            {
                var zCoord = (i+1) / 2 * roomSize * -1;
                var xCoord = j / 2 * roomSize;
                //if (maze[i, j] == '+')
                //    continue;

                if (maze[i, j] == '-')
                {
                    Instantiate(levelGeometry.GetRandomElement("horizontalWall"), new Vector3(xCoord, 0f, zCoord), Quaternion.identity, parent.transform); // mora i+1 jer ona krene od 0 i napuni mi gornju ogradu
                }
                else if (maze[i, j] == ' ')
                {
                    Instantiate(levelGeometry.GetRandomElement("horizontalPassage"), new Vector3(xCoord, 0f, zCoord), Quaternion.identity, parent.transform);

                }
                else
                {
                    Debug.LogWarning("DrawHorizontalMazeWalls: Invalid data in the matrix!");
                }

            }
        }
    }

    void DrawVerticalMazeWalls()
    {

        for (int i = 0; i < maze.GetLength(0); i += 2)
        {
            var parent = new GameObject($"{i}_verticalWall_parent");

            //Console.Write($" {verticallWall} ");
            for (int j = 1; j < maze.GetLength(1); j += 2)
            {
                //if (maze[i, j] == '+')
                //    continue;

                var zCoord = i / 2 * roomSize * -1;
                var xCoord = (j+1) / 2 * roomSize;

                if (maze[i, j] == '|')
                {
                    Instantiate(levelGeometry.GetRandomElement("verticalWall"), new Vector3(xCoord, 0f, zCoord), Quaternion.identity, parent.transform); // mora i+1 jer ona krene od 0 i napuni mi gornju ogradu
                }
                else if (maze[i, j] == ' ')
                {
                    Instantiate(levelGeometry.GetRandomElement("verticalPassage"), new Vector3(xCoord, 0f, zCoord), Quaternion.identity, parent.transform);

                }
                else
                {
                    Debug.LogWarning("DrawVerticalMazeWalls: Invalid data in the matrix!");
                }

            }
        }
    }


    void DrawNPC()
    {

    }
        

    void DrawMaze(char[,] lavirintToDisplay)
    {
        DrawHorizontalMazeBorder(0f);
        DrawHorizontalMazeBorder(roomSize * mazeSize * -1);

        DrawVerticalMazeBorder(0f);
        DrawVerticalMazeBorder(roomSize * mazeSize);

        DrawPillars();
        DrawRooms();
        DrawHorizontalMazeWalls();
        DrawVerticalMazeWalls();
    }
}





