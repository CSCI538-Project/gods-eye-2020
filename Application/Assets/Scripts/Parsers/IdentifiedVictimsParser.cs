using Assets.Scripts.Configs;
using Assets.Scripts.Database;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Parsers
{
    public class IdentifiedVictimsParser
    {
        private int numberOfIdentifiedVictims;
        private List<IdentifiedVictim> identifiedVictimList;
        private ProfileParser profileParser;

        public class IdentifiedVictim
        {
            public int SessionID { get; set; }
            public Profile ProfileInfo { get; set; }
        }

        public class Profile
        {
            public string Picture { get; set; }
            public string Name { get; set; }
            public string Privacy { get; set; }
            public ProfileParser VictimInfo { get; set; }
            public byte[] OriginContent { get; set; }       // It saves the JSON message obtained from the server
        }

        public IdentifiedVictimsParser(JObject content)
        {
            if (content["total_identified_victims"] != null)
            {
                this.numberOfIdentifiedVictims = content.Value<int>("total_identified_victims");
            }

            if (content["identified_victims"] != null)
            {
                this.identifiedVictimList = new List<IdentifiedVictim>();
                UnityMainThreadDispatcher.Instance().Enqueue(loadIdentifiedVictimInfo((JArray)content["identified_victims"]));
            }
        }

        private IEnumerator loadIdentifiedVictimInfo(JArray victims)
        {
            History history = new History();

            foreach (JObject i in victims)
            {
                IdentifiedVictim newIdentifiedVictim = new IdentifiedVictim();
                newIdentifiedVictim.SessionID = i.Value<int>("session_id");

                Profile identifiedProfile = new Profile();
                JObject identifiedProfile_info = (JObject)i["profile"];
                identifiedProfile.Picture = identifiedProfile_info.Value<string>("picture");
                identifiedProfile.Name = identifiedProfile_info.Value<string>("name");
                identifiedProfile.Privacy = identifiedProfile_info.Value<string>("privacy");
                // Obtain victim's detail info from the privacy
                identifiedProfile.VictimInfo = new ProfileParser();
                using (UnityWebRequest webRequest = UnityWebRequest.Get("http://" + Global.SERVER_IP_ADDR + "/" + identifiedProfile.Privacy))
                {
                    yield return webRequest.SendWebRequest();

                    if (webRequest.isHttpError || webRequest.isNetworkError)
                    {
                        Debug.LogError(webRequest.error + "\n" + webRequest.downloadHandler.text);
                    }
                    else
                    {
                        Debug.Log("The JSON file contained the Victim's Info Data Received.");
                        identifiedProfile.OriginContent = new byte[webRequest.downloadHandler.data.Length];
                        identifiedProfile.OriginContent = webRequest.downloadHandler.data;
                        identifiedProfile.VictimInfo = ProfileParser.parseProfile(System.Text.Encoding.UTF8.GetString(identifiedProfile.OriginContent));
                    }
                }

                newIdentifiedVictim.ProfileInfo = identifiedProfile;
                this.identifiedVictimList.Add(newIdentifiedVictim);

                // Save locally
                history.AddVictim(newIdentifiedVictim.ProfileInfo.VictimInfo.profile.first_name, newIdentifiedVictim.ProfileInfo.VictimInfo.profile.last_name, System.Text.Encoding.UTF8.GetString(newIdentifiedVictim.ProfileInfo.OriginContent), newIdentifiedVictim.ProfileInfo.Picture);
            }

            // Notify that the identified victims have been added.
            MainDataController.instance.DataCenterInstance.PublishNotification(MainDataController.DataCenter.UpdatedType.IdentifiedVictims);
        }

        public List<IdentifiedVictim> GetAllIdentifiedVictims()
        {
            return this.identifiedVictimList;   // The returned value could be null. It depends on when JSON files are received and finished parsing.
        }

        public int Length()
        {
            return this.numberOfIdentifiedVictims;
        }

        public IdentifiedVictim GetIdentifiedVictimBySessionID(int sessionID)
        {
            foreach (IdentifiedVictim v in this.GetAllIdentifiedVictims())
            {
                if (v.SessionID == sessionID) { return v; }
            }
            return null;
        }

        public IdentifiedVictim GetIdentifiedVictimByName(string name)
        {
            foreach (IdentifiedVictim v in this.GetAllIdentifiedVictims())
            {
                if (v.ProfileInfo.Name == name) { return v; }
            }
            return null;
        }
    }
}
