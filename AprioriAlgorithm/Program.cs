using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace AprioriAlgorithm
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string inputFile = null;
			string outputFile = null;
			int minSupPercent = 0;
			int minSupport = 0;

			if (args.Length != 3)
			{
				PrintUsage(); 
			}
			try 
			{
				minSupPercent = Convert.ToInt32(args[0]);
				inputFile = String.Copy(args[1]);
				outputFile = String.Copy(args[2]);

				// If input file don't exist, throw exception.
				if (File.Exists(inputFile) == false)
					throw new FileNotFoundException();

				// If output file already exists, the file will be deleted.
				if (File.Exists(outputFile) == true)
					File.Delete(outputFile);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				PrintUsage();
			}

			TransactionDatabase database = new TransactionDatabase(inputFile, outputFile);
			ItemSetTable candidateTable = new ItemSetTable();
			ItemSetTable frequentTable = null;

			/* Printing input arguments.. */
			minSupport = minSupPercent * database.Count / 100;
			Console.WriteLine("minimum support: {0} %", minSupPercent);
			Console.WriteLine("minimum support: {0}", minSupport);
			ItemSetTable.SetMinSupport(minSupport);

			/* Build C(1), the candidate item sets with size 1 */
			candidateTable = database.FindFirstCandidateItemSetTable();

			/* Build L(1), the frequent item sets with size 1 */
			if (candidateTable != null)
				frequentTable = candidateTable.BuildFrequentItemSet();

			for (int k=1; frequentTable != null; ++k)
			{
				// C(k+1) = candidates generated from L(k)
				candidateTable = frequentTable.BuildCandidateItemSet();

				// for each transaction t in database do increment the count of 
				// all candidates in C(k+1) that are contained in t
				database.IncrementCandidatesIn(candidateTable);

				// L(k+1) = candidates in C(K+1) with min_support
				frequentTable = candidateTable.BuildFrequentItemSet();
				if (frequentTable != null)
				{
					foreach (ItemSet frequentItemSet in frequentTable.Keys)
						database.FindAssociationRules(frequentItemSet);
				}
			}
			Console.WriteLine("Finding Association Rules is finished.");
		}

		/* Print all item sets in input item set table */
		public static void Print(ItemSetTable ist)
		{
			if (ist != null)
			{
				Console.WriteLine("Order: {0}", ist.order);
				Console.WriteLine(ist.ToString());
			}
			else
				Console.WriteLine("don't exist");
		}

		public static void PrintUsage()
		{
			Console.WriteLine("Usage of this program: ");
			Console.WriteLine("apriori.exe [min support] [input file] [output file]");
			Console.WriteLine("[min support] should be an integer.");
			Console.WriteLine("[input file] and [output file] should be an existing file.");
			System.Environment.Exit(-1);
		}
	}
}
