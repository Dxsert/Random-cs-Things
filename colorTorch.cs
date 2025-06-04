using UnityEngine;

public class ColorLamp : MonoBehaviour
{
    public Sprite[] colorSprites; // 4 sprites couleurs
    public int currentColorIndex = 0; // couleur équipée (0 à 3)

    public bool[] hasStone = new bool[4]; // Pierres possédées
    private bool[] isColorUsable = new bool[4] { true, true, true, true }; // Couleurs utilisables

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateLampColor();
    }

    void Update()
    {
        // Changer couleur vers la gauche avec F
        if (Input.GetKeyDown(KeyCode.F))
        {
            EquipPreviousColor();
        }
        // Changer couleur vers la droite avec G
        else if (Input.GetKeyDown(KeyCode.G))
        {
            EquipNextColor();
        }
    }

    public bool EquipColor(int colorIndex)
    {
        if (colorIndex < 0 || colorIndex >= colorSprites.Length)
        {
            Debug.LogWarning("Color index invalide !");
            return false;
        }

        if (!hasStone[colorIndex])
        {
            Debug.Log("Tu n'as pas la pierre pour cette couleur !");
            return false;
        }

        if (!isColorUsable[colorIndex])
        {
            Debug.Log("Cette couleur est verrouillée, récupère une nouvelle pierre !");
            return false;
        }

        currentColorIndex = colorIndex;
        UpdateLampColor();
        return true;
    }

    void UpdateLampColor()
    {
        spriteRenderer.sprite = colorSprites[currentColorIndex];
    }

    public void OnKillMob(int mobColorId)
    {
        if (mobColorId == currentColorIndex)
        {
            isColorUsable[currentColorIndex] = false;
            Debug.Log($"Couleur {currentColorIndex + 1} verrouillée après kill. Récupère une nouvelle pierre.");
        }
        else
        {
            Debug.Log("Cette couleur ne peut pas tuer ce mob.");
        }
    }

    public void CollectStone(int colorIndex)
    {
        if (colorIndex < 0 || colorIndex >= hasStone.Length) return;

        hasStone[colorIndex] = true;
        isColorUsable[colorIndex] = true;
        Debug.Log($"Pierre couleur {colorIndex + 1} récupérée et lampe réactivée.");
    }

    // Sélectionne la couleur précédente utilisable
    private void EquipPreviousColor()
    {
        int startIndex = currentColorIndex;
        int index = currentColorIndex;

        do
        {
            index--;
            if (index < 0) index = colorSprites.Length - 1;

            if (hasStone[index] && isColorUsable[index])
            {
                EquipColor(index);
                return;
            }
        } while (index != startIndex);

        Debug.Log("Aucune autre couleur utilisable à gauche.");
    }

    // Sélectionne la couleur suivante utilisable
    private void EquipNextColor()
    {
        int startIndex = currentColorIndex;
        int index = currentColorIndex;

        do
        {
            index++;
            if (index >= colorSprites.Length) index = 0;

            if (hasStone[index] && isColorUsable[index])
            {
                EquipColor(index);
                return;
            }
        } while (index != startIndex);

        Debug.Log("Aucune autre couleur utilisable à droite.");
    }
}
