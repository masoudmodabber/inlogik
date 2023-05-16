using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inLogikInterview
{
/*
posting: < user name > -> @< project name > < message >
reading: < project name >
following: < user name > follows < project name >
wall: < user name > wall
*/
    internal class LogManager
    {
        List<Log> messageLog = new List<Log>();
        Dictionary<String, List<string>> userProjects = new Dictionary<String, List<string>>();
        Dictionary<String, List<Log>> userFollowProjectsLogs = new Dictionary<String, List<Log>>();
        Dictionary<string, List<Log>> projectLogs = new Dictionary<String, List<Log>>();
        internal void follow(string username, string project)
        {
            addFollow(username, project);
        }

        private void addFollow(string username, string project)
        {
            if (userProjects.ContainsKey(username))
            {
                if (!userProjects[username].Contains(project))
                {
                    userProjects[username].Add(project);
                }
            }
            else
            {
                userProjects.Add(username, new List<string>());
                userProjects[username].Add(project);
            }

            userFollowProjectsLogs[username] = projectLogs[project];
        }

        internal void post(string username, string project, string message)
        {
            Log newLog = new Log(username, project, message);
            messageLog.Add(newLog);
            prepareViews(newLog);
        }

        private void prepareViews(Log newLog)
        {
            String project = newLog.GetProject();
            if (!projectLogs.ContainsKey(project)) 
            {
                projectLogs[project] = new List<Log> { newLog };
            }
            else
            {
                projectLogs[project].Add(newLog);
            }
        }

        /*
            Alice
            I'm working on the login screen (5 minutes ago)
            Bob
            Awesome, I'll start on the forgotten password screen (4 minutes ago)
            I'll give you the link to put on the log on screen shortly Alice (1 minute ago)
        */
        internal string read(string project)
        {
            string result = "";
            foreach (Log log in projectLogs[project])
            {
                result += log.GetUsername() + "\n" + log.GetMessage() + " " + getMinutesString(DateTime.Now - log.GetTime()) + "\n"; 
            }
            return result;
        }

        private string getMinutesString(TimeSpan timeSpan)
        {
            string result =  "(" + timeSpan.TotalMinutes + " minutes ago)";
            return result;
        }

        // Charlie wall
        // Apollo - Bob: Has anyone thought about the next release demo? (6 minutes ago)
        internal String wall(string username)
        {
            string result = "";
            foreach (List<Log> projectList in userFollowProjectsLogs.Values)
            {
                foreach (Log log in projectList)
                {
                    result += log.GetProject() + " - " + log.GetUsername() + ": " + log.GetMessage() + 
                        getMinutesString(DateTime.Now - log.GetTime()) + "\n";
                }
            }
            return result;
        }

    }

    internal class Log
    {
        private String username;
        private String project;
        private String message;
        private DateTime logTime;
        internal Log(String username, String project, String message) 
        {
            this.username = username;
            this.message = message;
            this.project = project;
            logTime = DateTime.Now;
        }
        public String GetUsername()
        {
            return username;
        }
        public String GetProject()
        {
            return project;
        }
        public String GetMessage()
        {
            return message;
        }
        public DateTime GetTime()
        {
            return logTime;
        }
    }
}
