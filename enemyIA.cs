using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // Vitesse de déplacement du monstre
    public float speed = 2f;
    // Distance à laquelle le monstre détecte le joueur pour commencer à le poursuivre
    public float detectionRange = 5f;
    // Durée entre chaque attaque (cooldown)
    public float attackCooldown = 1.5f;
    // Référence au transform du joueur pour connaitre sa position
    public Transform player;
    // Layer utilisé pour détecter les obstacles (murs, etc)
    public LayerMask obstacleLayer;

    // Direction aléatoire pour la patrouille
    private Vector2 randomDirection;
    // Temps entre les changements de direction quand le monstre patrouille
    private float changeDirectionTime = 2f;
    // Timer qui compte le temps restant avant de changer de direction
    private float directionTimer;
    // Timer pour gérer le cooldown entre attaques
    private float attackTimer = 0f;

    private Rigidbody2D rb; // Rigidbody2D du monstre pour gérer le mouvement
    private bool playerInAttackRange = false; // Booléen qui indique si le joueur est dans la zone d'attaque

    // États possibles pour l'IA du monstre
    private enum State { Patrolling, Chasing, Attacking }
    private State currentState = State.Patrolling; // État initial = patrouille

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Récupération du Rigidbody2D
        ChooseNewDirection(); // Choisir une première direction aléatoire dès le départ
    }

    void Update()
    {
        // Calcul de la distance entre le monstre et le joueur
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Réduction progressive du timer d'attaque
        if (attackTimer > 0)
            attackTimer -= Time.deltaTime;

        // Changement d'état selon la situation :
        // Si le joueur est dans la zone d'attaque, on attaque
        if (playerInAttackRange)
            currentState = State.Attacking;
        // Sinon si le joueur est détecté dans la range, on le poursuit
        else if (distanceToPlayer < detectionRange)
            currentState = State.Chasing;
        // Sinon on patrouille tranquillement
        else
            currentState = State.Patrolling;

        // En fonction de l'état actuel, on exécute le comportement correspondant
        switch (currentState)
        {
            case State.Patrolling:
                HandlePatrolling();
                break;
            case State.Chasing:
                HandleChasing();
                break;
            case State.Attacking:
                HandleAttacking();
                break;
        }
    }

    // Comportement quand le monstre patrouille
    void HandlePatrolling()
    {
        // On décrémente le timer qui indique quand changer de direction
        directionTimer -= Time.deltaTime;

        // Si le timer est écoulé OU si un obstacle est détecté devant le monstre
        if (directionTimer <= 0 || Physics2D.Raycast(transform.position, randomDirection, 0.5f, obstacleLayer))
        {
            ChooseNewDirection(); // On choisit une nouvelle direction aléatoire
        }

        // On applique le mouvement dans la direction choisie
        MoveInDirection(randomDirection);
    }

    // Comportement quand le monstre poursuit le joueur
    void HandleChasing()
    {
        // Calcul de la direction normalisée vers le joueur
        Vector2 directionToPlayer = (player.position - transform.position).normalized;

        // On applique le mouvement dans cette direction
        MoveInDirection(directionToPlayer);
    }

    // Comportement quand le monstre attaque
    void HandleAttacking()
    {
        rb.velocity = Vector2.zero; // On stoppe le mouvement pour attaquer

        // Si le cooldown d'attaque est écoulé, on peut attaquer
        if (attackTimer <= 0)
        {
            Debug.Log("Le monstre attaque !"); // Ici tu peux déclencher animation ou dégâts
            attackTimer = attackCooldown; // Reset du cooldown pour la prochaine attaque
        }
    }

    // Fonction qui choisit une nouvelle direction aléatoire pour la patrouille
    void ChooseNewDirection()
    {
        randomDirection = Random.insideUnitCircle.normalized; // Direction random normalisée
        directionTimer = changeDirectionTime; // Reset du timer pour changer de direction
    }

    // Fonction qui applique un mouvement au Rigidbody2D selon une direction donnée
    void MoveInDirection(Vector2 dir)
    {
        rb.velocity = dir * speed; // Vitesse multipliée par la direction
    }

    // Détecte quand un collider entre dans la zone trigger (zone d'attaque)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == player)
        {
            playerInAttackRange = true; // Le joueur est dans la zone d'attaque
        }
    }

    // Détecte quand un collider sort de la zone trigger (zone d'attaque)
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform == player)
        {
            playerInAttackRange = false; // Le joueur sort de la zone d'attaque
        }
    }
}
