// See https://aka.ms/new-console-template for more information
using Progressive.JobSearch;

Console.WriteLine("Start");
JobPage jp = new JobPage();
jp.NavTo();
jp.GetJobs();

Console.WriteLine("Close");
jp.Close();
