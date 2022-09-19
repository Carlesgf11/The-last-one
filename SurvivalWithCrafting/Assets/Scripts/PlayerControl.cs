using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public enum PlayerState { NORMAL, CLIMB, SHOOT, MOLOTOV, GLOVES, BENDAGES, LIFEBEST, INVENTORY};
    public PlayerState state;
    public PlayerState lastState;
    private bool climbArea;

    public Transform p1, p2;
    public Rigidbody2D rb;
    private bool isGrounded;
    public LayerMask groundMask;
    public int dir;
    //public bool playerRight;
    public int lifes;
    public int shield;

    public GameObject golpeNavaja;
    public EnemyPruebasControl enemyPruebas;
    private bool fenceSound;

    public GameObject bullet, molotov;
    private bool isAttack;
    private float attackTime;
    public Transform bulletInitPos;

    public Animator anim;

    private bool useDash;

    //Craft
    public List<InventoryProperties> inventory;
    public GameObject inventoryPanel;
    public GameObject baseInventory, baseBag;
    public Transform gridInventory, gridBag;

    public ProbInst prob;

    public bool haveGun, haveBullets, haveMolotov, haveBendages, haveGloves, haveLifeBest;
    public List<Image> hotBarSelect;
    public List<Image> hotBarIcon;
    private Color haveNoIcon, haveIcon;
    private bool craftTable;

    //Life & shield bar
    public Transform gridLife, gridShield;
    public GameObject oneLife, oneShield;

    private bool end;

    void Start()
  {
        rb = GetComponent<Rigidbody2D>();
        dir = 1;
        inventoryPanel.SetActive(false);
        lifes = 4;
        shield = 0;
        haveNoIcon.a = 0.3f;
        haveIcon.a = 0.3f;
  }
 
  void Update()
  {
        if (craftTable == true && Input.GetKeyDown(KeyCode.E)) ShowInventory();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (shield > 0)
            {
                shield--;
                Destroy(gridShield.transform.GetChild(gridShield.childCount - 1).gameObject);
            }
            else
            {
                lifes--;
                Destroy(gridLife.transform.GetChild(gridLife.childCount - 1).gameObject);
            }
            if (shield <= 0 && lifes <= 0)
            {
                //Muerte
                print("Moriste puto");
            }
        }
        HotBarSelectColors();
        if (state != PlayerState.CLIMB)
        {
            anim.SetBool("isClimbing", false);
            anim.SetBool("isMovingY", false);
        }
        if(fenceSound == true && Input.GetKeyDown(KeyCode.E))SoundManager.PlaySound("BerjaMetalicaSound");
        if (end == true && Input.GetKeyDown(KeyCode.E)) SceneManager.LoadScene(4);
        if(lifes <= 0) SceneManager.LoadScene(5);
        SelectHotBar();
        switch (state)
        {
            case PlayerState.NORMAL:
                PlayerMovement();
                RazorAttack();
                break;
            case PlayerState.CLIMB:
                rb.velocity = new Vector2(rb.velocity.x * 12, Input.GetAxis("Vertical") * 12);
                rb.isKinematic = true;
                if (Input.GetAxis("Vertical") > 0)
                {
                    anim.SetBool("isClimbing", true);
                    anim.SetBool("isMovingY", true);
                }
                else
                {
                    anim.SetBool("isClimbing", true);
                    anim.SetBool("isMovingY", false);
                }
                break;
            case PlayerState.SHOOT:
                PlayerMovement();
                ShootAttack();
                hotBarSelect[1].color = Color.white;
                break;
            case PlayerState.MOLOTOV:
                PlayerMovement();
                MolotovAttack();
                break;
            case PlayerState.BENDAGES:
                BendageUsing();
                PlayerMovement();
                break;
            case PlayerState.GLOVES:
                PlayerMovement();
                if (climbArea == true)
                {
                    if (Input.GetButtonDown("Vertical"))
                    {
                        state = PlayerState.CLIMB;
                        lastState = PlayerState.GLOVES;
                    }
                }
                break;
            case PlayerState.LIFEBEST:
                PlayerMovement();
                LifeBest();
                break;
            case PlayerState.INVENTORY:
                rb.velocity = new Vector2(0, 0);
                anim.SetBool("isMoving", false);
                break;
        }
        

    }
    //Crafteos
    public void AddItem(int _id, int _amount)
    {
        ItemsProperties _item = ItemsManager.GetItemById(_id);
        if(HasItem(_item, _amount) == false)
        {
            inventory.Add(new InventoryProperties(_item, _amount));
        }
        PrintCraftTable();
    }
    private bool HasItem(ItemsProperties _item, int _amount)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if(inventory[i].item.id == _item.id)
            {
                inventory[i].amount += _amount;
                return true;
            }
        }
        return false;
    }
    private void ShowInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        if(inventoryPanel.activeSelf == true)
        {
            PrintCraftTable();
        }

        if (inventoryPanel.activeSelf == true)
        {
            rb.isKinematic = true;
            state = PlayerState.INVENTORY;
        }
        else if (inventoryPanel.activeSelf == false)
        {
            rb.isKinematic = false;
            state = lastState;
        }
    }
    private void PrintCraftTable()
    {
        for (int i = gridInventory.childCount - 1; i >= 0; i--)
            Destroy(gridInventory.GetChild(i).gameObject);
       
        for (int i = 0; i < ItemsManager.items.Count; i++)
        {
            if(ItemsManager.items[i].craftAmountExport > 0)
            {
                GameObject newCrfat = Instantiate(baseInventory, gridInventory);
                newCrfat.transform.Find("Name").GetComponent<Text>().text = ItemsManager.items[i].name;
                newCrfat.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Items/Items" + ItemsManager.items[i].id);
                string craftList = "";
                for (int j = 0; j < ItemsManager.items[i].crafting.Count; j++)
                {
                    string itemName = ItemsManager.GetNameById(ItemsManager.items[i].crafting[j].id_Item);
                    int amount = ItemsManager.items[i].crafting[j].amount;
                    craftList += itemName + " x" + amount + "\n";
                }
                newCrfat.transform.Find("Craft").GetComponent<Text>().text = craftList;

                if(CanCraftItem(ItemsManager.items[i]) == true)
                {
                    newCrfat.GetComponent<Image>().color = Color.green;
                    ItemsProperties tempItem = ItemsManager.items[i];
                    newCrfat.GetComponent<Button>().onClick.AddListener(delegate { CraftItem(tempItem); });
                }
                else
                {
                    newCrfat.GetComponent<Image>().color = Color.gray;
                }
            }
        }

        for (int i = gridBag.childCount - 1; i >= 0; i--)
            Destroy(gridBag.GetChild(i).gameObject);

        for (int i = 0; i < inventory.Count; i++)
        {
            GameObject newItem = Instantiate(baseBag, gridBag);
            newItem.transform.Find("Amount").GetComponent<Text>().text = inventory[i].amount.ToString();
            newItem.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Items/Items" + inventory[i].item.id);
        }
        HaveCraft();
    }
    private void CraftItem(ItemsProperties _item)
    {
        SoundManager.PlaySound("SierraCrafteoSound");
        AddItem(_item.id, _item.craftAmountExport);
        for (int i = 0; i < _item.crafting.Count; i++)
        {
            RemoveItem(_item.crafting[i].id_Item, _item.crafting[i].amount);
        }
        PrintCraftTable();
    }
    private void RemoveItem(int _id, int _amount)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if(inventory[i].item.id == _id)
            {
                inventory[i].amount -= _amount;
                if(inventory[i].amount <= 0)
                {
                    inventory.RemoveAt(i);
                    state = PlayerState.NORMAL;
                    lastState = PlayerState.NORMAL;
                    if (_id == 15)//Molotov
                        haveMolotov = false;
                    if(_id == 10)//Bullets
                        haveBullets = false;
                    if (_id == 13)//Gun
                        haveGun = false;
                    if (_id == 11)//Bendages
                        haveBendages = false;
                    if (_id == 12)//Life Best
                        haveLifeBest = false;
                }
            }
        }
    }
    private bool CanCraftItem(ItemsProperties _item)
    {
        for (int i = 0; i < _item.crafting.Count; i++)
        {
            if(HasCraftItem(_item.crafting[i].id_Item, _item.crafting[i].amount) == false)
            {
                return false;
            }
        }
        return true;
    }
    private bool HasCraftItem(int _id, int _amount)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if(inventory[i].item.id == _id && inventory[i].amount >= _amount)
            {
                return true;
            }
        }
        return false;
    }
    public void SelectHotBar()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            state = PlayerState.NORMAL;
            lastState = PlayerState.NORMAL;
        } 
        if (haveGun == true)
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                state = PlayerState.SHOOT;
                lastState = PlayerState.SHOOT;
            }
        }
        if (haveMolotov == true)
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                state = PlayerState.MOLOTOV;
                lastState = PlayerState.MOLOTOV;
            }
        }
        if(haveBendages == true)
        {
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                state = PlayerState.BENDAGES;
                lastState = PlayerState.BENDAGES;
            }
        }
        if (haveLifeBest == true)
        {
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                state = PlayerState.LIFEBEST;
                lastState = PlayerState.LIFEBEST;
            }
        }
        if (haveGloves == true)
        {
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                state = PlayerState.GLOVES;
                lastState = PlayerState.GLOVES;
            }
        }
    }
    public void HaveCraft()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if(inventory[i].item.id == 13)//Gun
            {
                if (inventory[i].amount > 0) haveGun = true;
            }
            if (inventory[i].item.id == 10)//Bullets
            {
                if (inventory[i].amount > 0) haveBullets = true;
            }
            if (inventory[i].item.id == 15)//Molotov
            {
                if (inventory[i].amount > 0) haveMolotov = true;
            }
            if(inventory[i].item.id == 11)//Bendages
            {
                if (inventory[i].amount > 0) haveBendages = true;
            }
            if (inventory[i].item.id == 12)//Life Best
            {
                if (inventory[i].amount > 0) haveLifeBest = true;
            }
            if (inventory[i].item.id == 14)//Gloves
            {
                if (inventory[i].amount > 0) haveGloves = true;
            }
        }
    }
    private void HotBarSelectColors()
    {
        if (haveGun == true)
            hotBarIcon[1].color = Color.white;
        else
            hotBarIcon[1].color = haveNoIcon;

        if (haveMolotov == true)
            hotBarIcon[2].color = Color.white;
        else
            hotBarIcon[2].color = haveNoIcon;

        if (haveBendages == true)
            hotBarIcon[3].color = Color.white;
        else
            hotBarIcon[3].color = haveNoIcon;

        if (haveLifeBest == true)
            hotBarIcon[4].color = Color.white;
        else
            hotBarIcon[4].color = haveNoIcon;

        if (haveGloves == true)
            hotBarIcon[5].color = Color.white;
        else
            hotBarIcon[5].color = haveNoIcon;

        if(state == PlayerState.NORMAL)
        {
            hotBarSelect[0].color = Color.white;
            hotBarSelect[1].color = haveNoIcon;
            hotBarSelect[2].color = haveNoIcon;
            hotBarSelect[3].color = haveNoIcon;
            hotBarSelect[4].color = haveNoIcon;
            hotBarSelect[5].color = haveNoIcon;
        }
        if (state == PlayerState.SHOOT)
        {
            hotBarSelect[0].color = haveNoIcon;
            hotBarSelect[1].color = Color.white;
            hotBarSelect[2].color = haveNoIcon;
            hotBarSelect[3].color = haveNoIcon;
            hotBarSelect[4].color = haveNoIcon;
            hotBarSelect[5].color = haveNoIcon;
        }
        if (state == PlayerState.MOLOTOV)
        {
            hotBarSelect[0].color = haveNoIcon;
            hotBarSelect[1].color = haveNoIcon;
            hotBarSelect[2].color = Color.white;
            hotBarSelect[3].color = haveNoIcon;
            hotBarSelect[4].color = haveNoIcon;
            hotBarSelect[5].color = haveNoIcon;
        }
        if (state == PlayerState.BENDAGES)
        {
            hotBarSelect[0].color = haveNoIcon;
            hotBarSelect[1].color = haveNoIcon;
            hotBarSelect[2].color = haveNoIcon;
            hotBarSelect[3].color = Color.white;
            hotBarSelect[4].color = haveNoIcon;
            hotBarSelect[5].color = haveNoIcon;
        }
        if (state == PlayerState.LIFEBEST)
        {
            hotBarSelect[0].color = haveNoIcon;
            hotBarSelect[1].color = haveNoIcon;
            hotBarSelect[2].color = haveNoIcon;
            hotBarSelect[3].color = haveNoIcon;
            hotBarSelect[4].color = Color.white;
            hotBarSelect[5].color = haveNoIcon;
        }
        if (state == PlayerState.GLOVES)
        {
            hotBarSelect[0].color = haveNoIcon;
            hotBarSelect[1].color = haveNoIcon;
            hotBarSelect[2].color = haveNoIcon;
            hotBarSelect[3].color = haveNoIcon;
            hotBarSelect[4].color = haveNoIcon;
            hotBarSelect[5].color = Color.white;
        }
    }
    //Crafteos
    private void PlayerMovement()
    {
        isGrounded = Physics2D.OverlapArea(p1.position, p2.position, groundMask);
        anim.SetBool("isGrounded", isGrounded);
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * 12, rb.velocity.y);
        if (isGrounded == true)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector2.up * 22, ForceMode2D.Impulse);
            }
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            anim.SetBool("isMoving", true);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            dir = 1;
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            anim.SetBool("isMoving", true);
            transform.rotation = Quaternion.Euler(0, 180, 0);
            dir = -1;
        }
        if(Input.GetAxis("Horizontal") == 0) anim.SetBool("isMoving", false);
    }
    private void RazorAttack()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isAttack = true;
            anim.SetTrigger("razorAttack");
            if (dir == 1)
            {
                GameObject razorHit = Instantiate(golpeNavaja, new Vector2(transform.position.x + 2, transform.position.y), Quaternion.identity);
                Destroy(razorHit, 0.05f);
                //playerRight = true;
            }
            else
            {
                GameObject razorHit = Instantiate(golpeNavaja, new Vector2(transform.position.x - 2, transform.position.y), Quaternion.identity);
                Destroy(razorHit, 0.05f);
                //playerRight = false;
            }
        }
        TimeAttack();
    }
    private void TimeAttack()
    {
        if (isAttack == true)
        {
            rb.velocity *= 0;
            attackTime += Time.deltaTime;
            if (attackTime >= 0.30f)
            {
                isAttack = false;
                attackTime = 0;
            }
        }
    }
    private void ShootAttack()
    {
        if(Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("isShooting");
            if(haveBullets == true)
            {
                SoundManager.PlaySound("GunShotSound");
                if (dir == 1)
                {
                    GameObject PlayerBullet = Instantiate(bullet, bulletInitPos.transform.position, Quaternion.identity);
                }
                else
                {
                    GameObject PlayerBullet = Instantiate(bullet, bulletInitPos.transform.position, Quaternion.Euler(0, 0, 180));
                }
                for (int i = 0; i < inventory.Count; i++)
                {
                    if (inventory[i].item.id == 10) RemoveItem(inventory[i].item.id, 1);
                }
            }
            else
            {
                SoundManager.PlaySound("SinBalasSound");
            }
        }
    }
    private void MolotovAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isAttack = true;
            anim.SetTrigger("isMolotov");
            if (dir == 1)
            {
                GameObject molotov_ = Instantiate(molotov, transform.position, Quaternion.identity);
                molotov_.GetComponent<MolotovControl>().player = this;
            }
            else
            {
                GameObject molotov_ = Instantiate(molotov, transform.position, Quaternion.Euler(0, 180, 0));
                molotov_.GetComponent<MolotovControl>().player = this;
            }
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].item.id == 15) RemoveItem(inventory[i].item.id, 1);
            }
        }
        TimeAttack();
    }
    private void BendageUsing()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(lifes < 4)
            {
                lifes++;
                GameObject newLife = Instantiate(oneLife, gridLife);
            }
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].item.id == 11) RemoveItem(inventory[i].item.id, 1);
            }
            //if (lifes > 5) lifes = 5;
        }
    }
    private void LifeBest()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SoundManager.PlaySound("CremalleraSound");
            if (shield < 3)
            {
                shield++;
                GameObject newShield = Instantiate(oneShield, gridShield);
            }
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].item.id == 12) RemoveItem(inventory[i].item.id, 1);
            }
            //if (shield > 3) shield = 3;
        }
    }
    public void GetDamage()
    {
        //if(enemyPruebas.dir == -1)
        //{
        //    rb.AddForce(Vector2.up * 4, ForceMode2D.Impulse);
        //    rb.AddForce(Vector2.left * 20, ForceMode2D.Impulse); // <---
        //}
        //if(enemyPruebas.dir == 1)
        //{
        //    rb.AddForce(Vector2.up * 4, ForceMode2D.Impulse);
        //    rb.AddForce(Vector2.right * 20, ForceMode2D.Impulse); // <---
        //}
        SoundManager.PlaySound("Herido1Sound");
        if (shield > 0)
        {
            shield--;
            Destroy(gridShield.transform.GetChild(gridShield.childCount - 1).gameObject);
        }
        else
        {
            lifes--;
            Destroy(gridLife.transform.GetChild(gridLife.childCount - 1).gameObject);
        }
        if (shield <= 0 && lifes <= 0)
        {
            //Muerte
            print("Moriste puto");
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Item")
        {
            AddItem(other.GetComponent<ItemsControl>().id, other.GetComponent<ItemsControl>().amount);
            Destroy(other.gameObject);
        }
        if (other.tag == "EnemyHit") GetDamage();
        if (other.tag == "Climb") climbArea = true;
        if (other.tag == "CraftTable") craftTable = true;
        if (other.tag == "MetalFence") fenceSound = true;
        if (other.tag == "End") end = true;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Climb")
        {
            climbArea = false;
            if(state == PlayerState.CLIMB)
            {
                state = PlayerState.NORMAL;
                rb.isKinematic = false;
            }
        }
        if (other.tag == "CraftTable") craftTable = false;
        if (other.tag == "MetalFence") fenceSound = false;
        if (other.tag == "End") end = false;
    }
}

[System.Serializable]
public class InventoryProperties
{
    public ItemsProperties item;
    public int amount;

    public InventoryProperties(ItemsProperties _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }
}


//mejorar enemy
//Sonidos: pasos, molotov, ataque navaja, vendas
//Musica fondo
