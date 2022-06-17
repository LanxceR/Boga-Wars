using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAutoScrollbar : MonoBehaviour
{
    [Header("Main Setting")]
    [SerializeField] private float scrollSpeed = 1f;
    [SerializeField] private Transform content;

    private Scrollbar scrollbar;

    // Start is called before the first frame update
    void Start()
    {
        scrollbar = GetComponent<Scrollbar>();
    }

    // Update is called once per frame
    void Update()
    {
        if (scrollbar.value > 0)
        {
            content.position = new Vector3(content.position.x, content.position.y + Time.deltaTime * scrollSpeed);
        }
    }
}
