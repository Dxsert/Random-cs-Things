using UnityEngine;

public class StonePickup : MonoBehaviour
{
    public int colorIndex; // Index de la couleur de la pierre (0 à 3)
    private colorTorch torch;  // Référence au script colorTorch qui gère la lampe

    void Start()
    {
        // Recherche dans la scène le script colorTorch (attaché à la lampe)
        torch = FindObjectOfType<colorTorch>();
        if (torch == null)
        {
            Debug.LogError("Pas de lampe (colorTorch) trouvée dans la scène !");
        }
    }

    // Fonction appelée automatiquement quand un autre collider entre dans le trigger de la pierre
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Vérifie que c’est bien le joueur qui a touché la pierre (tag "Player")
        if (other.CompareTag("Player"))
        {
            if (torch != null)
            {
                // Si le joueur ne possède pas déjà la pierre de cette couleur
                if (!torch.hasStone[colorIndex])
                {
                    // Ajoute la pierre à la lampe
                    torch.CollectStone(colorIndex);

                    // Supprime la pierre ramassée de la scène
                    Destroy(gameObject);
                }
                else
                {
                    // Affiche un message dans la console si la pierre est déjà possédée
                    Debug.Log($"Tu as déjà une pierre de couleur {colorIndex + 1}");
                }
            }
        }
    }
}
