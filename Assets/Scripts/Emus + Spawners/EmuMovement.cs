using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmuMovement : MonoBehaviour
{
    // Movelist Attributes
    private List<Vector3> moves = new List<Vector3>();
    private int currentMoveIndex = 0;

    // Emu Attributes
    public float moveSpeed;
    public float damage;
    public bool isBoss;

    // Distance Attributes
    private Vector3 currentMoveDirection;
    private float remainingDistance = 0;

    // Passive Attributes
    private float totalDistanceRemaining = 0;

    // Icon attributes
    private bool isLinked = false;
    private bool onMap;
    private GameObject icon;
    [SerializeField] private Sprite sprite;
    [SerializeField] private float iconScale;

    private void Start()
    {
        onMap = false;
        
        // calc total path distance
        for (int i = 0; i < moves.Count; i++)
        {
            totalDistanceRemaining += moves[i].magnitude; // get scalar
        }
    }

    public void setMoves(List<Vector3> moves)
    {
        this.moves = moves;
    }

    private void Update()
    {
        // get new move
        if (remainingDistance <= 0)
        {
            // check if out of moves (dead)
            if (currentMoveIndex == moves.Count)
            {
                // Get Parent spawner, tell to make HQ lose health
                GetComponentInParent<SpawnerManagerScript>().damageHQ(damage);
                Unlink();
                Destroy(gameObject);
                return;
            }
            
            // If index 1, then Emu is on map
            if (currentMoveIndex == 1)
            {
                onMap = true;
                ShowIcon();

                if (isBoss)
                {
                    BossBehaviour boss = this.gameObject.GetComponent<BossBehaviour>();
                    boss.healthGUI = this.GetComponentInParent<SpawnerManagerScript>().bossGUI;
                    boss.healthSlider = this.GetComponentInParent<SpawnerManagerScript>().bossHealthSlider;
                    boss.StartBoss();
                }
            }
            
            currentMoveDirection = moves[currentMoveIndex].normalized; // get dir
            remainingDistance = moves[currentMoveIndex].magnitude; // get scalar
            transform.rotation = Quaternion.LookRotation(currentMoveDirection, Vector3.up); // update rotation
            currentMoveIndex++;
        }
        
        
        // calc the movement for this frame
        float movementThisFrame = moveSpeed * Time.deltaTime;

        // move at triple speed if not on map
        if (!onMap)
        {
            movementThisFrame *= 3f;
        }
        
        // dont overshoot intended dist
        float actualMovement = Mathf.Min(movementThisFrame, remainingDistance);

        // move emu 
        transform.position += currentMoveDirection * actualMovement;

        // mimic movement for minimap if linked to icon
        if (isLinked)
        {
            icon.transform.localPosition = MiniMapController.ConvertPosition(transform.position);
        }

        // update remaining dist
        remainingDistance -= actualMovement;
        totalDistanceRemaining -= actualMovement;
        //Debug.Log(totalDistanceRemaining);
    }

    // getter
    public float getTotalDistanceRemaining()
    {
        return totalDistanceRemaining;
    }

    // Add emu icon for minimap
    public void LinkIcon(GameObject newIcon)
    {
        isLinked = true;
        icon = newIcon;
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
            icon.GetComponent<Image>().sprite = sprite;
            icon.GetComponent<RectTransform>().sizeDelta 
                = new Vector2(icon.GetComponent<RectTransform>().sizeDelta.x * iconScale, 
                            icon.GetComponent<RectTransform>().sizeDelta.y * iconScale);
            icon.SetActive(true);
        }
    }
}
