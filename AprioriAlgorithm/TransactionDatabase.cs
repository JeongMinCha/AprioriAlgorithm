using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace AprioriAlgorithm
{
	public class TransactionDatabase : List<Transaction>
	{
		private string inputFile;
		private string outputFile;
		private int maxItemNum = 0;
		private int transactionNum = 0;

		public TransactionDatabase (string inputFile, string outputFile)
		{
			this.inputFile = inputFile;
			this.outputFile = outputFile;
			ConstructTransactions(inputFile);

			Console.WriteLine("input file: " + inputFile);
			Console.WriteLine("output file: " + outputFile);
		}

		// Read Input file and each of lines in the input file
		// is added into list of transactions.
		private void ConstructTransactions (string inputFile)
		{
			string[] lines = File.ReadAllLines(inputFile);
			foreach (string line in lines)
			{
				Transaction t = new Transaction();
				// integers in each line is used to make a transaction
				string[] words = line.Split('\t');
				foreach(string word in words)
					t.Add(Convert.ToInt32(word));
				int curMaxItem = t.MaxItem();
				if (maxItemNum < curMaxItem)
					maxItemNum = curMaxItem;
				base.Add(t);
				++ transactionNum;
			}
		}

		/* Each candidates in input candidate item set table is incre
		 * */
		public void IncrementCandidatesIn (ItemSetTable candidateTable)
		{
			foreach(Transaction t in this)
			{
				ICollection<ItemSet> keys = new List<ItemSet>(candidateTable.Keys);
				foreach(ItemSet itemSet in keys)
				{
					if (t.IsSupersetOf(itemSet) == true)
						candidateTable.Increment(itemSet);
				}
			}
		}

		/* Returns C(1), the candidate item sets with size 1 */
		public ItemSetTable FindFirstCandidateItemSetTable()
		{
			ItemSetTable candidateItemSets = new ItemSetTable();
			candidateItemSets.order = 1;

			SortedSet<int> set = new SortedSet<int>();
			foreach (Transaction t in this)
			{
				foreach (int item in t)
					set.Add(item);
			}

			foreach (int item in set)
			{
				ItemSet itemSet = new ItemSet();
				itemSet.Add(item);
				candidateItemSets.Add(itemSet, 0);
			}

			IncrementCandidatesIn(candidateItemSets);
			return candidateItemSets;
		}

		/* Generate all association rules in itemSet. 
		 * itemSet = subSet U restSet, rule: subSet -> restSet */
		public void FindAssociationRules (ItemSet itemSet)
		{
			List<ItemSet> subsetList = itemSet.Subsets();
			double itemSetSupport = GetSupport(itemSet);
			double support = (double)(itemSetSupport / (double)transactionNum) * 100;

			/* Generate a rule 'subSet - > restSet' */
			foreach(ItemSet subSet in subsetList)
			{
				String str = "";
				ItemSet restSet = itemSet.Subtract(subSet);
				int subSetSupport = GetSupport (subSet);
				double confidence = (itemSetSupport/(double)subSetSupport)*100;

				str += subSet.ToString() + "\t" + restSet.ToString() + "\t";
				str += Math.Round(support, 2).ToString("N2") + "\t";
				str += Math.Round(confidence, 2).ToString("N2") + "\r\n";
//				Console.WriteLine(outputFile);
				File.AppendAllText(outputFile, str);
			}
		}

		public int GetSupport (ItemSet itemSet)
		{
			int support = 0;
			foreach (Transaction t in this)
			{
				if (t.IsSupersetOf(itemSet) == true)
					++support;
			}
			return support;
		}
			
	}
}