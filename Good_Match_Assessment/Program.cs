using System;
//using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
//using CsvHelper;


namespace Good_Match_Assessment
{
    class Program
    {
        //private string listMales;
        //private static string listFemales;


        static string NumberReduce(string numberString){
            bool isEven = true;
            double first;
            double last;
            double tempAdd;
            string sum = "";

            if (numberString.Length % 2 != 0)//check if odd number of digits in number
            {
                isEven = false;
            }

            for (int i=0; i < numberString.Length/2; i++)
            {
                first = Char.GetNumericValue(numberString, i);
                last = Char.GetNumericValue(numberString, numberString.Length-1-i);
                tempAdd = first + last;
                sum += tempAdd.ToString();
            }
            if (isEven == false)
            {
                sum += numberString[numberString.Length/2];
            }
            
            return sum;
        }
        static string MatchingAlgorithm(string name_one, string name_two)
        {
            string joined = name_one.ToLower() + "matches" + name_two.ToLower(); //ensure lower-case to match character count frequencies easily
            string CheckedChars = "";
            string StrCount = "";
            int TempCount;
            //int Count;

            foreach (char c in joined){
                if (CheckedChars.Contains(c) == false) //if we are seeing the character for the first time, we count it
                {
                    CheckedChars += c; 
                    //Console.WriteLine("char not in str");
                    TempCount = joined.Split(c).Length - 1;
                    StrCount += TempCount.ToString();
                }
            }//after this point we need to reduce

            string reduced;
            if (StrCount.Length <= 2)
            {
                reduced = StrCount; 
            }
            else
            {
                reduced = NumberReduce(StrCount);
                while (reduced.Length > 2){
                    reduced = NumberReduce(reduced);
                }
            }
            
            int percentage = Int32.Parse(reduced);
            string result =  percentage > 80 ? (name_one + " matches "+ name_two + " "+ reduced +"%, good match") : name_one + " matches "+ name_two + " "+ reduced +"%";
            //Console.WriteLine(result);
            
            return(result);

        }


        static Tuple<string, int> MatchingAlgorithm2(string name_one, string name_two)
        {
            string joined = name_one.ToLower() + "matches" + name_two.ToLower(); //ensure lower-case to match character count frequencies easily
            string CheckedChars = "";
            string StrCount = "";
            int TempCount;
            //int Count;

            foreach (char c in joined){
                if (CheckedChars.Contains(c) == false) //if we are seeing the character for the first time, we count it
                {
                    CheckedChars += c; 
                    //Console.WriteLine("char not in str");
                    TempCount = joined.Split(c).Length - 1;
                    StrCount += TempCount.ToString();
                }
            }//after this point we need to reduce

            string reduced;
            if (StrCount.Length <= 2)
            {
                reduced = StrCount; 
            }
            else
            {
                reduced = NumberReduce(StrCount);
                while (reduced.Length > 2){
                    reduced = NumberReduce(reduced);
                }
            }
            
            int percentage = Int32.Parse(reduced);
            string result =  percentage > 80 ? (name_one + " matches "+ name_two + " "+ reduced +"%, good match") : name_one + " matches "+ name_two + " "+ reduced +"%";
            //Console.WriteLine(result);
            
            var myTuple = Tuple.Create(result,percentage);
            return(myTuple);

        }





