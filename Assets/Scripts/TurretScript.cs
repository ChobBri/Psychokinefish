using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : ClickableGameObject
{
    [SerializeField]
    GameObject projectile;
    Timer projectileTimer;
    static AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        
        projectileTimer = new Timer(0.15f);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame

    public override void ToggleActive()
    {
        base.ToggleActive();
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (isActive)
        {
            spriteRenderer.color = Color.green;
        } else
        {
            spriteRenderer.color = Color.cyan;
        }
    }

    protected override void OnActive()
    {
        projectileTimer.Tick();
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, mousePosition - (Vector2)transform.position));

        if (Input.GetMouseButton(0))
        {
            if (projectileTimer.ResetTimer())FireProjectile(mousePosition- (Vector2) transform.position, 30.0f);
        }

    }

    void FireProjectile(Vector2 direction, float speed) {
        audioSource.Play();
        direction = Quaternion.AngleAxis(Random.Range(-3,3), Vector3.forward) * direction;
        if (direction != Vector2.zero) direction.Normalize();
        else direction = Vector2.right;
        direction *= 0.8f;
        var projectileObj = Instantiate(projectile, transform.position + (Vector3) direction, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, direction)));
        projectileObj.GetComponent<Rigidbody2D>().velocity = speed * direction.normalized;
    }

    protected override void OnInactive()
    {
        
    }
}