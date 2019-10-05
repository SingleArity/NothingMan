using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour 
{
	public float xMargin = 1f;		// Distance in the x axis the player can move before the camera follows.
	public float yMargin = .5f;		// Distance in the y axis the player can move before the camera follows.
	 float xSmooth = 6f;      // How smoothly the camera catches up with it's target movement in the x axis.
     float ySmooth = 6f;	// How smoothly the camera catches up with it's target movement in the y axis.
	public Vector2 maxXAndY;		// The maximum x and y coordinates the camera can have.
	public Vector2 minXAndY;		// The minimum x and y coordinates the camera can have.


	public Transform player;		// Reference to the player's transform.
	public Transform target;		//the camera's target object of interest

	public bool trackingEnabled;

    public float targetX, targetY;

	void Awake() {

        //track the player from the get-go, cause intro cutscene needs this camera position
        //transform.position = new Vector3(target.position.x, target.position.y + 2f, transform.position.z);
        trackingEnabled = true;
        DontDestroyOnLoad(this.gameObject);
	}

	void Start ()
	{
		
	}

	public void SceneSetup(){
		// Setting up the player reference.
		
		player = target;
     
        trackingEnabled = true;
            
        
		Debug.Log ("tp:"+target.position);
		transform.position = new Vector3(target.position.x,transform.position.y, transform.position.z);
	}

	public void OnPlayerRespawn(){
		
		
		player = target;
		trackingEnabled = true;

		//transform.position = new Vector3(0f,0f,-10f);

	}



	bool CheckXMargin()
	{
		// Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
		return Mathf.Abs(transform.position.x - target.position.x) > xMargin;
	}

	public void SetYMargin(float newMargin){
		yMargin = newMargin;
	}

	bool CheckYMargin()
	{
		// Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
		return Mathf.Abs(transform.position.y - target.position.y) > yMargin;
	}


	void FixedUpdate ()
	{
		if (trackingEnabled) {
            if(target != null)
			TrackPlayer ();
		}
	}


	
	public void SetTrackingEnabled(bool b){
		trackingEnabled = b;
	}

	void TrackPlayer ()
	{
		// By default the target x and y coordinates of the camera are it's current x and y coordinates.
		targetX = target.position.x;
		targetY = target.position.y;
        
		// If the player has moved beyond the x margin...
		if(CheckXMargin())
			// ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
			targetX = Mathf.Lerp(transform.position.x, target.position.x + .2f, xSmooth * Time.deltaTime);

		// If the player has moved beyond the y margin...
		if(CheckYMargin())
			// ... the target y coordinate should be a Lerp between the camera's current y position and a bit above the player's current y position.
			targetY = Mathf.Lerp(transform.position.y, target.position.y, ySmooth * Time.deltaTime);

		// The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
		targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
		targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

		// Set the camera's position to the target position with the same z component.
		// NOTE: JUST CHANGES X POSITION RIGHT NOW
		/*if (player.GetComponent<PlayerController> ().FacingRight () //&& target == player) {
			transform.position = new Vector3 (targetX + .2f, transform.position.y, transform.position.z);
		} else {*/
			transform.position = new Vector3 (targetX, targetY, transform.position.z);
		//}
	}

	public void ScreenShake(float amount, float vrange, float hrange){
		StartCoroutine (Shake (amount, vrange, hrange));
	}

	//shakes screen for amt time, shake range is represented by vrng(vertical) and hrange(horizontal)
	public IEnumerator Shake(float amt, float vrng, float hrng){
		Vector3 origpos = transform.position;
		//shake for amt time
		float endTime = Time.time + amt;
		//every .05 second, add random direction shift
		while (Time.time < endTime) {
			transform.position += new Vector3(Random.Range (-1*hrng, hrng), Random.Range (-1*vrng, vrng), 0f);
			//shake by changing position 8 times
			yield return new WaitForSeconds (.02f);
		}
		//return camera to original pre-shake position
		transform.position = origpos;
	}

	public void MoveTo(Vector3 newPlace){
		transform.position = newPlace;
	}
}
