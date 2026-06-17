using UnityEngine;
using UnityEngine.UI; // Budeme potøebovat pro Slider

public class GemLogic : MonoBehaviour
{
    public float casKradeze = 10f;
    private float aktualniCas = 0f;
    private bool hracU_Gemu = false;

    public GameObject hrac; // Pøetáhni hráèe v Inspectoru
    public GameObject ikonaGemuNaHraci; // Sprite/objekt na hráèi, co se zapne

    void Update()
    {
        if (hracU_Gemu)
        {
            aktualniCas += Time.deltaTime;
            Debug.Log("Kradu... " + (int)aktualniCas);

            if (aktualniCas >= casKradeze)
            {
                SeberGem();
            }
        }
    }

    void SeberGem()
    {
        ikonaGemuNaHraci.SetActive(true); // Ukáže gem na hráèi
        Destroy(gameObject); // Smaže diamant ze zemì
        Debug.Log("GEM SEBRÁN! Uteè k Exitu!");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Hrac") hracU_Gemu = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Hrac")
        {
            hracU_Gemu = false;
            aktualniCas = 0f; // Reset pokroku pøi útìku
        }
    }
}