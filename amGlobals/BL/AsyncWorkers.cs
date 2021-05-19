using System;
using System.Collections.Generic;
using System.Threading;

namespace MdbCommonLib.Common
{
  public interface IWorkerObject
  {
    void DoWork( );
  }

  public class AsyncWorker<T> where T : IWorkerObject , new()
  {
    #region Data members

    private Queue<T> m_Queue;

    private Thread workThread;

    private volatile bool finishProcessing;

    private static AutoResetEvent waitEvent = new AutoResetEvent( false );
    private static Object lockQueueObj = new Object();
    #endregion

    public AsyncWorker( )
    {
      finishProcessing = false;
      m_Queue = new Queue<T>();
      workThread = new Thread( this.ProcessQueue );
      workThread.Start();
    }

    #region Interface methods
    public void Enqueue( T obj )
    {
      lock ( lockQueueObj ) {
        m_Queue.Enqueue( obj );
      }
      if ( !workThread.IsAlive ) {
        workThread = new Thread( this.ProcessQueue );
        workThread.Start();
      }

      waitEvent.Set();

    }

    public void Stop( )
    {
      finishProcessing = true;
      waitEvent.Set();
    }

    #endregion

    #region Hidden methods

    private void ProcessQueue( )
    {
      while ( !finishProcessing ) {

        T item;

        lock ( lockQueueObj ) {
          if ( m_Queue.Count > 0 ) {
            item = m_Queue.Dequeue();
          }
          else {
            item = default( T );
          }

        }
        if ( item != null ) {
          item.DoWork();
        }
        else {
          waitEvent.WaitOne();
        }
      }
    }

    #endregion

    #region Properties

    #endregion
  }

  public class AsyncWorkerMT<T> where T : IWorkerObject , new()
  {
    #region Data members

    private static Object lockQueueObj = new Object();

    #endregion

    public AsyncWorkerMT( )
    {
    }

    #region Interface methods

    public void Enqueue( T obj )
    {
      lock ( lockQueueObj ) {
        ThreadPool.QueueUserWorkItem( new WaitCallback( PerformWork ) , obj );
      }

    }

    #endregion

    #region Hidden methods

    private static void PerformWork( Object obj )
    {
      ( ( T ) obj ).DoWork();
    }

    #endregion

    #region Properties

    #endregion
  }
}
