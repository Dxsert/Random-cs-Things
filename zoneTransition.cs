using UnityEngine;

public class ZoneTransition : MonoBehaviour
{
    public Transform destinationPoint; // Où le joueur apparaîtra après la transition
    public string requiredTag = "Player"; // Pour que seul le player active la transition
    public bool lockX = false; // Si vrai, on garde la position X du joueur (utile si tu veux garder le Y)
    public bool lockY = false; // Pareil pour Y

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(requiredTag))
        {
            Vector3 newPosition = destinationPoint.position;

            // On garde certains axes si besoin
            if (lockX) newPosition.x = other.transform.position.x;
            if (lockY) newPosition.y = other.transform.position.y;

            other.transform.position = newPosition;
        }
    }
}
