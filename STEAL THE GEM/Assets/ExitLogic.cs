using UnityEngine;

public class ExitLogic : MonoBehaviour
{
    public GameObject ikonaGemuNaHraci;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Hrac")
        {
            // 1. Zkontrolujeme, jestli hráè sebral aspoò první gem (má zapnutou ikonku)
            if (ikonaGemuNaHraci.activeSelf)
            {
                // 2. spoèítáme, kolik objektù s názvem "Gem" ještì zbıvá na mapì
                // Hledáme podle názvu v Hierarchy. Pokud najdeme 0, znamená to, e na zemi u nic neleí.
                int zbyvajiciGemy = GameObject.FindObjectsByType<GemLogic>(FindObjectsSortMode.None).Length;

                if (zbyvajiciGemy == 0)
                {
                    Debug.Log("VİHRA! Všechny diamanty byly ukradeny a úspìšnì odneseny!");
                    Time.timeScale = 0; // Stop hry
                }
                else
                {
                    Debug.Log("Sice máš diamant, ale v bance ještì zbıvá " + zbyvajiciGemy + " další! Vra se pro nìj.");
                }
            }
            else
            {
                Debug.Log("Nemáš ádnı diamant! Vra se do trezoru.");
            }
        }
    }
}