using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    public bool IsFriendly { get; private set; } = true;

    [field: SerializeField]
    public int Damage { get; private set; } = 1;

    private const int friendlyBulletLayer = 7; //Està definit a les layers de física
    private const int enemyBulletLayer = 9;

    [SerializeField]
    private GameObject playerHit;
    [SerializeField]
    private GameObject enemyHit;

    [SerializeField]
    private float knockBackAmount = 5f;
    public void Shoot(float shootForce, bool isFriendly = true)
    {
        rb.AddForce(this.transform.right * shootForce, ForceMode2D.Impulse);

        IsFriendly = isFriendly;
        this.gameObject.layer = IsFriendly ? friendlyBulletLayer : enemyBulletLayer;
    }

    private const int playerLayer = 6;
    private const int enemyLayer = 8;

    public void OnCollisionEnter2D(Collision2D collision) 
    {
        HealthSystem hs = collision.gameObject.GetComponent<HealthSystem>();
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

        if (hs != null)
        {
            hs.TakeDamage(Damage);
            if (rb != null)
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(this.transform.right * knockBackAmount, ForceMode2D.Impulse);

            Instantiate(IsFriendly ? enemyHit : playerHit, this.transform.position, Quaternion.identity, null);
        }

        SpawnImpactVfx(collision);
        Destroy(this.gameObject);
    }



    // EL CODI A CONTINUACIÓ ESTÀ FET AMB CLAUDE CODE OPUS 5.5 HIGH
    // No hi ha cap part de l'examen que estigui a codi fet amb IA
    private void SpawnImpactVfx(Collision2D collision)
    {
        Vector2 point = this.transform.position;
        Vector2 normal = -(Vector2)this.transform.right;
        if (collision.contactCount > 0)
        {
            ContactPoint2D contact = collision.GetContact(0);
            point = contact.point;
            normal = contact.normal; // apunta desde la superficie hacia la bala
        }

        int layer = collision.gameObject.layer;
        if (layer == playerLayer || layer == enemyLayer)
        {
            // Impacto en un personaje: estallido de 360º con su color.
            GameObject body = collision.rigidbody != null ? collision.rigidbody.gameObject : collision.gameObject;
            SpriteRenderer bodySprite = body.GetComponentInChildren<SpriteRenderer>();
            Color bodyColor = bodySprite != null ? bodySprite.color : Color.white;
            BulletVfx.BodyImpact(point, -normal, bodyColor);
        }
        else
        {
            // Impacto en pared: abanico de 180º alrededor de la normal.
            BulletVfx.WallImpact(point, normal);
        }
    }
}
