using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCrawler
{
    public Vector2Int Position { get; set; }

    public DungeonCrawler(Vector2Int startPos)
    {
        Position = startPos;
    }

    // Get next position based on direction
    public Vector2Int GetNextPosition(Dictionary<Direction, Vector2Int> directionMovementMap)
    {
        // Generate random direction
        Direction randomDir = (Direction)Random.Range(0, directionMovementMap.Count);
        return GetNextPosition(directionMovementMap, randomDir);
    }
    public Vector2Int GetNextPosition(Dictionary<Direction, Vector2Int> directionMovementMap, Direction direction)
    {
        Vector2Int nextPos = Position + directionMovementMap[direction];
        return nextPos;
    }

    // Move
    public Vector2Int Move(Dictionary<Direction, Vector2Int> directionMovementMap)
    {
        // Generate random direction
        Direction randomDir = (Direction)Random.Range(0, directionMovementMap.Count);
        // Move vector2int Position in that direction using the generated movementmap Direction
        Position += directionMovementMap[randomDir];
        // Return Position
        return Position;
    }
    public Vector2Int Move(Dictionary<Direction, Vector2Int> directionMovementMap, Direction direction)
    {
        // Move vector2int Position in that direction using the generated movementmap Direction
        Position += directionMovementMap[direction];
        // Return Position
        return Position;
    }

    // Move to a position
    public Vector2Int MoveTo(Vector2Int nextPosition)
    {
        Position = nextPosition;

        // Return Position
        return Position;
    }
}
