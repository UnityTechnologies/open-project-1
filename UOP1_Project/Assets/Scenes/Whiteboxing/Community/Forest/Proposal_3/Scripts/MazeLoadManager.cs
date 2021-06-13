using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeLoadManager : MonoBehaviour
{
	public enum Tile { tile0, tile1, tile2, tile3, tile4, tile5, tile6, tile7, tile8 }
	public Tile tileId;

	public GameObject[] mazeAreas = new GameObject[9];  //An array with all 9 Maze Tiles. Each mazeAreas[x] slot should be populated by the MazeTile whose tileId==tileX

	//current tile position
	public float posX;
	public float posY;
	public float posZ;
	private float tileDistance = 25.0f;

	//positions of surrounding tiles of current tile
	private Vector3 northPos;
	private Vector3 northEastPos;
	private Vector3 eastPos;
	private Vector3 southEastPos;
	private Vector3 southPos;
	private Vector3 southWestPos;
	private Vector3 westPos;
	private Vector3 northWestPos;

	private void Start()
	{
		SurroundingTiles();
	}

	public void SurroundingTiles()
	{
		if (tileId==Tile.tile0)
		{
			ArrangeSurroundingTiles(mazeAreas[2], mazeAreas[3], mazeAreas[4], mazeAreas[5],
								    mazeAreas[6], mazeAreas[7], mazeAreas[8], mazeAreas[1]);
		}
		if (tileId==Tile.tile1)
		{
			ArrangeSurroundingTiles(mazeAreas[7], mazeAreas[6], mazeAreas[2], mazeAreas[0],
								    mazeAreas[8], mazeAreas[4], mazeAreas[3], mazeAreas[5]);
		}
		if (tileId == Tile.tile2)
		{
			ArrangeSurroundingTiles(mazeAreas[6], mazeAreas[5], mazeAreas[3], mazeAreas[4],
								    mazeAreas[0], mazeAreas[8], mazeAreas[1], mazeAreas[7]);
		}
		if (tileId == Tile.tile3)
		{
			ArrangeSurroundingTiles(mazeAreas[5], mazeAreas[7], mazeAreas[1], mazeAreas[8],
								    mazeAreas[4], mazeAreas[0], mazeAreas[2], mazeAreas[6]);
		}
		if (tileId == Tile.tile4)
		{
			ArrangeSurroundingTiles(mazeAreas[3], mazeAreas[1], mazeAreas[8], mazeAreas[7],
								    mazeAreas[5], mazeAreas[6], mazeAreas[0], mazeAreas[2]);
		}
		if (tileId == Tile.tile5)
		{
			ArrangeSurroundingTiles(mazeAreas[4], mazeAreas[8], mazeAreas[7], mazeAreas[1],
								    mazeAreas[3], mazeAreas[2], mazeAreas[6], mazeAreas[0]);
		}
		if (tileId == Tile.tile6)
		{
			ArrangeSurroundingTiles(mazeAreas[0], mazeAreas[4], mazeAreas[5], mazeAreas[3],
								    mazeAreas[2], mazeAreas[1], mazeAreas[7], mazeAreas[8]);
		}
		if (tileId == Tile.tile7)
		{
			ArrangeSurroundingTiles(mazeAreas[8], mazeAreas[0], mazeAreas[6], mazeAreas[2],
								    mazeAreas[1], mazeAreas[3], mazeAreas[5], mazeAreas[4]);
		}
		if (tileId == Tile.tile8)
		{
			ArrangeSurroundingTiles(mazeAreas[1], mazeAreas[2], mazeAreas[0], mazeAreas[6],
								    mazeAreas[7], mazeAreas[5], mazeAreas[4], mazeAreas[3]);
		}
	}

	public void ArrangeSurroundingTiles(GameObject north, GameObject northEast, GameObject east, GameObject southEast,
									    GameObject south, GameObject southWest, GameObject west, GameObject northWest)
	{
		#region
		northPos = new Vector3(posX, posY, posZ + tileDistance);
		northEastPos = new Vector3(posX + tileDistance, posY, posZ + tileDistance);
		eastPos = new Vector3(posX + tileDistance, posY, posZ);
		southEastPos = new Vector3(posX + tileDistance, posY, posZ - tileDistance);
		southPos = new Vector3(posX, posY, posZ - tileDistance);
		southWestPos = new Vector3(posX - tileDistance, posY, posZ - tileDistance);
		westPos = new Vector3(posX - tileDistance, posY, posZ);
		northWestPos = new Vector3(posX - tileDistance, posY, posZ + tileDistance);

		//Upper
		north.GetComponent<MazeTile>().tileDirectionId = MazeTile.TileDirection.North;
		north.transform.position = northPos;
		//UpperRight
		northEast.GetComponent<MazeTile>().tileDirectionId = MazeTile.TileDirection.NorthEast;
		northEast.transform.position = northEastPos;
		//Right
		east.GetComponent<MazeTile>().tileDirectionId = MazeTile.TileDirection.East;
		east.transform.position = eastPos;
		//BottomRight
		southEast.GetComponent<MazeTile>().tileDirectionId = MazeTile.TileDirection.SouthEast;
		southEast.transform.position = southEastPos;
		//Bottom
		south.GetComponent<MazeTile>().tileDirectionId = MazeTile.TileDirection.South;
		south.transform.position = southPos;
		//BottomLeft
		southWest.GetComponent<MazeTile>().tileDirectionId = MazeTile.TileDirection.SouthWest;
		southWest.transform.position = southWestPos;
		//Left
		west.GetComponent<MazeTile>().tileDirectionId = MazeTile.TileDirection.West;
		west.transform.position = westPos;
		//UpperLeft
		northWest.GetComponent<MazeTile>().tileDirectionId = MazeTile.TileDirection.NorthWest;
		northWest.transform.position = northWestPos;
		#endregion
	}
}

