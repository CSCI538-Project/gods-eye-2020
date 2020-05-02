using System;
using System.Collections.Generic;
using System.Text;
using Mono.Data.Sqlite;
using System.Data;

namespace Assets.Scripts.Database
{
    public class VictimHistory 
    {
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileInJSON { get; set; }
        public string FaceImagePath { get; set; }
        public bool IsValid { get; set; }
    }
    /*
     * This class will mainly handle saving all identifiled victims to History locally using SQLite.
     * The field of `profile_json` in the table of `VictimHistory` will store the value of `ProfileInJSON` in BASE64 format because of the special of the value.
     */
    class History
    {
        private static IDbConnection db;

        public History() 
        {
            if (db == null) 
            {
                db = new SqliteConnection("URI=file:" + Configs.Global.LocalStorage.Paths.DatabaseFile);
                db.Open();
                // this.deleteTable("VictimHistory");
                this.createVictimsTable();
            }
        }

        public void AddVictim(string firstName, string lastName, string profileJSON, string faceImagePath) 
        {
            VictimHistory victimHistory = this.GetVictimBy(firstName, lastName);
            IDbCommand dbcmd = db.CreateCommand();

            if (victimHistory.IsValid) 
            {
                // This victim has already added into the database, let's check any updates
                if (victimHistory.ProfileInJSON != profileJSON || victimHistory.FaceImagePath != faceImagePath) 
                {
                    // There are some updates, we should update the records in the database
                    byte[] updatedProfile = Encoding.UTF8.GetBytes(profileJSON);
                    dbcmd.CommandText = "UPDATE VictimHistory SET profile_json=\"" 
                        + Convert.ToBase64String(updatedProfile) +
                        "\",face_img_path=\""
                        + faceImagePath + 
                        "\" WHERE fullname=\""
                        + firstName + " " + lastName +
                        "\"";
                    _ = dbcmd.ExecuteNonQuery();
                }
                dbcmd.Dispose();
                return; 
            }

            byte[] profileByte = Encoding.UTF8.GetBytes(profileJSON);
            dbcmd.CommandText = "INSERT INTO VictimHistory (fullname,firstname,lastname,profile_json,face_img_path) VALUES(\""
                + firstName + " " + lastName +
                "\", \""
                + firstName +
                "\", \""
                + lastName +
                "\", \""
                + Convert.ToBase64String(profileByte) +
                "\", \""
                + faceImagePath +
                "\")";
            int result = dbcmd.ExecuteNonQuery();

            dbcmd.Dispose();
        }

        public VictimHistory GetVictimBy(string firstName, string lastName) 
        {
            IDbCommand dbcmd = db.CreateCommand();
            dbcmd.CommandText = "SELECT fullname, firstname, lastname, profile_json, face_img_path FROM VictimHistory WHERE fullname = \"" + firstName + " " + lastName +"\"";
            IDataReader reader = dbcmd.ExecuteReader();

            VictimHistory victim = new VictimHistory();

            while (reader.Read())
            {
                victim.FullName = reader.GetString(0);
                victim.FirstName = reader.GetString(1);
                victim.LastName = reader.GetString(2);
                victim.ProfileInJSON = Encoding.UTF8.GetString(Convert.FromBase64String(reader.GetString(3)));
                victim.FaceImagePath = reader.GetString(4);
                victim.IsValid = true;
                break;
            }

            reader.Close();
            dbcmd.Dispose();

            return victim;
        }

        public string GetProfileJSONBy(string firstName, string lastName) 
        {
            IDbCommand dbcmd = db.CreateCommand();
            dbcmd.CommandText = "SELECT profile_json FROM VictimHistory WHERE fullname = \"" + firstName + " " + lastName + "\"";
            IDataReader reader = dbcmd.ExecuteReader();

            string profileStr = "";

            while (reader.Read()) 
            {
                profileStr = Encoding.UTF8.GetString(Convert.FromBase64String(reader.GetString(0)));
                break;
            }

            reader.Close();
            dbcmd.Dispose();
            return profileStr;
        }

