using Microsoft.Deployment.Compression.Cab;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabPacker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter 'exit' to quit application.");
            Console.WriteLine("Packed files will be saved in the same directory.");
            string input = "";

            do
            {
                
                Console.WriteLine("Please insert directory name where files to pack are in:");

                input = Console.ReadLine();

                if (Directory.Exists(input))
                {
                    createCabFile(input, "InfoPathFile.xsn");
                }
            }
            while (input.ToLower() != "exit");
        }

        /// <summary>
        /// create a cab file from content of folder
        /// Save cab file into the same
        /// </summary>
        /// <param name="sourcefolder"></param>
        /// <param name="newFileName"></param>
        /// <returns></returns>
        private static string createCabFile(string sourcefolder, string newFileName)
        {
            //FileInfo fInfo = new FileInfo(oldFileName);

            //string directory = fInfo.DirectoryName;

            // path + fileName
            string newFileFullName = Path.Combine(sourcefolder, newFileName);

            CabInfo cab;

            bool isAscii = IsAscii(newFileName);

            // if only ascii characters in string then just pack file
            if (isAscii)
            {
                cab = new CabInfo(newFileFullName);
                cab.Pack(sourcefolder);
            }
            // if others than ascii characters are in filename, we need to pack to other file and then rename. CabInfo can't handle all unicode characters
            else
            {
                // use this is a temp file
                string tempCabFile = Path.Combine(sourcefolder, "TEMPCABFILE.xsn");

                cab = new CabInfo(tempCabFile);
                cab.Pack(sourcefolder);

                // delete already existing file
                if (File.Exists(newFileFullName))
                    File.Delete(newFileName);

                // move files to this location and rename (both happens in .Move)
                File.Move(tempCabFile, newFileFullName);
            }

            // return filepath of newly generated file
            return newFileFullName;
        }

        /// <summary>
        /// check if ascii character is in new FileName
        /// </summary>
        /// <param name="newFileName"></param>
        /// <returns></returns>
        private static bool IsAscii(string newFileName)
        {
            foreach (var c in newFileName)
            {
                if (c >= sbyte.MaxValue)
                    return false;
            }

            return true;
        }
    }
}
