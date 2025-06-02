using Progressive.JobSearch;

Console.WriteLine("Start");
JobPage jp = new JobPage();
jp.NavTo();
jp.GetJobs();

// Perform your desired actions using the driver

// Close the browser
Console.WriteLine("Close");
jp.Close();

