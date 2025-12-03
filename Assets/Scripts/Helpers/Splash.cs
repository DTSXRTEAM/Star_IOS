using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour {


	public void Start()
	{
		StartCoroutine(GoToMainScene());
	}

	private IEnumerator GoToMainScene()
	{
		yield return new WaitForSeconds(2);
		SceneManager.LoadScene(1);
	}
}
