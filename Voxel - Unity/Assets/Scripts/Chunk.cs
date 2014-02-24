using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chunk : MonoBehaviour {
	
	public GameObject worldGO;
	private World world;
	
	private List<Vector3> newVertices = new List<Vector3>();
	private List<Vector2> newUV = new List<Vector2>();
    private List<int> newTriangles = new List<int>();
	  
	private Vector2 tStone = new Vector2(1, 0);
	private Vector2 tGrass = new Vector2(0, 1);
	private Vector2 tGrassTop = new Vector2(1, 1);
	private float tUnit = 0.25f;
	  
	private Mesh mesh;
	private MeshCollider col;
	  
	private int faceCount;
	public int chunkSize = 16;
	
	public int chunkX;
	public int chunkY;
	public int chunkZ;
	
	public bool update;

	void Start() {
		world = worldGO.GetComponent("World") as World;
		
		mesh = GetComponent<MeshFilter>().mesh;
		col = GetComponent<MeshCollider>(); 
		
		GenerateMesh();
	}
	
	byte Block(int x, int y, int z) {
		return world.Block(x + chunkX, y + chunkY, z + chunkZ);
	}
	
	void CubeTop(int x, int y, int z, byte block) {
		newVertices.Add(new Vector3(x, y, z + 1));
		newVertices.Add(new Vector3(x + 1, y, z + 1));
		newVertices.Add(new Vector3(x + 1, y, z));
		newVertices.Add(new Vector3(x, y, z));
		
  		Cube(DecideTexture(Block(x, y, z)));
 	}
	
	void CubeBot(int x, int y, int z, byte block) {
		newVertices.Add(new Vector3(x, y - 1, z));
		newVertices.Add(new Vector3(x + 1, y - 1, z));
		newVertices.Add(new Vector3(x + 1, y - 1, z + 1));
		newVertices.Add(new Vector3(x, y - 1, z + 1));
		
  		Cube(DecideTexture(Block(x, y, z)));
 	}
	
	void CubeWest(int x, int y, int z, byte block) {
		newVertices.Add(new Vector3(x, y - 1, z + 1));
		newVertices.Add(new Vector3(x, y, z + 1));
		newVertices.Add(new Vector3(x, y, z));
		newVertices.Add(new Vector3(x, y - 1, z));
		
  		Cube(DecideTexture(Block(x, y, z)));
 	}
	
	void CubeEast(int x, int y, int z, byte block) {
		newVertices.Add(new Vector3(x + 1, y - 1, z));
		newVertices.Add(new Vector3(x + 1, y, z));
		newVertices.Add(new Vector3(x + 1, y, z + 1));
		newVertices.Add(new Vector3(x + 1, y - 1, z + 1));
		
  		Cube(DecideTexture(Block(x, y, z)));
 	}
	
	void CubeNorth(int x, int y, int z, byte block) {
		newVertices.Add(new Vector3(x + 1, y - 1, z + 1));
		newVertices.Add(new Vector3(x + 1, y, z + 1));
		newVertices.Add(new Vector3(x, y, z + 1));
		newVertices.Add(new Vector3(x, y - 1, z + 1));
		
  		Cube(DecideTexture(Block(x, y, z)));
 	}
	
	void CubeSouth(int x, int y, int z, byte block) {
		newVertices.Add(new Vector3(x, y - 1, z));
		newVertices.Add(new Vector3(x, y, z));
		newVertices.Add(new Vector3(x + 1, y, z));
		newVertices.Add(new Vector3(x + 1, y - 1, z));
		
  		Cube(DecideTexture(Block(x, y, z)));
 	}
	
	Vector2 DecideTexture(int value) {
		if(value == 1) {
			return tStone;
		} else if(value == 2) {
			return tGrassTop;
		} else {
			return new Vector2(0, 0);
		}
	}
	
	void Cube(Vector2 texturePos) {
		newTriangles.Add(faceCount * 4);
		newTriangles.Add(faceCount * 4 + 1);
		newTriangles.Add(faceCount * 4 + 2);
		newTriangles.Add(faceCount * 4);
		newTriangles.Add(faceCount * 4 + 2);
		newTriangles.Add(faceCount * 4 + 3);

		newUV.Add(new Vector2(tUnit * texturePos.x + tUnit, tUnit * texturePos.y));
		newUV.Add(new Vector2(tUnit * texturePos.x + tUnit, tUnit * texturePos.y + tUnit));
		newUV.Add(new Vector2(tUnit * texturePos.x, tUnit * texturePos.y + tUnit));
		newUV.Add(new Vector2(tUnit * texturePos.x, tUnit * texturePos.y));
				  
		faceCount++;
	}
	
	public void GenerateMesh() {   
  		for (int x = 0; x < chunkSize; x++) {
   			for (int y = 0; y < chunkSize; y++) {
    			for (int z = 0; z < chunkSize; z++) {      
     				if(Block(x, y, z) != 0) { 
						//Top
      					if(Block(x, y + 1, z) == 0) {
       						CubeTop(x, y, z, Block(x, y, z));
      					}
						
						//Bot
						if(Block(x, y - 1, z) == 0) {
       						CubeBot(x, y, z, Block(x, y, z));
						}
       
						//East
      					if(Block(x + 1, y, z) == 0) {
       						CubeEast(x, y, z, Block(x, y, z));
						}
       
						//West
      					if(Block(x - 1, y, z) == 0) {
							CubeWest(x, y, z, Block(x, y, z));
						}
       
						//North
						if(Block(x, y, z + 1) == 0) {
							CubeNorth(x, y, z, Block(x, y, z));   
						}
       
						//South
						if(Block(x, y, z - 1) == 0) {
							CubeSouth(x, y, z, Block(x, y, z));
						}
					}
				}
			}
		}
   
  		UpdateMesh();
 	}
 
	void UpdateMesh() {
		mesh.Clear();
		mesh.vertices = newVertices.ToArray();
		mesh.uv = newUV.ToArray();
		mesh.triangles = newTriangles.ToArray();
		mesh.Optimize();
		mesh.RecalculateNormals();
		
		col.sharedMesh = null;
		col.sharedMesh = mesh;
		  
		newVertices.Clear();
		newUV.Clear();
		newTriangles.Clear();
		  
		faceCount = 0;
	}
	
	void Update() {
	
	}
 
 	void LateUpdate() {
		if(update) {
			GenerateMesh();
			update = false;
		}
	}
}
