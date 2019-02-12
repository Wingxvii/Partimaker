using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PartiMaker : MonoBehaviour {

	//base attributes
	public GameObject particle;

	//Buttons
	public bool play = false;
	public bool parse = false;

	//customizable attributes
	public int maxParticles = 100;
	public int rate = 10;
	public float spawnTime = 2.0f;
	public float gravity = 0.0f;
	public float mass = 1.0f;
	public Vector3 minRangePos = new Vector3(0.0f,0.0f,0.0f);
	public Vector3 maxRangePos = new Vector3(0.0f, 0.0f, 0.0f);
	public Vector2 rangeVel = new Vector2(0.1f,1.0f);
	public Vector2 rangeLifetime = new Vector2(1.0f,10.0f);
	public Vector2 lerpAlpha = new Vector2(1.0f,0.0f);
	public Vector2 lerpSize = new Vector2(1.0f,0.0f);
	public Vector2 minInitialVel = new Vector2(1.0f, -1.0f);
	public Vector2 maxInitialVel = new Vector2(1.0f, -1.0f);

	//parsing data
	public string fileName = "test.txt";

	public float spawnerTime = 0.0f;
	public int currParticles;
	public bool playing = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		int NumToSpawn = rate;

		if (spawnerTime < 0.0f) {
			playing = false;
		}
		spawnerTime -= Time.deltaTime;

		while (currParticles < maxParticles && NumToSpawn > 0 && playing) {
			//spawn 
			GameObject currPart = Instantiate(particle,this.GetComponent<Transform>());

			ParticleList currlist = currPart.GetComponent<ParticleList>();

			currlist.position = new Vector3(Random.Range(minRangePos.x, maxRangePos.x), Random.Range(minRangePos.y, maxRangePos.y), Random.Range(minRangePos.z, maxRangePos.z));
			currlist.velocity = new Vector3(Random.Range(minInitialVel.x, maxInitialVel.x), Random.Range(minInitialVel.y,maxInitialVel.y),0.0f) * Random.Range(rangeVel.x, rangeVel.y);
			currlist.size = lerpSize.x;
			currlist.alpha = lerpAlpha.x;
			currlist.age = 0.0f;
			currlist.lifetime = Random.Range(rangeLifetime.x, rangeLifetime.y);
			currlist.mass = mass;
			currlist.gravity = gravity;
			currlist.lerpAlpha = lerpAlpha;
			currlist.LerpSize = lerpSize;

			currParticles++;
			NumToSpawn--;
		}


		if (play) {
			play = false;
			playing = true;
			spawnerTime = spawnTime;
		}

		if (parse) {
			parse = false;
			OnParse();
		}

	}

	//used to parse
	private void OnParse()
	{
		string output;
		string docPath = Application.dataPath;

		StreamWriter parser = new StreamWriter(Path.Combine(docPath, fileName), true);

		output = string.Format("a{0}", maxParticles);
		parser.WriteLine(output);
		output = string.Format("b{0}", rate);
		parser.WriteLine(output);
		output = string.Format("c{0}", spawnTime);
		parser.WriteLine(output);
		output = string.Format("d{0}", gravity);
		parser.WriteLine(output);
		output = string.Format("e{0}", mass);
		parser.WriteLine(output);
		output = string.Format("f{0},{1}/{2},{3}/{4},{5}", minRangePos.x, maxRangePos.x, minRangePos.y, maxRangePos.y,minRangePos.z,maxRangePos.z);
		parser.WriteLine(output);
		output = string.Format("g{0},{1}", rangeVel.x,rangeVel.y);
		parser.WriteLine(output);
		output = string.Format("h{0},{1}", rangeLifetime.x,rangeLifetime.y);
		parser.WriteLine(output);
		output = string.Format("i{0},{1}", lerpAlpha.x, lerpAlpha.y);
		parser.WriteLine(output);
		output = string.Format("j{0},{1}", lerpSize.x, lerpSize.y);
		parser.WriteLine(output);
		output = string.Format("k{0},{1}/{2},{3}", minInitialVel.x, maxInitialVel.x, minInitialVel.y,maxInitialVel.y);
		parser.WriteLine(output);


		parser.Close();
	}
}
