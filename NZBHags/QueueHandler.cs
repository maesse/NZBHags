using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace NZBHags
{
    public sealed class QueueHandler
    {
        static readonly QueueHandler instance = new QueueHandler();
        private FileJob currentFileJob;
        private int nextId = 0;
        public ArrayList collections;
        public bool changed;
        
        QueueHandler()
        {
            collections = new ArrayList();
            currentFileJob = new FileJob();
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
            lock (currentFileJob)
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
                    else
                        return null;
                }
                return (Segment)currentFileJob.queue.Dequeue();
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
    }
}
