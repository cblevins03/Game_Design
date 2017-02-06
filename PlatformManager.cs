using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class PlatformManager : MonoBehaviour {

    public int columns = 20;                                          //Number of columns, how far right the level is.  Farthest right top row = (columns, rows)
    public int rows = 8;                                             //Number of rows, how high the level is.  Bottom left = (0,0)
    public GameObject platform;                                    //Array to hold platform tiles to be chosen from
    public GameObject fillerTile;                                  //Array to hold filler tiles that will be placed from (column,platformLocation-1) -> (column,0)
    public GameObject exit;                                        //Array to hold the exit tile that will be placed above the last platform tile.  This will restart the level and add one to the World count.
    public GameObject deadzone;                                    //Array to hold the Dead Zone tiles which will be placed at the bottom of each space column.  This will restart the player when they fall down a space.
    public GameObject entrance;
    private int columnCount = 0;                                     //A variable to hold the value of the current column.
    private int rowCount = 0;                                        //A variable to hold the value of the current row.
    private Transform boardHolder;                                   //A variable to store a reference to the transform of our Board object
    private List<Vector3> gridPositions = new List<Vector3>();       //A list of possible locations to place tiles.


    /*Clears our list gridPositions and prepares it to generate a new board.
    void InitializeList()
    {
        //Clear our list gridPositions.
        gridPositions.Clear();

        //Loop through x axis (columns).
        for (int x = 0; x < columns; x++)
        {
            //Within each column, loop through y axis (rows).
            for (int y = 0; y < rows; y++)
            {
                //At each index add a new Vector3 to our list with the x and y coordinates of that position.
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    /*Sets up the background tiles 
    void BackgroundSetup()
    {
        //Instantiate Board and set boardHolder to its transform.
        boardHolder = new GameObject("Board").transform;

        //Loop along x axis
        for (int x = 0; x < columns; x++)
        {
            //Loop along y axis
            for (int y = 0; y < rows; y++)
            {
                //Choose a random tile from our array of background tiles and prepares to instantiate it.
                GameObject toInstantiate = background[Random.Range(0, background.Length)];

                //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    //RandomPosition returns a random position from our list gridPositions.
    Vector3 RandomPosition()
    {
        //Declare an integer randomIndex, set it's value to a random number between rows + 1 and rows - 1.
        int randomIndex = Random.Range(rowCount-1, rowCount+1);

        //Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
        Vector3 randomPosition = gridPositions[randomIndex];

        //Remove the entry at randomIndex from the list so that it can't be re-used.
        gridPositions.RemoveAt(randomIndex);

        //Return the randomly selected Vector3 position.
        return randomPosition;
    }
    */
    //LayoutNextPlatformTile accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.  Lays out the first tile within the bottom and halfway up the rows.
    //This avoids the player being originally placed at a high altitude or even offscreen.
    int LayoutNextPlatformTile(GameObject tileArray, int columnPosition, int minimum, int maximum)
    {
        //Sets the value of rowCount to the value of the row when the new tile is chosen between rowCount-1 and rowCount+1.
        int rowPosition = Random.Range(minimum, maximum);

        //Choose a position for the next platform tile.
        Vector3 randomPosition = new Vector3(columnPosition, rowPosition, 0f);

        //Choose a random tile from tileArray and assign it to tileChoice
        //GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];

        //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
        Instantiate(tileArray, randomPosition, Quaternion.identity);

        return rowPosition;
    }

    //Places filler tiles under each platform tile.
    void PlaceFillerTiles(GameObject tileArray, int columnPosition, int rowPosition)
    {
        //Loops from current row-1 to row=0.
        for (int i = rowPosition-1; i >= 0; i--)
        {
            //Choose a position for the next filler tile.
            Vector3 randomPosition = new Vector3(columnPosition, i, 0f);

            //Choose a random tile from tileArray and assign it to tileChoice
            //GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];

            //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
            Instantiate(tileArray, randomPosition, Quaternion.identity);
        }
    }
    //SetupScene initializes the level and calls the previous functions to lay out the game board
    public void SetupScene(int level)
    {
        //Reset our list of gridpositions.
        //InitializeList();

        //Creates the background of the level.
        //BackgroundSetup();

        rowCount = rows;

        //Instatiate the beginning tile from the bottom of the level to halway up the rows.
        //This sets the player at a midway or lower level to avoid the player starting offscreen.
        rowCount = LayoutNextPlatformTile(platform, columnCount, rowCount/4, rowCount/4);

        rowCount = LayoutNextPlatformTile(entrance, columnCount, rowCount + 1, rowCount + 1);
        //Instantiate a series of filler tiles under the Platform tile to the bottom of the level.
        PlaceFillerTiles(fillerTile, columnCount, rowCount);

        columnCount++;

        //For the length of columns create the platforms
        for (int i = 1; i < columns; i++)
        {
            //Avoids trying to place a Platform Tile below row=0.
            if (rowCount==0)
            {
                rowCount++;
            }
            //Creates a random number that chooses whether a platform tile or space will be placed.  Platform tiles are more likely. 0-3 = Space; 4-10 = Platform Tile
            int platformOrSpace = Random.Range(0,10);

            //Chooses if a Platform tile or Space will be placed.  Platform tiles are more likely. 0->1 = Space; 2->10 = Platform Tile
            if (platformOrSpace >= 2)
            {
                //Instantiate a Platform Tile in the location provided and assigns the new rowCount value.
                rowCount = LayoutNextPlatformTile(platform, columnCount, rowCount-1, rowCount+2);

                //Instantiate a series of filler tiles under the Platform tile to the bottom of the level.
                PlaceFillerTiles(fillerTile, columnCount, rowCount);

                //Go to the next column;
                columnCount++;
            }
            else
            {
                LayoutNextPlatformTile(deadzone, columnCount, 0, 0);

                //Go to the next column without placing a platform tile, producing 
                columnCount++;
            }
        }

        //Place the exit tile above the last platform tile.
        LayoutNextPlatformTile(exit, columnCount-1, rowCount+1, rowCount+1);

    }
}
