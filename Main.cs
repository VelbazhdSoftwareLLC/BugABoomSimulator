/*******************************************************************************
* Bug A Boom Slot Simulation version 0.9.0                                     *
* Copyrights (C) 2014 Velbazhd Software LLC                                    *
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
using System.Collections.Generic;

namespace CSharpSimulation
{
	/**
	 * Main application class.
	 *
	 * @author Todor Balabanov
	 *
	 * @email todor.balabanov@gmail.com
	 *
	 * @date 13 Jul 2014
	 */
	class MainClass
	{
		/**
		 * Pseudo-random number generator.
		 */
		private static Random prng = new Random();

		/**
		 * List of symbols names.
		 */
		private static String[] symbols = {
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

		/**
		 * Slot game paytable.
		 */
		private static int[][] paytable = {
			new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new int[]{0,0,0,0,0,0,0,0,2,2,2,10,2},
			new int[]{5,5,5,10,10,10,15,15,25,25,50,250,5},
			new int[]{25,25,25,50,50,50,75,75,125,125,250,2500,0},
			new int[]{125,125,125,250,250,250,500,500,750,750,1250,10000,0},
		};

		/**
		 * Lines combinations.
		 */
		private static int[][] lines = {
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

		/**
		 * Stips in base game.
		 */
		private static int[][] baseReels = {
			new int[]{0,4,11,1,3,2,5,9,0,4,2,7,8,0,5,2,6,10,0,5,1,3,9,4,2,7,8,0,5,2,6,9,0,5,2,4,10,0,5,1,7,9,2,5},
			new int[]{4,1,11,2,7,0,9,5,1,3,8,4,2,6,12,4,0,3,1,8,4,2,6,0,10,4,1,3,2,12,4,0,7,1,8,2,4,0,9,1,6,2,8,0},
			new int[]{1,7,11,5,1,7,8,6,0,3,12,4,1,6,9,5,2,7,10,1,3,2,8,1,3,0,9,5,1,3,10,6,0,3,8,7,1,6,12,3,2,5,9,3},
			new int[]{5,2,11,3,0,6,1,5,12,2,4,0,10,3,1,7,3,2,11,5,4,6,0,5,12,1,3,7,2,4,8,0,3,6,1,4,12,2,5,7,0,4,9,1},
			new int[]{7,0,11,4,6,1,9,5,10,2,7,3,8,0,4,9,1,6,5,10,2,8,3},
		};

		/**
		 * Stips in free spins.
		 */
		private static int[][] freeReels = {
			new int[]{2,4,11,0,3,7,1,4,8,2,5,6,0,5,9,1,3,7,2,4,10,0,3,1,8,4,2,5,6,0,4,1,10,5,2,3,7,0,5,9,1,3,6},
			new int[]{4,2,11,0,5,2,12,1,7,0,9,2,3,0,12,2,4,0,5,8,2,6,0,12,2,7,1,3,10,6,0},
			new int[]{1,4,11,2,7,8,1,5,12,0,3,9,1,7,8,1,5,12,2,6,10,1,4,9,3,1,8,0,12,6,9},
			new int[]{6,4,11,2,7,3,9,1,6,5,12,0,4,10,2,3,8,1,7,5,12,0},
			new int[]{3,4,11,0,6,5,3,8,1,7,4,9,2,5,10,0,3,8,1,4,10,2,5,9},
		};

		/**
		 * Use reels stops in brute force combinations generation.
		 */
		private static int[] reelsStops = new int[]{0, 0, 0, 0, 0};

		/**
		 * Current visible symbols on the screen.
		 */
		private static int[][] view = {
			new int[]{ -1, -1, -1 },
			new int[]{ -1, -1, -1 },
			new int[]{ -1, -1, -1 },
			new int[]{ -1, -1, -1 },
			new int[]{ -1, -1, -1 }
		};

		/**
		 * Current free spins multiplier.
		 */
		private static int freeGamesMultiplier = 0;

		/**
		 * If wild is presented in the line multiplier.
		 */
		private static int wildInLineMultiplier = 0;

		/**
		 * If scatter win is presented on the screen.
		 */
		private static int scatterMultiplier = 1;

		/**
		 * Total bet in single base game spin.
		 */
		private static int singleLineBet = 1;

		/**
		 * Total bet in single base game spin.
		 */
		private static int totalBet = singleLineBet * lines.Length;

		/**
		 * Free spins to be played.
		 */
		private static int freeGamesNumber = 0;

		/**
		 * Total amount of won money.
		 */
		private static long wonMoney = 0L;

		/**
		 * Total amount of lost money.
		 */
		private static long lostMoney = 0L;

		/**
		 * Total amount of won money in base game.
		 */
		private static long baseMoney = 0L;

		/**
		 * Total amount of won money in free spins.
		 */
		private static long freeMoney = 0L;

		/**
		 * Max amount of won money in base game.
		 */
		private static long baseMaxWin = 0L;

		/**
		 * Max amount of won money in free spins.
		 */
		private static long freeMaxWin = 0L;

		/**
		 * Total number of base games played.
		 */
		private static long totalNumberOfGames = 0L;

		/**
		 * Total number of free spins played.
		 */
		private static long totalNumberOfFreeGames = 0L;

		/**
		 * Total number of free spins started.
		 */
		private static long totalNumberOfFreeGameStarts = 0L;

		/**
		 * Total number of free spins started.
		 */
		private static long totalNumberOfFreeGameRestarts = 0L;

		/**
		 * Hit rate of wins in base game.
		 */
		private static long baseGameHitRate = 0L;

		/**
		 * Hit rate of wins in free spins.
		 */
		private static long freeGamesHitRate = 0L;

		/**
		 * Verbose output flag.
		 */
		private static bool verboseOutput = false;

		/**
		 * Free spins flag.
		 */
		private static bool freeOff = false;

		/**
		 * Wild substitution flag.
		 */
		private static bool wildsOff = false;

		/**
		 * Brute force all winning combinations in base game only flag.
		 */
		private static bool bruteForce = false;

		/**
		 * Symbols win hit rate in base game.
		 */
		private static long[][] baseSymbolMoney = {
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0}
		};

		/**
		 * Symbols hit rate in base game.
		 */
		private static long[][] baseGameSymbolsHitRate = {
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0}
		};

		/**
		 * Symbols win hit rate in base game.
		 */
		private static long[][] freeSymbolMoney = {
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0}
		};

		/**
		 * Symbols hit rate in base game.
		 */
		private static long[][] freeGameSymbolsHitRate = {
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
			new long[]{0,0,0,0,0,0,0,0,0,0,0,0,0}
		};

		/**
		 * Static constructor for discrete distributions shuffling.
		 *
		 * @author Todor Balabanov
		 *
		 * @email todor.balabanov@gmail.com
		 *
		 * @date 13 Jul 2014
		 */
		static MainClass() {
		}
		
		/**
		 * Single reels spin to fill view with symbols.
		 *
		 * @param reels Reels strips.
		 *
		 * @author Todor Balabanov
		 *
		 * @email todor.balabanov@gmail.com
		 *
		 * @date 06 Aug 2014
		 */
		private static void nextCombination(int[] reelsStops) {
			reelsStops [0] += 1;
			for(int i=0; i<reelsStops.Length; i++) {
				if(reelsStops[i]>=baseReels[i].Length) {
					reelsStops [i] = 0;
					if (i < reelsStops.Length - 1) {
						reelsStops [i + 1] += 1;
					}
				}
			}
		}

		/**
		 * Single reels spin to fill view with symbols.
		 *
		 * @param reels Reels strips.
		 *
		 * @author Todor Balabanov
		 *
		 * @email todor.balabanov@gmail.com
		 *
		 * @date 13 Jul 2014
		 */
		private static void spin(int[][] reels) {
			for (int i = 0, r, u, d; i < view.Length && i < reels.Length; i++) {
				if (bruteForce == true) {
					r = reelsStops [i];
					u = r - 1;
					d = r + 1;
				} else {
					r = prng.Next (reels [i].Length);
					u = r - 1;
					d = r + 1;
				}

				if (u < 0) {
					u = reels[i].Length - 1;
				}

				if (d >= reels[i].Length) {
					d = 0;
				}

				view[i][0] = reels[i][u];
				view[i][1] = reels[i][r];
				view[i][2] = reels[i][d];
			}
		}

		/**
		 * Calculate win in particular line.
		 *
		 * @param line Single line.
		 *
		 * @return Calculated win.
		 *
		 * @author Todor Balabanov
		 *
		 * @email todor.balabanov@gmail.com
		 *
		 * @date 13 Jul 2014
		 */
		private static int[] wildLineWin(int[] line) {
			int []values = {11, 0, 0};

			/*
			 * If there is no leading wild there is no wild win.
			 */
			if(line[0] != values[0]) {
				return(values);
			}

			/*
			 * Wild symbol passing to find first regular symbol.
			 */
			for (int i = 0; i < line.Length; i++) {
				/*
				 * First no wild symbol found.
				 */
				if (line[i] != values[0]) {
					break;
				}

				values [1]++;
			}

			values[2] = singleLineBet * paytable[values[1]][values[0]];

			return(values);
		}

		/**
		 * Calculate win in particular line.
		 *
		 * @param line Single line.
		 *
		 * @return Calculated win.
		 *
		 * @author Todor Balabanov
		 *
		 * @email todor.balabanov@gmail.com
		 *
		 * @date 13 Jul 2014
		 */
		private static int lineWin(int[] line) {
			int []wildWin = wildLineWin(line);

			/*
			 * Line win without wild is multiplied by one.
			 */
			wildInLineMultiplier = 1;

			/*
			 * Keep first symbol in the line.
			 */
			int symbol = line [0];

			/*
			 * Wild symbol passing to find first regular symbol.
			 */
			for (int i=0; i<line.Length; i++) {
				/*
				 * First no wild symbol found.
				 */
				if(symbol != 11) {
					break;
				}

				symbol = line [i];

				/*
				 * Line win with wild is multiplied by two.
				 */
				wildInLineMultiplier = 2;
			}

			/*
			 * Wild symbol substitution. Other wild are artificial they are not part of the pay table.
			 */
			for (int i = 0; i<line.Length && wildsOff==false; i++) {
				if (line[i] == 11) {
					/*
					 * Substitute wild with regular symbol.
					 */
					line[i] = symbol;

					/*
					 * Line win with wild is multiplied by two.
					 */
					wildInLineMultiplier = 2;
				}
			}

			/*
			 * Line win with five wilds is multiplied by one.
			 */
			if(symbol == 11) {
				wildInLineMultiplier = 1;
			}

			/*
			 * Count symbols in winning line.
			 */
			int number = 0;
			for (int i = 0; i < line.Length; i++) {
				if (line [i] == symbol) {
					number++;
				} else {
					break;
				}
			}

			/*
			 * Cleare unused symbols.
			 */
			for (int i = number; i < line.Length; i++) {
				line[i] = -1;
			}

			int win = singleLineBet * paytable[number][symbol] * wildInLineMultiplier;
			if(win < wildWin[2]) {
				symbol = wildWin[0];
				number = wildWin[1];
				win = wildWin[1];
			}

			/*
			 * There is multiplier in free games mode.
			 */
			if(freeGamesNumber > 0) {
				win *= freeGamesMultiplier;
			}

			if(win > 0 && freeGamesNumber==0) {
				baseSymbolMoney[number][symbol] += win;
				baseGameSymbolsHitRate[number][symbol]++;
			} else if (win > 0 && freeGamesNumber>0) {
				freeSymbolMoney[number][symbol] += win;
				freeGameSymbolsHitRate[number][symbol]++;
			}

			return( win );
		}

		/**
		 * Calculate win in all possible lines.
		 *
		 * @param view Symbols visible in screen view.
		 *
		 * @return Calculated win.
		 *
		 * @author Todor Balabanov
		 *
		 * @email todor.balabanov@gmail.com
		 *
		 * @date 13 Jul 2014
		 */
		private static int linesWin (int[][] view) {
			int win = 0;

			/*
			 * Check wins in all possible lines.
			 */
			for (int l = 0; l < lines.Length; l++) {
				int[] line = { -1, -1, -1, -1, -1 };

				/*
				 * Prepare line for combination check.
				 */
				for (int i = 0; i < line.Length; i++) {
					int index = lines [l] [i];
					line [i] = view [i] [index];
				}

				int result = lineWin( line );

				/*
				 * Accumulate line win.
				 */
				win += result;
			}

			return( win );
		}

		/**
		 * Calculate win from scatters.
		 *
		 * @retur Win from scatters.
		 *
		 * @author Todor Balabanov
		 *
		 * @email todor.balabanov@gmail.com
		 *
		 * @date 13 Jul 2014
		 */
		private static int scatterWin() {
			int numberOfScatters = 0;
			for (int i = 0; i < view.Length; i++) {
				for (int j = 0; j < view[i].Length; j++) {
					if (view[i][j] == 12) {
						numberOfScatters++;
					}
				}
			}

			int win = paytable[numberOfScatters][12]*totalBet*scatterMultiplier;

			if(freeGamesNumber>0) {
				win *= freeGamesMultiplier;
			}

			if(win > 0 && freeGamesNumber==0) {
				baseSymbolMoney[numberOfScatters][12] += win;
				baseGameSymbolsHitRate[numberOfScatters][12]++;
			} else if (win > 0 && freeGamesNumber>0) {
				freeSymbolMoney[numberOfScatters][12] += win;
				freeGameSymbolsHitRate[numberOfScatters][12]++;
			}

			return( win );
		}

		/**
		 * Setup parameters for free spins mode.
		 *
		 * @author Todor Balabanov
		 *
		 * @email todor.balabanov@gmail.com
		 *
		 * @date 13 Jul 2014
		 */
		private static void freeGamesSetup() {
			if (bruteForce == true) {
				return;
			}

			if(freeOff == true) {
				return;
			}

			int numberOfScatters = 0;
			for (int i = 0; i < view.Length; i++) {
				for (int j = 0; j < view[i].Length; j++) {
					if (view[i][j] == 12) {
						numberOfScatters++;
					}
				}
			}

			/*
			 * In base game 3+ scatters turn into free spins.
			 */
			if(numberOfScatters<3 && freeGamesNumber==0) {
				return;
			} else if(numberOfScatters>=3 && freeGamesNumber==0) {
				freeGamesNumber = 15;
				freeGamesMultiplier = 4;
				totalNumberOfFreeGameStarts++;
			} else if(numberOfScatters>=3 && freeGamesNumber>0) {
				freeGamesNumber += 15;
				freeGamesMultiplier = 4;
				totalNumberOfFreeGameRestarts++;
			}
		}

		/**
		 * Play single free spin game.
		 *
		 * @author Todor Balabanov
		 *
		 * @email todor.balabanov@gmail.com
		 *
		 * @date 13 Jul 2014
		 */
		private static void singleFreeGame() {
			if (bruteForce == true) {
				return;
			}

			if(freeOff == true) {
				return;
			}

			/*
			 * Spin reels.
			 * In retriggered games from FS1 to FS2 and from FS2 to FS3. FS3 can not rettriger FS.
			 */
			spin( freeReels );

			freeGamesSetup();

			/*
			 * Win accumulated by lines.
			 */
			int win = linesWin (view) + scatterWin();

			/*
			 * Add win to the statistics.
			 */
			freeMoney += win;
			wonMoney += win;
			if(freeMaxWin < win) {
				freeMaxWin = win;
			}

			/*
			 * Count free games hit rate.
			 */
			if(win > 0) {
				freeGamesHitRate++;
			}
		}

		/**
		 * Play single base game.
		 *
		 * @author Todor Balabanov
		 *
		 * @email todor.balabanov@gmail.com
		 *
		 * @date 13 Jul 2014
		 */
		private static void singleBaseGame() {
			/*
			 * Spin reels.
			 */
			spin( baseReels );
			if (bruteForce == true) {
				nextCombination (reelsStops);
			}

			freeGamesSetup();

			/*
			 * Win accumulated by lines.
			 */
			int win = linesWin (view) + scatterWin();

			/*
			 * Add win to the statistics.
			 */
			baseMoney += win;
			wonMoney += win;
			if(baseMaxWin < win) {
				baseMaxWin = win;
			}

			/*
			 * Count base game hit rate.
			 */
			if(win > 0) {
				baseGameHitRate++;
			}

			/*
			 * Play all free games.
			 */
			while(freeGamesNumber > 0) {
				totalNumberOfFreeGames++;

				singleFreeGame();

				freeGamesNumber--;
			}
			freeGamesMultiplier = 1;
		}

		/**
		 * Print help information.
		 *
		 * @author Todor Balabanov
		 *
		 * @email todor.balabanov@gmail.com
		 *
		 * @date 13 Jul 2014
		 */
		private static void printHelp () {
			Console.WriteLine( "*******************************************************************************" );
			Console.WriteLine( "* Bug A Boom Slot Simulation version 0.9.0                                     *" );
			Console.WriteLine( "* Copyrights (C) 2014 Velbazhd Software LLC                                    *" );
			Console.WriteLine( "*                                                                             *" );
			Console.WriteLine( "* developed by Todor Balabanov ( todor.balabanov@gmail.com )                  *" );
			Console.WriteLine( "* Sofia, Bulgaria                                                             *" );
			Console.WriteLine( "*                                                                             *" );
			Console.WriteLine( "* This program is free software: you can redistribute it and/or modify        *" );
			Console.WriteLine( "* it under the terms of the GNU General Public License as published by        *" );
			Console.WriteLine( "* the Free Software Foundation, either version 3 of the License, or           *" );
			Console.WriteLine( "* (at your option) any later version.                                         *" );
			Console.WriteLine( "*                                                                             *" );
			Console.WriteLine( "* This program is distributed in the hope that it will be useful,             *" );
			Console.WriteLine( "* but WITHOUT ANY WARRANTY; without even the implied warranty of              *" );
			Console.WriteLine( "* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the               *" );
			Console.WriteLine( "* GNU General Public License for more details.                                *" );
			Console.WriteLine( "*                                                                             *" );
			Console.WriteLine( "* You should have received a copy of the GNU General Public License           *" );
			Console.WriteLine( "* along with this program. If not, see <http://www.gnu.org/licenses/>.        *" );
			Console.WriteLine( "*                                                                             *" );
			Console.WriteLine( "*******************************************************************************" );
			Console.WriteLine( "*                                                                             *" );
			Console.WriteLine( "* -h              Help screen.                                                *" );
			Console.WriteLine( "* -help           Help screen.                                                *" );
			Console.WriteLine( "*                                                                             *" );
			Console.WriteLine( "* -g<number>      Number of games (default 10 000 000).                       *" );
			Console.WriteLine( "* -p<number>      Progress on each iteration number (default 10 000 000).     *" );
			Console.WriteLine( "*                                                                             *" );
			Console.WriteLine( "* -freeoff        Switch off free spins.                                      *" );
			Console.WriteLine( "* -wildsoff       Switch off wilds.                                           *" );
			Console.WriteLine( "* -bruteforce     Switch on brute force only for the base game.               *" );
			Console.WriteLine( "*                                                                             *" );
			Console.WriteLine( "* -verify         Print input data structures.                                *" );
			Console.WriteLine( "*                                                                             *" );
			Console.WriteLine( "*******************************************************************************" );
		}

		/**
		 * Print all simulation input data structures.
		 *
		 * @author Todor Balabanov
		 *
		 * @email todor.balabanov@gmail.com
		 *
		 * @date 13 Jul 2014
		 */
		private static void printDataStructures() {
			Console.WriteLine("Paytable:");
			for(int i=0; i<paytable.Length; i++) {
				Console.Write("\t" + i + " of");
			}
			Console.WriteLine();
			for(int j=0; j<paytable[0].Length; j++) {
				Console.Write(symbols[j] + "\t");
				for(int i=0; i<paytable.Length; i++) {
					Console.Write(paytable[i][j] + "\t");
				}
				Console.WriteLine();
			}
			Console.WriteLine();

			Console.WriteLine("Lines:");
			for(int i=0; i<lines.Length; i++) {
				for(int j=0; j<lines[0].Length; j++) {
					Console.Write(lines[i][j] + " ");
				}
				Console.WriteLine();
			}
			Console.WriteLine();

			Console.WriteLine("Base Game Reels:");
			for(int i=0; i<baseReels.Length; i++) {
				for(int j=0; j<baseReels[i].Length; j++) {
					if(j % 10 == 0) {
						Console.WriteLine();
					}
					Console.Write(symbols[ baseReels[i][j] ] + " ");
				}
				Console.WriteLine();
			}
			Console.WriteLine();

			Console.WriteLine("Free Games Reels:");
			for(int i=0; i<freeReels.Length; i++) {
				for(int j=0; j<freeReels[i].Length; j++) {
					if(j % 10 == 0) {
						Console.WriteLine();
					}
					Console.Write(symbols[ freeReels[i][j] ] + " ");
				}
				Console.WriteLine();
			}
			Console.WriteLine();

			Console.WriteLine("Base Game Reels:");
			/* Count symbols in reels. */ {
				int[][] counters = {
					new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
					new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
					new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
					new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
					new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
				};
				for(int i=0; i<baseReels.Length; i++) {
					for(int j=0; j<baseReels[i].Length; j++) {
						counters[i][baseReels[i][j]]++;
					}
				}
				for(int i=0; i<baseReels.Length; i++) {
					Console.Write("\tReel " + (i+1));
				}
				Console.WriteLine();
				for(int j=0; j<counters[0].Length; j++) {
					Console.Write(symbols[ j ] + "\t");
					for(int i=0; i<counters.Length; i++) {
						Console.Write(counters[i][j] + "\t");
					}
					Console.WriteLine();
				}
				Console.WriteLine("---------------------------------------------");
				Console.Write("Total:\t");
				long combinations = 1L;
				for(int i=0; i<counters.Length; i++) {
					int sum = 0;
					for(int j=0; j<counters[0].Length; j++) {
						sum += counters[i][j];
					}
					Console.Write(sum + "\t");
					if(sum != 0) {
						combinations *= sum;
					}
				}
				Console.WriteLine();
				Console.WriteLine("---------------------------------------------");
				Console.WriteLine("Combinations:\t" + combinations);
			}
			Console.WriteLine();

			Console.WriteLine("Free Games Reels:");
			/* Count symbols in reels. */ {
				int[][] counters = {
					new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
					new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
					new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
					new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
					new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
				};
				for(int i=0; i<freeReels.Length; i++) {
					for(int j=0; j<freeReels[i].Length; j++) {
						counters[i][freeReels[i][j]]++;
					}
				}
				for(int i=0; i<freeReels.Length; i++) {
					Console.Write("\tReel " + (i+1));
				}
				Console.WriteLine();
				for(int j=0; j<counters[0].Length; j++) {
					Console.Write(symbols[ j ] + "\t");
					for(int i=0; i<counters.Length; i++) {
						Console.Write(counters[i][j] + "\t");
					}
					Console.WriteLine();
				}
				Console.WriteLine("---------------------------------------------");
				Console.Write("Total:\t");
				long combinations = 1L;
				for(int i=0; i<counters.Length; i++) {
					int sum = 0;
					for(int j=0; j<counters[0].Length; j++) {
						sum += counters[i][j];
					}
					Console.Write(sum + "\t");
					if(sum != 0) {
						combinations *= sum;
					}
				}
				Console.WriteLine();
				Console.WriteLine("---------------------------------------------");
				Console.WriteLine("Combinations:\t" + combinations);
			}
			Console.WriteLine();
		}

		/**
		 * Print simulation statistics.
		 *
		 * @author Todor Balabanov
		 *
		 * @email todor.balabanov@gmail.com
		 *
		 * @date 13 Jul 2014
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
			for (int i=0; i<baseSymbolMoney.Length; i++) {
				Console.Write ("" + i + "of\t");
			}
			Console.WriteLine ();
			for (int j=0; j<baseSymbolMoney[0].Length; j++) {
				Console.Write (symbols[ j ] + "\t");
				for (int i=0; i<baseSymbolMoney.Length; i++) {
					Console.Write ((double)baseSymbolMoney [i] [j] / (double)lostMoney + "\t");
				}
				Console.WriteLine ();
			}
			Console.WriteLine ();
			Console.WriteLine ("Base Game Symbols Hit Frequency:");
			Console.Write ("\t");
			for (int i=0; i<baseGameSymbolsHitRate.Length; i++) {
				Console.Write ("" + i + "of\t");
			}
			Console.WriteLine ();
			for (int j=0; j<baseGameSymbolsHitRate[0].Length; j++) {
				Console.Write (symbols[ j ] + "\t");
				for (int i=0; i<baseGameSymbolsHitRate.Length; i++) {
					Console.Write ((double)baseGameSymbolsHitRate [i] [j] / (double)totalNumberOfGames + "\t");
				}
				Console.WriteLine ();
			}

			Console.WriteLine ();
			Console.WriteLine ("Free Games Symbols RTP:");
			Console.Write ("\t");
			for (int i=0; i<freeSymbolMoney.Length; i++) {
				Console.Write ("" + i + "of\t");
			}
			Console.WriteLine ();
			for (int j=0; j<freeSymbolMoney[0].Length; j++) {
				Console.Write (symbols[ j ] + "\t");
				for (int i=0; i<freeSymbolMoney.Length; i++) {
					Console.Write ((double)freeSymbolMoney [i] [j] / (double)lostMoney + "\t");
				}
				Console.WriteLine ();
			}
			Console.WriteLine ();
			Console.WriteLine ("Free Games Symbols Hit Frequency:");
			Console.Write ("\t");
			for (int i=0; i<freeGameSymbolsHitRate.Length; i++) {
				Console.Write ("" + i + "of\t");
			}
			Console.WriteLine ();
			for (int j=0; j<freeGameSymbolsHitRate[0].Length; j++) {
				Console.Write (symbols[ j ] + "\t");
				for (int i=0; i<freeGameSymbolsHitRate.Length; i++) {
					Console.Write ((double)freeGameSymbolsHitRate [i] [j] / (double)totalNumberOfGames + "\t");
				}
				Console.WriteLine ();
			}
		}

		/**
		 * Print screen view.
		 *
		 * @author Todor Balabanov
		 *
		 * @email todor.balabanov@gmail.com
		 *
		 * @date 13 Jul 2014
		 */
		private static void printView () {
			int max = view[0].Length;
			for (int i=0; i<view.Length; i++) {
				if(max < view[i].Length) {
					max = view[i].Length;
				}
			}

			for(int j=0; j<max; j++) {
				for (int i=0; i<view.Length && j<view[i].Length; i++) {
					if(view[i][j] < 10 && view[i][j]>=0) {
						Console.Write(" ");
					}
					Console.Write(view[i][j] + " ");
				}

				Console.WriteLine();
			}
		}

		/**
		 * Print simulation execution command.
		 *
		 * @param args Command line arguments list.
		 *
		 * @author Todor Balabanov
		 *
		 * @email todor.balabanov@gmail.com
		 *
		 * @date 13 Jul 2014
		 */
		private static void printExecuteCommand(string[] args) {
			Console.WriteLine( "Execute command:" );
			Console.WriteLine();
			Console.Write( System.IO.Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().Location)+" " );
			for(int i=0; i<args.Length; i++) {
				Console.Write(args[i] + " ");
			}
			Console.WriteLine();
		}

		/**
		 * Main application entry point.
		 *
		 * @param args Command line arguments list.
		 *
		 * @author Todor Balabanov
		 *
		 * @email todor.balabanov@gmail.com
		 *
		 * @date 13 Jul 2014
		 */
		public static void Main (string[] args) {
			printExecuteCommand(args);
			Console.WriteLine();

			long numberOfSimulations = 10000000L;
			long progressPrintOnIteration = 10000000L;

			/*
			 * Parse command line arguments.
			 */
			for(int a=0; a<args.Length; a++) {
				if(args.Length > 0 && args[a].Contains("-g")) {
					String parameter = args[a].Substring(2);

					if(parameter.Contains("k")) {
						parameter = parameter.Substring(0, parameter.Length-1);
						parameter += "000";
					}

					if(parameter.Contains("m")) {
						parameter = parameter.Substring(0, parameter.Length-1);
						parameter += "000000";
					}

					try {
						numberOfSimulations = Int64.Parse(parameter );
					} catch(Exception) {
					}
				}

				if(args.Length > 0 && args[a].Contains("-p")) {
					String parameter = args[a].Substring(2);

					if(parameter.Contains("k")) {
						parameter = parameter.Substring(0, parameter.Length-1);
						parameter += "000";
					}

					if(parameter.Contains("m")) {
						parameter = parameter.Substring(0, parameter.Length-1);
						parameter += "000000";
					}

					try {
						progressPrintOnIteration = Int64.Parse(parameter );
						verboseOutput = true;
					} catch(Exception) {
					}
				}

				if(args.Length > 0 && args[a].Contains("-freeoff")) {
					freeOff = true;
				}

				if(args.Length > 0 && args[a].Contains("-wildsoff")) {
					wildsOff = true;
				}

				if(args.Length > 0 && args[a].Contains("--bruteforce")) {
					bruteForce = true;
				}

				if(args.Length > 0 && args[a].Contains("-verify")) {
					printDataStructures();
					Environment.Exit(0);
				}

				if(args.Length > 0 && args[a].Contains("-help")) {
					printHelp();
					Console.WriteLine();
					Environment.Exit(0);
				}

				if(args.Length > 0 && args[a].Contains("-h")) {
					printHelp();
					Console.WriteLine();
					Environment.Exit(0);
				}
			}

			/*
			 * Calculate all combinations in base game.
			 */
			if (bruteForce == true) {
				reelsStops = new int[]{0, 0, 0, 0, 0};
				numberOfSimulations = 1;
				for (int i=0; i<baseReels.Length; i++) {
					numberOfSimulations *= baseReels [i].Length;
				}
			}

			/*
			 * Simulation main loop.
			 */
			for (long g = 0L; g < numberOfSimulations; g++) {
				if(verboseOutput == true && g==0) {
					Console.WriteLine("Games\tRTP\tRTP(Base)\tRTP(Free)");
				}

				/*
				 * Print progress report.
				 */
				if(verboseOutput == true && g%progressPrintOnIteration == 0) {
					try {
						Console.Write(g);
						Console.Write("\t");
						Console.Write(String.Format("  {0:F6}", ((double) wonMoney / (double) lostMoney)));
						Console.Write("\t");
						Console.Write(String.Format("  {0:F6}", ((double) baseMoney / (double) lostMoney)));
						Console.Write("\t");
						Console.Write(String.Format("  {0:F6}", ((double) freeMoney / (double) lostMoney)));
					} catch( Exception ) {
					}
					Console.WriteLine();
				}

				totalNumberOfGames++;

				lostMoney += totalBet;

				singleBaseGame();
			}

			Console.WriteLine("********************************************************************************");
			printStatistics();
			Console.WriteLine("********************************************************************************");
		}
	}
}

