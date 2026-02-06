using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class BolaGuaoController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winText;
    private Rigidbody rb;
    private int count;
    private float moveX;
    private float moveY;

    void Start()
    {
        rb = GetComponent <Rigidbody>();
        count = 0;

        SetCountText();
        winText.SetActive(false);
    }

    void OnMove (InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>(); 

        moveX = movementVector.x;
        moveY = movementVector.y;
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count == 7){
            countText.text = "Count: " + count.ToString() + " Only one left!";
        }

        if (count >= 8)
        {
            winText.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        }
    }

     void FixedUpdate() 
    {
        Vector3 movement = new Vector3(moveX, 0.0f, moveY);
        rb.AddForce(movement * speed); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cube"))
        {
            other.gameObject.SetActive(false);
            count++;

            SetCountText();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Destroy the current object
            Destroy(gameObject); 
            // Update the winText to display "You Lose!"
            winText.gameObject.SetActive(true);
            winText.GetComponent<TextMeshProUGUI>().text = "You Lose!";
        }
    }
}
