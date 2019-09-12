using System.Linq.Expressions;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

namespace UMA24.Manager
{
    public static class FileManager
    {
        static string filePath = @"tempNumber.txt";



        public static bool VerifyFile()
        {

            if (!File.Exists(filePath))
            {
                FileInfo file = new FileInfo(filePath);
                return true;
            }
            return false;
        }

        public static void AddNumberToFile(string number)
        {
            if (number != null && Contain(number) == false)
            {
                using (StreamWriter saveFile = File.AppendText(filePath))
                {
                    saveFile.WriteLine(number);

                }
            }

        }
        public static void RemoveNumberFromFile(string number)
        {

            List<string> tempFile = GetListFromFile();
            if (tempFile != null)
            {
                tempFile.Remove(number);
                ReWriteFile(tempFile);
            }
        }
        public static void ReWriteFile(List<string> tempFile)
        {
            using (StreamWriter saveFile = new StreamWriter(filePath))
            {
                foreach (var p in tempFile)
                {
                    saveFile.WriteLine(p);
                }
            }
        }

        public static List<string> GetNumberFromFile()
        {
            List<string> tempFile = GetListFromFile();
            if (tempFile  != null)
            {
                return tempFile;
            }
            else
            {
                return null;
            }
        }

        private static List<string> GetListFromFile()
        {
            FileInfo file = new FileInfo(filePath);
            

                if (file.Length > 0)
                {
                    return new List<string>(File.ReadLines(filePath));
                }
                else
                {
                    return null;
                }
            
        }

        private static bool Contain(string number)
        {
            List<string> tempFile = GetListFromFile();
            if (tempFile != null)
            {
                return tempFile.Contains(number);
            }
            else
            {
                return false;
            }
        }


    }
}
