using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    private GameObject activedText;
    public List<GameObject> text;
    public int currentText;
    void Start()
    {
        currentText = 0;
        for (int i = 0; i < text.Count; i++)
        {
            if (text[i] != text[currentText])
            {
                text[i].SetActive(false);
            }
        }
    }

    void Update()
    {
        activedText = text[currentText];
        text[currentText].SetActive(true);
    }

    public void NextText()
    {
        currentText++;
        for (int i = 0; i < text.Count; i++)
        {
            if(text[i] != text[currentText])
            {
                text[i].SetActive(false);
            }
        }
        if (currentText == text.Count - 1) SceneManager.LoadScene(2);
    }
}
