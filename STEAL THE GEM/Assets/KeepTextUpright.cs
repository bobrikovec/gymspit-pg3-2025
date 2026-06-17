using UnityEngine;

public class KeepTextUpright : MonoBehaviour
{
    void Update()
    {
        // Vynutí rotaci na 0, i když se Hlídaè toèí
        transform.rotation = Quaternion.identity;
    }
}