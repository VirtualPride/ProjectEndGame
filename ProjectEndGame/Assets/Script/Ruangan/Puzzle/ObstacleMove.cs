using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    [SerializeField] private PuzzleTesInteract puzzleTesInteract;
    [SerializeField] private PuzzleMove puzzleMove;
    [SerializeField] private Rigidbody2D rb;
    private float moveSpeed = 2.0f;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (puzzleTesInteract.inPuzzle)
        {
            if (puzzleTesInteract.player1Move == true)
            {
                HandleMovementInputPlayer2();
            }
            else if (puzzleTesInteract.player2Move == true)
            {
                HandleMovementInputPlayer1();
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }

    }

    private void HandleMovementInputPlayer1()
    {
        float moveHorizontal = 0f;
        float moveVertical = 0f;
        if (Input.GetKey(KeyCode.W))
            moveVertical = 1f;
        if (Input.GetKey(KeyCode.S))
            moveVertical = -1f;
        if (Input.GetKey(KeyCode.A))
            moveHorizontal = -1f;
        if (Input.GetKey(KeyCode.D))
            moveHorizontal = 1f;
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rb.velocity = movement.normalized * moveSpeed;

        if (Input.GetKey(KeyCode.C))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;

        }

    }

    private void HandleMovementInputPlayer2()
    {
        float moveHorizontal = 0f;
        float moveVertical = 0f;
        if (Input.GetKey(KeyCode.UpArrow))
            moveVertical = 1f;
        if (Input.GetKey(KeyCode.DownArrow))
            moveVertical = -1f;
        if (Input.GetKey(KeyCode.LeftArrow))
            moveHorizontal = -1f;
        if (Input.GetKey(KeyCode.RightArrow))
            moveHorizontal = 1f;
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rb.velocity = movement.normalized * moveSpeed;

        if (Input.GetKey(KeyCode.M))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;

        }

    }
}
