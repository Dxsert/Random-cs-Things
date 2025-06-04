using UnityEngine;

public class Mob : MonoBehaviour
{
    public int mobColorId; // ID couleur du mob (0 à 3)

    private ColorLamp lamp;

    void Start()
    {
        lamp = FindObjectOfType<ColorLamp>();

        if (lamp == null)
            Debug.LogError("Lampe non trouvée dans la scène !");
    }

    // Le mob meurt si la lampe a la bonne couleur et qu’il touche la lampe (trigger)
    void OnTriggerEnter2D(Collider2D other)
    {
        // Vérifie que c’est la lampe ou sa zone d'attaque
        ColorLamp lampHit = other.GetComponent<ColorLamp>();
        if (lampHit != null)
        {
            if (lamp.currentColorIndex == mobColorId &&
                lamp.hasStone[mobColorId] &&
                lamp.isColorUsable[mobColorId])
            {
                Die();
            }
            else
            {
                Debug.Log("Couleur de lampe non correspondante ou verrouillée.");
            }
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} tué par la lampe couleur {mobColorId + 1}");
        lamp.OnKillMob(mobColorId);
        Destroy(gameObject);
    }
}
