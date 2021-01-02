public class WinCalculation
{
	public int mMostRepeatedSymbol = 0, mRepetitionCount = 0;

	//Winlines pattern
	private static readonly int[][] mLines = 
	{
		new int[]{0,0,0,0,0},
		new int[]{1,1,1,1,1},
		new int[]{2,2,2,2,2},
	};

	/// <summary>
	/// Cals the Line Function to check win line
	/// </summary>
	/// <param name="View"></param>
	public void Views(int[][] View)
	{
		mMostRepeatedSymbol = mRepetitionCount = 0;
		LinesWin(View);
		//linesWin(this.mView);
	}

	/// <summary>
	/// Check Winning Lines on all 3 ReelViews
	/// </summary>
	/// <param name="aview"></param>
	void LinesWin(int[][] aview)
	{
		// Check wins in all possible lines.
		for (int l = 0; l < mLines.Length; l++)
		{
			
			int[] line = { -1, -1, -1,-1,-1};

			//Prepare line for combination check.
			for (int i = 0; i < line.Length; i++)
			{
				int index = mLines[l][i];
				line[i] = aview[i][index];
			}

			LineWin(line);
			// Accumulate line win. for multiple Winlines Win
			//win += result;
		}
	}

	/// <summary>
	/// Check Reels against every combination
	/// </summary>
	/// <param name="aline"></param>
	void LineWin(int[] aline)
	{
		//Keep first symbol in the line.
		int mSymbol = aline[0];

		//Count symbols in line.
		int mNumber = 0;
		for (int i = 0; i < aline.Length; i++)
		{
			if (aline[i] == -1)
			{
				break;
			}
				
			if (aline[i] == mSymbol)
			{
				mNumber++;
			}
			else
			{
				break;
			}

			if (mRepetitionCount < mNumber)
			{
				mMostRepeatedSymbol = mSymbol;
				mRepetitionCount = mNumber;
			}
		}
		//Cleare unused symbols.
		for (int i = mNumber; i < aline.Length; i++)
		{
			aline[i] = -1;
		}
	}
}
