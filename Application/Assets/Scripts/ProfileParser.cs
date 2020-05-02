//using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class ProfileParser
{

    public int score;

    public RootObject profile;

    public ProfileParser() { }

    public ProfileParser(RootObject profile, int score)
    {
        this.profile = profile;
        this.score = score;
    }

    public class Education
    {
        public string school { get; set; }
        public string degree { get; set; }
        public string major { get; set; }
        public string tenure { get; set; }
    }

    public class ContactInfo
    {
        public string mobile { get; set; }
        public string home { get; set; }
    }

    public class Residence
    {
        public string apt { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public int zipcode { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class PersonalInfo
    {
        public int age { get; set; }
        public List<Education> education { get; set; }
        public string email { get; set; }
        public ContactInfo contact_info { get; set; }
        public Residence residence { get; set; }
        public string recent_images { get; set; }
    }

    public class VenmoTx
    {
        public string amount { get; set; }
        public string recipient { get; set; }
        public string payer { get; set; }
        public string date { get; set; }
    }

    public class FinancialInfo
    {
        public string current_company { get; set; }
        public string company_logo { get; set; }
        public string position { get; set; }
        public string location { get; set; }
        public int salary { get; set; }
        public string salary_string { get; set; }
        public List<VenmoTx> venmo_tx { get; set; }
    }

    public class FamilyMember
    {
        public string name { get; set; }
        public string relation { get; set; }
        public string url { get; set; }
    }

    public class SocialMediaFriend
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Connections
    {
        public string relationship_status { get; set; }
        public int no_of_dependents { get; set; }
        public string family_members_images_dir { get; set; }
        public List<FamilyMember> family_members { get; set; }
        public string social_media_friends_images_dir { get; set; }
        public List<SocialMediaFriend> social_media_friends { get; set; }
    }

    public class Facebook
    {
        public string username { get; set; }
        public string url { get; set; }
    }

    public class Instagram
    {
        public string username { get; set; }
        public string url { get; set; }
    }

    public class Linkedin
    {
        public string username { get; set; }
        public string url { get; set; }
    }

    public class Twitter
    {
        public string username { get; set; }
        public string url { get; set; }
    }

    public class SocialMediaInfo
    {
        public Facebook facebook { get; set; }
        public Instagram instagram { get; set; }
        public Linkedin linkedin { get; set; }
        public Twitter twitter { get; set; }
    }

    public class Checkin
    {
        public string location { get; set; }
        public string date { get; set; }
        public string time { get; set; }
    }

    public class InterestsInfo
    {
        public List<Checkin> checkins { get; set; }
        public List<string> recent_likes { get; set; }
    }

    public class VictimBaseInfo
    {
        public string name { get; set; }
        public string image { get; set; }
        public string vulnerabilityScore { get; set; }
    }
    public class ConnectedVictimInfo
    {
        public string name { get; set; }
        public string picture { get; set; }
        public string privacy { get; set; }
    }

    public class RootObject
    {
        public string id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string face_image { get; set; }
        public PersonalInfo personal_info { get; set; }
        public FinancialInfo financial_info { get; set; }
        public Connections connections { get; set; }
        public SocialMediaInfo social_media_info { get; set; }
        public InterestsInfo interests_info { get; set; }
    }

    // public class Profile{
    //     public RootObject profile;

    //     public int score;

    //     public Profile(RootObject profile, int score){
    //         this.profile = profile;
    //         this.score = score;
    //     }
    // }

    public static Dictionary<int, int> residenceMap = new Dictionary<int, int>
            {
                {90007, 7}, //USC Area
                {90031, 9}, //Beverly Hills
                {90017, 8}, //Downtown LA
                {91001, 7}, //Pasadena
                {90291, 8}, //Santa Monica
                {0, 0}      //If zipcode doesn't exist, hence json has 0 value
            };



    public static int calVulnerabilityScore(int res, int sal, int dependents, List<Checkin> checkins, int age)
    {


        Dictionary<string, int> priorities = new Dictionary<string, int>
            {
                { "Residence" ,5},
                { "Salary", 4},
                { "Check-ins" , 3},
                { "Dependents" , 2},
                { "Age" ,1}
            };

        //No presence on social media
        int score = 0;
        if (res == 0 && sal == 0 && dependents == 0 && checkins.Count == 0 && age == 0){
            score = Random.Range(25, 35);
            return score;
        }
        
        //salary computation
        int normalized_sal = 0;
        if (sal > 0 && sal < 25000)
            normalized_sal = 1;
        else if (sal >= 25000 && sal < 50000)
            normalized_sal = 2;
        else if (sal >= 50000 && sal < 100000)
            normalized_sal = 3;
        else if (sal >= 10000 && sal < 120000)
            normalized_sal = 4;
        else if (sal >= 120000 && sal < 140000)
            normalized_sal = 5;
        else if (sal >= 140000 && sal < 160000)
            normalized_sal = 6;
        else if (sal >= 160000 && sal < 200000)
            normalized_sal = 7;
        else if (sal >= 20000 && sal < 300000)
            normalized_sal = 8;
        else if (sal >= 300000 && sal < 500000)
            normalized_sal = 9;
        else if (sal > 500000)
            normalized_sal = 10;

        // Debug.Log("normalized sal : " + normalized_sal);


        //age computation
        int normalized_age = 0;
        if (age > 0 && age < 20)
            normalized_age = 4;
        else if (age >= 20 && age < 25)
            normalized_age = 6;
        else if (age >= 25 && age < 30)
            normalized_age = 8;
        else if (age >= 30 && age < 35)
            normalized_age = 9;
        else if (age >= 35 && age < 40)
            normalized_age = 7;
        else if (age >= 40 && age < 50)
            normalized_age = 5;
        else
            normalized_age = 3;

        // Debug.Log("normalized age : " + normalized_age);

        //score calculation
        score += priorities["Age"] * normalized_age;
        //Console.WriteLine("Score : " + score);
        int length_checkins = checkins.Count ;
        if (length_checkins > 5)
            length_checkins = 5;
        score += priorities["Check-ins"] * length_checkins;
       // Console.WriteLine("Score : " + score);
        score += priorities["Dependents"] * dependents;
       // Console.WriteLine("Score : " + score);
        score += priorities["Salary"] * normalized_sal;
       // Console.WriteLine("Score : " + score);
        score += priorities["Residence"] * residenceMap[res];
        //Console.WriteLine("Score : " + score);

        return (int)(score);

    }


    public static ProfileParser parseProfile(string text)
    {

        //using (StreamReader r = new StreamReader("Assets/Resources/profile_dir/profile_" + id + ".json"))
        //using (StreamReader r = new StreamReader(path))
        //{
        //var json = r.ReadToEnd();

        var items = JsonConvert.DeserializeObject<RootObject>(text);
        int vulScore = calVulnerabilityScore(items.personal_info.residence.zipcode, items.financial_info.salary, items.connections.no_of_dependents, items.interests_info.checkins, items.personal_info.age);
        ProfileParser profileObj = new ProfileParser(items, vulScore);
        return profileObj;
        //}

    }

    // public void setText(string userId)
    //     {
    //         Text buttonText = transform.Find("Text").GetComponent<Text>();
    //         buttonText.text = parseProfile(userId).score;
    // }

    // static void Main(string[] args){
    //     ProfileParser currentProf = ProfileParser.parseProfile("gagan");
    //     Console.WriteLine("+++++++++++++++++");
    //     Console.WriteLine(currentProf.score);
    //     Console.WriteLine(currentProf.profile.first_name);
    //     Console.WriteLine(currentProf.profile.personal_info.age);
    //     Console.WriteLine(currentProf.profile.financial_info.company_logo);
    // }
}