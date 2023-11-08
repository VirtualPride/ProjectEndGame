using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleMove : MonoBehaviour
{
    [SerializeField] private PuzzleJalanMentokGerak puzzleJalanMentokGerak;
    [SerializeField] private PuzzleTesInteract puzzleTesInteract;
    [HideInInspector] public bool isMoving = false;
    [SerializeField] private Rigidbody2D rb;
 
    private bool isFinish = false;
    public float MoveSpeed = 2.0f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        HandleMovementInput();
    }

    private void HandleMovementInput()
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
        if (collision.gameObject.name == "TesObstacle")
        {
            Debug.Log("Collide with obstacle");
            isMoving = false; // Set isMoving ke false ketika bertabrakan
            rb.velocity = Vector2.zero;
        }
    }
}
