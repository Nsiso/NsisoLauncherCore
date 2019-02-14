using System;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;

namespace NsisoLauncherCore.Net
{
    public class DownloadTask : INotifyPropertyChanged
    {
        public DownloadTask(string name, string from, string to)
        {
            this.TaskName = name;
            this.From = from;
            this.To = to;
        }

        public string TaskName { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public Func<Exception> Todo { get; set; }

        private long _totalSize = 1;
        public long TotalSize
        {
            get { return _totalSize; }
            private set {
                _totalSize = value;
                OnPropertyChanged("TotalSize");
            }
        }

        private long _downloadSize = 0;
        public long DownloadSize
        {
            get { return _downloadSize; }
            private set {
                _downloadSize = value;
                OnPropertyChanged("DownloadSize");
            }
        }

        private string _state;
        public string State
        {
            get { return _state; }
            private set
            {
                _state = value;
                OnPropertyChanged("State");
            }
        }


        public void SetTotalSize(long size)
        {
            TotalSize = size;
        }

        public void IncreaseDownloadSize(long size)
        {
            DownloadSize += size;
        }

        public void SetDone()
        {
            DownloadSize = TotalSize;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string strPropertyInfo)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(strPropertyInfo));
        }
    }

    public static class DownloadTaskHelper
    {
        public static async Task<long> GetFileSize(DownloadTask task)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(task.From);
            HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync();
            long size = response.ContentLength;
            response.Close();
            return size;
        }

        public static long GetFileSize(HttpWebResponse response)
        {
            return response.ContentLength;
        }
    }
}
