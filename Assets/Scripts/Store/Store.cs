using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Store : MonoBehaviour
{
    //Store items:
    //0. Large bin 50
    //1. Medium bin 20 
    //2. Small bin 10
    //3. Detergent 100 wipingSpeed *= 2
    [SerializeField] float detergentSpeedUpFactor = 2f;
    //4. Upgrade Claw 100 maxDistance *= 1.5
    [SerializeField] float clawUpgradeFactor = 1.5f;
    //5. AI Sweeper

    public PlayerController player;
    public Claw claw;

    public List<TextMeshProUGUI> priceTexts;

    public Button[] buttons;
    public List<GameObject> itemPrefabs;
    public List<int> itemPrices;
    public MyGrid grid;
    public LayerMask nonGroundMask;

    public GameObject itemPanel;

    private GameObject itemSpawned;
    private int idxItemToSpawn;

    public ParticleSystem upgradeEffectPrefab;

    public Transform trashbins;


    // Start is called before the first frame update
    void Start()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        player = playerGO.GetComponent<PlayerController>();
        claw = playerGO.GetComponent<Claw>();
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<MyGrid>();
        for (int i=0; i<priceTexts.Count; i++)
        {
            priceTexts[i].text = itemPrices[i].ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (itemSpawned != null)
        {
            if (Input.GetMouseButton(0) && !IsPointerOverUIObject())//Mouse.current.leftButton.isPressed
            {
                MouseFollow mouseFollow = itemSpawned.GetComponent<MouseFollow>();
                if(mouseFollow.isPlaceEmpty)
                {
                    mouseFollow.enabled = false;
                    if (idxItemToSpawn < 3)
                    {
                        itemSpawned.GetComponent<Trashbin>().isPlaced = true;
                        
                        itemSpawned.GetComponent<BoxCollider>().isTrigger = false;

                    }
                    else
                        itemSpawned.GetComponent<AISweeper>().isPlaced = true;
                    itemSpawned.GetComponent<DrawCircle>().enabled = false;
                    itemSpawned.GetComponent<LineRenderer>().enabled = false;

                    player.Buy(itemPrices[idxItemToSpawn]);
                    itemSpawned.GetComponent<Trashbin>().isBought = true;
                    buttons[idxItemToSpawn].interactable = true;
                    itemSpawned = null;

                }
                
            }
        }
            
    }

    private bool IsPointerOverUIObject()
    {
        Vector3 mouseInput = Input.mousePosition;//player.playerInputActions.Player.MousePosition.ReadValue<Vector2>();
        var eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            
            position = new Vector2(mouseInput.x, mouseInput.y)
        };
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }


    public void onStoreButton()
    {
        itemPanel.SetActive(!itemPanel.activeSelf);
        if (itemSpawned != null)
        {
            Destroy(itemSpawned);
            itemSpawned = null;
        }
    }

    public void SetAllButtonsInteractable()
    {
        for (int i= 0; i < buttons.Length; i++)
        {
            if(i!=4)
                buttons[i].interactable = true;
        }
    }

    public void OnButtonClicked(Button clickedButton)
    {
        if (itemSpawned != null)
        {
            Destroy(itemSpawned);
            itemSpawned = null;
        }
        int index = System.Array.IndexOf(buttons, clickedButton);

        if (index == -1)
            return;

        SetAllButtonsInteractable();
        clickedButton.interactable = false;
        if (player.coinCount >= itemPrices[index])
        {
            idxItemToSpawn = index;

            if (index < 3 || index == 5)//trashbins + AI Sweeper
            {
                itemSpawned = Instantiate(itemPrefabs[idxItemToSpawn], player.transform.position+Vector3.forward*3f, Quaternion.identity);
                itemSpawned.transform.parent = trashbins;
                //TODO: make unplaced object transparent
                //Color transparent_color = itemSpawned.GetComponent<MeshRenderer>().material.color;
                //transparent_color.a = 0.5f;
                //itemSpawned.GetComponent<MeshRenderer>().material.color = transparent_color;
            }

            if(index==3)//Detergent 
            {
                player.GetDetergent(detergentSpeedUpFactor);
                player.Buy(itemPrices[index]);
                upgradeEffectPrefab.Play();
            }

            if (index == 4)//Upgrade Claw
            {
                claw.upgradeClaw(clawUpgradeFactor);
                player.Buy(itemPrices[index]);
                upgradeEffectPrefab.Play();
            }



        } else
        {
            //TODO: cannot afford warning
            clickedButton.interactable = true;
        }
            
        

    }

    private bool PlaceTrashbinNear(Vector3 position,GameObject item)
    {
        var finalPosition = grid.GetNearestPointOnGrid(position);
        if (grid.isGridFree((int)finalPosition.x, (int)finalPosition.z))
        {
            finalPosition.y = 0.5f;
            Instantiate(item, finalPosition, Quaternion.identity);
            grid.setGridStatus(finalPosition.x, finalPosition.z);
            return true;
        }
        return false;
    }

}