//using System;
//
//namespace BruteForceRTP
//{
//	class MainClass
//	{
//
//		private static String[] symbols = {
//			"SHIRT   ",
//			"SPEAKER ",
//			"MIC     ",
//			"AIRPLANE",
//			"MONEY   ",
//			"MAGAZINE",
//			"DRUM    ",
//			"BASS    ",
//			"SOLO    ",
//			"VOCAL   ",
//			"POSTER  ",
//			"WILD    ",
//			"SCATTER ",
//		};
//
//		private static ulong[][] paytable = {
//			new ulong[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
//			new ulong[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
//			new ulong[]{0,0,0,0,0,0,0,0,2,2,2,10,2},
//			new ulong[]{5,5,5,10,10,10,15,15,25,25,50,250,5},
//			new ulong[]{25,25,25,50,50,50,75,75,125,125,250,2500,0},
//			new ulong[]{125,125,125,250,250,250,500,500,750,750,1250,10000,0},
//		};
//		
//		private static int[][] baseReels = {
//			new int[]{0,4,11,1,3,2,5,9,0,4,2,7,8,0,5,2,6,10,0,5,1,3,9,4,2,7,8,0,5,2,6,9,0,5,2,4,10,0,5,1,7,9,2,5},
//			new int[]{4,1,11,2,7,0,9,5,1,3,8,4,2,6,12,4,0,3,1,8,4,2,6,0,10,4,1,3,2,12,4,0,7,1,8,2,4,0,9,1,6,2,8,0},
//			new int[]{1,7,11,5,1,7,8,6,0,3,12,4,1,6,9,5,2,7,10,1,3,2,8,1,3,0,9,5,1,3,10,6,0,3,8,7,1,6,12,3,2,5,9,3},
//			new int[]{5,2,11,3,0,6,1,5,12,2,4,0,10,3,1,7,3,2,11,5,4,6,0,5,12,1,3,7,2,4,8,0,3,6,1,4,12,2,5,7,0,4,9,1},
//			new int[]{7,0,11,4,6,1,9,5,10,2,7,3,8,0,4,9,1,6,5,10,2,8,3},
//		};
//
//		private static int[][] freeReels = {
//			new int[]{2,4,11,0,3,7,1,4,8,2,5,6,0,5,9,1,3,7,2,4,10,0,3,1,8,4,2,5,6,0,4,1,10,5,2,3,7,0,5,9,1,3,6},
//			new int[]{4,2,11,0,5,2,12,1,7,0,9,2,3,0,12,2,4,0,5,8,2,6,0,12,2,7,1,3,10,6,0},
//			new int[]{1,4,11,2,7,8,1,5,12,0,3,9,1,7,8,1,5,12,2,6,10,1,4,9,3,1,8,0,12,6,9},
//			new int[]{6,4,11,2,7,3,9,1,6,5,12,0,4,10,2,3,8,1,7,5,12,0},
//			new int[]{3,4,11,0,6,5,3,8,1,7,4,9,2,5,10,0,3,8,1,4,10,2,5,9},
//		};
//
//		private static int[][] reels = null;
//
//		private static int[][] extendedReels = null;
//
//		private static ulong totalNumberOfCombinations = 0;
//
//		private static ulong[][] hits = {
//			new ulong[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
//			new ulong[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
//			new ulong[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
//			new ulong[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
//			new ulong[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
//			new ulong[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
//		};
//
//		private static ulong[] numberOfScatters = {0,0,0,0,0,0};
//		
//		public static void Main (string[] args){
//			/*Parse input parameters.*/{
//				for(int ar=0; ar<args.Length; ar++) {
//					if(args.Length > 0 && args[ar].Contains("-base")) {
//						reels = baseReels;
//					}
//					if(args.Length > 0 && args[ar].Contains("-free")) {
//						reels = freeReels;
//					}
//				}
//			}
//			if(reels == null) {
//				Environment.Exit(0);
//			}
//
//			totalNumberOfCombinations = (ulong)reels[0].Length * (ulong)reels[1].Length * (ulong)reels[2].Length * (ulong)reels[3].Length * (ulong)reels[4].Length;
//
//			extendedReels = new int[reels.Length][];
//			for(int i=0;i<reels.Length; i++) {
//				extendedReels[i] = new int[reels[i].Length+2];
//				for(int j=0; j<extendedReels[i].Length; j++) {
//					extendedReels[i][j] = reels[i][j%reels[i].Length];
//				}
//			}
//
//			Console.WriteLine("Reels:");
//			for(int i=0; i<reels.Length; i++) {
//				for(int j=0; j<reels[i].Length; j++) {
//					Console.Write(""+symbols[reels[i][j]] + "\t");
//				}
//				Console.WriteLine();
//			}
//
//			Console.WriteLine();
//			Console.WriteLine("Reels:");
//			int[][] counters = {
//				new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
//				new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
//				new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
//				new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
//				new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0},
//			};
//			for(int i=0; i<reels.Length; i++) {
//				for(int j=0; j<reels[i].Length; j++) {
//					counters[i][reels[i][j]]++;
//				}
//			}
//			for(int j=0; j<counters[0].Length; j++) {
//				Console.Write(symbols[j] + "\t");
//				for(int i=0; i<counters.Length; i++) {
//					Console.Write(counters[i][j] + "\t");
//				}
//				Console.WriteLine();
//			}
//
//			int a = -1;
//			int b = -1;
//			int c = -1;
//			int d = -1;
//			int e = -1;
//
//			int sa = 0;
//			int sb = 0;
//			int sc = 0;
//			int sd = 0;
//			int se = 0;
//			
//			int number1 = 0;
//			int number2 = 0;
//			int number = 0;
//			int mul1 = 1;
//			int mul2 = 1;
//			int symbol1 = -1;
//			int symbol2 = -1;
//			int symbol = -1;
//			for (e = 0; e < reels[4].Length; e++) {
//				Console.WriteLine();
//				Console.WriteLine((100.0D*e/reels[4].Length) + "%");
//
//				se = (extendedReels[4][e]==12?1:0)+(extendedReels[4][e+1]==12?1:0)+(extendedReels[4][e+2]==12?1:0);
//
//				for (d = 0; d < reels[3].Length; d++) {
//					Console.Write("=");
//
//					sd = (extendedReels[3][d]==12?1:0)+(extendedReels[3][d+1]==12?1:0)+(extendedReels[3][d+2]==12?1:0);
//					
//					for (c = 0; c < reels[2].Length; c++) {
//						sc = (extendedReels[2][c]==12?1:0)+(extendedReels[2][c+1]==12?1:0)+(extendedReels[2][c+2]==12?1:0);
//						
//						for (b = 0; b < reels[1].Length; b++) {
//							sb = (extendedReels[1][b]==12?1:0)+(extendedReels[1][b+1]==12?1:0)+(extendedReels[1][b+2]==12?1:0);
//							
//							for (a = 0; a < reels[0].Length; a++) {
//								sa = (extendedReels[0][a]==12?1:0)+(extendedReels[0][a+1]==12?1:0)+(extendedReels[0][a+2]==12?1:0);
//								
//								numberOfScatters[sa+sb+sc+sd+se]++;
//
//								/*
//								 * First symbol is not wild.
//								 */ 
//								if(extendedReels[0][a+1] != 11) {
//									mul1 = 1;
//									number1 = 1;
//									symbol1 = extendedReels [0] [a + 1];
//									if(extendedReels[0][a+1] == extendedReels[1][b+1] || extendedReels[1][b+1]==11) {
//										if (extendedReels [1] [b + 1] == 11) {
//											mul1 = 2;
//										}
//										number1 = 2;
//										if(extendedReels[0][a+1] == extendedReels[2][c+1] || extendedReels[2][c+1]==11) {
//											if (extendedReels [1] [c + 1] == 11) {
//												mul1 = 2;
//											}
//											number1 = 3;
//											if(extendedReels[0][a+1] == extendedReels[3][d+1] || extendedReels[3][d+1]==11) {
//												if (extendedReels [1] [d + 1] == 11) {
//													mul1 = 2;
//												}
//												number1 = 4;
//												if(extendedReels[0][a+1] == extendedReels[4][e+1] || extendedReels[4][e+1]==11) {
//													if (extendedReels [1] [e + 1] == 11) {
//														mul1 = 2;
//													}
//													number1 = 5;
//												}
//											}
//										}
//									}
//									hits[number1][symbol1]++;
//								}
//
//								/*
//								 * First symbol is wild.
//								 */ 
//								if(extendedReels[0][a+1] == 11) {
//									mul2 = 1;
//									number2 = 1;
//									symbol2 = extendedReels [0] [a + 1];
//									if(symbol2 == extendedReels[1][b+1]) {
//										number2 = 2;
//										if(paytable[number2][symbol2] <= 2*paytable[number2+1][extendedReels[2][c+1]]) {
//											symbol2 = extendedReels[2][c+1];
//											number2 = 3;
//											mul2 = 2;
//										}
//										if(symbol2 == extendedReels[2][c+1]) {
//											number2 = 3;
//											if(paytable[number2][symbol2] <= 2*paytable[number2+1][extendedReels[3][d+1]]) {
//												symbol2 = extendedReels[3][d+1];
//												number2 = 4;
//												mul2 = 2;
//											}
//											if(symbol2 == extendedReels[3][d+1]) {
//												number2 = 4;
//												if(paytable[number2][symbol2] <= 2*paytable[number2+1][extendedReels[4][e+1]]) {
//													symbol2 = extendedReels[4][e+1];
//													number2 = 5;
//													mul2 = 2;
//												}
//												if(symbol2 == extendedReels[4][e+1]) {
//													number2 = 5;
//													mul2 = 1;
//												}
//											}
//										}
//									}
//
//									hits[number2][symbol2]++;
//								}
//							}
//						}
//					}
//				}
//			}
//
//			Console.WriteLine();
//			Console.WriteLine("Symbols Combinations:");
//			for(int i=0; i<hits.Length; i++) {
//				for(int j=0; j<hits[i].Length; j++) {
//					Console.Write( hits[i][j] + "\t" );
//				}
//				Console.WriteLine();
//			}
//
//			Console.WriteLine();
//			Console.WriteLine("Paytable:");
//			for(int i=0; i<paytable.Length; i++) {
//				for(int j=0; j<paytable[i].Length; j++) {
//					Console.Write( paytable[i][j] + "\t" );
//				}
//				Console.WriteLine();
//			}
//
//			Console.WriteLine();
//			Console.WriteLine("Symbols Wins:");
//			for(int i=0; i<paytable.Length&&i<hits.Length; i++) {
//				for(int j=0; j<paytable[i].Length&&j<paytable[i].Length; j++) {
//					Console.Write( (hits[i][j]*(ulong)paytable[i][j]) + "\t" );
//				}
//				Console.WriteLine();
//			}
//
//			Console.WriteLine();
//			Console.WriteLine("Symbols Frequencies:");
//			for(int i=0; i<hits.Length; i++) {
//				for(int j=0; j<hits[i].Length; j++) {
//					Console.Write( (double)hits[i][j]/totalNumberOfCombinations + "\t" );
//				}
//				Console.WriteLine();
//			}
//
//			Console.WriteLine();
//			Console.WriteLine("Symbols RTP:");
//			double rtp = 0.0;
//			for(int i=0; i<paytable.Length&&i<hits.Length; i++) {
//				for(int j=0; j<paytable[i].Length&&j<hits[i].Length; j++) {
//					rtp += (double)hits[i][j]/totalNumberOfCombinations*(double)paytable[i][j];
//					Console.Write( (double)hits[i][j]/totalNumberOfCombinations*(double)paytable[i][j] + "\t" );
//				}
//				Console.WriteLine();
//			}
//			Console.WriteLine();
//			rtp += numberOfScatters[3] *  50.0D / totalNumberOfCombinations;
//			rtp += numberOfScatters[4] * 100.0D / totalNumberOfCombinations;
//			rtp += numberOfScatters[5] * 150.0D / totalNumberOfCombinations;
//			
//			Console.WriteLine();
//			Console.WriteLine("Total Number of Combinations:");
//			Console.WriteLine( totalNumberOfCombinations );
//
//			Console.WriteLine();
//			Console.WriteLine("RTP:");
//			Console.WriteLine( rtp );
//			
//			Console.WriteLine();
//			Console.WriteLine("Scatters Hit Frequency:");
//			for(int i=0; i<numberOfScatters.Length; i++) {
//				Console.WriteLine(i + "\t" + numberOfScatters[i] + "\t" + (double)numberOfScatters[i]/totalNumberOfCombinations );
//			}
//		}
//	}
//}
