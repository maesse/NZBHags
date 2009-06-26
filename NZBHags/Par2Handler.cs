using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace NZBHags
{
    public sealed class Par2Handler
    {
        static readonly Par2Handler instance = new Par2Handler();

        private string par2filename = "par2.exe";
        private System.Diagnostics.Process process;
        private List<FileCollection> collections;
        private bool KeepRunning = true;
        private bool idle = true;

        // Constructor
        Par2Handler()
        {
            process = new System.Diagnostics.Process();
            process.StartInfo.FileName = par2filename;

            collections = new List<FileCollection>();

            //// Inits thread
            //ThreadStart job = new ThreadStart(Run);
            //Thread thread = new Thread(job);
            //thread.IsBackground = true;
            //thread.Start();
        }

        // Starting point for handling a collection
        private void HandleCollection(FileCollection collection)
        {
            idle = false;

            StringBuilder strBuilder = new StringBuilder();
            string mainPar2filename = null;
            string dir = Properties.Settings.Default.outputPath + "\\" + collection.name;

            string[] files = Directory.GetFiles(dir);


            for (int i = 0; i < files.Length; i++)
            {
                if (mainPar2filename == null && files[i].EndsWith(".par2"))
                    mainPar2filename = files[i];
                System.Console.WriteLine(files[i]);
            }


            idle = true;
        }

        // Thread loop
        private void Run()
        {
            FileCollection collection = null;
            while (KeepRunning)
            {
                lock (collections)
                {
                    if (collections.Count > 0)
                        collection = collections[0];
                }
                if (collection != null)
                {
                    HandleCollection(collection);
                    collection = null;
                }
                Thread.Sleep(100);
            }
        }

        // Adds a collection to be handled
        public void AddCollection(FileCollection collection)
        {
            //lock (collections)
            //{
            //    collections.Add(collection);
            //}
            HandleCollection(collection);
        }

        // Sets arguments for the executable (filenames)
        private void SetArguments(string args)
        {
            // TODO: Add properties loading
            process.StartInfo.Arguments = "r " + args;
        }

        // Singleton implementation
        public static Par2Handler Instance
        {
            get
            {
                return instance;
            }
        }

        public void Shutdown()
        {
            KeepRunning = false;
            if (!idle)
            {
                throw new Exception("TODO: Add par2 exit handling when not idle");
            }
        }
    }
}
