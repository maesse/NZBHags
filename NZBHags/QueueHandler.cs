using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace NZBHags
{
    public class QueueHandler
    {
        private FileJob currentFileJob;
        public ArrayList collections { get; set; }
        public QueueHandler()
        {
            collections = new ArrayList();
        }

        public Segment getNextQueueItem()
        {
            lock (typeof(QueueHandler))
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
                else if (currentFileJob.queue.Count == 0)
                {
                    //currentFileJob.Complete();
                    if (collections.Count == 0)
                        return null;
                    FileCollection coll = (FileCollection)collections[0];
                    if (coll.queue.Count != 0)
                        currentFileJob = (FileJob)coll.queue.Dequeue();
                    else
                        return null;
                }

                return (Segment) currentFileJob.queue.Dequeue();
            }
        }
    }
}
