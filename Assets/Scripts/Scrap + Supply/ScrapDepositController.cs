using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapDepositController : MonoBehaviour
{
    public float resourcesLeft;
    [SerializeField] private GameObject mineParticles;
    
    private bool isLinked = false;
    private GameObject icon;
    
    
    // Decrement Resource
    public float resourceDecrement(float decrement)
    {
        // decrement resources
        resourcesLeft -= decrement;
        
        // Shirnk self
        transform.localScale *= 0.95f;
        if (isLinked)
        {
            icon.transform.localScale *= 0.95f;
        }
            
        // destroy if no resources left
        if (resourcesLeft <= 0)
        {
            Unlink();
            Destroy(this.gameObject);
        }
        // spawn and play particles
        GameObject particles = Instantiate(mineParticles, transform.position + Vector3.up, Quaternion.identity);
        particles.GetComponent<ParticleSystem>().Play();
        return decrement;
    }
    
    // Link Icon
    public void LinkIcon(GameObject newIcon)
    {
        isLinked = true;
        icon = newIcon;
        icon.transform.localPosition = MiniMapController.ConvertPosition(transform.position);
    }

    public bool IsLinked()
    {
        return isLinked;
    }

    public void Unlink()
    {
        if (isLinked)
        {
            isLinked = false;
            Destroy(icon);
        }
    }
    
    public void HideIcon()
    {
        if (isLinked)
        {
            icon.SetActive(false);
        }
    }
    
    public void ShowIcon()
    {
        if (isLinked)
        {
            icon.SetActive(true);
        }
    }
}
