using System;
using System.Collections.Generic;
using UnityEngine; 

public enum MazeCell
{
    Wall,
    Path,
    Border
}

public class MazeGenerator
{
    public List<List<MazeCell>> CreateBaseMaze(int width = 7, int height = 7)
    {
        if (width % 2 == 0)
            width += 1;
        if (height % 2 == 0)
            height += 1;

        var array = new List<List<MazeCell>>();

        // Fill base maze with walls
        for (int i = 0; i < height; i++)
        {
            var row = new List<MazeCell>();
            for (int j = 0; j < width; j++)
                row.Add(MazeCell.Wall);

            array.Add(row);
        }

        // Top and bottom borders
        var borderRow = new List<MazeCell>();
        for (int i = 0; i < width + 2; i++)
            borderRow.Add(MazeCell.Border);

        array.Insert(0, new List<MazeCell>(borderRow)); // Top
        array.Add(new List<MazeCell>(borderRow));       // Bottom

        // Left and right borders
        for (int i = 1; i < height + 1; i++)
        {
            array[i].Insert(0, MazeCell.Border); // Left
            array[i].Add(MazeCell.Border);       // Right
        }

        return array;
    }

    public List<List<MazeCell>> MazePath(int width = 7, int height = 7, int seed = 10000000, float density = 0.5f)
    {
        var rng = new System.Random(seed);

        var maze = CreateBaseMaze(width, height);

        var moveList = new List<string> { "d", "r" };
        int pathNum = 2;
        int errorNum = 0;
        var mazeCords = new List<Vector2>();

        int x = 1;
        int y = 1;

        maze[x][y] = MazeCell.Path;

        bool mazeCreate = true;
        mazeCords.Add(new Vector2(y, x));

        while (mazeCreate)
        {
            string choice = moveList[rng.Next(moveList.Count)];

            int previousPathNum = pathNum;
            string previousChoice = choice;

            switch (choice)
            {
                case "u":
                    if (y > 1 && maze[y - 2][x] == MazeCell.Wall)
                    {
                        maze[y - 2][x] = MazeCell.Path;
                        maze[y - 1][x] = MazeCell.Path;
                        y -= 2;
                        pathNum++;
                    }
                    break;

                case "d":
                    if (y < height && maze[y + 2][x] == MazeCell.Wall)
                    {
                        maze[y + 2][x] = MazeCell.Path;
                        maze[y + 1][x] = MazeCell.Path;
                        y += 2;
                        pathNum++;
                    }
                    break;

                case "l":
                    if (x > 1 && maze[y][x - 2] == MazeCell.Wall)
                    {
                        maze[y][x - 2] = MazeCell.Path;
                        maze[y][x - 1] = MazeCell.Path;
                        x -= 2;
                        pathNum++;
                    }
                    break;

                case "r":
                    if (x < width && maze[y][x + 2] == MazeCell.Wall)
                    {
                        maze[y][x + 2] = MazeCell.Path;
                        maze[y][x + 1] = MazeCell.Path;
                        x += 2;
                        pathNum++;
                    }
                    break;
            }

            if (previousPathNum == pathNum)
            {
                errorNum++;
                moveList.Remove(previousChoice);
            }
            else
            {
                errorNum = 0;
                moveList = new List<string> { "u", "d", "l", "r" };
                mazeCords.Add(new Vector2(y, x));
            }

            if (errorNum == 4)
            {
                errorNum = 0;
                pathNum--;
                moveList = new List<string> { "u", "d", "l", "r" };
                previousPathNum = pathNum;
                mazeCords.RemoveAt(mazeCords.Count - 1);

                if (mazeCords.Count > 0 && pathNum - 2 >= 0)
                {
                    var current = mazeCords[pathNum - 2];
                    x = (int)current.y;
                    y = (int)current.x;
                }
                else
                {
                    break;
                }
            }
        }

        for (int i = 0; i < maze.Count; i++)
        {
            for (int j = 0; j < maze[i].Count; j++)
            {
                if (maze[i][j] == MazeCell.Wall && rng.NextDouble() > density)
                {
                    maze[i][j] = MazeCell.Path;
                }
            }
        }
        return maze;
    }
}