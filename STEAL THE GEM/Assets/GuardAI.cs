using UnityEngine;
using TMPro; // Knihovna pro práci s textem

public class GuardAI : MonoBehaviour
{
    // Stavy, ve kterých se hlídaè mùže nacházet
    enum StavHlidaèe { Patrola, Pronasledovani }
    private StavHlidaèe aktualniStav = StavHlidaèe.Patrola;

    [Header("UI a Efekty")]
    public TextMeshPro textStavu;

    [Header("Pohyb (Patrola)")]
    public float rychlostPatroly = 2f;
    public Transform[] waypoints;
    private int currentPointIndex = 0;

    [Header("Pohyb (Honièka)")]
    public float rychlostBehu = 4f; // Hlídaè pøi honièce zrychlí
    public float maxVzdalenostOdTrasy = 7f; // Rajón, ze kterého neuteèe

    [Header("Zrak (Zorný kužel)")]
    public Transform hrac;
    public float dohled = 8f;
    [Range(0, 360)] public float zornyUhel = 72f; // Úhel kužele pøed Hlídaèem

    void Start()
    {
        // Paprsek ignoruje vlastní collider hlídaèe
        Physics2D.queriesStartInColliders = false;
        if (textStavu != null) textStavu.text = ""; // Vyèistíme text na startu
    }

    void Update()
    {
        // Podle toho, v jakém stavu hlídaè je, dìlá jinou èinnost
        switch (aktualniStav)
        {
            case StavHlidaèe.Patrola:
                LogikaPatroly();
                KontrolaZraku(); // Neustále kontroluje, jestli nevidí hráèe
                break;

            case StavHlidaèe.Pronasledovani:
                LogikaHonièky();
                break;
        }
    }

    void LogikaPatroly()
    {
        if (waypoints.Length == 0) return;

        Transform cil = waypoints[currentPointIndex];

        // Natoèení hlídaèe èelem (osa Y) k waypointu
        Vector2 smerPohybu = cil.position - transform.position;
        if (smerPohybu != Vector2.zero)
        {
            transform.up = smerPohybu;
        }

        // Chùze k waypointu
        transform.position = Vector2.MoveTowards(transform.position, cil.position, rychlostPatroly * Time.deltaTime);

        // Pokud došel k bodu, pøepne na další, 0,1f staèí, nepotøebujeme pøesnost
        if (Vector2.Distance(transform.position, cil.position) < 0.1f)
        {
            currentPointIndex++;
            if (currentPointIndex >= waypoints.Length) currentPointIndex = 0;
        }
    }

    void KontrolaZraku()
    {
        if (HracJeV_ZornemPole())
        {
            aktualniStav = StavHlidaèe.Pronasledovani;
            if (textStavu != null) textStavu.text = "FREEZE!"; // Vypíše hlášku
        }
    }

    void LogikaHonièky()
    {
        // 1. Natoèí se pøímo na hráèe
        Vector2 smerKHraci = hrac.position - transform.position;
        if (smerKHraci != Vector2.zero)
        {
            transform.up = smerKHraci;
        }

        // 2. Rozbìhne se za ním vyšší rychlostí
        transform.position = Vector2.MoveTowards(transform.position, hrac.position, rychlostBehu * Time.deltaTime);

        // 3. KONTROLA: Ztratil hráèe z dohledu? (Utekl za zeï nebo moc daleko)
        if (!HracJeV_ZornemPole())
        {
            VratSeK_Patrole();
            return;
        }

        // 4. KONTROLA RAJÓNU: Neutekl hlídaè moc daleko od své trasy?
        Transform nejblizsiBodTrasy = NajdiNejblizsiWaypoint();
        if (nejblizsiBodTrasy != null)
        {
            float vzdalenostOdTrasy = Vector2.Distance(transform.position, nejblizsiBodTrasy.position);
            if (vzdalenostOdTrasy > maxVzdalenostOdTrasy)
            {
                Debug.Log("Hráè je moc daleko, hlídaè se vrací do svého rajónu.");
                VratSeK_Patrole();
            }
        }
    }

