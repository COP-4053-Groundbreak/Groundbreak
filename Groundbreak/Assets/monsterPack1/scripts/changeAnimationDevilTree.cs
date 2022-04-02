using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeAnimationDevilTree : MonoBehaviour {

	Animator animator;

	AudioSource source;
	public AudioClip clipFx;

	public int numberOfAnimations = 5;
	int actualAnimation;


	void Awake(){
		animator = GetComponent<Animator> ();
		source = GetComponent<AudioSource> ();
	}

	void OnGUI(){

		if (GUI.Button (new Rect (Screen.width *0.54f, Screen.height * 0.8f, Screen.width * 0.15f, Screen.height * 0.1f), "Change to next animation")) {
			changeAnimation ();
		}
	}

	void changeAnimation(){
		source.PlayOneShot (clipFx, 1f);
		actualAnimation++;

		if (actualAnimation > numberOfAnimations) {
			actualAnimation = 0;
		}

		if (actualAnimation == 0) {
			actualAnimation = 0;
			lockOtherAnimations ();
			animator.SetBool ("changeToAnim0", true);
		} 

		else if (actualAnimation == 1) {
			actualAnimation = 1;
			lockOtherAnimations ();
			animator.SetBool ("changeToAnim1", true);
		} 

		else if (actualAnimation == 2) {
			actualAnimation = 2;
			lockOtherAnimations ();
			animator.SetBool ("changeToAnim2", true);
		} 

		else if (actualAnimation == 3) {
			actualAnimation = 3;
			lockOtherAnimations ();
			animator.SetBool ("changeToAnim3", true);
		} 

		else if (actualAnimation == 4) {
			actualAnimation = 4;
			lockOtherAnimations ();
			animator.SetBool ("changeToAnim4", true);
		} 

		else if (actualAnimation == 5) {
			actualAnimation = 5;
			lockOtherAnimations ();
			animator.SetBool ("changeToAnim5", true);
		} 

	}

	void lockOtherAnimations(){
		animator.SetBool ("changeToAnim0", false);
		animator.SetBool ("changeToAnim1", false);
		animator.SetBool ("changeToAnim2", false);
		animator.SetBool ("changeToAnim3", false);
		animator.SetBool ("changeToAnim4", false);
		animator.SetBool ("changeToAnim5", false);
	}

}
