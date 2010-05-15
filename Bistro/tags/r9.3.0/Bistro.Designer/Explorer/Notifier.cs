using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bistro.Designer.Explorer
{
    /// <summary>
    /// Compound notifications: All notifications are compounded together in a single package. 
    /// The package is submitted for processing if there is no new notifications for a certain period of time (at this time the delay is set to 2 sec). 
    /// The compounding is done as follows:
    /// 1. If a ‘file modified’ notification is received then remove all other notifications from list and add ‘file modified’ notifications.
    /// 2. If a ‘file added’ notification is received then remove all other notifications from list and add ‘file modified’ notifications.
    /// 3. If a ‘file removed’ notification is received then remove all other notifications from list and add ‘file removed’ notifications.
    /// </summary>
    class Notifier : IDisposable
    {
        // Time to wait before the batch of changes will be reported to the parser            
        const int TIMEOUT = 2;
        FileManager fileManager;
        volatile bool stopped = false;

        public Notifier(FileManager fileManager)
        {
            this.fileManager = fileManager;
            Thread t = new Thread(new ThreadStart(run));
            t.Start();
        }

        bool dirty = true;
        void run()
        {
            List<string> temp;

            try
            {
                while (!stopped)
                {
                    lock (files)
                    {
                        while (dirty)
                        {
                            dirty = false;
                            if (files.Count == 0)
                                Monitor.Wait(files);  // nothing to report - we can wait indefinitely
                            else
                                Monitor.Wait(files, TIMEOUT * 1000);
                        }
                        dirty = true;
                        temp = new List<string>(files);
                        files.Clear();
                    }
                    if (!stopped)
                    {
                        fileManager.tracker.RebuildNodes(temp);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        List<string> files = new List<string>();

        /// <summary>
        /// 1. Clear list with the file
        /// 2. Add th file in the list with '*'
        /// </summary>
        /// <param name="file"></param>
        public void FileModified(string file)
        {
            lock (files)
            {
                // delete all events and add '*'
                if (files.Contains("-" + file))
                    files.Remove("-" + file);
                if (!files.Contains("*" + file))
                    files.Add("*" + file);
                dirty = true;
                Monitor.Pulse(files);
            }
        }

        /// <summary>
        /// 1. Add wathcer
        /// 2. Do the same what we do with changed files
        /// </summary>
        /// <param name="file"></param>
        public void FileAdded(string file)
        {
            // the point is '+','*' is the same operation 
            FileModified(file);
        }

        /// <summary>
        /// 1. Remove watcher. 
        /// 2. Remove generated file.
        /// 3. Clear list with the file.
        /// 4. Add the file in the list with '-'.
        /// </summary>
        /// <param name="file"></param>
        public void FileRemoved(string file)
        {
            fileManager.removeWatcher(file);

            lock (files)
            {
                if (files.Contains("*" + file))
                    files.Remove("*" + file);

                files.Add("-" + file);
                dirty = true;
                Monitor.Pulse(files);
            }
        }

        #region IDisposable Members
        /// <summary>
        /// Releases the resources used by the <see cref="Notifier"/>
        /// </summary>
        public void Dispose()
        // Implements IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue 
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the 
        // runtime from inside the finalizer and you should not reference 
        // other objects. Only unmanaged resources can be disposed.
        private bool disposed = false;
        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="Activity"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources; <b>false</b> to release only unmanaged resources. 
        /// </param>
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            lock (this)
            {
                if (this.disposed)
                    return;
                disposed = true;
            }

            // If disposing equals true, dispose all managed 
            // and unmanaged resources.
            if (disposing)
                DisposeManaged();

            // Call the appropriate methods to clean up 
            // unmanaged resources here.
            // If disposing is false, 
            // only the following code is executed.
            DisposeUnmanaged();
        }

        protected virtual void DisposeManaged()
        {
            stopped = true;
            lock (files)
            {
                Monitor.Pulse(files);
            }
        }

        protected virtual void DisposeUnmanaged()
        {
        }

        // Use C# destructor syntax for finalization code.
        // This destructor will run only if the Dispose method 
        // does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide destructors in types derived from this class.
        /// <summary>
        /// <exclude/>
        /// </summary>
        ~Notifier()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }
        #endregion

    }

    /*private class ObservedItemInfo
    {
        ObservedItemInfo(FileManager fileManager,uint id, uint cookie)
        {
            this.itemID = id;
            this.fileChangeCookie = cookie;
            this.fileManager = fileManager;
        }
        private FileManager fileManager;
        /// <summary>
        /// Defines the id of the item that is to be reloaded.
        /// </summary>
        private uint itemID;
        /// <summary>
        /// Defines the file change cookie that is returned when listening on file changes on the nested project item.
        /// </summary>
        private uint fileChangeCookie;
        internal uint ItemID
        {
            get { return this.itemID;}
            set { this.itemID = value;}
        }
        /// <summary>
        /// Defines the file change cookie that is returned when listenning on file changes on the nested project item.
        /// </summary>
        internal uint FileChangeCookie
        {
            get { return this.fileChangeCookie;}
            set { this.fileChangeCookie = value;}
        }
        ~ObservedItemInfo()
        {
        }
    }*/
}

