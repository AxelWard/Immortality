using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerationJobManager : MonoBehaviour
{
    ThreadJobQueue<GenerateChunkJob> generationQueue = new ThreadJobQueue<GenerateChunkJob>();
    ThreadJobQueue<GenerateMeshFromChunkJob> meshGenerationQueue = new ThreadJobQueue<GenerateMeshFromChunkJob>();

    GenerateMeshFromChunkJob completedJob;

    GenerateChunkJob activeGenerationJob;
    GenerateMeshFromChunkJob activeMeshJob;

    bool hasActiveGenerationJob = false;
    bool hasActiveMeshJob = false;

    public bool hasCompletedJobWaiting = false;

    public void AddGenerationJob(GenerateChunkJob jobToAdd)
    {
        generationQueue.Enqueue(jobToAdd);
    }

    public void RemoveGenerationJob(GenerateChunkJob jobToRemove)
    {
        if(jobToRemove.Equals(activeGenerationJob))
        {
            activeGenerationJob.Abort();
            activeGenerationJob = null;
            hasActiveGenerationJob = false;

        } else if(!generationQueue.Contains(jobToRemove)) {
            if(jobToRemove.Equals(activeMeshJob))
            {
                activeMeshJob.Abort();
                activeMeshJob = null;
                hasActiveMeshJob = false;
            }
            else
            {
                meshGenerationQueue.Remove(jobToRemove.ToMeshJob());
            }
        }
        else
        {
            generationQueue.Remove(jobToRemove);
        }
    }

    public GenerateMeshFromChunkJob GetCompletedJob()
    {
        hasCompletedJobWaiting = false;
        return completedJob;
    }

    public bool IsJobInQueue(Vector3 chunkLocation)
    {
        GenerateChunkJob check = new GenerateChunkJob();
        check.chunkLocationToGenerate = chunkLocation;

        if (check.Equals(activeGenerationJob))
        {
            return true;

        }
        else if (generationQueue.Contains(check))
        {
            return true;
        }
        else if (check.Equals(activeMeshJob))
        {
            return true;
        }
        else if (meshGenerationQueue.Contains(check.ToMeshJob()))
        {
            return true;
        }

        return false;
    }

    private void Update()
    {
        CheckActiveGenerationJob();
        if(!hasActiveGenerationJob && generationQueue.HasAJobToProcess())
        {
            StartNextGenerationJob();
        }

        CheckActiveMeshJob();
        if(!hasActiveMeshJob && !hasCompletedJobWaiting && meshGenerationQueue.HasAJobToProcess())
        {
            StartNextMeshJob();
        }
    }

    private void CheckActiveGenerationJob()
    {
        if (activeGenerationJob != null)
        {
            if (activeGenerationJob.Update())
            {
                hasActiveGenerationJob = false;

                GenerateMeshFromChunkJob meshJob = new GenerateMeshFromChunkJob();
                meshJob.chunkCoordinates = activeGenerationJob.chunkLocationToGenerate;
                meshJob.chunkVoxelArray = activeGenerationJob.chunkVoxelArray;
                AddMeshJob(meshJob);

                activeGenerationJob = null;
            }
        }
    }

    private void StartNextGenerationJob()
    {
        activeGenerationJob = generationQueue.Dequeue();
        hasActiveGenerationJob = true;
        activeGenerationJob.Start();
    }

    private void AddMeshJob(GenerateMeshFromChunkJob jobToAdd)
    {
        meshGenerationQueue.Enqueue(jobToAdd);
    }

    private void CheckActiveMeshJob()
    {
        if (activeMeshJob != null)
        {
            if (activeMeshJob.Update())
            {
                hasActiveMeshJob = false;
                hasCompletedJobWaiting = true;
                completedJob = activeMeshJob;
                activeMeshJob = null;
            }
        }
    }

    private void StartNextMeshJob()
    {
        activeMeshJob = meshGenerationQueue.Dequeue();
        hasActiveMeshJob = true;
        activeMeshJob.Start();
    }
}