        public int GetNumberOfVictims() 
        {
            IDbCommand dbcmd = db.CreateCommand();
            dbcmd.CommandText = "SELECT COUNT(*) FROM VictimHistory";
            IDataReader reader = dbcmd.ExecuteReader();

            int num = 0;
            while (reader.Read())
            {
                num = reader.GetInt32(0);
                break;
            }

            reader.Close();
            dbcmd.Dispose();
            return num;
        }

        public List<VictimHistory> GetAllReviewedVictims() 
        {
            IDbCommand dbcmd = db.CreateCommand();
            dbcmd.CommandText = "SELECT fullname, firstname, lastname, profile_json, face_img_path FROM VictimHistory";
            IDataReader reader = dbcmd.ExecuteReader();

            List<VictimHistory> victims = new List<VictimHistory>();

            while (reader.Read()) 
            {
                VictimHistory victim = new VictimHistory();
                victim.FullName = reader.GetString(0);
                victim.FirstName = reader.GetString(1);
                victim.LastName = reader.GetString(2);
                victim.ProfileInJSON = Encoding.UTF8.GetString(Convert.FromBase64String(reader.GetString(3)));
                victim.FaceImagePath = reader.GetString(4);
                victim.IsValid = true;

                victims.Add(victim);
            }

            reader.Close();
            dbcmd.Dispose();

            return victims;
        }

        public void Close() 
        {
            db.Close();
            db.Dispose();
            db = null;
        }

        public void onDistory() 
        {
            this.Close();
        }

        private void deleteTable(string tableName)
        {
            IDbCommand dbcmd = db.CreateCommand();
            // DROP TABLE VictimHistory;
            dbcmd.CommandText = "DROP TABLE " + tableName;
            int result = dbcmd.ExecuteNonQuery();
            dbcmd.Dispose();
        }

        private void createVictimsTable()
        {
            IDbCommand dbcmd = db.CreateCommand();
            dbcmd.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='VictimHistory' ";
            IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                int existTable = reader.GetInt32(0);
                if (existTable == 0)
                {
                    // Table does not exist
                    reader.Close();
                    dbcmd.CommandText = "CREATE TABLE \"VictimHistory\" ( \"fullname\"  TEXT, \"firstname\" TEXT, \"lastname\"  TEXT, \"profile_json\"  TEXT, \"face_img_path\"	TEXT, PRIMARY KEY(\"fullname\") )";
                    dbcmd.ExecuteNonQuery();
                }
                break;
            }

            dbcmd.Dispose();
        }

        /*
        public void SaveProfileLocally(string firstName, string lastName) 
        {
            IdentifiedVictimsParser identifiedVictimsParser = MainDataController.instance.DataCenterInstance.GetIdentifiedVictimsParser();
            if (identifiedVictimsParser == null) { return; }
            IdentifiedVictim identifiedVictim = identifiedVictimsParser.GetIdentifiedVictimByName(firstName);
            if (identifiedVictim == null) { return; }

            FileStream fs = new FileStream(pathProfiles + "profile_"+ firstName + " " + lastName + ".json", FileMode.Create);
            fs.Write(identifiedVictim.ProfileInfo.OriginContent, 0, identifiedVictim.ProfileInfo.OriginContent.Length);
            fs.Close();
        }

        public byte[] ReadProfile(string firstName, string lastName) 
        {
            FileStream fs = new FileStream(pathProfiles + "profile_" + firstName + " " + lastName + ".json", FileMode.Open);
            byte[] readBuffer = new byte[fs.Length];        // Because the JSON file is not too large, I just assign the size of JSON file.
            fs.Read(readBuffer, 0, (int)fs.Length);
            fs.Close();
            return readBuffer;
        }
        */
    }
}
