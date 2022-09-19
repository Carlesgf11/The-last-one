using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesactiveChangeScene : MonoBehaviour
{
    public Animator anim;
    public GameObject changeScene;

    public void DesactiveCinematic()
    {
        anim.SetBool("changeScene", false);
        changeScene.SetActive(false);
    }
}
