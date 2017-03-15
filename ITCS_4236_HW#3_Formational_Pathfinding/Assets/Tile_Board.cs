using UnityEngine;
using System.Collections;

public class Tile_Board : MonoBehaviour
{
    //number of tiles the board will be divided into
    public static int numTileRow = 30;
    public static int numbTileColumn = 30;

    //the distence the board is floting above the ground
    public float boardYOffset = 3;
    //array holding all tiles
    public Ground_Tile[,] board = new Ground_Tile [numTileRow, numbTileColumn];

    //the dimintions of the tile
    float tileHight, tileWidth;

    //the dimintions of the board
    float boardHight, boardWidth;

    //offset to get in the center of each tile
    float offsetX, offsetZ;

    float sTileX, sTileZ;

    int[] endTileLocation = new int[2];
    int[] startTileLocation = new int[2];
    // Use this for initialization
    void Start()
    {
        //getting the board hight and with
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        boardHight = mesh.bounds.size.x * transform.lossyScale.x;
        boardWidth = mesh.bounds.size.z * transform.lossyScale.z;

        //getting the size the tiles will be
        tileHight = boardHight / numbTileColumn;
        tileWidth = boardWidth / numTileRow;

        //determoning the offfset to get the cordonent in the center of the tile from the left top edge
        offsetX = tileHight / 2;
        offsetZ = tileWidth / 2;

        //the world cordonents of the first tile
        sTileX = ((boardHight / 2) * -1) + offsetX + transform.position.x;
        sTileZ = ((boardWidth / 2) * -1) + offsetZ + transform.position.z;

        //the world cordonents of the curent tile
        float tileX;
        float tileZ;

        //create the tiles in there correct locations
        for (int x = 0; x < numTileRow; x++)
        {
            //shifting curent tile down by the Highth of the tiles
            tileX = sTileX + (tileHight * x);
            for (int z = 0; z < numbTileColumn; z++)
            {
                //shifting curent tile location right by the length of the tiles
                tileZ = sTileZ + (tileHight * z);
                //creating a tile
                board[x, z] = new Ground_Tile(new Vector3(tileX, transform.position.y + boardYOffset, tileZ), x, z);
                //Instantiate(Resources.Load("Obstacle"), new Vector3(tileX, transform.position.y + boardYOffset, tileZ), new Quaternion(0, 0, 0, 1));
            }
        }

        //create empty board of tile objects
    }

    // Update is called once per frame
    void Update()
    {
        Update_Board();
    }

    //updates the patability of each tile in the board
    public void Update_Board()
    {
        RaycastHit hit;

        //raycasting from each tile in the board to see what is under it
        for (int i = 0; i < numTileRow; i++)
        {
            for (int j = 0; j < numbTileColumn; j++)
            {
               // Debug.Log("Board_Tile = [" + i +"]["+j+"]");
                Ray groundLook = new Ray(board[i, j].tileLocation, transform.up * -1 * boardYOffset);
                Debug.DrawRay(board[i, j].tileLocation, groundLook.direction * boardYOffset, Color.blue);
                //cheking to see if the raycast hit the ground 
                if (Physics.Raycast(groundLook, out hit, boardYOffset) && hit.collider.CompareTag("Ground"))
                {
                    Debug.DrawRay(board[i, j].tileLocation, groundLook.direction * boardYOffset, Color.green);
                    board[i, j].pathable = true;
                }
                //the forward position of an Obstacle
                else if (Physics.Raycast(groundLook, out hit, boardYOffset) && hit.collider.CompareTag("Obstacle"))
                {
                    Debug.DrawRay(board[i, j].tileLocation, groundLook.direction * boardYOffset, Color.red);
                    board[i, j].pathable = false;
                }
            }
        }
    }

    public void setHeuristic(int[] tileAddress)
    {
        //calculating how far away the end pont is from the created tile and sets it
        board[tileAddress[0], tileAddress[1]].updateValues((Mathf.Abs(tileAddress[0] - endTileLocation[0]))+(Mathf.Abs(tileAddress[1] - endTileLocation[1]))); 
    }

    public void setG(int[] tileAddress, int newG)
    {
        board[tileAddress[0], tileAddress[1]].updateValues(newG);
    }

    public void setStartEndPoints(Vector3 start, Vector3 end)
    {
        //Debug.Log("Tile_Board setStartEndPoints class with passed \n startLocaiton =" + start + "\n endLocation = " + end);
        board[startTileLocation[0], startTileLocation[1]].start = false;
        board[endTileLocation[0], endTileLocation[1]].end = false;

        endTileLocation = worldToTile(end);
        startTileLocation = worldToTile(start);
        //Debug.Log("startLocaiton =" + startTileLocation[0] +" "+startTileLocation[1] + "\n endLocation = " + endTileLocation[0] + " " + endTileLocation[1]);
        board[startTileLocation[0], startTileLocation[1]].start = true;
        board[endTileLocation[0], endTileLocation[1]].end = true;

        for (int i = 0; i < numTileRow; i++)
        {
            for(int j = 0; j < numbTileColumn; j++)
            {
                setHeuristic(new int[] { i,j});
            }
        }
    }

     public int[] worldToTile(Vector3 worldCord)
    {
        int[] arrayIndex = new int[2];

        //converting to local cordonents
        //Vector3 relotiveCord = transform.InverseTransformPoint(worldCord);
        Vector3 relotiveCord = worldCord;
        arrayIndex[0] = (int)Mathf.Floor((relotiveCord.x + (boardHight/2)) / tileHight);
        arrayIndex[1] = (int)Mathf.Floor((relotiveCord.z + (boardWidth/2)) / tileWidth);

        //Debug.Log(" pont is " + arrayIndex[0] +" " + arrayIndex[1]);
        return arrayIndex;
    }
}
