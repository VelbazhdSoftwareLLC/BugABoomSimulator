﻿/*******************************************************************************
* Bug A Boom Slot Simulation version 0.9.1                                     *
* Copyrights (C) 2014-2023 Velbazhd Software LLC                               *
*                                                                              *
* developed by Todor Balabanov ( todor.balabanov@gmail.com )                   *
* Sofia, Bulgaria                                                              *
*                                                                              *
* This program is free software: you can redistribute it and/or modify         *
* it under the terms of the GNU General Public License as published by         *
* the Free Software Foundation, either version 3 of the License, or            *
* (at your option) any later version.                                          *
*                                                                              *
* This program is distributed in the hope that it will be useful,              *
* but WITHOUT ANY WARRANTY; without even the implied warranty of               *
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the                *
* GNU General Public License for more details.                                 *
*                                                                              *
* You should have received a copy of the GNU General Public License            *
* along with this program. If not, see <http://www.gnu.org/licenses/>.         *
*                                                                              *
*******************************************************************************/

using System;

namespace CSharpSimulation
{
	/**
	* Main application class.
	*/
	class MainClass
	{
		/** Pseudo-random number generator. */
		private static Random prng = new Random ();

		/** List of symbols names. */
		private static String [] symbolsNames = {
			"SHIRT   ",
			"SPEAKER ",
			"MIC     ",
			"AIRPLANE",
			"MONEY   ",
			"MAGAZINE",
			"DRUM    ",
			"BASS    ",
			"SOLO    ",
			"VOCAL   ",
			"POSTER  ",
			"WILD    ",
			"SCATTER ",
		};

		/** Slot game paytable. */
		private static int [] [] paytable = {
			new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new int[]{0,0,0,0,0,0,0,0,2,2,2,10,2},
			new int[]{5,5,5,10,10,10,15,15,25,25,50,250,5},
			new int[]{25,25,25,50,50,50,75,75,125,125,250,2500,0},
			new int[]{125,125,125,250,250,250,500,500,750,750,1250,10000,0},
		};

		/** Lines combinations. */
		private static int [] [] lines = {
			new int[]{1,1,1,1,1},
			new int[]{0,0,0,0,0},
			new int[]{2,2,2,2,2},
			new int[]{0,1,2,1,0},
			new int[]{2,1,0,1,2},
			new int[]{0,0,1,2,2},
			new int[]{2,2,1,0,0},
			new int[]{1,0,1,2,1},
			new int[]{1,2,1,0,1},
			new int[]{1,0,0,1,0},
			new int[]{1,2,2,1,2},
			new int[]{0,1,0,0,1},
			new int[]{2,1,2,2,1},
			new int[]{0,2,0,2,0},
			new int[]{2,0,2,0,2},
			new int[]{1,0,2,0,1},
			new int[]{1,2,0,2,1},
			new int[]{0,1,1,1,0},
			new int[]{2,1,1,1,2},
			new int[]{0,2,2,2,0},
		};

		/** Current visible symbols on the screen. */
		private static int [] [] view = {
			new int[]{ -1, -1, -1 },
			new int[]{ -1, -1, -1 },
			new int[]{ -1, -1, -1 },
			new int[]{ -1, -1, -1 },
			new int[]{ -1, -1, -1 }
		};

		/** Stips in base game. */
		private static int [] [] baseReels = {
			new int[]{0,4,11,1,3,2,5,9,0,4,2,7,8,0,5,2,6,10,0,5,1,3,9,4,2,7,8,0,5,2,6,9,0,5,2,4,10,0,5,1,7,9,2,5},
			new int[]{4,1,11,2,7,0,9,5,1,3,8,4,2,6,12,4,0,3,1,8,4,2,6,0,10,4,1,3,2,12,4,0,7,1,8,2,4,0,9,1,6,2,8,0},
			new int[]{1,7,11,5,1,7,8,6,0,3,12,4,1,6,9,5,2,7,10,1,3,2,8,1,3,0,9,5,1,3,10,6,0,3,8,7,1,6,12,3,2,5,9,3},
			new int[]{5,2,11,3,0,6,1,5,12,2,4,0,10,3,1,7,3,2,11,5,4,6,0,5,12,1,3,7,2,4,8,0,3,6,1,4,12,2,5,7,0,4,9,1},
			new int[]{7,0,11,4,6,1,9,5,10,2,7,3,8,0,4,9,1,6,5,10,2,8,3},
		};

		/** Stips in free spins. */
		private static int [] [] freeReels = {
			new int[]{2,4,11,0,3,7,1,4,8,2,5,6,0,5,9,1,3,7,2,4,10,0,3,1,8,4,2,5,6,0,4,1,10,5,2,3,7,0,5,9,1,3,6},
			new int[]{4,2,11,0,5,2,12,1,7,0,9,2,3,0,12,2,4,0,5,8,2,6,0,12,2,7,1,3,10,6,0},
			new int[]{1,4,11,2,7,8,1,5,12,0,3,9,1,7,8,1,5,12,2,6,10,1,4,9,3,1,8,0,12,6,9},
			new int[]{6,4,11,2,7,3,9,1,6,5,12,0,4,10,2,3,8,1,7,5,12,0},
			new int[]{3,4,11,0,6,5,3,8,1,7,4,9,2,5,10,0,3,8,1,4,10,2,5,9},
		};

