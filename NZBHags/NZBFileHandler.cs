using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using System.Collections;
using System.Timers;

namespace NZBHags
{
    class NZBFileHandler
    {

        // Loads an .nzb file
        public static FileCollection loadFile(string filename)
        {
            // Loadtime measuring
            Performance.Stopwatch sw = new Performance.Stopwatch();
            sw.Start();
            
            XmlDocument document = new XmlDocument();
            FileStream stream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            // Inits xmlreader with custom resolver for instant dtd file lookup
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ProhibitDtd = false;
            settings.XmlResolver = new XmlFileResolver();

            // Create new FIleCOllection object
            FileCollection collection = new FileCollection(getPrettyFilename(filename));

            // Parse file into XmlDocument
            document.Load(XmlReader.Create(stream, settings));
            stream.Close();

            ArrayList files = ParseXML(document, collection);
            collection.files = files;
            foreach (FileJob job in files)
            {
                collection.size += job.size;
            }
            // Loadtime measuring
            sw.Stop();
            Logging.Log("Loading NZB took {0} ms", sw.GetElapsedTimeSpan().TotalMilliseconds);
            return collection;
        }

        private static string getPrettyFilename(string filename)
        {
            filename = filename.Substring(0, filename.IndexOf(".nzb")); // cut off .nzb
            int dir = filename.LastIndexOf('\\');
            return filename.Substring(dir, filename.Length-dir);
        }

        // Generates queues for a filecollection object
        public static Queue genQueue(ArrayList list)
        {
            Queue queue = new Queue(list.Count);
            foreach (FileJob job in list)
            {
                job.queue = new Queue(job.segments.Count);
                foreach (Segment seg in job.segments)
                {
                    job.queue.Enqueue(seg);
                }
                queue.Enqueue(job);
            }
            return queue;
        }


        // Parses XML structure into file objects
        private static ArrayList ParseXML(XmlDocument document, FileCollection filecollection)
        {
            XmlNodeList nodes = document.GetElementsByTagName("file");
            ArrayList files = new ArrayList();
            Random random = new Random();
            int collectionID = random.Next();
            int fileId = 0;
            // Parsing for each file node
            foreach (XmlElement fileNode in nodes)
            {
                FileJob filejob = new FileJob(ref filecollection);
                filejob.filename = ""+collectionID;
                XmlAttributeCollection collection = fileNode.Attributes;

                // File Attributes
                foreach (XmlAttribute attr in collection)
                {
                    switch (attr.Name)
                    {
                        case "poster":
                            filejob.poster = attr.Value;
                            break;
                        case "date":
                            filejob.date = int.Parse(attr.Value);
                            break;
                        case "subject":
                            if (attr.Value.Contains("\""))
                            {
                                // try to get inner filename...
                                try
                                {
                                    int first = attr.Value.IndexOf("\"") + 1;
                                    string filename = attr.Value.Substring(first, attr.Value.Length - first);
                                    filename = filename.Substring(0, filename.IndexOf("\""));
                                    filejob.filename = filejob.filename + "-"+ filename.Substring(0, filename.LastIndexOf(".")+4);
                                }
                                catch (Exception ex)
                                {
                                    filejob.filename = filejob.filename+"-"+fileId;
                                }

                            }
                            else
                                filejob.filename = filejob.filename + "-" + fileId;
                            
                            filejob.subject = attr.Value;
                            break;
                    }
                }
                int maxnumber = 0;
                foreach (XmlElement subNode in fileNode.ChildNodes)
                {
                    if (subNode.Name == "segments")
                    {
                        foreach (XmlElement segmentNode in subNode)
                        {
                            Segment segment = new Segment();
                            segment.addr = "<"+segmentNode.InnerXml+">";
                            segment.addr = segment.addr.Replace("&amp;", "&");
                            segment.setParent(ref filejob);
                            foreach (XmlAttribute attr in segmentNode.Attributes)
                            {
                                switch (attr.Name)
                                {
                                    case "bytes":
                                        segment.bytes = int.Parse(attr.Value);
                                        filejob.size += (ulong)segment.bytes;
                                        break;
                                    case "number":
                                        segment.id = int.Parse(attr.Value);
                                        if (segment.id > maxnumber)
                                            maxnumber = segment.id;
                                        break;
                                }
                            }
                            
                            // Check and add element to arraylist
                            if (segment.id != 0 && segment.bytes != 0 && segment.addr.Length > 10)
                                filejob.segments.Add(segment);
                        }
                    } else if (subNode.Name == "groups")
                    {
                        filejob.groups.Add(subNode.FirstChild.InnerText);
                    }
                }
                filejob.yparts = maxnumber; // Will fail if source isnt yenc
                // Sort segments
                filejob.segments.Sort(new SegmentComparer());

                files.Add(filejob);
                fileId++;
            }
            return files;
        }
    }
}
