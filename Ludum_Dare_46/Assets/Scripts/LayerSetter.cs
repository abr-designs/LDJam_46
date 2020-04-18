using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LayerSetter : MonoBehaviour
{
    [SerializeField]
    private int currentLayer;
    [SerializeField]
    private int offset;
    
    private SpriteRenderer renderer;

    private new Transform transform;
    // Start is called before the first frame update
    private void Start()
    {
        transform = gameObject.transform;
        
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        currentLayer =  Mathf.RoundToInt(transform.position.y * -10f);

        renderer.sortingOrder = currentLayer + offset;
    }
}
