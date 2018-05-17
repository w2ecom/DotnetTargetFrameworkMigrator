using System;
using System.Collections.Generic;
using System.Xml;

namespace TargetFrameworkMigrator
{
    class Program
    {
        static string slnPath = "";
        static string ver = "";
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("CITI, Falls Church, VA, USA");
                Console.WriteLine("All rights reserved");
                Console.WriteLine("Dotnet Target Framework Migrator:");
                Console.WriteLine("------" + DateTime.Now.ToLongDateString());
                Console.WriteLine("------");
                Console.WriteLine("Enter complete path to the solution file: ");
                slnPath =  Console.ReadLine();
                Console.WriteLine("New Target Framework Version (for eg: v4.7.1) : ");
                ver = Console.ReadLine();
                if (!System.IO.File.Exists(slnPath))
                    Console.WriteLine("Solution does not exist!");

                List<string> projDetailsList = ReadSolutionFile(slnPath);

                if (projDetailsList.Count > 0)
                {
                    UpdateFrameWorkVersion(projDetailsList);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Press any key to exit...");
            Console.Read();

        }
        /// <summary>
        /// Reads the provided sln file and find out the projects that are part of the Solution file.
        /// </summary>
        /// <param name="slnPath"></param>
        /// <returns></returns>
        static List<string>  ReadSolutionFile(string slnPath)
        {
            List<string> projPathList = new List<string>();
            try { 
                string line;
                string[] projFileDetails;
                System.IO.StreamReader file = new System.IO.StreamReader(slnPath);
                System.Console.WriteLine("Found the following projects");
                while ((line = file.ReadLine()) != null)
                {
                    if(line.Trim().IndexOf("Project(") == 0)
                    {
                        projFileDetails = line.Split(',');
                        foreach(var cont in projFileDetails)
                        {
                            if(cont.Trim().IndexOf(".csproj") > 0)
                            {
                                projPathList.Add(cont.Trim());
                                Console.WriteLine(cont.Trim());
                            }
                        }
                    }
                }
                file.Close();
                Console.WriteLine("--------");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.Message);
            }
            return projPathList;
        }

        /// <summary>
        /// Update the <TargetFrameworkVersion> element with .csproj files.
        /// </summary>
        /// <param name="projDetailsList"></param>
        static void UpdateFrameWorkVersion(List<string> projDetailsList)
        {
            try
            {
                string newValue = string.Empty;
                string path = new System.IO.FileInfo(slnPath).Directory.FullName;
                string xmlFile = "";
                XmlDocument xmlDoc = new XmlDocument();
                XmlNodeList nodeList0 = null;
                bool foundTargetFrameworkElement = false;
                foreach (var loc in projDetailsList)
                {
                    xmlFile = System.IO.Path.Combine(path, loc.Replace("\"", ""));
                    xmlDoc.Load(xmlFile);

                    foundTargetFrameworkElement = false;
                    nodeList0 = xmlDoc.DocumentElement.ChildNodes;
                    foreach(XmlNode node0 in nodeList0)
                    {
                        foreach (XmlNode node1 in node0.ChildNodes)
                        {
                            if (node1 != null && node1.Name == "TargetFrameworkVersion")
                            {
                                node1.InnerText = ver;
                                foundTargetFrameworkElement = true;
                                break;
                            }
                        }
                        if (foundTargetFrameworkElement)
                            break;
                    }
                    xmlDoc.Save(xmlFile);
                    Console.WriteLine("Updated the following file : " + xmlFile);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.Message);
            }
        }
    }
}
