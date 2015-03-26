using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace AprioriAlgorithm
{
	public class Transaction : ItemSet
	{
		private static int currentTID = 0;
		private int tid = 0;	// Transaction Identifier. (TID)

		public Transaction ()
		{
			++ currentTID;
			this.tid = currentTID;
		}

		new public String ToString()
		{
			return "Transaction ID: " + tid + "\t" + base.ToString();
		}

		public int MaxItem ()
		{
			int max = 0;
			foreach(int item in this)
			{
				if (max < item)
					max = item;
			}
			return max;
		}
	}
}