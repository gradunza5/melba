using Godot;
using System;

public class TileMap : Godot.TileMap
{

    private const int WALL = 1;
    private const int FLOOR = 0;
    private const double WallChance = 0.45;
    private const int deathLimit = 3;
    private const int birthLimit = 4;

    private Vector2 MapSize;
    private int[,] tiles;
    private int[,] oldTiles;
    private Random random;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        random = new Random();
    }

    public void Init(Vector2 size)
    {
        MapSize = size;
        tiles = new int[(int) MapSize.x, (int)  MapSize.y];

        for (int x = 0; x < MapSize.x; x++) {
            for (int y = 0; y < MapSize.y; y++) {
                tiles[x,y] = (random.NextDouble() < WallChance) ? WALL : FLOOR;
            }
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Update();
    }

    public void NextStep() {
        oldTiles = tiles;
        tiles = SimulationStep(oldTiles);
    }

    public override void _Draw() {
        for (int x = 0; x < MapSize.x; x++) {
            for (int y = 0; y < MapSize.y; y++) {
                SetCell(x, y, tiles[x,y]);
            }
        }
    }

    public int countAliveNeighbors(int[,] map, int x, int y) 
    {
        int count = 0;

        for (int i = -1; i < 2; i++) 
        {
            for (int j = -1; j < 2; j++) 
            {
                int neighborX = x+i;
                int neighborY = y+j;

                if (i == 0 && j == 0) {
                    // do nothing
                } else if (neighborX < 0 || neighborY < 0 || neighborX >= MapSize.x || neighborY >= MapSize.y) {
                    // we're looking at/off the edge of the map
                    count++;
                } else if (map[neighborX, neighborY] == WALL) {
                    count++;
                } 
            }
        }

        return count;
    }

    private int[,] SimulationStep(int[,] someTiles) {
        int [,] newTiles = new int[(int) MapSize.x, (int) MapSize.y];

        for (int x = 0; x < MapSize.x; x++) {
            for (int y = 0; y < MapSize.y; y++) {
                
                int nbs = countAliveNeighbors(someTiles, x, y);

                if (oldTiles[x,y] == WALL) {
                    newTiles[x,y] = nbs < deathLimit ? FLOOR : WALL;
                } else {
                    newTiles[x,y] = nbs > birthLimit ? WALL : FLOOR;
                }
            }
        }

        return newTiles;
    }
}
