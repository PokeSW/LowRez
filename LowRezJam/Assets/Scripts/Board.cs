using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{//                                                                                                         Key
    float clearWeight = 0.3f;//                                                                             0 = clear moveable tile no effects              Base Weight = 30%
    float solidWeight = 0.3f;//                                                                             1 = solid tile, cant move into                  Base Weight = 30%
    float bEastWeight = 0.2f;//                                                                             2 = boost tile East, moves player forward one   Base Weight = 20%
    float bNorthWeight = 0.2f;//                                                                            3 = boost tile North, Moves player up one       Base Weight = 20%
    float bSouthWeight = 0.2f;//                                                                            4 = boost tile South, Moves player down one     Base Weight = 20%
    int[,] pLocation = new int[1, 2];// value stored in 1,1 is column, value stored in 1,2 is row
    int[,] board = new int[5, 5] {
        {0,0,0,0,0},//row 1
        {0,0,0,0,0},//row 2
        {0,0,0,0,0},//row 3
        {0,0,0,0,0},//row 4
        {0,0,0,0,0}};//row 5
    bool completeable=false;
    int[,] reachableTiles = new int[1, 5]; //fill in CanComplete(); with tiles in column 4 that the player can reach
    public void Moved()
    {
        //update pLocation to its new location

        //move board back 1 and generate new column 5
        for (int i = 1; i <= 5; i++)//move board back
        {
            for (int j = 1; j <= 4; j++)
            {
                board[i, j] = board[i, j+1];
            }
        }
        pLocation[1, 1]--;//move player back
        if (pLocation[1,1]<0)//is player off the board
        {
            //losecon, end game
        }
        else
        {
            CalcWeight();
            CanComplete();
            if (!completeable)//player moved to a spot that has lost them the run, spawn 5 walls cause L bozo
            {
                for (int i = 1; i <= 5; i++)
                {
                    board[5, i] = 1;
                }
            }
            else
            {
                int numReachable=0;
                for (int i = 1; i <= 5; i++)
                {
                    if (reachableTiles[1, i] == 1) numReachable++;
                }
                if (numReachable == 5)//player can reach all 5 tiles, so just pure random on the spawning
                {
                    EnsureNot5Walls();
                }
                else//1 or more reachable but not all, close to random spawning but ensure 1 possible way from one of the reachables
                {
                    bool routeFound = false;
                    int forcedIndex=0;
                    while (!routeFound)
                    {
                        int selected = Random.Range(1, 5);
                        if (reachableTiles[1, selected] == 1)
                        {
                            forcedIndex = selected;
                            routeFound = true;
                        }
                    }
                    TwoToFourReachable(forcedIndex);
                }
            }
        }
    }
    private void TwoToFourReachable(int reachable)
    {
        for (int i = 1; i <= 5; i++)
        {
            if (i == reachable)//guarentee coninueable, aka cant be solid
            {
                int newTile = 0;
                float randomValue = Random.Range(0f, 1f);
                if (randomValue <= clearWeight ) newTile = 0;
                else if (randomValue <= clearWeight + solidWeight + bEastWeight) newTile = 2;
                else if (randomValue <= clearWeight + solidWeight + bEastWeight + bNorthWeight) newTile = 3;
                else newTile = 4;
                board[5, i] = newTile;
            }
            else
            {
                int newTile = 0;
                float randomValue = Random.Range(0f, 1f);
                if (randomValue <= clearWeight) newTile = 0;
                else if (randomValue <= clearWeight + solidWeight) newTile = 1;
                else if (randomValue <= clearWeight + solidWeight + bEastWeight) newTile = 2;
                else if (randomValue <= clearWeight + solidWeight + bEastWeight + bNorthWeight) newTile = 3;
                else newTile = 4;
                board[5, i] = newTile;
            }
        }
    }
    private void EnsureNot5Walls()
    {
        int wallCount = 0;
        for (int i = 1; i <= 5; i++)
        {
            int newTile = 0;
            float randomValue = Random.Range(0f, 1f);
            if (randomValue <= clearWeight) newTile = 0;// probably the worse way someone has ever implemented a system like this but like... it works....
            else if (randomValue <= clearWeight + solidWeight) { newTile = 1; wallCount++; }
            else if (randomValue <= clearWeight + solidWeight + bEastWeight) newTile = 2;
            else if (randomValue <= clearWeight + solidWeight + bEastWeight + bNorthWeight) newTile = 3;
            else newTile = 4;
            board[5, i] = newTile;
        }
        if (wallCount >= 5)
        {
            EnsureNot5Walls();
        }
    }
    private void CanComplete()//check to see if player can reach the current column 4
    {
        completeable = false;
        //check, if so then fill reachabletiles with correct ones
        completeable = true;
    }
    private void CalcWeight()
    {
        clearWeight = 0.3f; solidWeight = 0.3f; bEastWeight = 0.2f; bNorthWeight = 0.2f; bSouthWeight = 0.2f; //reset to base values
        if (pLocation[1, 1] >= 1)// if player is in the back 2 columns, increase weight of BEast tiles decreasing clear
        {
            bEastWeight += .1f;
            clearWeight -= .1f;
        }
        else if (pLocation[1, 1] >= 1)//if player is in the front 2 columns, increase weight of Solid Tiles decreasing BEast
        {
            solidWeight += .1f;
            bEastWeight -= .1f;
        }
    }
}
