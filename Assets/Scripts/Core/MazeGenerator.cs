using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class MazeGenerator
{
    public char[,] Maze { get; set; }

    int inputSize;
    int realSize;

    //Random rand;

    HashSet<(int row, int col)> deadEnds;


    public MazeGenerator(int inputSize)
    {
        deadEnds = new HashSet<(int row, int col)>();
        //rand = new Random();

        if (inputSize < 4)
        {
            this.inputSize = 4;
        }
        else
        {
            this.inputSize = inputSize;
        }

        realSize = inputSize * 2 - 1;
        this.Maze = new char[realSize, realSize];

        InitializeMazeData();
        GeneratePathways();
        GeneratePlayer();
        //Console.WriteLine($"Deadends count: {deadEnds.Count}");
        GeneratePickups();
        GenerateKeyAndExit();
    }

    void GeneratePlayer()
    {
        this.Maze[0, 0] = 'P';
    }

    void GenerateKeyAndExit()
    {
        if (deadEnds.Count > 0)
        {
            deadEnds = deadEnds.OrderByDescending(x => (x.row + x.col)).ToHashSet();
            var exitCoords = deadEnds.ElementAt(0);
            this.Maze[exitCoords.row, exitCoords.col] = 'E';

            if (deadEnds.Count == 1)
            {
                while (true)
                {
                    int newRow = UnityEngine.Random.Range(0, realSize / 2) * 2;
                    int newCol = UnityEngine.Random.Range(0, realSize / 2) * 2;

                    if (this.Maze[newRow, newCol] == 'v')
                    {
                        this.Maze[newRow, newCol] = 'K';
                        break;
                    }
                }
            }
            else
            {
                int index = UnityEngine.Random.Range(1, deadEnds.Count);
                (int row, int col) keyCoords = deadEnds.ElementAt(index);
                this.Maze[keyCoords.row, keyCoords.col] = 'K';
            }

            foreach (var coord in deadEnds)
            {
                if (this.Maze[coord.row, coord.col] == 'E' || this.Maze[coord.row, coord.col] == 'K' || this.Maze[coord.row, coord.col] == 'P')
                {
                    deadEnds.Remove(coord);
                }
            }

        }
    }

    void GeneratePickups()
    {
        HashSet<(int row, int col)> pickupCoords = new HashSet<(int row, int col)>();

        int coinsCount = inputSize - 3;

        // ako nema deadEndova na raspolaganju prvi je otisao na Exit drugi na kljuc
        // ako ima 3 i vise deadendova: vidi da li je deadends-2 >= coinsCount
        if (deadEnds.Count >= coinsCount) // ako ima vise deadendova nego sto je potrebno
        {
            while (pickupCoords.Count < coinsCount)
            {
                int rndIndex = UnityEngine.Random.Range(0, deadEnds.Count);
                pickupCoords.Add(deadEnds.ElementAt(rndIndex)); // on ovde proba da ubaci ali ako ima duplih elemenata on preskoci da ubaci
            }
        }
        else
        {
            if (deadEnds.Any())
            {
                pickupCoords = pickupCoords.Concat(deadEnds).ToHashSet();
            }


            while (pickupCoords.Count < coinsCount) // ostalo napuni random koordinatama
            {
                //Console.WriteLine("While");

                int newRow = UnityEngine.Random.Range(0, realSize / 2) * 2;
                int newCol = UnityEngine.Random.Range(0, realSize / 2) * 2;

                pickupCoords.Add((newRow, newCol));
            }
        }

        foreach (var coord in pickupCoords)
        {
            this.Maze[coord.row, coord.col] = 'C';
        }
        //Console.WriteLine($"coins count: {pickupCoords.Count}");
    }

    void InitializeMazeData()
    {
        for (int i = 0; i < realSize; i++)
        {
            for (int j = 0; j < realSize; j++)
            {
                if (i % 2 == 0 && j % 2 == 0) // definise sobe
                {
                    Maze[i, j] = ' ';
                    continue;
                }

                if (i % 2 == 0 && j % 2 != 0) // definise vertikalne zidove
                {
                    Maze[i, j] = '|';
                    continue;
                }

                if (i % 2 != 0 && j % 2 == 0) // definise horizontalne zidove
                {
                    Maze[i, j] = '-';
                    continue;
                }

                if (i % 2 != 0 && j % 2 != 0) // definise stubove / polja u koja igrac nikada nece uci
                {
                    Maze[i, j] = '+';
                    continue;
                }

                Maze[i, j] = '+'; // definise prolaze. svi su zatvoreni
            }
        }
    }

    void GeneratePathways()
    {
        Stack<(int row, int col)> rooms = new Stack<(int row, int col)>();

        // kreni od polja 0,0
        rooms.Push((0, 0));
        //int count = 1;

        int count = 0;

        while (rooms.Count > 0)
        {
            (int row, int col) current = rooms.Peek();
            bool isDeadEnd = BreakRandomWall(current.row, current.col, out (int row, int col) nextRoom);

            if (isDeadEnd) // ukoliko si naisao na corsokak, vrati se polje nazad i proveri da li je ono corsokak. Vracaj se nazad dok ne nadjes polje gde mozes da nastavis da generises lavirint dalje
            {
                rooms.Pop();
            }
            else
            {
                //count++;
                rooms.Push(nextRoom);
            }

            count++;

            if (count == (inputSize * inputSize) * inputSize)
            {
                Console.WriteLine("Stuck in a while loop!");
                break;
            }
        }
    }

    Func<int, int, bool> validateCoord = (coord, matrixSize) => coord >= 0 && coord < matrixSize;

    //bool isCoordinateValid<T>(int row, int col, T[,] matrix)
    bool isCoordinateValid(int row, int col) => validateCoord(row, Maze.GetLength(0)) && validateCoord(col, Maze.GetLength(1));

    bool isRoomNotVisited(int row, int col) => Maze[row, col] != 'v'; // || Maze[row, col] != 'o';



    bool BreakRandomWall(int row, int col, out (int row, int col) nextRoom)
    {
        bool isDeadEnd = true;
        bool isFirstTimeVisited = true;

        if (Maze[row, col] == 'v')
        {
            isFirstTimeVisited = false;
        }
        else
        {
            Maze[row, col] = 'v'; // mark as visited
        }


        nextRoom = new(row, col); // ukoliko ne vrati ni jednu sobu vratice ulazne vrednosti, mada ih nece iskoristiti

        Dictionary<string, (int row, int col)> neighbouringRooms = new();

        // severna soba:
        int northRow = row - 2;
        int northCol = col;

        if (isCoordinateValid(northRow, northCol) && isRoomNotVisited(northRow, northCol))
        {
            neighbouringRooms.Add("north", (northRow, northCol));
        }

        // juzna soba:
        int southRow = row + 2;
        int southCol = col;

        if (isCoordinateValid(southRow, southCol) && isRoomNotVisited(southRow, southCol))
        {
            neighbouringRooms.Add("south", (southRow, southCol));
        }

        // istocna soba:
        int eastRow = row;
        int eastCol = col + 2;

        if (isCoordinateValid(eastRow, eastCol) && isRoomNotVisited(eastRow, eastCol))
        {
            neighbouringRooms.Add("east", (eastRow, eastCol));
        }

        // zapadna soba:
        int westRow = row;
        int westCol = col - 2;

        if (isCoordinateValid(westRow, westCol) && isRoomNotVisited(westRow, westCol))
        {
            neighbouringRooms.Add("west", (westRow, westCol));
        }

        // metoda moze da vrati sledece vrednosti
        // ukoliko vrati nulu
        // ukoliko vrati 1
        // ukoliko vrati vise

        // Console.WriteLine(String.Join(" ", neighbouringRooms));

        if (neighbouringRooms.Count > 0)
        {
            //Random rnd = new Random();

            var newRoom = neighbouringRooms.ElementAt(UnityEngine.Random.Range(0, neighbouringRooms.Count));

            nextRoom = neighbouringRooms[newRoom.Key]; // ovde se definise out vrednost koja je presudna za push and pop opcije

            switch (newRoom.Key)
            {
                case "north":
                    Maze[row - 1, col] = ' ';
                    //Maze[row - 2, col] = 'v';
                    break;

                case "south":
                    Maze[row + 1, col] = ' ';
                    //Maze[row + 2, col] = 'v';
                    break;

                case "east":
                    Maze[row, col + 1] = ' ';
                    //Maze[row, col + 2] = 'v';
                    break;

                case "west":
                    Maze[row, col - 1] = ' ';
                    //Maze[row, col - 2] = 'v';
                    break;
            }
            isDeadEnd = false; // not a dead end
        }

        //if (isDeadEnd == true && isFirstTimeVisited == true)
        if (isDeadEnd && isFirstTimeVisited)
        {
            deadEnds.Add((row, col));
            //Console.WriteLine("CorsokakReached");
        }

        return isDeadEnd;
    }
}
