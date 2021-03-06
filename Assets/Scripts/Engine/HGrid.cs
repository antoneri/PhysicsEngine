﻿using UnityEngine;
using System.Collections.Generic;
using System;

/* This implementation is taken from Christer Ericsons book 
 * Real Time Collision Detection (2005)
*/

namespace PE
{
    public class HGrid
    {

        const int NUM_BUCKETS = 1024;
        const int HGRID_MAX_LEVELS = 32;
        const float MIN_CELL_SIZE = 1;

        const float SPHERE_TO_CELL_RATIO = 1.0f / 4.0f; // Largest sphere in cell is 1/4*cell size
        const float CELL_TO_CELL_RATIO = 2.0f; // Cells at next level are 2*side of current cell

        uint occupiedLevelsMask = 0;
        int[] objectsAtLevel = new int[HGRID_MAX_LEVELS];
        HGridObject[] objectBucket = new HGridObject[NUM_BUCKETS];
        int[] timeStamp = new int[NUM_BUCKETS];
        int tick;

        public HGrid()
        {

        }

        public void reset()
        {
            occupiedLevelsMask = 0;
            for (int i = 0; i < objectsAtLevel.Length; i++)
            {
                objectsAtLevel[i] = 0;
            }
            for (int i = 0; i < NUM_BUCKETS; i++)
            {
                objectBucket[i] = null;
            }
            for (int i = 0; i < NUM_BUCKETS; i++)
            {
                timeStamp[i] = 0;
            }
            tick = 0;
        }

        public bool isEmpty()
        {
            return occupiedLevelsMask == 0;
        }

        public void AddObject(HGridObject obj)
        {
            // Find lowest level where object fully fits inside cell, taking RATIO into account
            int level;
            float size = MIN_CELL_SIZE;
            float diameter = 2.0f * obj.radius;

            for (level = 0; size * SPHERE_TO_CELL_RATIO < diameter; level++)
                size *= CELL_TO_CELL_RATIO;

            // If object is larger than largest grid cell throw exception
            if (level > HGRID_MAX_LEVELS)
            {
                throw new Exception("Object to large");
            }

            // Add object to grid square, and remember cell and level numbers
            Cell cellPos;
            cellPos.x = (int)(obj.pos.x / size); cellPos.y = (int)(obj.pos.y / size); cellPos.z = level;
            int bucket = ComputeHashBucketIndex(cellPos);
            obj.bucket = bucket;
            obj.level = level;
            obj.pNextObject = objectBucket[bucket];
            objectBucket[bucket] = obj;

            // Mark this level as having one more object. Also indicate level is in use
            objectsAtLevel[level]++;
            occupiedLevelsMask |= (uint)(1 << level);
        }

        public void RemoveObject(HGridObject obj)
        {
            // One less object on this grid level. Mark level as unused if no objects left
            if (--objectsAtLevel[obj.level] == 0)
                occupiedLevelsMask &= (uint)~(1 << obj.level);

            // Now scan through list and unlink object ’obj’
            int bucket = obj.bucket;
            HGridObject p = objectBucket[bucket];

            // Special-case updating list header when object is first in list
            if (p == obj)
            {
                objectBucket[bucket] = p.pNextObject;
                return;
            }

            // Traverse rest of list, unlinking ’obj’ when found
            while (p != null)
            {
                // Keep q as trailing pointer to previous element
                HGridObject q = p;
                p = p.pNextObject;
                if (p == obj)
                {
                    q.pNextObject = p.pNextObject; // unlink by bypassing
                    return;
                }
            }

            throw new Exception("Object does not exist in HGrid"); // No such object in hgrid
        }


        // Test collisions between object and all objects in hgrid
        public List<Sphere> CheckObjAgainstGrid(HGridObject obj)
        {

            List<Sphere> collisions = new List<Sphere>();

            const float EPSILON = 0.001f;
            float size = MIN_CELL_SIZE;
            int startLevel = 0;
            uint occupiedLevelsMask = this.occupiedLevelsMask;
            Vec3 pos = obj.pos;

            // For each new query, increase time stamp counter
            tick++;

            for (int level = startLevel; level < HGRID_MAX_LEVELS;
            size *= CELL_TO_CELL_RATIO, occupiedLevelsMask >>= 1, level++)
            {
                // If no objects in rest of grid, stop now
                if (occupiedLevelsMask == 0) break;

                // If no objects at this level, go on to the next level
                if ((occupiedLevelsMask & 1) == 0) continue;

                // Compute ranges [x1..x2, y1..y2, z1..z2] of cells overlapped on this level. To
                // make sure objects in neighboring cells are tested, by increasing range by
                // the maximum object overlap: size * SPHERE_TO_CELL_RATIO

                float delta = obj.radius + size * SPHERE_TO_CELL_RATIO + EPSILON;
                float ooSize = 1.0f / size; /* Not sure about this */
                int x1 = (int)Math.Floor((pos.x - delta) * ooSize);
                int y1 = (int)Math.Floor((pos.y - delta) * ooSize);
                //int z1 = (int)Math.Floor((pos.z - delta) * ooSize);
                int x2 = (int)Math.Ceiling((pos.x + delta) * ooSize);
                int y2 = (int)Math.Ceiling((pos.y + delta) * ooSize);
                //int z2 = (int)Math.Ceiling((pos.z + delta) * ooSize);


                // Check all the grid cells overlapped on current level
                for (int x = x1; x <= x2; x++)
                {
                    for (int y = y1; y <= y2; y++)
                    {
                        Cell cellPos;
                        cellPos.x = x; cellPos.y = y; cellPos.z = level;

                        int bucket = ComputeHashBucketIndex(cellPos);

                        // Has this hash bucket already been checked for this object?
                        if (timeStamp[bucket] == tick) continue;

                        timeStamp[bucket] = tick;

                        // Loop through all objects in the bucket to find nearby objects
                        HGridObject p = objectBucket[bucket];
                        while (p != null)
                        {
                            if (p != obj)
                            {
                                float dist2 = (float)(pos - p.pos).SqLength;
                                if (dist2 <= (float)Math.Pow(obj.radius + p.radius + EPSILON, 2))
                                {
                                    collisions.Add(p.rb);
                                }
                            }
                            p = p.pNextObject;
                        }
                    }   
                }
            } // end for level

            return collisions;
        }


        int ComputeHashBucketIndex(Cell cellPos)
        {
            uint h1 = 2376512323; // 0x8da6b343 Large multiplicative constants;
            uint h2 = 3625334849; // 0xd8163841 here arbitrarily chosen primes
            uint h3 = 3407524639; // 0xcb1ab31f

            long n = h1 * cellPos.x + h2 * cellPos.y + h3 * cellPos.z;
            n = n % NUM_BUCKETS;
            if (n < 0) n += NUM_BUCKETS;

            return (int)n;
        }

    }

    public struct Cell
    {
        public int x, y, z;

        public Cell(int px, int py, int pz)
        {
            x = px; y = py; z = pz;
        }
    }

    /* Uses sphere as bounding volume */
    public class HGridObject
    {
        public HGridObject pNextObject; // Embedded link to next hgrid object
        public Vec3 pos;                // x, y, z position for sphere
        public float radius;            // Radius for bounding sphere
        public int bucket;              // Index of hash bucket object is in
        public int level;               // Grid level for the object
        public Sphere rb;            // Object data

        public HGridObject(Sphere rb, Vec3 pos, float radius)
        {
            this.rb = rb;
            this.pos = pos;
            this.radius = radius;
        }
    }

}
