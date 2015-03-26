using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace AprioriAlgorithm
{
	public class ItemSetTable : Dictionary<ItemSet, int>
	{
		// It holds set of item sets of size 'order'
		public int order { get; set; }
		// limit for determining frequent item set.
		public static int minSupport { get; set; }

		public ItemSetTable () : base()
		{
		}

		public ItemSetTable (int order)
		{
			this.order = order;
		}

		public static void SetMinSupport (int minSupport)
		{
			ItemSetTable.minSupport = minSupport;
		}

		public void Increment(ItemSet itemSet)
		{
			if (ContainsKey(itemSet))
				this[itemSet] = (int) this[itemSet] + 1;
			else
				Add(itemSet, 1);
		}

		new public String ToString()
		{
			String result = "";
			foreach(ItemSet itemSet in this.Keys)
				result += itemSet.ToString() + "\t" + this[itemSet] + "\r\n";
			return result;
		}

		/* Returns L(k+1), candidates in C(k+1) with min_support */
		public ItemSetTable BuildFrequentItemSet()
		{
			int count = 0;
			ItemSetTable frequentTable = new ItemSetTable(this.order);

			ICollection<ItemSet> keys = new List<ItemSet>(this.Keys);
			foreach(ItemSet itemSet in keys)
			{
				int support = this[itemSet];
//				Console.WriteLine(itemSet.ToString() + ", support: " + support);
				if (support >= minSupport)
				{
					frequentTable.Add(itemSet, support);
					++count;
				}
			}

			// If no item set has higher support than min_support, return null.
			if (count == 0)
				return null;
			else
				return frequentTable;
		}

		/* Returns the candidates C(k+1) generated from L(k) */
		public ItemSetTable BuildCandidateItemSet()
		{
			ItemSetTable candidateTable = new ItemSetTable(this.order + 1);
			foreach (ItemSet I1 in this.Keys)		// this = L(k)
			{
				foreach (ItemSet I2 in this.Keys)
				{
					ItemSet c = null;
					IList<int> iList1 = new List<int>(I1);
					IList<int> iList2 = new List<int>(I2);

					bool joinCondition = false;
					for (int i=0; i<this.order; ++i)
					{
						// last element
						if (i == this.order - 1)
						{
							if (iList1[i] < iList2[i])
								joinCondition = true;
						}
						else
						{
							if (iList1[i] != iList2[i])
							{
								joinCondition = false;
								break;
							}
						}
					}
					if (joinCondition == true)
					{
						c = ItemSet.UnionBetween(I1, I2);	// join step: generate candidates
//						Console.WriteLine("join! " + c.ToString());
						try 
						{
							if (hasInfrequentSubSet(c) == false)
							{
//								Console.WriteLine("input: " + c.ToString() + "<- " + I1.ToString() + ", " + I2.ToString());
								candidateTable.Add(c, 0);
							}
						} 
						catch (Exception e)
						{
							Console.WriteLine(e.Message);
							Console.WriteLine("exception!");
							Console.WriteLine(c.ToString()) ;
							Console.WriteLine("!!!-> " + I1.ToString() + I2.ToString());
//							Console.WriteLine(candidateTable.ToString());
							System.Environment.Exit(-1);
						}
					}
				}
			}
			return candidateTable;
		}

		/* the method called 'has_infrequent_subset' in the text book. (3rd, 253p) */
		// this = L(k), c = candidate item set with size (k+1)
		public bool hasInfrequentSubSet (ItemSet c)
		{
			foreach (ItemSet s in c.Subsets_k_1())
			{
				// 'this' is frequent itemsets of size k, If 'this' doesn't
				//  contain a subset of C(k+1), C(k+1) has_infrequent_subset!
				ICollection<ItemSet> keys = new List<ItemSet>(this.Keys);
				if (keys.Contains(s) == false)
					return true;
			}
			return false;
		}
	}
}