		/** Slices in base game. */
		private static int [] [] [] baseSlicesReels = { };

		/** Slices in free spins. */
		private static int [] [] [] freeSlicesReels = { };

		/** Use reels stops in brute force combinations generation. */
		private static int [] reelsStops = new int [] { 0, 0, 0, 0, 0 };

		/** Current free spins multiplier. */
		private static int freeGamesMultiplier = 0;

		/** If wild is presented in the line multiplier. */
		private static int wildInLineMultiplier = 0;

		/** If scatter win is presented on the screen. */
		private static int scatterMultiplier = 1;

		/** Total bet in single base game spin. */
		private static int singleLineBet = 1;

		/** Total bet in single base game spin. */
		private static int totalBet = singleLineBet * lines.Length;

		/** Free spins to be played. */
		private static int freeGamesNumber = 0;

		/** Total amount of won money. */
		private static long wonMoney = 0L;

		/** Total amount of lost money. */
		private static long lostMoney = 0L;

		/** Total amount of won money in base game. */
		private static long baseMoney = 0L;

		/** Total amount of won money in free spins. */
		private static long freeMoney = 0L;

		/** Max amount of won money in base game. */
		private static long baseMaxWin = 0L;

		/** Max amount of won money in free spins. */
		private static long freeMaxWin = 0L;

		/** Total number of base games played. */
		private static long totalNumberOfGames = 0L;

		/** Total number of free spins played. */
		private static long totalNumberOfFreeGames = 0L;

		/** Total number of free spins started. */
		private static long totalNumberOfFreeGameStarts = 0L;

		/** Total number of free spins started. */
		private static long totalNumberOfFreeGameRestarts = 0L;

		/** Hit rate of wins in base game. */
		private static long baseGameHitRate = 0L;

		/** Hit rate of wins in free spins. */
		private static long freeGamesHitRate = 0L;

		/** Verbose output flag. */
		private static bool verboseOutput = false;

		/** Free spins flag. */
		private static bool freeOff = false;

		/** Wild substitution flag. */
		private static bool wildsOff = false;

		/** Brute force all winning combinations in base game only flag. */
		private static bool bruteForce = false;

		/** Volatility estimation flag. */
		private static bool volatilityEstimation = false;

		/** Confidence interval, from 0.0 to 1.0. */
		private static double confidenceInterval = 0.95;

		/** Symbols win hit rate in base game. */
		private static long [] [] baseSymbolMoney = {
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0}
		};

