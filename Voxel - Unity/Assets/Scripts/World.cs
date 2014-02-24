using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {
	
	public byte[,,] data;
	public int worldX = 160;
	public int worldY = 32;
	public int worldZ = 160;
	
	public GameObject chunk;
	public Chunk[,,] chunks;
	public int chunkSize = 16;

	void Start() {
		data = new byte[worldX, worldY, worldZ];		
		for (int x = 0; x < worldX; x++) {
			for (int z = 0; z < worldZ; z++) {
				int stone = PerlinNoise(x, 0, z, 10, 2, 1.2f);
				stone += PerlinNoise(x, 300, z, 20, 3, 0) + 8;
				int dirt = PerlinNoise(x, 200, z, 50, 2, 0) + 1;
				for (int y = 0; y < worldY; y++) {
					if(y <= stone) {
						data[x, y, z] = 1;
					} else if(y <= dirt + stone) {
						data[x, y, z] = 2;
					} else {
						data[x, y, z] = 0;
					}
				}
			}
		}
		
		chunks = new Chunk[Mathf.FloorToInt(worldX / chunkSize), Mathf.FloorToInt(worldY / chunkSize), Mathf.FloorToInt(worldZ / chunkSize)];
		for (int x = 0; x < chunks.GetLength(0); x++) {
 			for (int y = 0; y < chunks.GetLength(1); y++) {
  				for (int z = 0; z < chunks.GetLength(2); z++) {
					GameObject newChunk = Instantiate(chunk, new Vector3(x * chunkSize - 0.5f, y * chunkSize + 0.5f, z * chunkSize - 0.5f), new Quaternion(0, 0, 0, 0)) as GameObject;
					
					chunks[x, y, z] = newChunk.GetComponent("Chunk") as Chunk;
					chunks[x, y, z].worldGO = gameObject;
					chunks[x, y, z].chunkSize = chunkSize;
					chunks[x, y, z].chunkX = x * chunkSize;
					chunks[x, y, z].chunkY = y * chunkSize;
					chunks[x, y, z].chunkZ = z * chunkSize;
				}
			}
		}
		
		chunk.SetActive(false);
	}
	
	int PerlinNoise(int x, int y, int z, float scale, float height, float power) {
		float rValue = Noise.GetNoise(((double) x) / scale, ((double) y) / scale, ((double) z) / scale);
		rValue *= height;
 		if(power != 0) {
			rValue = Mathf.Pow(rValue, power);
		}
		
		return (int) rValue;
	}
	
	public byte Block(int x, int y, int z) {
 		if(x >= worldX || x < 0 || y >= worldY || y < 0 || z >= worldZ || z < 0) {
  			return (byte) 1;
 		} else if (data == null) {
			return (byte) 0;
		} else {
			return data[x, y, z];
		}
	}
	
	void Update() {
	
	}
}