    void VratSeK_Patrole()
    {
        aktualniStav = StavHlidaèe.Patrola;
        // Aby hlídaè nešel blbì na druhý konec mapy, najde si nejbližší bod a pokraèuje od nìj
        currentPointIndex = NajdiIndexNejblizsihoWaypointu();

        if (textStavu != null) textStavu.text = "LOST HIM";
        Invoke("VymazText", 2f); // Za 2 vteøiny zavolá funkci na smazání textu
    }

    void VymazText()
    {
        // Smaže text pouze pokud se mezitím hlídaè zase nerozbìhl za hráèem
        if (aktualniStav == StavHlidaèe.Patrola && textStavu != null)
        {
            textStavu.text = "";
        }
    }

    // Pomocná funkce, která vyhodnotí úhel a zdi (Raycast)
    bool HracJeV_ZornemPole()
    {
        Vector2 smerKHraci = hrac.position - transform.position;
        float vzdalenost = smerKHraci.magnitude;

        // Je hráè v okruhu dohledu?
        if (vzdalenost <= dohled)
        {
            // Je hráè uvnitø našeho zorného úhlu? (Porovnáváme kam hlídaè kouká vs. kde je hráè)
            float uhelKHraci = Vector2.Angle(transform.up, smerKHraci);
            if (uhelKHraci <= zornyUhel / 2f)
            {
                // Vystøelíme Raycast, abychom zjistili, jestli v cestì nestojí zeï
                RaycastHit2D hit = Physics2D.Raycast(transform.position, smerKHraci.normalized, dohled);

                if (hit.collider != null && hit.collider.gameObject == hrac.gameObject)
                {
                    return true; // Vidí hráèe! Úhel sedí, zeï v cestì není.
                }
            }
        }
        return false;
    }

    // --- SMRT HRÁÈE ---
    // Tato funkce se v Unity zavolá automaticky, když do sebe narazí dva Collidery
    void OnCollisionEnter2D(Collision2D kolize)
    {
        // Pokud hlídaè právì pronásleduje A ZÁROVEÒ narazil do hráèe
        if (aktualniStav == StavHlidaèe.Pronasledovani && kolize.gameObject.name == "Hrac")
        {
            Debug.Log("GAME OVER! Byl jsi chycen.");
            Time.timeScale = 0; // Zmrazí celý herní èas (zastaví hru)
        }
    }

    // Vyhledávaè nejbližšího bodu trasy pro kontrolu tìch 7 metrù (7f)
    Transform NajdiNejblizsiWaypoint()
    {
        if (waypoints.Length == 0) return null;
        Transform nejblizsi = waypoints[0];
        float nejkratsiVzdalenost = Vector2.Distance(transform.position, nejblizsi.position);

        foreach (Transform wp in waypoints)
        {
            float v = Vector2.Distance(transform.position, wp.position);
            if (v < nejkratsiVzdalenost)
            {
                nejkratsiVzdalenost = v;
                nejblizsi = wp;
            }
        }
        return nejblizsi;
    }

    int NajdiIndexNejblizsihoWaypointu()
    {
        if (waypoints.Length == 0) return 0;
        int nejblizsiIndex = 0;
        float nejkratsiVzdalenost = Vector2.Distance(transform.position, waypoints[0].position);

        for (int i = 0; i < waypoints.Length; i++)
        {
            float v = Vector2.Distance(transform.position, waypoints[i].position);
            if (v < nejkratsiVzdalenost)
            {
                nejkratsiVzdalenost = v;
                nejblizsiIndex = i;
            }
        }
        return nejblizsiIndex;
    }

    // Nakreslí zorné pole pøímo do okna Scene (vidíš ho poøád)
    void OnDrawGizmos()
    {
        // Žlutý kruh maximálního dosahu
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, dohled);

        // Modré èáry zorného kužele
        Vector3 levySmer = Quaternion.Euler(0, 0, zornyUhel / 2f) * transform.up;
        Vector3 pravySmer = Quaternion.Euler(0, 0, -zornyUhel / 2f) * transform.up;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + levySmer * dohled);
        Gizmos.DrawLine(transform.position, transform.position + pravySmer * dohled);

        // Pokud zrovna TEÏ vidí hráèe, vykreslí se tlustá èervená èára pøímo k nìmu
        if (hrac != null && HracJeV_ZornemPole())
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, hrac.position);
        }
    }
}