		/** Symbols hit rate in base game. */
		private static long [] [] baseGameSymbolsHitRate = {
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0}
		};

		/** Symbols win hit rate in base game. */
		private static long [] [] freeSymbolMoney = {
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0}
		};

		/** Symbols hit rate in base game. */
		private static long [] [] freeGameSymbolsHitRate = {
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0}
		};

		/** RTP calculated for volatility index calculation. */
		private static double volatilityRTP = 0;

		/** Sum collected for volatility index calculation. */
		private static double volatilitySum = 0;

		/** Particular win payment. */
		private static double [] [] Pi = {
			new double[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new double[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new double[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new double[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new double[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new double[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
		};

		/** Particular win hit rate. */
		private static double [] [] Hi = {
			new double[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new double[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new double[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new double[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new double[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new double[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
		};

		/** Slice reels flag. */
		private static bool slicesReels = false;

		/** Sum collected for volatility index calculation. */
		private static int sampleSize = 100;

		/**
		 * Static constructor for discrete distributions shuffling.
		 */
		static MainClass ()
		{
		}

		/**
		 * Slice reels for estimated statistics.
		 *
		 * @param reels Original reels.
		 *
		 * @param view Screen view.
		 *
		 * @param size Sample size.
		 */
		private static int [] [] [] sliceReels (int [] [] reels, int [] [] view, int size)
		{
			int [] [] [] slices = new int [reels.Length] [] [];
			for (int i = 0; i < reels.Length; i++) {
				slices [i] = new int [size] [];
				for (int j = 0; j < size; j++) {
					slices [i] [j] = new int [view [i].Length];

					int r = prng.Next (reels [i].Length);
					for (int k = 0; k < slices [i] [j].Length; k++) {
						slices [i] [j] [k] = reels [i] [r];

						r++;
						if (r == reels [i].Length) {
							r = 0;
						}
					}
				}
			}

			return slices;
		}

		/**
		 * Single reels spin to fill view with symbols.
		 *
		 * @param reels Reels strips.
		 */
		private static void nextCombination (int [] stops)
		{
			stops [0] += 1;
			for (int i = 0; i < stops.Length; i++) {
				if (stops [i] >= baseReels [i].Length) {
					stops [i] = 0;
					if (i < stops.Length - 1) {
						stops [i + 1] += 1;
					}
				}
			}
		}

		/**
		 * Single reels spin to fill view with symbols.
		 *
		 * @param reels Reels strips.
		 */
		private static void spin (int [] [] reels)
		{
			for (int i = 0, r, u, d; i < view.Length && i < reels.Length; i++) {
				if (bruteForce == true) {
					u = reelsStops [i];
					r = u + 1;
					d = u + 2;
				} else {
					u = prng.Next (reels [i].Length);
					r = u + 1;
					d = u + 2;
				}

				r = r % reels [i].Length;
				d = d % reels [i].Length;

				view [i] [0] = reels [i] [u];
				view [i] [1] = reels [i] [r];
				view [i] [2] = reels [i] [d];
			}
		}

		/**
		 * Single reels spin to fill view with symbols.
		 *
		 * @param reels Reels slices.
		 */
		private static void spin (int [] [] [] reels)
		{
			for (int i = 0; i < view.Length && i < reels.Length; i++) {
				int r = prng.Next (reels [i].Length);
				view [i] [0] = reels [i] [r] [0];
				view [i] [1] = reels [i] [r] [1];
				view [i] [2] = reels [i] [r] [2];
			}
		}

		/**
		 * Calculate win in particular line.
		 *
		 * @param line Single line.
		 *
		 * @return Calculated win.
		 */
		private static int [] wildLineWin (int [] line)
		{
			int [] values = { 11, 0, 0 };

			/* If there is no leading wild there is no wild win. */
			if (line [0] != values [0]) {
				return (values);
			}

			/* Wild symbol passing to find first regular symbol. */
			for (int i = 0; i < line.Length; i++) {
				/* First no wild symbol found. */
				if (line [i] != values [0]) {
					break;
				}

				values [1]++;
			}

			values [2] = singleLineBet * paytable [values [1]] [values [0]];

			return (values);
		}

		/**
		 * Calculate win in particular line.
		 *
		 * @param line Single line.
		 *
		 * @return Calculated win.
		 */
		private static int lineWin (int [] line)
		{
			int [] wildWin = wildLineWin (line);

			/* Line win without wild is multiplied by one. */
			wildInLineMultiplier = 1;

			/* Keep first symbol in the line. */
			int symbol = line [0];

			/* Wild symbol passing to find first regular symbol. */
			for (int i = 0; i < line.Length; i++) {
				/* First no wild symbol found. */
				if (line [i] != 11) {
					if (line [i] != 12) {
						symbol = line [i];
					}
					break;
				}

				if (line [i] == 12) {
					break;
				}

				/* Line win with wild is multiplied by two. */
				if (i < line.Length - 1) {
					wildInLineMultiplier = 2;
				} else if (i == line.Length - 1) {
					/* Line win with five wilds is multiplied by one. */
					wildInLineMultiplier = 1;
				}
			}

			/* Wild symbol substitution. Other wild are artificial they are not part of the pay table. */
			for (int i = 0; i < line.Length && wildsOff == false; i++) {
				if (line [i] == 11) {
					/* Substitute wild with regular symbol. */
					line [i] = symbol;

					/* Line win with wild is multiplied by two. */
					wildInLineMultiplier = 2;
				}
			}

			/* Count symbols in winning line. */
			int number = 0;
			for (int i = 0; i < line.Length; i++) {
				if (line [i] == symbol) {
					number++;
				} else {
					break;
				}
			}

			/* Cleare unused symbols. */
			for (int i = number; i < line.Length; i++) {
				line [i] = -1;
			}

			int win = singleLineBet * paytable [number] [symbol] * wildInLineMultiplier;
			if (win < wildWin [2]) {
				symbol = wildWin [0];
				number = wildWin [1];
				win = wildWin [2];
			}

			if (win > 0 && freeGamesNumber == 0) {
				baseSymbolMoney [number] [symbol] += win;
				baseGameSymbolsHitRate [number] [symbol]++;
			} else if (win > 0 && freeGamesNumber > 0) {
				freeSymbolMoney [number] [symbol] += win * freeGamesMultiplier;
				freeGameSymbolsHitRate [number] [symbol]++;
			}

			/* Collect information for the volatility index. */
			if (volatilityEstimation == true) {
				volatilitySum += Hi [number] [symbol] * (win - volatilityRTP) * (win - volatilityRTP);
			}

			return (win);
		}

		/**
		 * Calculate win in all possible lines.
		 *
		 * @param view Symbols visible in screen view.
		 *
		 * @return Calculated win.
		 */
		private static int linesWin (int [] [] view)
		{
			int win = 0;

			/* Check wins in all possible lines. */
			for (int l = 0; l < lines.Length; l++) {
				int [] line = { -1, -1, -1, -1, -1 };

				/* Prepare line for combination check. */
				for (int i = 0; i < line.Length; i++) {
					int index = lines [l] [i];
					line [i] = view [i] [index];
				}

				int result = lineWin (line);

				/* Accumulate line win. */
				win += result;

				/* Volatility is calculated only on a single line. */
				if (volatilityEstimation == true && l == 0) {
					break;
				}
			}

			return (win);
		}

		/**
		 * Calculate win from scatters.
		 *
		 * @return Win from scatters.
		 */
		private static int scatterWin (int [] [] view)
		{
			int numberOfScatters = 0;
			for (int i = 0; i < view.Length; i++) {
				for (int j = 0; j < view [i].Length; j++) {
					if (view [i] [j] == 12) {
						numberOfScatters++;
					}
				}
			}

			int win = paytable [numberOfScatters] [12] * totalBet * scatterMultiplier;

			if (win > 0 && freeGamesNumber == 0) {
				baseSymbolMoney [numberOfScatters] [12] += win;
				baseGameSymbolsHitRate [numberOfScatters] [12]++;
			} else if (win > 0 && freeGamesNumber > 0) {
				freeSymbolMoney [numberOfScatters] [12] += win * freeGamesMultiplier;
				freeGameSymbolsHitRate [numberOfScatters] [12]++;
			}

			/* Collect information for the volatility index. */
			if (volatilityEstimation == true) {
				volatilitySum += Hi [numberOfScatters] [12] * (win - volatilityRTP) * (win - volatilityRTP);
			}

			return (win);
		}

		/**
		 * Setup parameters for free spins mode.
		 */
		private static void freeGamesSetup ()
		{
			if (bruteForce == true) {
				return;
			}

			if (freeOff == true) {
				return;
			}

			int numberOfScatters = 0;
			for (int i = 0; i < view.Length; i++) {
				for (int j = 0; j < view [i].Length; j++) {
					if (view [i] [j] == 12) {
						numberOfScatters++;
					}
				}
			}

			/* In base game 3+ scatters turn into free spins. */
			if (numberOfScatters < 3 && freeGamesNumber == 0) {
				return;
			} else if (numberOfScatters >= 3 && freeGamesNumber == 0) {
				freeGamesNumber = 15;
				freeGamesMultiplier = 4;
				totalNumberOfFreeGameStarts++;
			} else if (numberOfScatters >= 3 && freeGamesNumber > 0) {
				freeGamesNumber += 15;
				freeGamesMultiplier = 4;
				totalNumberOfFreeGameRestarts++;
			}
		}

		/**
		 * Play single free spin game.
		 */
		private static void singleFreeGame ()
		{
			if (bruteForce == true) {
				return;
			}

			if (freeOff == true) {
				return;
			}

			/* Spin reels. In retriggered games from FS1 to FS2 and from FS2 to FS3. FS3 can not rettriger FS. */
			if (slicesReels == false) {
				spin (freeReels);
			} else if (slicesReels == true) {
				spin (freeSlicesReels);
			}

			/* Win accumulated by lines. */
			int win = linesWin (view) + scatterWin (view);
			win *= freeGamesMultiplier;

			/* Add win to the statistics. */
			freeMoney += win;
			wonMoney += win;
			if (freeMaxWin < win) {
				freeMaxWin = win;
			}

			/* Count free games hit rate. */
			if (win > 0) {
				freeGamesHitRate++;
			}

			/* Check for free games. */
			freeGamesSetup ();
		}

		/**
		 * Play single base game.
		 */
		private static void singleBaseGame ()
		{
			/* Spin reels. */
			if (slicesReels == false) {
				spin (baseReels);
			} else if (slicesReels == true) {
				spin (baseSlicesReels);
			}

			if (bruteForce == true) {
				nextCombination (reelsStops);
			}

			/* Win accumulated by lines. */
			int win = linesWin (view) + scatterWin (view);

			/* Add win to the statistics. */
			baseMoney += win;
			wonMoney += win;
			if (baseMaxWin < win) {
				baseMaxWin = win;
			}

			/* Count base game hit rate. */
			if (win > 0) {
				baseGameHitRate++;
			}

			/* Volatility is estimated only for the base game. */
			if (volatilityEstimation == true) {
				return;
			}

			/* Check for free games. */
			freeGamesSetup ();

			/* Play all free games. */
			while (freeGamesNumber > 0) {
				totalNumberOfFreeGames++;

				singleFreeGame ();

				freeGamesNumber--;
			}
			freeGamesMultiplier = 1;
		}

		/**
		 * Simulation with full reels.
		 *
		 * @param numberOfSimulations Total number of game runs.
		 *
		 * @param progressPrintOnIteration Interval for intermediate progress report.
		 */
		static void fullSimulation (long numberOfSimulations, long progressPrintOnIteration)
		{
			for (long g = 0L; g < numberOfSimulations; g++) {
				if (verboseOutput == true && g == 0) {
					Console.WriteLine ("Games\tRTP\tRTP(Base)\tRTP(Free)");
				}

				/* Print progress report. */
				if (verboseOutput == true && g % progressPrintOnIteration == 0) {
					try {
						Console.Write (g);
						Console.Write ("\t");
						Console.Write (String.Format ("  {0:F6}", ((double)wonMoney / (double)lostMoney)));
						Console.Write ("\t");
						Console.Write (String.Format ("  {0:F6}", ((double)baseMoney / (double)lostMoney)));
						Console.Write ("\t");
						Console.Write (String.Format ("  {0:F6}", ((double)freeMoney / (double)lostMoney)));
					} catch (Exception) {
					}
					Console.WriteLine ();
				}

				totalNumberOfGames++;

				lostMoney += totalBet;

				singleBaseGame ();
			}

			Console.WriteLine ("********************************************************************************");
			printStatistics ();
			Console.WriteLine ("********************************************************************************");
		}

		/**
		 * Volatility partial simulation.
		 *
		 * @param numberOfSimulations Total number of game runs.
		 */
		static void volatilitySimulation (long numberOfSimulations)
		{
			/* Collect volatility statistics. */
			wonMoney = 0;
			baseMoney = 0;
			lostMoney = 0;
			volatilitySum = 0;
			totalNumberOfGames = 0;
			baseSymbolMoney = new long [] []{
					new long [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
					new long [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
					new long [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
					new long [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
					new long [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
					new long [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
				};
			baseGameSymbolsHitRate = new long [] []{
					new long [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
					new long [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
					new long [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
					new long [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
					new long [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
					new long [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
				};

			/* Collect base statistics. */
			for (long g = 0L; g < numberOfSimulations; g++) {
				totalNumberOfGames++;
				lostMoney += totalBet;
				singleBaseGame ();
			}

			/* Calculate hit rate and RTP. */
			volatilityRTP = (double)wonMoney / (double)lostMoney;
			for (int i = 0; i < paytable.Length; i++) {
				for (int j = 0; j < paytable [i].Length; j++) {
					Pi [i] [j] = paytable [i] [j];
					Hi [i] [j] = (double)baseGameSymbolsHitRate [i] [j] / (double)totalNumberOfGames;
				}
			}

			/* Collect volatility statistics. */
			wonMoney = 0;
			baseMoney = 0;
			lostMoney = 0;
			volatilitySum = 0;
			totalNumberOfGames = 0;
			baseSymbolMoney = new long [] []{
					new long [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
					new long [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
					new long [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
					new long [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
					new long [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
					new long [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
				};
			baseGameSymbolsHitRate = new long [] []{
					new long [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
					new long [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
					new long [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
					new long [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
					new long [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
					new long [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
				};

			for (long g = 0L; g < numberOfSimulations; g++) {
				totalNumberOfGames++;
				lostMoney += totalBet;
				singleBaseGame ();
			}

			double N = totalNumberOfGames;
			double C = confidenceInterval;
			double R = (double)wonMoney / (double)lostMoney;
			double V = C * Math.Sqrt (volatilitySum / N);

			Console.WriteLine ("********************************************************************************");
			Console.WriteLine (volatilityRTP);
			Console.WriteLine (R);
			Console.WriteLine (V);
			Console.WriteLine ("********************************************************************************");
			printStatistics ();
			Console.WriteLine ("********************************************************************************");
		}

		/**
		 * Print help information.
		 */
		private static void printHelp ()
		{
			Console.WriteLine ("*******************************************************************************");
			Console.WriteLine ("* Bug A Boom Slot Simulation version 0.9.1                                    *");
			Console.WriteLine ("* Copyrights (C) 2014-2023 Velbazhd Software LLC                              *");
			Console.WriteLine ("*                                                                             *");
			Console.WriteLine ("* developed by Todor Balabanov ( todor.balabanov@gmail.com )                  *");
			Console.WriteLine ("* Sofia, Bulgaria                                                             *");
			Console.WriteLine ("*                                                                             *");
			Console.WriteLine ("* This program is free software: you can redistribute it and/or modify        *");
			Console.WriteLine ("* it under the terms of the GNU General Public License as published by        *");
			Console.WriteLine ("* the Free Software Foundation, either version 3 of the License, or           *");
			Console.WriteLine ("* (at your option) any later version.                                         *");
			Console.WriteLine ("*                                                                             *");
			Console.WriteLine ("* This program is distributed in the hope that it will be useful,             *");
			Console.WriteLine ("* but WITHOUT ANY WARRANTY; without even the implied warranty of              *");
			Console.WriteLine ("* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the               *");
			Console.WriteLine ("* GNU General Public License for more details.                                *");
			Console.WriteLine ("*                                                                             *");
			Console.WriteLine ("* You should have received a copy of the GNU General Public License           *");
			Console.WriteLine ("* along with this program. If not, see <http://www.gnu.org/licenses/>.        *");
			Console.WriteLine ("*                                                                             *");
			Console.WriteLine ("*******************************************************************************");
			Console.WriteLine ("*                                                                             *");
			Console.WriteLine ("* -h              Help screen.                                                *");
			Console.WriteLine ("* -help           Help screen.                                                *");
			Console.WriteLine ("*                                                                             *");
			Console.WriteLine ("* -g<number>      Number of games (default 10 000 000).                       *");
			Console.WriteLine ("* -p<number>      Progress on each iteration number (default 10 000 000).     *");
			Console.WriteLine ("*                                                                             *");
			Console.WriteLine ("* -freeoff        Switch off free spins.                                      *");
			Console.WriteLine ("* -wildsoff       Switch off wilds.                                           *");
			Console.WriteLine ("* -bruteforce     Switch on brute force only for the base game.               *");
			Console.WriteLine ("*                                                                             *");
			Console.WriteLine ("* -volatility     Volatility calculation instead of simulation.               *");
			Console.WriteLine ("* -ci<number>     Confidence interval (default 0.95).                         *");
			Console.WriteLine ("*                                                                             *");
			Console.WriteLine ("* -slices         Volatility calculation instead of simulation.               *");
			Console.WriteLine ("* -s<number>      Slices sample size (default 100).                           *");
			Console.WriteLine ("*                                                                             *");
			Console.WriteLine ("* -verify         Print input data structures.                                *");
			Console.WriteLine ("*                                                                             *");
			Console.WriteLine ("*******************************************************************************");
		}

		/**
		 * Print all simulation input data structures.
		 */
		private static void printDataStructures ()
		{
			Console.WriteLine ("Paytable:");
			for (int i = 0; i < paytable.Length; i++) {
				Console.Write ("\t" + i + " of");
			}
			Console.WriteLine ();
			for (int j = 0; j < paytable [0].Length; j++) {
				Console.Write (symbolsNames [j] + "\t");
				for (int i = 0; i < paytable.Length; i++) {
					Console.Write (paytable [i] [j] + "\t");
				}
				Console.WriteLine ();
			}
			Console.WriteLine ();

			Console.WriteLine ("Lines:");
			for (int i = 0; i < lines.Length; i++) {
				for (int j = 0; j < lines [0].Length; j++) {
					Console.Write (lines [i] [j] + " ");
				}
				Console.WriteLine ();
			}
			Console.WriteLine ();

			Console.WriteLine ("Base Game Reels:");
			for (int i = 0; i < baseReels.Length; i++) {
				for (int j = 0; j < baseReels [i].Length; j++) {
					if (j % 10 == 0) {
						Console.WriteLine ();
					}
					Console.Write (symbolsNames [baseReels [i] [j]] + " ");
				}
				Console.WriteLine ();
			}
			Console.WriteLine ();

			Console.WriteLine ("Free Games Reels:");
			for (int i = 0; i < freeReels.Length; i++) {
				for (int j = 0; j < freeReels [i].Length; j++) {
					if (j % 10 == 0) {
						Console.WriteLine ();
					}
					Console.Write (symbolsNames [freeReels [i] [j]] + " ");
				}
				Console.WriteLine ();
			}
			Console.WriteLine ();

			Console.WriteLine ("Base Game Reels:");
			/* Count symbols in reels. */
			{
				int [] [] counters = {
					new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
					new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
					new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
					new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
					new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
				};
				for (int i = 0; i < baseReels.Length; i++) {
					for (int j = 0; j < baseReels [i].Length; j++) {
						counters [i] [baseReels [i] [j]]++;
					}
				}
				for (int i = 0; i < baseReels.Length; i++) {
					Console.Write ("\tReel " + (i + 1));
				}
				Console.WriteLine ();
				for (int j = 0; j < counters [0].Length; j++) {
					Console.Write (symbolsNames [j] + "\t");
					for (int i = 0; i < counters.Length; i++) {
						Console.Write (counters [i] [j] + "\t");
					}
					Console.WriteLine ();
				}
				Console.WriteLine ("---------------------------------------------");
				Console.Write ("Total:\t");
				long combinations = 1L;
				for (int i = 0; i < counters.Length; i++) {
					int sum = 0;
					for (int j = 0; j < counters [0].Length; j++) {
						sum += counters [i] [j];
					}
					Console.Write (sum + "\t");
					if (sum != 0) {
						combinations *= sum;
					}
				}
				Console.WriteLine ();
				Console.WriteLine ("---------------------------------------------");
				Console.WriteLine ("Combinations:\t" + combinations);
			}
			Console.WriteLine ();

			Console.WriteLine ("Free Games Reels:");
			/* Count symbols in reels. */
			{
				int [] [] counters = {
					new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
					new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
					new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
					new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
					new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
				};
				for (int i = 0; i < freeReels.Length; i++) {
					for (int j = 0; j < freeReels [i].Length; j++) {
						counters [i] [freeReels [i] [j]]++;
					}
				}
				for (int i = 0; i < freeReels.Length; i++) {
					Console.Write ("\tReel " + (i + 1));
				}
				Console.WriteLine ();
				for (int j = 0; j < counters [0].Length; j++) {
					Console.Write (symbolsNames [j] + "\t");
					for (int i = 0; i < counters.Length; i++) {
						Console.Write (counters [i] [j] + "\t");
					}
					Console.WriteLine ();
				}
				Console.WriteLine ("---------------------------------------------");
				Console.Write ("Total:\t");
				long combinations = 1L;
				for (int i = 0; i < counters.Length; i++) {
					int sum = 0;
					for (int j = 0; j < counters [0].Length; j++) {
						sum += counters [i] [j];
					}
					Console.Write (sum + "\t");
					if (sum != 0) {
						combinations *= sum;
					}
				}
				Console.WriteLine ();
				Console.WriteLine ("---------------------------------------------");
				Console.WriteLine ("Combinations:\t" + combinations);
			}
			Console.WriteLine ();
		}

		/**
		 * Print simulation statistics.
		 */
		private static void printStatistics ()
		{
			Console.WriteLine ("Won money:\t" + wonMoney);
			Console.WriteLine ("Lost money:\t" + lostMoney);
			Console.WriteLine ("Total Number of Games:\t" + totalNumberOfGames);
			Console.WriteLine ();
			Console.WriteLine ("Total RTP:\t" + ((double)wonMoney / (double)lostMoney) + "\t\t" + (100.0D * (double)wonMoney / (double)lostMoney) + "%");
			Console.WriteLine ("Base Game RTP:\t" + ((double)baseMoney / (double)lostMoney) + "\t\t" + (100.0D * (double)baseMoney / (double)lostMoney) + "%");
			Console.WriteLine ("Free Game RTP:\t" + ((double)freeMoney / (double)lostMoney) + "\t\t" + (100.0D * (double)freeMoney / (double)lostMoney) + "%");
			Console.WriteLine ();
			Console.WriteLine ("Hit Frequency in Base Game:\t" + ((double)baseGameHitRate / (double)totalNumberOfGames) + "\t\t" + (100.0D * (double)baseGameHitRate / (double)totalNumberOfGames) + "%");
			Console.WriteLine ("Hit Frequency in Free Game:\t" + ((double)freeGamesHitRate / (double)totalNumberOfFreeGames) + "\t\t" + (100.0D * (double)freeGamesHitRate / (double)totalNumberOfFreeGames) + "%");
			Console.WriteLine ("Hit Frequency Base Game into Free Game:\t" + ((double)totalNumberOfFreeGameStarts / (double)totalNumberOfGames) + "\t\t" + (100.0D * (double)(totalNumberOfFreeGameStarts) / (double)totalNumberOfGames) + "%");
			Console.WriteLine ("Hit Frequency Free Game into Free Game:\t" + ((double)totalNumberOfFreeGameRestarts / (double)totalNumberOfFreeGameStarts) + "\t\t" + (100.0D * (double)(totalNumberOfFreeGameRestarts) / (double)totalNumberOfFreeGameStarts) + "%");
			Console.WriteLine ();
			Console.WriteLine ("Max Win in Base Game:\t" + baseMaxWin);
			Console.WriteLine ("Max Win in Free Game:\t" + freeMaxWin);

			Console.WriteLine ();
			Console.WriteLine ();
			Console.WriteLine ("Base Game Symbols RTP:");
			Console.Write ("\t");
			for (int i = 0; i < baseSymbolMoney.Length; i++) {
				Console.Write ("" + i + "of\t");
			}
			Console.WriteLine ();
			for (int j = 0; j < baseSymbolMoney [0].Length; j++) {
				Console.Write (symbolsNames [j] + "\t");
				for (int i = 0; i < baseSymbolMoney.Length; i++) {
					Console.Write ((double)baseSymbolMoney [i] [j] / (double)lostMoney + "\t");
				}
				Console.WriteLine ();
			}
			Console.WriteLine ();
			Console.WriteLine ("Base Game Symbols Hit Rate:");
			Console.Write ("\t");
			for (int i = 0; i < baseGameSymbolsHitRate.Length; i++) {
				Console.Write ("" + i + "of\t");
			}
			Console.WriteLine ();
			for (int j = 0; j < baseGameSymbolsHitRate [0].Length; j++) {
				Console.Write (symbolsNames [j] + "\t");
				for (int i = 0; i < baseGameSymbolsHitRate.Length; i++) {
					Console.Write ((double)baseGameSymbolsHitRate [i] [j] + "\t");
				}
				Console.WriteLine ();
			}
			Console.WriteLine ();
			Console.WriteLine ("Base Game Symbols Hit Frequency:");
			Console.Write ("\t");
			for (int i = 0; i < baseGameSymbolsHitRate.Length; i++) {
				Console.Write ("" + i + "of\t");
			}
			Console.WriteLine ();
			for (int j = 0; j < baseGameSymbolsHitRate [0].Length; j++) {
				Console.Write (symbolsNames [j] + "\t");
				for (int i = 0; i < baseGameSymbolsHitRate.Length; i++) {
					Console.Write ((double)baseGameSymbolsHitRate [i] [j] / (double)totalNumberOfGames + "\t");
				}
				Console.WriteLine ();
			}

			Console.WriteLine ();
			Console.WriteLine ("Free Games Symbols RTP:");
			Console.Write ("\t");
			for (int i = 0; i < freeSymbolMoney.Length; i++) {
				Console.Write ("" + i + "of\t");
			}
			Console.WriteLine ();
			for (int j = 0; j < freeSymbolMoney [0].Length; j++) {
				Console.Write (symbolsNames [j] + "\t");
				for (int i = 0; i < freeSymbolMoney.Length; i++) {
					Console.Write ((double)freeSymbolMoney [i] [j] / (double)lostMoney + "\t");
				}
				Console.WriteLine ();
			}
			Console.WriteLine ();
			Console.WriteLine ("Free Games Symbols Hit Frequency:");
			Console.Write ("\t");
			for (int i = 0; i < freeGameSymbolsHitRate.Length; i++) {
				Console.Write ("" + i + "of\t");
			}
			Console.WriteLine ();
			for (int j = 0; j < freeGameSymbolsHitRate [0].Length; j++) {
				Console.Write (symbolsNames [j] + "\t");
				for (int i = 0; i < freeGameSymbolsHitRate.Length; i++) {
					Console.Write ((double)freeGameSymbolsHitRate [i] [j] / (double)totalNumberOfGames + "\t");
				}
				Console.WriteLine ();
			}
			Console.WriteLine ();
			Console.WriteLine ("Free Games Symbols Hit Rate:");
			Console.Write ("\t");
			for (int i = 0; i < freeGameSymbolsHitRate.Length; i++) {
				Console.Write ("" + i + "of\t");
			}
			Console.WriteLine ();
			for (int j = 0; j < freeGameSymbolsHitRate [0].Length; j++) {
				Console.Write (symbolsNames [j] + "\t");
				for (int i = 0; i < freeGameSymbolsHitRate.Length; i++) {
					Console.Write ((double)freeGameSymbolsHitRate [i] [j] + "\t");
				}
				Console.WriteLine ();
			}
		}

		/**
		 * Print screen view.
		 */
		private static void printView ()
		{
			int max = view [0].Length;
			for (int i = 0; i < view.Length; i++) {
				if (max < view [i].Length) {
					max = view [i].Length;
				}
			}

			for (int j = 0; j < max; j++) {
				for (int i = 0; i < view.Length && j < view [i].Length; i++) {
					Console.Write (symbolsNames [view [i] [j]] + "   ");
				}

				Console.WriteLine ();
			}
		}

		/**
		 * Print simulation execution command.
		 *
		 * @param args Command line arguments list.
		 */
		private static void printExecuteCommand (string [] args)
		{
			Console.WriteLine ("Execute command:");
			Console.WriteLine ();
			Console.Write (System.IO.Path.GetFileName (System.Reflection.Assembly.GetEntryAssembly ().Location) + " ");
			for (int i = 0; i < args.Length; i++) {
				Console.Write (args [i] + " ");
			}
			Console.WriteLine ();
		}

		/**
		 * Main application entry point.
		 *
		 * @param args Command line arguments list.
		 */
		public static void Main (string [] args)
		{
			printExecuteCommand (args);
			Console.WriteLine ();

			long numberOfSimulations = 10000000L;
			long progressPrintOnIteration = 10000000L;

			/* Parse command line arguments. */
			for (int a = 0; a < args.Length; a++) {
				if (args.Length > 0 && args [a].Contains ("-g")) {
					String parameter = args [a].Substring (2);

					if (parameter.Contains ("k")) {
						parameter = parameter.Substring (0, parameter.Length - 1);
						parameter += "000";
					}

					if (parameter.Contains ("m")) {
						parameter = parameter.Substring (0, parameter.Length - 1);
						parameter += "000000";
					}

					try {
						numberOfSimulations = Int64.Parse (parameter);
					} catch (Exception) {
					}
				}

				if (args.Length > 0 && args [a].Contains ("-p")) {
					String parameter = args [a].Substring (2);

					if (parameter.Contains ("k")) {
						parameter = parameter.Substring (0, parameter.Length - 1);
						parameter += "000";
					}

					if (parameter.Contains ("m")) {
						parameter = parameter.Substring (0, parameter.Length - 1);
						parameter += "000000";
					}

					try {
						progressPrintOnIteration = Int64.Parse (parameter);
						verboseOutput = true;
					} catch (Exception) {
					}
				}

				if (args.Length > 0 && args [a].Contains ("-freeoff")) {
					freeOff = true;
				}

				if (args.Length > 0 && args [a].Contains ("-wildsoff")) {
					wildsOff = true;
				}

				if (args.Length > 0 && args [a].Contains ("-bruteforce")) {
					bruteForce = true;
				}

				if (args.Length > 0 && args [a].Contains ("-volatility")) {
					volatilityEstimation = true;
				}

				if (args.Length > 0 && args [a].Contains ("-ci")) {
					String parameter = args [a].Substring (3);

					try {
						confidenceInterval = Double.Parse (parameter);
					} catch (Exception) {
					}
				}

				if (args.Length > 0 && args [a].Contains ("-slices")) {
					slicesReels = true;
				}

				if (args.Length > 0 && args [a].Contains ("-s")) {
					String parameter = args [a].Substring (2);

					try {
						sampleSize = Int32.Parse (parameter);
					} catch (Exception) {
					}

					if (sampleSize < 0) {
						sampleSize = 0;
					}

					baseSlicesReels = sliceReels (baseReels, view, sampleSize);
					freeSlicesReels = sliceReels (freeReels, view, sampleSize);
				}

				if (args.Length > 0 && args [a].Contains ("-verify")) {
					printDataStructures ();
					Environment.Exit (0);
				}

				if (args.Length > 0 && args [a].Contains ("-help")) {
					printHelp ();
					Console.WriteLine ();
					Environment.Exit (0);
				}

				if (args.Length > 0 && args [a].Contains ("-h")) {
					printHelp ();
					Console.WriteLine ();
					Environment.Exit (0);
				}
			}

			/* Calculate all combinations in base game. */
			if (bruteForce == true) {
				reelsStops = new int [] { 0, 0, 0, 0, 0 };
				numberOfSimulations = 1;
				for (int i = 0; i < baseReels.Length; i++) {
					numberOfSimulations *= baseReels [i].Length;
				}
			}

			/* Simulation main loop. */
			if (volatilityEstimation == false) {
				fullSimulation (numberOfSimulations, progressPrintOnIteration);
			}

			/* Volatility estimation loop. */
			if (volatilityEstimation == true) {
				volatilityEstimation = false;
				fullSimulation (numberOfSimulations, progressPrintOnIteration);
				volatilityEstimation = true;

				volatilitySimulation (numberOfSimulations);
			}
		}

	}
}
