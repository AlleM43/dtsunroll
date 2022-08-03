class dtsunroll{
	static string version = "0.1";
	public static void Main(string[] args){
		
		if(args.Length==0){
			printHelp();
		}
		foreach (string String in args)
		{
			if((String=="-h") || (String =="--help")){
				printHelp();
			}
		}
		if(File.Exists(args[0])){
			if (File.Exists(args[1])){
				Console.WriteLine("Error: Output file already exists");
				Environment.Exit(1);
			}
			List<string> outText = unrollDTS(args[0], Directory.GetCurrentDirectory());
			File.WriteAllLines(args[1], outText);
			Environment.Exit(0);
		} else {printHelp();}
	}

	static void printHelp(){
		Console.Write("dtsunroll version " + dtsunroll.version + "\nUsage: dtsunroll [INPUT] [OUTPUT]\nUnroll references found in .dts files\n  -h, --help:		Writes this information and exits\n");
		Environment.Exit(0);
	}
	static List<string> unrollDTS(string filename, string path){
		string[] Input = File.ReadAllLines(filename);
		List<string> Output = new List<string>();
		foreach (string line in Input)
		{
			
			try{string test2c = line.Substring(0,2);
			if((test2c=="/*")||(test2c==" *")){
				//Do nothing, it's a comment
				//Console.WriteLine("About as intelligent as a reddit comment section");
			} else try
			{
				string test10c = line.Substring(0, 10);
				if (test10c=="#include <"){
					//I'm not handling these
					//Console.WriteLine("Comprehension Not Included");
				} else if (test10c=="#include \""){
					string includefilename = line.Split('"')[1];
					string fullPath=path + "/" + includefilename;
					//Console.WriteLine("Including " + includefilename + " with full file path " + fullPath);
					List<string> fullPathFragments=fullPath.Split("/").ToList<string>();
					//Console.WriteLine("fullPathFragments at " + fullPathFragments.Count);
					fullPathFragments.RemoveAt(fullPathFragments.Count-1);
					//Console.WriteLine("fullPathFragments at " + fullPathFragments.Count);
					string inDirectory = String.Join("/", fullPathFragments);
					//Console.WriteLine("Passing " + fullPath + " in directory " + inDirectory);
					List<string> toOutput = unrollDTS(fullPath, inDirectory);
					foreach (string outline in toOutput)
					{
						Output.Add(outline);
					}
					//Console.WriteLine("Something's working");
				} else Output.Add(line);
			}
			catch (ArgumentOutOfRangeException)
			{
				//Console.WriteLine("Medium-sized");
				Output.Add(line);
			}} catch(ArgumentOutOfRangeException){
				//Console.WriteLine("Wow, that was short");
				Output.Add(line);
				}
		}
		
		return Output;
	}
}