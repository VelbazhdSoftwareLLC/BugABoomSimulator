using System;

namespace CSharpSimulation
{
	class MersenneTwister : Random {
		private const int N = 624;
		private const int M = 397;
		private const uint MATRIX_A = 0x9908b0dfU;
		private const uint UPPER_MASK = 0x80000000U;
		private const uint LOWER_MASK = 0x7fffffffU;
		private const int MAX_RAND_INT = 0x7fffffff;
		
		private uint[] mag01 = { 0x0U, MATRIX_A };
		
		private uint[] mt = new uint[N];
		
		private int mti = N + 1;
		
		public MersenneTwister(){
			initGenRand( (uint)DateTime.Now.Millisecond );
		}
		
		public MersenneTwister( int seed ){
			initGenRand( (uint)seed );
		}
		
		public MersenneTwister( int[] init ){
			uint[] initArray = new uint[init.Length];
			for ( int i = 0; i < init.Length; ++i ) {
				initArray[i] = (uint)init[i];
			}
			
			initByArray( initArray, (uint)initArray.Length );
		}
		
		public static int MaxRandomInt {
			get{
				return 0x7fffffff;
			}
		}
		
		public override int Next(){
			return genRandInt31();
		}
		
		public override int Next( int maxValue ){
			return Next( 0, maxValue-1 );
		}
		
		public override int Next( int minValue, int maxValue ){
			if ( minValue > maxValue ){
				int swap = maxValue;
				maxValue = minValue;
				minValue = swap;
			}
			
			return (int)(Math.Floor( (maxValue - minValue + 1) * genRandReal2() + minValue ));
		}
		
		public float NextFloat(){
			return (float)genRandReal2();
		}
		
		public float NextFloat( bool includeOne ){
			if ( includeOne == true ){
				return (float)genRandReal1();
			}
			return (float)genRandReal2();
		}
		
		public float NextFloatPositive(){
			return (float)genRandReal3();
		}
		
		public override double NextDouble(){
			return genRandReal2();
		}
		
		public double NextDouble( bool includeOne ){
			if ( includeOne == true ){
				return genRandReal1();
			}
			return genRandReal2();
		}
		
		public double NextDoublePositive(){
			return genRandReal3();
		}
		
		public double Next53BitRes(){
			return genRandRes53();
		}
		
		public void Initialize(){
			initGenRand( (uint)DateTime.Now.Millisecond );
		}
		
		public void Initialize( int seed ){
			initGenRand( (uint)seed );
		}
		
		public void Initialize( int[] init ){
			uint[] initArray = new uint[init.Length];
			for ( int i = 0; i < init.Length; ++i ) {
				initArray[i] = (uint)init[i];
			}
			
			initByArray( initArray, (uint)initArray.Length );
		}
		
		private void initGenRand( uint seed ){
			mt[0] = seed & 0xffffffffU;
			for ( mti = 1; mti < N; mti++ ){
				mt[mti] = (uint)(1812433253U * (mt[mti - 1] ^ (mt[mti - 1] >> 30)) + mti);
				mt[mti] &= 0xffffffffU;
			}
		}
		
		private void initByArray( uint[] initKey, uint keyLength ) {
			int i, j, k;
			initGenRand( 19650218U );
			i = 1; j = 0;
			k = (int)(N > keyLength ? N : keyLength);

			for ( ; k > 0; k-- ) {
				mt[i] = (uint)((uint)(mt[i] ^ ((mt[i - 1] ^ (mt[i - 1] >> 30)) * 1664525U)) + initKey[j] + j); /* non linear */
				mt[i] &= 0xffffffffU;
				i++; j++;
				if ( i >= N ) { mt[0] = mt[N - 1]; i = 1; }
				if ( j >= keyLength ) j = 0;
			}

			for ( k = N - 1; k > 0; k-- ) {
				mt[i] = (uint)((uint)(mt[i] ^ ((mt[i - 1] ^ (mt[i - 1] >> 30)) * 1566083941U)) - i); /* non linear */
				mt[i] &= 0xffffffffU;
				i++;
				if ( i >= N ) { mt[0] = mt[N - 1]; i = 1; }
			}
			
			mt[0] = 0x80000000U;
		}
		
		uint genRandInt32() {
			uint y;

			if ( mti >= N ) { 
				int kk;
				
				if ( mti == N + 1 ) {
					initGenRand( 5489U ); 
				}
				
				for ( kk = 0; kk < N - M; kk++ ) {
					y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
					mt[kk] = mt[kk + M] ^ (y >> 1) ^ mag01[y & 0x1U];
				}

				for ( ; kk < N - 1; kk++ ) {
					y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
					mt[kk] = mt[kk + (M - N)] ^ (y >> 1) ^ mag01[y & 0x1U];
				}

				y = (mt[N - 1] & UPPER_MASK) | (mt[0] & LOWER_MASK);
				mt[N - 1] = mt[M - 1] ^ (y >> 1) ^ mag01[y & 0x1U];
				
				mti = 0;
			}
			
			y = mt[mti++];
			
			y ^= (y >> 11);
			y ^= (y << 7) & 0x9d2c5680U;
			y ^= (y << 15) & 0xefc60000U;
			y ^= (y >> 18);
			
			return y;
		}
		
		private int genRandInt31() {
			return (int)(genRandInt32() >> 1);
		}
		
		double genRandReal1() {
			return genRandInt32() * (1.0 / 4294967295.0);
		}
		
		double genRandReal2() {
			return genRandInt32() * (1.0 / 4294967296.0);
		}
		
		double genRandReal3() {
			return (((double)genRandInt32()) + 0.5) * (1.0 / 4294967296.0);
		}
		
		double genRandRes53() {
			uint a = genRandInt32() >> 5, b = genRandInt32() >> 6;
			return (a * 67108864.0 + b) * (1.0 / 9007199254740992.0);
		}
	}
}

