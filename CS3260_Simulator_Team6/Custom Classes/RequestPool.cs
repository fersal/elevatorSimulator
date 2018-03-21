using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace CS3260_Simulator_Team6
{
    public class RequestPool
    {
        private List<string> requestPool;
        private string currentRequest;
        private ListBox lstBoxRequestPool;

        public RequestPool(ListBox listBox)
        {
            requestPool = new List<string>();
            currentRequest = "";
            this.lstBoxRequestPool = listBox;
        }

        public void AddRequest(string request)
        {
            requestPool.Add(request);
            lstBoxRequestPool.Items.Add(request);
            currentRequest = request;
        }

        public void CompleteRequest(string request)
        {
            for (int i = 0; i < lstBoxRequestPool.Items.Count; i++)
            {
                if (lstBoxRequestPool.Items[i].ToString().Contains(request))
                {
                    lstBoxRequestPool.Items.RemoveAt(i);
                }
            }
            requestPool.Remove(request);
        }

        public string GetLastRequest()
        {
            return requestPool.Last();
        }

        public string GetCurrentRequest()
        {
            return currentRequest;
        }
    }
}
