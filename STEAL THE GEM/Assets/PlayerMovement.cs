using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Rychlost, kterou uvidíš i v Inspectoru

    void Update()
    {
        // Naètení vstupu z klávesnice (šipky nebo WASD)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Vytvoøení smìru pohybu
        Vector2 movement = new Vector2(moveX, moveY).normalized;

        // Fyzický posun objektu
        transform.Translate(movement * speed * Time.deltaTime);
    }
}
