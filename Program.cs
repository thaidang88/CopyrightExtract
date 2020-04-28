using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SearchCopyright
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test();
            if (args.Length == 3)
            {
                if (args[0] == "copyright")
                {
                    SearchCopyright(args[1],args[2]);
                }
                if (args[0] == "convert")
                {
                    WriteNewFile(args[1], args[2]);
                }
            }
        }

        static void Test()
        {
            //Regex regex = new Regex(@"s..t");
            //string strRegex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

            //Match match = regex.Match("This is my seat");

            Regex regex = new Regex(@"^Copyright");
          

            Match match = regex.Match("Copyright (C) 2006-2017 Free Software Foundation, Inc.");

            if (match.Success)
            {
                Console.WriteLine("Match Value: " + match.Value);
            }
        }

        static List<string> SearchCopyright(string filename,string outfile)
        {
            List<string> ret = new List<string>();
            StreamReader sr = new StreamReader(filename);
            string input;
            string pattern = @"(Copyright \(C\).)";
            //string pattern = "^Copyright";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);

            while (sr.Peek() >= 0)
            {
                input = sr.ReadLine();
                MatchCollection matches = rgx.Matches(input);
                if (matches.Count > 0)
                {
                    Console.WriteLine("{0} ({1} matches):", input, matches.Count);
                    foreach (Match match in matches)
                        Console.WriteLine("   " + match.Value);
                    String[] temp = input.Split('.');
                    foreach (string s in temp)
                    {
                        if (s.Contains("Copyright"))
                        {
                            //Console.WriteLine(s + ".");
                            string temp2 = extractCopyrightFromString(s) + ".";
                            if (!ret.Contains(temp2))
                                ret.Add(temp2);

                        }
                    }
                }
            }
            sr.Close();

            using (StreamWriter sw = new StreamWriter(outfile))
            {
                foreach (string s  in ret)
                {
                    sw.WriteLine(s);
                }
            }
            return ret;
        }

        static string extractCopyrightFromString (string s)
        {
            string ret=null;
            string[] temp = s.Split(' ');
            if(temp.Length==0)
            {
                return null;
            }
            else
            {
                for(int i=0;i<temp.Length;i++)
                {
                    if(temp[i]=="Copyright")
                    {
                        for(int j=i;j<temp.Length;j++)
                        {
                            if (j!= (temp.Length - 1))
                            {
                                ret += temp[j] + " ";
                            }
                            else
                            {
                                ret += temp[j];
                            }
                        }
                    }

                }
                return ret;
            }
            
        }

        static void WriteNewFile(string infile, string outfile)
        {
            try
            {
                string text = File.ReadAllText(infile);
                text = text.Replace("\r\n", ",");
                File.WriteAllText(outfile, text);
                Console.WriteLine("Finish convert");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
