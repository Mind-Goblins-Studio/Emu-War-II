using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Transform sourceObject;
    public Transform targetObject;

    [SerializeField] private GameObject arrowPointer;
    [SerializeField] private GameObject arrowHead1;
    [SerializeField] private GameObject arrowHead2;


    private Material arrowMaterial;
    private Material arrowHead1Material;
    private Material arrowHead2Material;
    private Color originalColor;
    private Coroutine flashCoroutine;

    [SerializeField] private float flashDuration = 0.5f;

    private void Start()
    {
        arrowMaterial = arrowPointer.GetComponent<Renderer>().material;
        arrowHead1Material = arrowPointer.transform.Find("Head1").GetComponent<Renderer>().material;
        arrowHead2Material = arrowPointer.transform.Find("Head2").GetComponent<Renderer>().material;

        originalColor = arrowMaterial.color;
    }

    private void Update()
    {
        // Rotate the object to always face towards the target object
        transform.position = sourceObject.position;
        transform.LookAt(targetObject);
    }


    public void SetDistance(float distance)
    {
        arrowPointer.transform.localPosition = new Vector3(0, 0, distance);
    }

    [ContextMenu("Hide")]
    public void Hide()
    {
        arrowPointer.SetActive(false);
        // Restore the original color when hiding the arrow
        arrowMaterial.color = originalColor;
        arrowHead1Material.color = originalColor;
        arrowHead2Material.color = originalColor;

        // Stop the flashing coroutine when hiding the arrow
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
        }
    }

    [ContextMenu("Show")]
    public void Show()
    {
        arrowPointer.SetActive(true);
        // Start the flash coroutine when showing the arrow
        if (flashCoroutine == null)
        {
            flashCoroutine = StartCoroutine(FlashArrow());
        }
    }

    public bool IsActive()
    {
        return arrowPointer.activeSelf;
    }

    private IEnumerator FlashArrow()
    {
        while (true)
        {
            float elapsedTime = 0f;
            while (elapsedTime < flashDuration)
            {
                // Lerp the arrow color between white and the original color
                float t = Mathf.PingPong(elapsedTime, flashDuration) / flashDuration;
                arrowMaterial.color = Color.Lerp(Color.white, originalColor, t);
                arrowHead1Material.color = Color.Lerp(Color.white, originalColor, t);
                arrowHead2Material.color = Color.Lerp(Color.white, originalColor, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            // Ensure the arrow's color is set back to the original color
            arrowMaterial.color = originalColor;
            arrowHead1Material.color = originalColor;
            arrowHead2Material.color = originalColor;
        }
    }
}

