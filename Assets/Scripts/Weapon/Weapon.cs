using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	private Animator anim; 
	private AudioSource _AudioSource;

	public float range = 100f; // Maximum range of the weapon
	public int bulletsPerMag = 30; // Bullets Per Magazine
	public int bulletsLeft = 200; // Total bullets we have

	public int currentBullets; // The current bullets in our magazine

	public Transform shootPoint; // The point from which the bullet leaves the muzzle
	public ParticleSystem muzzleFlash; // Muzzle flash?
	public AudioClip shootSound;


	public float fireRate = 0.1f; // The delay between each shot 100ms

	float fireTimer; // Time counter for the delay

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		_AudioSource = GetComponent<AudioSource>();

		currentBullets = bulletsPerMag;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButton ("Fire1")) {
			// print("FIRED");
			Fire (); //Execute the fire function if we press/hold the left mouse button
		}

		if (fireTimer < fireRate) {
			fireTimer += Time.fixedDeltaTime; //Add into time counter
		}

	}

	void FixedUpdate(){
		AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo (0);

		// if (info.IsName ("Fire")) anim.SetBool ("Fire", false);
	}

	private void Fire(){
		if (fireTimer < fireRate || currentBullets <= 0)
			return;

		RaycastHit hit;

		if(Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, range)){
			Debug.Log (hit.transform.name + " FOUND");
		}

		anim.CrossFadeInFixedTime ("Fire", 0.1f);
		muzzleFlash.Play();
		_AudioSource.PlayOneShot(shootSound, 0.7f);

		//anim.SetBool ("Fire", true);
		currentBullets--;
		fireTimer = 0.0f; //Reset Fire Timer
	}
}
