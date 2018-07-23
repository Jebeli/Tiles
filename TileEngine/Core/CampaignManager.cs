using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine.Events;

namespace TileEngine.Core
{
    public class CampaignManager
    {
        private List<string> status;
        public CampaignManager()
        {
            status = new List<string>();
        }

        public bool CheckStatus(IEnumerable<string> ss)
        {
            if (ss != null)
            {
                foreach(var s in ss)
                {
                    if (!CheckStatus(s)) return false;
                }
                return true;
            }
            return false;
        }

        public bool CheckStatus(string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            foreach (var st in status)
            {
                if (st.Equals(s, StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }

        public void SetStatus(IEnumerable<string> ss)
        {
            if (ss != null)
            {
                foreach (var s in ss) SetStatus(s);
            }
        }

        public void SetStatus(string s)
        {
            if (string.IsNullOrEmpty(s)) return;
            if (CheckStatus(s)) return;
            status.Add(s);
        }

        public void UnsetStatus(IEnumerable<string> ss)
        {
            if (ss != null)
            {
                foreach (var s in ss) UnsetStatus(s);
            }
        }

        public void UnsetStatus(string s)
        {
            if (string.IsNullOrEmpty(s)) return;
            for (int i = status.Count; i > 0; i--)
            {
                string st = status[i - 1];
                if (st.Equals(s, StringComparison.OrdinalIgnoreCase))
                {
                    status.RemoveAt(i - 1);
                    return;
                }
            }
        }

        public bool CheckAllRequirements(EventComponent ec)
        {
            switch (ec.Type)
            {
                case EventComponentType.RequiresStatus:
                    if (CheckStatus(ec.StringParams)) return true;
                    break;
                case EventComponentType.RequiresNotStatus:
                    if (!CheckStatus(ec.StringParams)) return true;
                    break;
                default:
                    return true;
            }
            return false;
        }
    }
}
