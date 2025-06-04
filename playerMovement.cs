using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private float moveInput;
    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isAttacking)
        {
            // Si en train d'attaquer, on bloque le mouvement
            moveInput = 0;
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        // Gestion déplacement AZERTY
        if (Input.GetKey(KeyCode.D))
            moveInput = 1;
        else if (Input.GetKey(KeyCode.Q))
            moveInput = -1;
        else
            moveInput = 0;

        // Flip du sprite selon direction
        if (moveInput != 0)
            spriteRenderer.flipX = moveInput < 0;

        // Attaque si on appuie sur la touche espace par exemple
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }

        // On update les animations marche/idle
        animator.SetBool("isWalking", moveInput != 0);
    }

    void FixedUpdate()
    {
        if (!isAttacking)
        {
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        }
    }

    void Attack()
    {
        isAttacking = true;
        animator.SetBool("isAttacking", true);
        rb.velocity = Vector2.zero;

        // Lancer une coroutine pour stopper l'attaque après la durée de l'anim
        float attackDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        StartCoroutine(StopAttackAfterDelay(attackDuration));
    }

    System.Collections.IEnumerator StopAttackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }
}
