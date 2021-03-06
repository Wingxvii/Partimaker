﻿using System.Collections;
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
	public bool useNoise;
	public float noiseStrength;
	public int noiseFrequency;
	public bool smoothing;

	private int frequencyCurrent;
	private Vector3 startScale;

	private void Start()
	{
		self = this.GetComponent<Transform>();
		selfSprite = this.GetComponent<SpriteRenderer>().color;
		startScale = self.localScale;
		spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<PartiMaker>();
		frequencyCurrent = noiseFrequency;
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
		//increments
		frequencyCurrent++;

		//noise
		if (frequencyCurrent >= noiseFrequency && useNoise)
		{
			velocity += new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0) * noiseStrength;
			frequencyCurrent = 0;
		}

		//base physics
		Vector3 force = new Vector3(0.0f, 0.0f - gravity, 0.0f);
		Vector3 acceleration = force / mass;
		velocity += acceleration;
		position += velocity;

		//lerping over time
		float interp = age / lifetime;
		alpha = Mathf.Lerp(lerpAlpha.x, lerpAlpha.y, interp);
		size = Mathf.Lerp(LerpSize.x, LerpSize.y, interp);
	}

}
