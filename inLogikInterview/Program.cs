//Assumptions
//I assume shouldn't use tools like Kafka and I should implement my event log system myself
//project names and usernames are one syllubes only
//You didn't intend me to asynchornously create views on the data for the read parts as I have to deal with thread safety and all!
//I'll try to implement threads if I have extra time
//As the read query is user agnostic, the seen logs can't be omitted for the next time that someone asks for them. For the wall command
//this feature can be implememnted that after seeing logs, don't show them the next time. I'll implement this if I have time

using inLogikInterview;
using System.Xml.Linq;

internal class Program
{
    private static void Main(string[] args)
    {
        while (true)
        {
            string query = "";
            query = Console.ReadLine();
            handleQuery(query);
        }


        /*
        posting: < user name > -> @< project name > < message >
        reading: < project name >
        following: < user name > follows < project name >
        wall: < user name > wall
        */



        void handleQuery(string query)
        {
            string username = "";
            string project = "";
            string message = "";
            LogManager logManager = new LogManager();
            // Alice -> @Moonshot I'm working on the log on screen
            if (query.Contains("->"))
            {
                username = query.Split(" ")[0];
                query = query.Substring(username.Length + 5);
                project = query.Split(" ")[0];
                message = query.Substring(username.Length + 1);
                logManager.post(username, project, message);
            }
            else if (query.Split(' ').Length == 1)
            {
                project = query;
                Console.WriteLine(logManager.read(project));
            }
            else if (query.Split(' ').Length == 3)
            {
                username = query.Split(' ')[0];
                project = query.Split(" ")[2];
                logManager.follow(username, project);
            }
            else
            {
                username = query.Split(' ')[0];
                Console.WriteLine(logManager.wall(username));
            }
        }
    }
}