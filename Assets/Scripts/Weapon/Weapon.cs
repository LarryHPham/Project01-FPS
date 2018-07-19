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

	private bool isReloading = false;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		_AudioSource = GetComponent<AudioSource>();

		currentBullets = bulletsPerMag;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButton ("Fire1")) {
			if(currentBullets > 0) Fire (); //Execute the fire function if we press/hold the left mouse button
			// else // play no clip sound
		}

		if (Input.GetKeyDown(KeyCode.R)){
			if(currentBullets < bulletsPerMag && bulletsLeft > 0)
			DoReload();
		}
		if (fireTimer < fireRate) {
			fireTimer += Time.fixedDeltaTime; //Add into time counter
		}

	}

	void FixedUpdate(){
		AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo (0);

		isReloading = info.IsName("Reload");
		// if (info.IsName ("Fire")) anim.SetBool ("Fire", false);
	}

	private void Fire(){
		if (fireTimer < fireRate)
			return;

		RaycastHit hit;

		if(Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, range)){
			Debug.Log (hit.transform.name + " FOUND");
		}

		anim.CrossFadeInFixedTime ("Fire", 0.1f);
		muzzleFlash.Play(); // show muzzle flash
		_AudioSource.PlayOneShot(shootSound, 0.7f); // play shooting sound at 0.0f - 1.0f volume

		//anim.SetBool ("Fire", true);
		currentBullets--;
		fireTimer = 0.0f; //Reset Fire Timer
	}

	private void Reload(){
		if(bulletsLeft <= 0) return;

		int bulletsToLoad = bulletsPerMag - currentBullets;
		int bulletsToDeduct = (bulletsLeft >= bulletsToLoad) ? bulletsToLoad : bulletsLeft;

		bulletsLeft -= bulletsToDeduct;
		currentBullets += bulletsToDeduct;
	}

	private void DoReload(){
		AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo (0);

		if(isReloading) return;

		anim.CrossFadeInFixedTime("Reload", 0.01f);
	}
}
