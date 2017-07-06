using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemyPrefab;

	public float widht = 10f;
	public float height = 5f;
	public float speed;
	public float spawnDelay = 0.5f;

	float xmin;
	float xmax;

    private bool toLeft = true;

	void Start () {
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftMost = Camera.main.ViewportToWorldPoint (new Vector3 (0,0,distance));
		Vector3 rightMost = Camera.main.ViewportToWorldPoint (new Vector3 (1,0,distance));
		xmax = rightMost.x - widht / 2;
		xmin = leftMost.x + widht / 2;
		spawnTheEnemies ();
	}

	public void OnDrawGizmos(){
		Gizmos.DrawWireCube(transform.position, new Vector3(widht,height));
	}
	
	void Update () {
		if (toLeft) {
			this.transform.position += Vector3.left * speed * Time.deltaTime;
		} else {
			this.transform.position += Vector3.right * speed * Time.deltaTime;
		}

		if (transform.position.x <= xmin)
			toLeft = false;
		if (transform.position.x >= xmax)
			toLeft = true;
		float nexX = Mathf.Clamp (transform.position.x, xmin, xmax);
		transform.position = new Vector3 (nexX, transform.position.y, transform.position.z);

	}

	public bool AllMembersDead(){
		foreach (Transform childObject in transform) {
            if (childObject.childCount > 0) {
                return false;
			}
		}
		return true;
	}

	void SpawnUntilFull(){
		Transform transformEnemy = NextFreePosition ();
		if (transformEnemy != null) {
            spawnEnemyInstance(transformEnemy);
            Invoke ("SpawnUntilFull", spawnDelay);
		} 
	}

	Transform NextFreePosition(){
		foreach (Transform childObject in transform) {
			if(childObject.childCount <= 0){
				return childObject.transform;
			}
		}
		return null;
	}

	void spawnTheEnemies(){
		foreach (Transform child in transform) {
            float randomTime = Random.Range(0, 2f);
            StartCoroutine(SpawnEnemy(child));
        }
	}

    public void spawnEnemyInstance(Transform t)
    {
        GameObject enemy = Instantiate(enemyPrefab, t.transform.position, Quaternion.identity) as GameObject;
        enemy.transform.parent = t;
    }

    public IEnumerator SpawnEnemy(Transform t)
    {
        float randomSeconds = Random.Range(0f, 1f);
        yield return new WaitForSeconds(randomSeconds);
        spawnEnemyInstance(t);
    }

    public void notifyShipDestroyed()
    {
        Invoke("CheckSpaceShips",1f);
    }

    private void CheckSpaceShips()
    {
        if (AllMembersDead())
        {
            SpawnUntilFull();
        }
    }
}