        static Tuple<string,int>[,] MatchCSV(List<string> males, List<string> females)
        {

            Tuple<string,int>[,] ResultsArray = new Tuple<string,int>[males.Count, females.Count]; // this array will store all the results
            int i = 0;
            int j;

            foreach (string boy in males){
                j=0;

                foreach(string girl in females){
                    Tuple<string,int> result = MatchingAlgorithm2(boy, girl);
                    ResultsArray[i,j] = result; 

                    j = j+1;
                    //Console.WriteLine("j is : ");
                    //Console.WriteLine(j);

                }
                i = i+ 1;
                //Console.WriteLine("i is: ");
                //Console.WriteLine(i);

            }
            
            return(ResultsArray);

        }
        static void Main(string[] args)
        {
            
            Console.WriteLine("Welcome to the Tennis Good Match Assessment algorithm.\nPlease enter names when prompted and only use alphabetic characters.");
            // Read two names from the console as input, and ensure they are only alphabetic
            Console.WriteLine("Enter first name: \n");
            string userName1 = Console.ReadLine();
            if (userName1.All(char.IsLetter))
            {
                Console.WriteLine("First name is: " + userName1 + "\n");
            }
            else
            {
                while( userName1.All(char.IsLetter) != true )
                {
                    Console.WriteLine("Please only use alphabetic characters");
                    Console.WriteLine("Enter first name: \n");
                    userName1 = Console.ReadLine();
                }
                
            }
            
            Console.WriteLine("Enter second name: \n");
            string userName2 = Console.ReadLine();
            if (userName2.All(char.IsLetter))
            {
                Console.WriteLine("Second name is: " + userName2 + "\n");
            }
            else
            {
                while( userName2.All(char.IsLetter) != true )
                {
                    Console.WriteLine("Please only use alphabetic characters");
                    Console.WriteLine("Enter second name: \n");
                    userName2 = Console.ReadLine();
                }
                
            }

            //timer to time how long the algorithm itelf runs
            var watch = new System.Diagnostics.Stopwatch();
            
            watch.Start();


            //string name1 = "Jack" ;
            //string name2 = "Jill" ;

            /* Strings name1 and name2 above are from the example, and were used in the algoithm to test it.
               You can use these parameters as arguments to test any custom names */
            string OutputString = MatchingAlgorithm(userName1, userName2); // this is the algorithm that matches two names as required in path 1

            watch.Stop();

            Console.WriteLine("Output for Part 1; \n");
            Console.WriteLine(OutputString ); // outputs the expected result of the matching algorithm for part 1,  

            Console.WriteLine($"Time to match {userName1} and {userName2}: {watch.ElapsedMilliseconds} ms\n");
            //File.WriteAllLines("log_time_part1.txt", $"Time to match {userName1} and {userName2}: {watch.ElapsedMilliseconds} ms" );
            File.WriteAllText("log_time_part1.txt", $"Time to match {userName1} and {userName2}: {watch.ElapsedMilliseconds} ms\n" );

            List<string> listMales = new List<string>();
            List<string> listFemales = new List<string>();
            

            string basePath = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(basePath, "names.csv");
            //Console.WriteLine(filePath);

            try{
                using(var reader = new StreamReader(filePath))
                {
                    /* can do a lot of error checking here, can remove whitespaces from elements (atm i include them),
                    can convert to lower case to deal with case-insensitivity, can check the csv file path, separator values, and handle all these errors or at least log them, etc.*/
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        values[1] = values[1].Replace(" ", String.Empty);
                        
                        if (values[1] == "m"){
                            listMales.Add(values[0]);
                            //Console.WriteLine("yay this guy was added" );
                        }
                        else
                        {
                            listFemales.Add(values[0]);
                            //Console.WriteLine("yahoo this girl was added");
                        } 
                        
                    }
                }

            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File not found! Please check the file name and directory are correct! File name should be 'names.csv' and it should be in the same folder as the program.");
                Console.WriteLine(e.ToString());
            }
            /* we now remove duplicates, as well as sort the lists using an in-place sort so when we return matches later,
               if there are equal results they will be returned alphabetical in order
               the list is sorted and then reversed so it is in decesnding order as needed*/ 
            listMales = listMales.Distinct().ToList();
            listFemales = listFemales.Distinct().ToList();
            listMales.Sort();
            listMales.Reverse();
            listFemales.Sort();
            listFemales.Reverse();
            
            //Console.WriteLine(listMales[0]);
            //Console.WriteLine(listMales.Count);
            //Console.WriteLine(String.Join(",",listMales));

            watch.Start();
            
            Tuple<string,int>[,] MyResultsArr = new Tuple<string,int>[listMales.Count, listFemales.Count]; 
            MyResultsArr = MatchCSV(listMales,listFemales);

            watch.Stop();
            Console.WriteLine($"Time to match all possible names from CSV file: {watch.ElapsedMilliseconds} ms\n");
            File.WriteAllText("log_time_match_part2.txt", $"Time to match all possible names from CSV file: {watch.ElapsedMilliseconds} ms\n" );

            watch.Start();
            // now we have the results, we have to sort them and then save to a txt file
            // the sorting could be improved by doing it in-place to save on memory, and by sorting/ordering it after it has been matched 
            List<string> ResStr = new List<string>();
            List<int> ResInt = new List<int>();

            foreach (Tuple<string, int> res in MyResultsArr)
            {
                ResStr.Add(res.Item1);
                ResInt.Add(res.Item2);
                
            } 

            //we create an ordering index using the percentage values
            var newOrdering = ResInt
            .Select((Int32, index) => new { Int32, index })
            .OrderBy(item => item.Int32)
            .ToArray();

            //we now sort the list of string using the ordering we obtained 
            ResStr = newOrdering.Select(item => ResStr[item.index]).ToList();

            //now  we reverse the list so it is in descending order
            ResStr.Reverse();

            watch.Stop();
            Console.WriteLine($"Time to sort the matched results: {watch.ElapsedMilliseconds} ms\n");
            File.WriteAllText("log_time_sort_part2.txt", $"Time to sort the matched results: {watch.ElapsedMilliseconds} ms\n" );

            File.WriteAllLines("output.txt", ResStr.Select(x => string.Join(",", x)));
            
            Console.WriteLine("Output Part 2 : \n");
            Console.WriteLine("Results After Sorting (can also be found in output.txt): \n");
            Console.WriteLine(String.Join(",\n",ResStr));

        }
    }
}
