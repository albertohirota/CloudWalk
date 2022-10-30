# CloudWalk
Test intended for candidates applying to Quality Engineering positions

The solution was craated in Visual Studio using C# and Newtonsoft.Json package.
Our main goal is to read Quake log files, group them according to each match, and collect kill data in a JSON report.
Each game round has its own grouped information with the following data: total_kills, players, and kills ranked from the highest kills ranking to the lowest. In addition, it included the death cause for each match.
The project consists of an entry point in the CloudWalk project where the Main function calls the ProcessLog class to execute the report.
ProcessReport is the method called, and it should pass two string parameters. First,  it should be the full file path and log name, and second, it should be the full path and Json file name where it should be saved in the JSON report.

The solution consists of 3 main projects:
The first project is where the primary location where the log is called
The second project is the LogParser project, where the log is analyzed and created the JSON.
A third project is a Unit and Integration testing framework where the main functionality is tested.

