using UnityEngine;
using System.Collections;

public class BoomerangController : MonoBehaviour {

    Rigidbody2D rb;
    GameObject player;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
    IEnumerator waitasecond()
    {
        yield return new WaitForSeconds(1);
        gameObject.tag = "Catchable";
    }

	// Update is called once per frame
	void Update ()
    {
        rb.AddForce((player.transform.position - transform.position).normalized * 5);
	}
}
