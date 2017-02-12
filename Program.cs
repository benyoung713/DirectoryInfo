using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace FolderIteration
{
    class Program
    {
        static void Main(string[] args)
        {
            //creates the folder for the txt file if it does not exist
            if (!Directory.Exists(@"C:\Users\Public\TestResults\"))
            {
                Directory.CreateDirectory(@"C:\Users\Public\TestResults\");
            }
            //creates a header for the file
            string[] header = { "List of Directories and files on this computer" } ;
            File.WriteAllLines(@"C:\Users\Public\TestResults\ComputerFiles.txt", header);

            //a list for all of the folders and directories to be stored in to be sorted later
            List<Folder> DList = new List<Folder>();

            //create instances of drives which contain files
            string[] drives = Environment.GetLogicalDrives();
            //goes into each drive on machine to display info
            foreach (string dr in drives)
            {
                //creates instance of each drive
                DriveInfo di = new DriveInfo(dr);
                //makes sure drive can be read
                if (!di.IsReady)
                {
                    Console.WriteLine("The drive {0} could not be read", di.Name);
                    continue;
                }

                //find the root directory path
                DirectoryInfo dirInfo = di.RootDirectory;


                try
                {
                    //EnumerateFiles increases the performance and sort them
                    foreach (var fi in dirInfo.EnumerateFiles())
                    {
                        try
                        {
                            //adds the files immediately below the directory to the list
                            DList.Add(new Folder(fi.FullName, fi.Length));

                        }
                        catch (UnauthorizedAccessException UnAuthTop)
                        {
                            Console.WriteLine("{0}", UnAuthTop.Message);
                        }
                    }


                    DirectoryInfo[] dirInfos = dirInfo.GetDirectories("*.*");


                    foreach (DirectoryInfo d in dirInfos)
                    {

                        try
                        {
                            //gets the size of the directories as there is no inbuilt method
                            long size = GetDirectorySize(dr + d.Name + "\\");
                            //adds the directories to the list to be compared later
                            DList.Add(new Folder(d.FullName, size));

                        }
                        catch (DirectoryNotFoundException) { }
                        catch (UnauthorizedAccessException e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                    //delves into the subdirectories and gets the files
                    foreach (var d in dirInfo.EnumerateDirectories("*"))
                    {
                        try
                        {
                            foreach (var fi in d.EnumerateFiles("*", SearchOption.AllDirectories))
                            {
                                try
                                {
                                    DList.Add(new Folder(fi.FullName, fi.Length));

                                    
                                }
                                catch (UnauthorizedAccessException UnAuthFile)
                                {
                                    Console.WriteLine("UnAuthFile: {0}", UnAuthFile.Message);
                                }
                            }
                        }
                        catch (DirectoryNotFoundException) { }
                        catch (UnauthorizedAccessException) { }


                        //gets the list holding the paths and sizes of each directory and folder and orders them
                        foreach (var mem in DList.OrderByDescending(s => s.size))
                        {
                            //gives the results on the console log as well
                            Console.WriteLine("File path: {0}, File size: {1} bytes", mem.path, mem.size);
                            
                        }





                    }

                }
                catch (DirectoryNotFoundException) { }
                catch (UnauthorizedAccessException) { }


            }
            //appends the list of files and sizes to the text document
            using (StreamWriter file = new StreamWriter(@"C: \Users\Public\TestResults\ComputerFiles.txt", true))
            {
                foreach (var mem in DList.OrderByDescending(s => s.size))
                {
                    file.WriteLine("File path: {0}, File size: {1} bytes", mem.path, mem.size);
                }
            }


        }

        //method that gets the size of the directory
        static long GetDirectorySize(string p)
        {
            // Get array of all file names.
            string[] a = Directory.GetFiles(p, "*.*", SearchOption.AllDirectories);

            // Calculate total bytes of all files in a loop.
            long b = 0;
            foreach (string name in a)
            {
                // Use FileInfo to get length of each file and add them together
                FileInfo info = new FileInfo(name);
                b += info.Length;
            }
            // Return total size
            return b;
        }
    }
    }

    