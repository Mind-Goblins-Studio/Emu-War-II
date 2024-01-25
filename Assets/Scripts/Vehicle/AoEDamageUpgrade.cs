using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoEDamageUpgrade : MonoBehaviour
{
    // enemy detection + damage
    [SerializeField] private float damage;
    [SerializeField] private string enemyTag;
    
    // timer attributes
    private float timer = 0;
    [SerializeField] private float tickRate = 0.5f;
    
    // Particles
    [SerializeField] private ParticleSystem particles;

    // Update is called once per frame
    void Update()
    {
        if (timer >= tickRate)
        {
            timer = 0;
            DoDamage();
        }
        else
        {
            timer += Time.deltaTime;
        }
    }


    private void DoDamage()
    {
        // Check for enemies with the specified tag in the detection radius
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, transform.localScale.x / 2);

        foreach (Collider enemyCollider in hitEnemies)
        {
            if (enemyCollider.CompareTag(enemyTag))
            {
                // Assuming the enemy script has a 'distanceToEnd' variable
                EmuBulletCollision enemyScript = enemyCollider.GetComponent<EmuBulletCollision>();

                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(damage);
                }
            }
        }
    }
    
    public void SetRadius(float radius)
    {
        transform.localScale = new Vector3(radius, transform.localScale.y, radius);
        
        // update particle system
        var shape = particles.shape;
        shape.radius = radius * 0.25f;
    }
}
