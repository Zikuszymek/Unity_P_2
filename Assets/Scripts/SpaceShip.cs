using UnityEngine;
using System.Collections;

public class SpaceShip : MonoBehaviour {

	public float speed = 5.0f;
	public float padding = 1f;
	public GameObject beam;
	public float projectileSpeed;
	public float firingRate = 0.2f;

	float xmin,ymin,xmax,ymax;

	public AudioClip fireGuns;

	void Start(){
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftMost = Camera.main.ViewportToWorldPoint (new Vector3 (0,0,distance));
		Vector3 rightMost = Camera.main.ViewportToWorldPoint (new Vector3 (1,0,distance));
        xmax = rightMost.x - padding;
		xmin = leftMost.x + padding;
        ymin = leftMost.y +0.5f;
        ymax = ymin + 3f;
	}

	void Update () {
		if (Input.GetKey (KeyCode.LeftArrow)) {
			this.transform.position += Vector3.left * speed * Time.deltaTime;

		} else if (Input.GetKey (KeyCode.RightArrow)) {
			this.transform.position += Vector3.right * speed * Time.deltaTime;

		} 
		if (Input.GetKey (KeyCode.DownArrow)) {
			this.transform.position += Vector3.down * speed * Time.deltaTime;

		} else if (Input.GetKey (KeyCode.UpArrow)) {
			this.transform.position += Vector3.up * speed * Time.deltaTime;

		}

		float nexX = Mathf.Clamp (transform.position.x, xmin, xmax);
        float nextY = Mathf.Clamp(transform.position.y, ymin, ymax);
		transform.position = new Vector3 (nexX, nextY, transform.position.z);

		if (Input.GetKeyDown (KeyCode.Space)) {
			InvokeRepeating("ShootBeam", 0.1f, firingRate);
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			CancelInvoke("ShootBeam");
		}
	}

	private void ShootBeam(){
		GameObject laserBeam = Instantiate (beam, transform.position + new Vector3(0,0.5f,0), Quaternion.identity) as GameObject;
		laserBeam.GetComponent<Rigidbody2D>().velocity = new Vector3 (0, projectileSpeed, 0);
		AudioSource.PlayClipAtPoint (fireGuns, transform.position);
	}

	void OnTriggerEnter2D(Collider2D collider){
		Projectile missile = collider.gameObject.GetComponent<Projectile> ();
		if (missile != null) {
			LevelManager levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
			levelManager.loadLevel("Lose");
			missile.Hit();
			Destroy (this.gameObject);
		}
	}
}
