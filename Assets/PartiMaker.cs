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

	/*Spawning Attributes*/
	public int maxParticles = 100;								//maximum amount of particles that can be present
	public int rate = 10;										//number of particles that can spawn per second
	public float spawnTime = 2.0f;								//the life of the particle spawner in one instance of particle spawning

	/*Physics Attributes*/
	public float gravity = 0.0f;								//the force of gravity on the particle
	public float mass = 1.0f;									//virtual mass of the particle
	public Vector3 minRangePos = new Vector3(0.0f,0.0f,0.0f);	//minimum spawn position (becomes an offset if particle has parent)
	public Vector3 maxRangePos = new Vector3(0.0f, 0.0f, 0.0f); //maximum spawn position (becomes an offset if particle has parent)

	/*Lerping Lifetime Attributes*/
	public Vector2 rangeLifetime = new Vector2(1.0f, 10.0f);	//range of the lifetime of the particle
	public Vector2 lerpAlpha = new Vector2(1.0f,0.0f);          //size of the alpha lerp from x to y within the lifetime
	public Vector2 lerpSize = new Vector2(1.0f,0.0f);			//size of the size lerp from x to y within the lifetime

	/*Innitial Values*/
	public Vector2 rangeVel = new Vector2(0.1f, 1.0f);          //range of the magnitude of innitial velocity
	public Vector2 minInitialVel = new Vector2(1.0f, -1.0f);	//minimum direction of the innitial velocity
	public Vector2 maxInitialVel = new Vector2(1.0f, -1.0f);	//maximum direction of the innitial velocity

	/*Noise Module*/
	public bool noiseOn = false;								//use noise
	public float noiseStrength = 0.0f;							//strength determines effect of noise
	public int noiseFrequency = 0;                              //frequency determines rate of noise addition, up to maximum of 100

	/*Parenting Module*/
	public bool parentToPlayer = false;							//sets origin to player's location

	//parsing data
	public string fileName = "test.txt";						//output file string (must have .txt)

	//privates
	private float spawnerTime = 0.0f;
	public int currParticles;
	private bool playing = false;

	
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
			currlist.noiseStrength = noiseStrength;
			currlist.noiseFrequency = noiseFrequency;
			currlist.useNoise = noiseOn;

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
		int parent = 0;
		if (parentToPlayer) {
			parent = 1;
		}

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
		if (noiseOn)
		{
			output = string.Format("l{0},{1}", noiseStrength, noiseFrequency);
			parser.WriteLine(output);
		}
		output = string.Format("m{0}", parent);
		parser.WriteLine(output);

		parser.Close();
	}
}
