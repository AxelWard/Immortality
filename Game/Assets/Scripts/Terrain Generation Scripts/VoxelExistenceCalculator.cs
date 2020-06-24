using UnityEngine;

public class VoxelExistenceCalculator
{
    public static int WORLD_FLOOR = 0;
    public static int WORLD_CEILING = 100;

    public static bool IsVoxelOn(Vector3 coordinates)
    {
        return CheckCoordinates((int)coordinates.x, (int)coordinates.y, (int)coordinates.z);
    }

    public static bool IsVoxelOn(int xCoordinate, int yCoordinate, int zCoordinate)
    {
        return CheckCoordinates(xCoordinate, yCoordinate, zCoordinate);
    }

    private static bool CheckCoordinates(int xCoordinate, int yCoordinate, int zCoordinate)
    {
        bool coordinateCheck = false;

        if(CheckBoundaries(yCoordinate))
        {
            coordinateCheck = CheckNoiseLayers(xCoordinate, yCoordinate, zCoordinate) || CheckIsWorldFloor(yCoordinate);
        }

        return coordinateCheck;
    }

    private static bool CheckBoundaries(int yCoordinate)
    {
        bool boundaryCheck = true;

        if (CheckIsBelowWorldFloor(yCoordinate))
        {
            boundaryCheck = false;
        }
        else if (CheckIsAboveWorldCeiling(yCoordinate))
        {
            boundaryCheck = false;
        }

        return boundaryCheck;
    }

    private static bool CheckNoiseLayers(int xCoordinate, int yCoordinate, int zCoordinate)
    {
        bool noiseCheck = CheckIsLessThanPerlinHeight(xCoordinate, yCoordinate, zCoordinate);

        // Implement any other noise layer checks here
        // i.e. cave generation, overhangs, etc.

        return noiseCheck;
    }

    private static bool CheckIsBelowWorldFloor(int yCoordinate)
    {
        if (yCoordinate < WORLD_FLOOR)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private static bool CheckIsAboveWorldCeiling(int yCoordinate)
    {
        if(yCoordinate > WORLD_CEILING)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private static bool CheckIsWorldFloor(int yCoordinate)
    {
        if (yCoordinate == WORLD_FLOOR)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private static bool CheckIsLessThanPerlinHeight(int xCoordinate, int yCoordinate, int zCoordinate)
    {
        int perlinHeight = PerlinCalculator.GetHeightAtCoords(xCoordinate, zCoordinate, 1);

        if (yCoordinate <= perlinHeight || yCoordinate == 0)
        {
            return true;
        } 
        else
        {
            return false;
        }
    }
}
