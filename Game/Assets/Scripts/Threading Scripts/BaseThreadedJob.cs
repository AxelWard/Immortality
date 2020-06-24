using System.Collections;
using System.Diagnostics;

public class BaseThreadedJob
{
    private bool isDone = false;
    private object handle = new object();
    private System.Threading.Thread thread = null;

    public bool jobIsDone
    {
        get
        {
            bool tmp;
            lock (handle)
            {
                tmp = isDone;
            }
            return tmp;
        }
        set
        {
            lock (handle)
            {
                isDone = value;
            }
        }
    }

    public virtual void Start()
    {
        thread = new System.Threading.Thread(Run);
        thread.Start();
    }

    public virtual void Abort()
    {
        thread.Abort();
    }

    protected virtual void ThreadFunction()
    {

    }

    protected virtual void OnFinished()
    {

    }

    public virtual bool Update()
    {
        if(jobIsDone)
        {
            OnFinished();
            return true;
        }
        return false;
    }

    private void Run()
    {
        ThreadFunction();
        jobIsDone = true;
    }
}
