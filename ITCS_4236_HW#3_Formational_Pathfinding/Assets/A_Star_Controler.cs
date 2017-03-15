using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class A_Star_Controler
{

    //stack of moves to make
    public Stack movesToMake = new Stack();

    int[] curentTile;
    Ground_Tile curentTileObj;

    int strMoveCost = 10;
    int diagMoveCost = 15;

    //must be resorted by Ground_Tile.F on all additions and deleations
    private MinHeap<Ground_Tile> oppenList = new MinHeap<Ground_Tile>();

    private Dictionary<int, Ground_Tile> closedList = new Dictionary<int, Ground_Tile>();

    //the board script in the Ground object
    public Tile_Board Board = GameObject.Find("Ground").GetComponent<Tile_Board>();

    public A_Star_Controler(Vector3 startLocaiton, Vector3 endLocaiton)
    {
        //Debug.Log("A* constructer with passed \n startLocaiton ="+ startLocaiton+ "\n endLocation = "+ endLocaiton);
        //setting the start and end points on the baord
        Board.setStartEndPoints(startLocaiton, endLocaiton);

        //adding the start tile to the oppen list and setting it as the curent Tile
        curentTile = Board.worldToTile(startLocaiton);
        curentTileObj = Board.board[curentTile[0], curentTile[1]];
        oppenList.Insert(curentTileObj);

        //looping thrue untill hitting the end point
        while (oppenList.Count > 0)
        {
            //setting the curent tile to the next one in the oppen list
            curentTileObj = oppenList.ExtractMin();
            curentTile[0] = curentTileObj.tileX;
            curentTile[1] = curentTileObj.tileZ;

            //cheking to see if you are at the end
            if (curentTileObj.end)
            {
                createPath(curentTileObj);
                break;
            }

            //else creating the tiles adjacent to curent tile
            createNeighbors();
            //adding curent tile to closed list
            //Debug.Log("adding Ground_Tile at Board Position \n" + curentTile[0] + " "+ curentTile[1]+  " \n To closed list");
            closedList.Add(curentTile[0] * Board.board.GetLength(1) + curentTile[1], curentTileObj);
        }

    }

    private void createNeighbors()
    {
        int startI = curentTile[0] - 1;
        int maxI = curentTile[0] + 1;
        int startJ = curentTile[1] - 1;
        int maxJ = curentTile[1] + 1;
        //cheking to see if you are at the edge of the board array
        if (curentTile[0] == 0)
            startI += 1;
        else if (curentTile[0] == Board.board.GetLength(1))
            maxI -= 1;
        if (curentTile[1] == 0)
            startJ += 1;
        else if (curentTile[1] == Board.board.GetLength(0))
            maxJ -= 1;

        //Debug.Log("Creating Neibors for tile " + curentTile[0] + " " + curentTile[1]);
        //looping thru adjacent tiles
        for (int i = startI; i <= maxI; i++)
        {
            //Debug.Log("maxI = " + maxI);
            //Debug.Log("curent i = " + i);
            for (int j = startJ; j <= maxJ; j++)
            {
                //skipping over curent tile
                if (i == curentTile[0] && j == curentTile[1])
                    j++;
                //Debug.Log("maxJ = " + maxJ);
                //Debug.Log("curent j = " + j);
                int[] lookLocation = new int[2];
                lookLocation[0] = i;
                lookLocation[1] = j;
                int newG = strMoveCost;
                Ground_Tile lookTile = Board.board[i, j];
                //setting the move increace to diagonal or streate values
                if (i != curentTile[0] && j != curentTile[1])
                    newG = diagMoveCost;

                //Debug.Log("neighbor " + i + " " + j + " is pathable ="+ lookTile.pathable);
                //Debug.Log("neighbor " + i + " " + j + " is in closed list =" + closedList.ContainsKey(lookLocation[0] + "" + lookLocation[1]));
                //cheking to see if the tile is pathable or if it is in the closed list
                if (lookTile.pathable == true && !closedList.ContainsKey(lookLocation[0] * Board.board.GetLength(1) + lookLocation[1]))
                {
                    //Debug.Log("neighbor "+ i +" "+j +" is pathable and not in closed list");
                    //setting its G equal to the cost of its position from the curent point added to the curent points G
                    newG += curentTileObj.G;
                    //cheking if the new tile is in the oppen list
                    if (oppenList.contains(lookTile))
                    {
                        //Debug.Log(i + " " + j + " is in oppen list");
                        //check to see if its g is smaller or greater then the curent one
                        if (lookTile.G < oppenList.Peek(lookTile).G)
                        {
                            int heapIndexOfLook = oppenList.PeekL(lookTile);
                            //change the G value and Parent
                            oppenList.changeValuesAt(heapIndexOfLook, newG, curentTileObj);
                        }
                    }
                    else
                    {
                        //Debug.Log(i + " " + j + " is new");
                        //setting the tiles values and puting it in the oppen list
                        lookTile.updateValues(newG);
                        lookTile.parent = curentTileObj;
                        oppenList.Insert(lookTile);
                    }
                }
            }
        }
    }

    private void createPath(Ground_Tile end)
    {
        Debug.Log("path found");
        Debug.Log("end tile location =" + end.tileX + "" + end.tileZ);
        Ground_Tile curentTile = end;
        while (curentTile.parent != null)
        {
            Debug.Log("previose tile location in path =" + curentTile.tileX + "" + curentTile.tileZ);
            movesToMake.Push(curentTile.tileLocation);
            curentTile = curentTile.parent;
        }
        if (curentTile.start)
        {
            Debug.Log("start tile location =" + curentTile.tileX + "" + curentTile.tileZ);
            movesToMake.Push(curentTile.tileLocation);
        }
    }
}
