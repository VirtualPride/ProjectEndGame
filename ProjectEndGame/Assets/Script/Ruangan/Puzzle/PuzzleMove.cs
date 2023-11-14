using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleMove : MonoBehaviour
{
    [SerializeField] private PuzzleTesInteract puzzleTesInteract;
    [HideInInspector] public bool isMoving = false;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ObstacleMove obstacleMove;

    public bool isFinish = false;
    public float MoveSpeed = 2.0f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (puzzleTesInteract.inPuzzle)
        {
            if (puzzleTesInteract.player1Move == true)
            {
                HandleMovementInputPlayer1();
            }
            else if (puzzleTesInteract.player2Move == true)
            {
                HandleMovementInputPlayer2();
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }

    }

    private void HandleMovementInputPlayer1()
    {
        if (puzzleTesInteract.player1Move && !isMoving)
        {
            if (Input.GetKey(KeyCode.W))
            {
                StartMoving(Vector2.up);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                StartMoving(Vector2.left);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                StartMoving(Vector2.down);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                StartMoving(Vector2.right);
            }
        }
    }
    private void HandleMovementInputPlayer2()
    {
        if (puzzleTesInteract.player2Move && !isMoving)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                StartMoving(Vector2.up);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                StartMoving(Vector2.left);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                StartMoving(Vector2.down);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                StartMoving(Vector2.right);
            }
        }
    }

    private void StartMoving(Vector2 direction)
    {
        isMoving = true;
        StartCoroutine(MoveObject(direction));
    }

    private IEnumerator MoveObject(Vector2 direction)
    {
        while (isMoving)
        {
            rb.velocity = direction * MoveSpeed;
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PuzzleObstacle")
        {
            Debug.Log("Collide with obstacle");
            isMoving = false; // Set isMoving ke false ketika bertabrakan
            rb.velocity = Vector2.zero;
        }
        else if (collision.gameObject.name == "Finish")
        {
            isFinish = true;

        }
    }
}
