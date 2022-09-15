using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class YellowUsamyuDelete : MonoBehaviour
{
    [SerializeField] GameObject sparklePrefab;
 
    void OnMouseDown()
    {
        Instantiate (sparklePrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}