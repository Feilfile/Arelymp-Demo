using Matchmaker.Configuration;

var myConfig = ConfigurationLoader.LoadConfiguration();

Director.Director.Initialize(myConfig);
Director.Director.Start();