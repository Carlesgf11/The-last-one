using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazorReturn : MonoBehaviour
{
    public PlayerControl player;
    public void RazorReturns()
    {
        player.rb.isKinematic = true;
        //player.state = PlayerState.NORMAL;
    }
}
