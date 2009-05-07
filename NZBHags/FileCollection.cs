using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace NZBHags
{
    public enum CollectionStatus
    {
        PAUSE,
        ACTIVE,
    }

    public class FileCollection
    {
        public string name { get; set; }
        public ArrayList files { get; set; }
        public Queue queue { get; set; }
        private CollectionStatus _status = CollectionStatus.ACTIVE;
        public CollectionStatus status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }


        public FileCollection(string name, ArrayList files)
        {
            this.name = name;
            this.files = files;
        }

        public FileCollection(string name)
        {
            this.name = name;
            this.files = files;
        }
    }
}
