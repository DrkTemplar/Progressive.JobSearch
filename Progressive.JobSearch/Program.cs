using Progressive.JobSearch;

Console.WriteLine("Start the job search");
JobPage jp = new JobPage();
jp.NavTo();
jp.GetJobs();
// Close the browser
Console.WriteLine("Complete");
jp.Close();

