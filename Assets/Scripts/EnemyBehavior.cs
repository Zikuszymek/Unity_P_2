using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {

	public float laserSpeed = 5f;
	public float shotsPerSecond = 0.5f;
	public float health = 150;
	public GameObject enemyLaser;
	public int scoreValue = 150;
	private ScroreKeeper scoreKeeper;

    public bool shipArrived = false;
	public AudioClip fireSound;
	public AudioClip deathSound;

    private EnemySpawner enemySpawner;

	void Start(){
		scoreKeeper = GameObject.Find ("Score").GetComponent<ScroreKeeper>();
        enemySpawner = GameObject.FindObjectOfType<EnemySpawner>();
	}

	void OnTriggerEnter2D(Collider2D collider){
		Projectile missile = collider.gameObject.GetComponent<Projectile> ();
		if (missile != null) {
			health -= missile.GetDamage();
			if(health <= 0){
				Die ();
			}
			missile.Hit();
		}
	}

	void Die(){
		Destroy (this.gameObject);
		scoreKeeper.Score(scoreValue);
		AudioSource.PlayClipAtPoint(deathSound, transform.position);
        enemySpawner.notifyShipDestroyed();

    }
	
	void Update(){
		float probability = Time.deltaTime * shotsPerSecond;
		if (shipArrived && Random.value < probability) {
			Fire ();
		}
	}

	void Fire(){
		Vector3 startPosition = transform.position + Vector3.down * 0.5f;
		GameObject laser = Instantiate (enemyLaser, startPosition, Quaternion.identity) as GameObject;
		laser.transform.GetComponent<Rigidbody2D>().velocity = new Vector3 (0, -laserSpeed, 0);
		AudioSource.PlayClipAtPoint (fireSound, transform.position);
	}

    void setShipArrived()
    {
        shipArrived = true;
    }
}
