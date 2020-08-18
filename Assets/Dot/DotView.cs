using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotView : MonoBehaviour
{
    SpriteRenderer rd;
    private void Awake()
    {
        rd = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update

    public void ConfigColor(Color _color)
    {
        rd.color = _color;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
