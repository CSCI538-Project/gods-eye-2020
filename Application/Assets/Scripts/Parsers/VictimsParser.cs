using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Assets.Scripts.Parsers
{
    public class VictimsParser
    {
        private int numberOfVictims;
        private List<Victim> victimList;

        public class Victim 
        {
            public int SessionID { get; set; }
            public Info VictimInfo { get; set; }
            public NetAddress Address { get; set; }
            public bool IsIdentified { get; set; }
        }

        public class Info
        {
            public string UserName { get; set; }
            public string HostName { get; set; }
            public string Type { get; set; }
        }

        public class NetAddress 
        {
            public string IPAddress { get; set; }
            public int Port { get; set; }
        }

        public VictimsParser(JObject content) 
        {
            if (content["total_victims"] != null) 
            {
                this.numberOfVictims = content.Value<int>("total_victims");
            }
            
            if (content["victims"] != null)
            {
                this.victimList = new List<Victim>();

                foreach (JObject v in (JArray)content["victims"])
                {
                    Victim newVictim = new Victim();
                    newVictim.SessionID = v.Value<int>("session_id");
                    newVictim.IsIdentified = v.Value<bool>("identified");

                    NetAddress netAddress = new NetAddress();
                    JArray netInfo = v.Value<JArray>("ip_address");
                    netAddress.IPAddress = netInfo[0].ToString();
                    netAddress.Port = (int)netInfo[1];

                    Info info = new Info();
                    JObject v_info = (JObject)v["victim_info"];
                    info.UserName = v_info.Value<string>("username");
                    info.HostName = v_info.Value<string>("hostname");
                    info.Type = v_info.Value<string>("type");

                    newVictim.Address = netAddress;
                    newVictim.VictimInfo = info;

                    this.victimList.Add(newVictim);
                }
            }
        }

        public List<Victim> GetAllVictims() 
        {
            return this.victimList;
        }

        public int Length() 
        {
            return this.numberOfVictims;
        }

        public Victim GetVictimBySessionID(int sessionID) 
        {
            foreach (Victim v in this.GetAllVictims()) 
            {
                if (v.SessionID == sessionID) { return v; }
            }
            return null;
        }
    }
}
