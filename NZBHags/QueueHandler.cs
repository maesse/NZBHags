using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;

namespace NZBHags
{
    public sealed class QueueHandler
    {
        static readonly QueueHandler instance = new QueueHandler();
        private FileJob currentFileJob;
        private int nextId = 0;
        public ArrayList collections;
        public bool changed;
        private bool KeepRunning = true;
        
        QueueHandler()
        {
            collections = new ArrayList();
            currentFileJob = new FileJob();
            // Inits thread
            ThreadStart job = new ThreadStart(Run);
            Thread thread = new Thread(job);
            thread.IsBackground = true;
            thread.Start();
        }

        private void Run()
        {
            while (KeepRunning)
            {
                for (int i = 0; i < collections.Count; i++)
                {
                    FileCollection coll = (FileCollection)collections[i];
                    if (coll.progress >= (coll.size / 2))
                    {
                        bool allComplete = false;
                        foreach (FileJob job in coll.files)
                        {
                            if (!job.complete)
                            {
                                allComplete = false;
                                break;
                            } else
                                allComplete = true;
                        }
                        if (allComplete == true)
                        {
                            coll.status = CollectionStatus.DOPAR2;
                            Par2Handler.Instance.AddCollection(coll);
                        }
                    }
                }

                Thread.Sleep(1000);
            }
        }

        public void AddCollection(FileCollection collection)
        {
            collection.status = CollectionStatus.QUEUED;
            collection.queue = NZBFileHandler.genQueue(collection.files);
            collection.id = nextId;
            nextId++;
            collections.Add(collection);
            changed = true;
        }

        public FileCollection[] getCollections()
        {
            FileCollection[] coll = new FileCollection[collections.Count];
            for (int i = 0; i < nextId; i++)
            {
                foreach (FileCollection col in collections)
                {
                    if (col.id == i)
                    {
                        coll[i] = col;
                        break;
                    }
                }
            }
            return coll;
        }

        public void setCollectionID(FileCollection col, int index)
        {
            if (index >= 0 && index < nextId)
            {
                if (col.id > index)
                {
                    // decrement
                    foreach (FileCollection collection in collections)
                    {
                        if(collection.id == index) {
                            collection.id++;
                            break;
                        }
                    }
                    col.id--;
                }
                else
                {
                    // increment
                    foreach (FileCollection collection in collections)
                    {
                        if (collection.id == index)
                        {
                            collection.id--;
                            break;
                        }
                    }
                    col.id++;
                }
            }
            changed = true;
        }

        public void removeCollection(FileCollection collection)
        {
            if (currentFileJob != null && currentFileJob.parent == collection)
                currentFileJob = null;
            if (collection.id != nextId - 1)
            {
                for (int i = collection.id + 1; i < nextId; i++)
                {
                    foreach (FileCollection col in collections)
                    {
                        if (col.id == i)
                        {
                            col.id--;
                            break;
                        }
                    }
                }
            }
            else
                nextId--;
            collections.Remove(collection);
            changed = true;
        }

        public Segment getNextQueueItem()
        {
            if (currentFileJob == null)
            {
                if (collections.Count == 0)
                {
                    return null;
                }
                FileCollection coll = (FileCollection)collections[0];
                currentFileJob = (FileJob)coll.queue.Dequeue();
            }
            lock (this)
            {

                if (currentFileJob.queue.Count == 0)
                {
                    if (collections.Count != 0)
                    {
                        foreach (FileCollection col in collections)
                        {
                            if (col.status != CollectionStatus.PAUSE && col.queue.Count > 0)
                            {
                                col.status = CollectionStatus.DOWNLOADING;
                                currentFileJob = (FileJob)col.queue.Dequeue();
                                break;
                            }
                        }
                    }
                    return null;
                }
                else if (currentFileJob.parent.status != CollectionStatus.PAUSE)
                {
                    return (Segment)currentFileJob.queue.Dequeue();
                }
                else
                    return null;
            }
        }

        // Singleton implementation
        public static QueueHandler Instance
        {
            get
            {
                return instance;
            }
        }

        public void Shutdown()
        {
            KeepRunning = false;
        }
    }
}
