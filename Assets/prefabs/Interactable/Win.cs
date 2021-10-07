using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Win : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Text YouWinText;

    private void Start()
    {
        YouWinText.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Player>() != null)
        {
            YouWinText.enabled = true;
        }
    }
}
