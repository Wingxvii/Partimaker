using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleList : MonoBehaviour {
	public Transform self;
	public Color selfSprite;
	public PartiMaker spawner;

	public Vector3 position;
	public Vector3 velocity;

	public float size;
	public float alpha;
	public float age;
	public float lifetime;
	public float mass;
	public float gravity;
	public Vector2 lerpAlpha;
	public Vector2 LerpSize;

	private Vector3 startScale;

	private void Start()
	{
		self = this.GetComponent<Transform>();
		selfSprite = this.GetComponent<SpriteRenderer>().color;
		startScale = self.localScale;
		spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<PartiMaker>();
	}

	// Update is called once per frame
	void Update () {
		age += Time.deltaTime;

		//delete if aged
		if (age > lifetime) {
			spawner.currParticles--;
			Object.Destroy(this.gameObject);
		}

		//update info
		self.position = position;
		self.localScale = startScale * size;
		selfSprite.a = alpha;
	}

	private void FixedUpdate()
	{
		//update math

		Vector3 force = new Vector3(0.0f, 0.0f - gravity, 0.0f);
		Vector3 acceleration = force / mass;

		velocity += acceleration;
		position += velocity;

		float interp = age / lifetime;
		alpha = Mathf.Lerp(lerpAlpha.x, lerpAlpha.y, interp);
		size = Mathf.Lerp(LerpSize.x, LerpSize.y, interp);
	}


}
