using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] int boardWidth = 5, boardHeight = 5;
    int xValue;
    int yValue;
    [SerializeField] float tileSize = 1;

    [SerializeField] Vector2 currentTile = Vector2.zero;

    void Start()
    {
        xValue = boardWidth/2;
        yValue = boardHeight/2;
    }

    void Update()
    { 
        // Vertical Movement
        if (Input.GetKeyDown(KeyCode.W) && currentTile.y < yValue)
        {
            currentTile.y++;
        }
        else if(Input.GetKeyDown(KeyCode.S) && currentTile.y > -yValue)
        {
            currentTile.y--;
        }

        // Horizontal Movement
        else if (Input.GetKeyDown(KeyCode.A) && currentTile.x > -xValue)
        {
            currentTile.x--;
        }
        else if (Input.GetKeyDown(KeyCode.D) && currentTile.x < xValue)
        {
            currentTile.x++;
        }

        transform.position = new Vector3 (currentTile.x * tileSize, currentTile.y * tileSize, 0.0f);
    }
}
