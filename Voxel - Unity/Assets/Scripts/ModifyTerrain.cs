﻿using UnityEngine;
using System.Collections;

public class ModifyTerrain : MonoBehaviour {
	
	World world;
	GameObject cameraGO;

	void Start() {
		world = gameObject.GetComponent("World") as World;
 		cameraGO = GameObject.FindGameObjectWithTag("MainCamera");
	}
	
 	//Replaces the block directly in front of the player
	public void ReplaceBlockCenter(byte block, float range) {
		Ray ray = new Ray(cameraGO.transform.position, cameraGO.transform.forward);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit)) {
			if(hit.distance < range) {
				ReplaceBlockAt(hit, block);
			}
		}
	}
 
 	//Adds the block specified directly in front of the player
	public void AddBlockCenter(byte block, float range){
 		Ray ray = new Ray(cameraGO.transform.position, cameraGO.transform.forward);
 		RaycastHit hit;
 		if(Physics.Raycast(ray, out hit)) {
  			if(hit.distance < range) {
   				AddBlockAt(hit, block);
  			}
			
  			Debug.DrawLine(ray.origin, ray.origin + (ray.direction * hit.distance), Color.green, 2);
 		}
	}
	
 	//Replaces the block specified where the mouse cursor is pointing
	public void ReplaceBlockCursor(byte block) {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit)) {
			ReplaceBlockAt(hit, block);
			Debug.DrawLine(ray.origin, ray.origin + (ray.direction * hit.distance), Color.green, 2);
		}
	}
	
  	//Adds the block specified where the mouse cursor is pointing
	public void AddBlockCursor(byte block) {	  
	 	Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	 	RaycastHit hit;
		if(Physics.Raycast(ray, out hit)) {
			AddBlockAt(hit, block);
			Debug.DrawLine(ray.origin, ray.origin + (ray.direction * hit.distance), Color.green, 2);
	 	}
	}
	
	//Removes a block at these impact coordinates, you can raycast against the terrain and call this with the hit.point
	public void ReplaceBlockAt(RaycastHit hit, byte block) {
	 	Vector3 position = hit.point;
 		position += (hit.normal * -0.5f);
  
 		SetBlockAt(position, block);
	}
	
	//Adds the specified block at these impact coordinates, you can raycast against the terrain and call this with the hit.point
	public void AddBlockAt(RaycastHit hit, byte block) {
		Vector3 position = hit.point;
 		position += (hit.normal * 0.5f);
   
 		SetBlockAt(position, block);
	}
	
	//Sets the specified block at these coordinates
	public void SetBlockAt(Vector3 position, byte block) {
		int x = Mathf.RoundToInt(position.x);
 		int y = Mathf.RoundToInt(position.y);
 		int z = Mathf.RoundToInt(position.z);
		
		SetBlockAt(x, y, z, block);
	}
	
	//Adds the specified block at these coordinates
	public void SetBlockAt(int x, int y, int z, byte block) {
		print("Adding: " + x + ", " + y + ", " + z);
		world.data[x, y, z] = block;
		UpdateChunkAt(x, y, z);
	}
	 
	public void UpdateChunkAt(int x, int y, int z) {
		int updateX = Mathf.FloorToInt(x / world.chunkSize);
		int updateY = Mathf.FloorToInt(y / world.chunkSize);
		int updateZ = Mathf.FloorToInt(z / world.chunkSize);
		
		print("Updating: " + updateX + ", " + updateY + ", " + updateZ);
		world.chunks[updateX, updateY, updateZ].update = true;
		
		if(x - (world.chunkSize * updateX) == 0 && updateX != 0){
 			world.chunks[updateX - 1, updateY, updateZ].update = true;
		}
		
		if(x - (world.chunkSize * updateX) == 15 && updateX != world.chunks.GetLength(0) - 1){
		 	world.chunks[updateX + 1, updateY, updateZ].update = true;
		}
		
		if(y - (world.chunkSize * updateY) == 0 && updateY != 0){
		 	world.chunks[updateX, updateY - 1, updateZ].update = true;
		}
		
		if(y - (world.chunkSize * updateY) == 15 && updateY != world.chunks.GetLength(1) - 1) {
		 	world.chunks[updateX, updateY + 1, updateZ].update = true;
		}
		
		if(z - (world.chunkSize * updateZ) == 0 && updateZ != 0) {
		 	world.chunks[updateX, updateY, updateZ - 1].update = true;
		}
		
		if(z - (world.chunkSize * updateZ) == 15 && updateZ != world.chunks.GetLength(2) - 1) {
		 	world.chunks[updateX, updateY, updateZ + 1].update = true;
		}
	}
	
	void Update() {
		if(Input.GetMouseButtonDown(0)) {
			ReplaceBlockCenter(0, 50);
		}
		if(Input.GetMouseButtonDown(1)) {
			AddBlockCenter(1, 50);
		}
	}
}
