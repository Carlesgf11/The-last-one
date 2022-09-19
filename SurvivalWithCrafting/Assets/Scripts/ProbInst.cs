using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbInst : MonoBehaviour
{
    public List<PercentProperties> allItems;
    public int finalId, finalAmount;

    //public Animator anim;
    public GameObject animBarril;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "RazorHit")
        {
            //anim.SetBool("isDestroyed", true);
            SoundManager.PlaySound("GolpeMetalSound");
            finalId = -1;
            finalAmount = 0;
            int totalPercent = 0;
            for (int i = 0; i < allItems.Count; i++)
            {
                totalPercent += allItems[i].percent;
            }
            int randomPercent = Random.Range(0, totalPercent);
            for (int i = 0; i < allItems.Count; i++)
            {
                randomPercent -= allItems[i].percent;
                if(randomPercent <= 0)
                {
                    finalId = allItems[i].item.id;
                    finalAmount = Random.Range(1, allItems[i].amount + 1);
                    //print(allItems[i].item.ItemName);
                    GameObject itemPref = Instantiate(allItems[i].pref, transform.position, Quaternion.identity);
                    itemPref.GetComponent<ItemsControl>().amount = finalAmount;
                    break;
                }
            }
            //GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().AddItem(finalId, finalAmount);
            GameObject newBarrilAnim = Instantiate(animBarril, transform.position, Quaternion.identity);
            Destroy(gameObject);
            //anim.SetBool("isDestroyed", true);
        }
    }
    
}
[System.Serializable]
public class PercentProperties
{
    public GameObject pref;
    public ItemsControl item;
    [Range(0,100)]
    public int percent;
    [Range(1, 6)]
    public int amount;
}
