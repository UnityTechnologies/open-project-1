using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeTile : MonoBehaviour
{
	public GameObject mazeManager;
	public enum TileDirection { Center, North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest } 
	public TileDirection tileDirectionId; //tileDirectionId is only used for debug reasons
	public enum Tile { tile0, tile1, tile2, tile3, tile4, tile5, tile6, tile7, tile8 }
	public Tile tileId;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			//Inform the mazeManager about the position of the tile we just entered
			mazeManager.GetComponent<MazeLoadManager>().posX = this.transform.position.x;
			mazeManager.GetComponent<MazeLoadManager>().posY = this.transform.position.y;
			mazeManager.GetComponent<MazeLoadManager>().posZ = this.transform.position.z;

			//Inform the mazeManager about the tileId of the tile we just entered so it can rearrange all surrounding tiles according to the tile's current position
			if (tileId == Tile.tile0)
			{
				mazeManager.GetComponent<MazeLoadManager>().tileId = MazeLoadManager.Tile.tile0;
				tileDirectionId = TileDirection.Center;
				mazeManager.GetComponent<MazeLoadManager>().SurroundingTiles();  
			}

			if (tileId == Tile.tile1)
			{
				mazeManager.GetComponent<MazeLoadManager>().tileId = MazeLoadManager.Tile.tile1;
				tileDirectionId = TileDirection.Center;
				mazeManager.GetComponent<MazeLoadManager>().SurroundingTiles();   	
			}

			if (tileId == Tile.tile2)
			{
				mazeManager.GetComponent<MazeLoadManager>().tileId = MazeLoadManager.Tile.tile2;
				tileDirectionId = TileDirection.Center;
				mazeManager.GetComponent<MazeLoadManager>().SurroundingTiles(); 
			}

			if (tileId == Tile.tile3)
			{
				mazeManager.GetComponent<MazeLoadManager>().tileId = MazeLoadManager.Tile.tile3;
				tileDirectionId = TileDirection.Center;
				mazeManager.GetComponent<MazeLoadManager>().SurroundingTiles();  
			}

			if (tileId == Tile.tile4)
			{
				mazeManager.GetComponent<MazeLoadManager>().tileId = MazeLoadManager.Tile.tile4;
				tileDirectionId = TileDirection.Center;
				mazeManager.GetComponent<MazeLoadManager>().SurroundingTiles();   
			}
			if (tileId == Tile.tile5)
			{
				mazeManager.GetComponent<MazeLoadManager>().tileId = MazeLoadManager.Tile.tile5;
				tileDirectionId = TileDirection.Center;
				mazeManager.GetComponent<MazeLoadManager>().SurroundingTiles(); 
			}
			if (tileId == Tile.tile6)
			{
				mazeManager.GetComponent<MazeLoadManager>().tileId = MazeLoadManager.Tile.tile6;
				tileDirectionId = TileDirection.Center;
				mazeManager.GetComponent<MazeLoadManager>().SurroundingTiles();   
			}
			if (tileId == Tile.tile7)
			{
				mazeManager.GetComponent<MazeLoadManager>().tileId = MazeLoadManager.Tile.tile7;
				tileDirectionId = TileDirection.Center;
				mazeManager.GetComponent<MazeLoadManager>().SurroundingTiles();   
			}
			if (tileId == Tile.tile8)
			{
				mazeManager.GetComponent<MazeLoadManager>().tileId = MazeLoadManager.Tile.tile8;
				tileDirectionId = TileDirection.Center;
				mazeManager.GetComponent<MazeLoadManager>().SurroundingTiles();   
			}
		}
	}				
}

