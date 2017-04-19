using System.Collections.Generic;
using System.Linq;

namespace HSDHelper.Utils {
	/// <summary>
	/// Array used to calculate average of n latest values
	/// </summary>
    public class CircularArray {
		private double[] _data;
		private int _firstIndex;
		private int _lastIndex;
		private double _limit;
		private int _numSamples;

		public CircularArray() {
			_data = null;
			_numSamples = 0;
			_firstIndex = 0;
			_lastIndex = 0;
			_limit = 0.0d;
		}

		/// <summary>
		/// Inserts value to the array
		/// </summary>
		/// <param name="d">Value to add</param>
		public void AddValue(double d) {
			_firstIndex++;
			if (_firstIndex == _data.Length) {
				_firstIndex = 0;
			}
			_data[_firstIndex] = d;

			if (_numSamples == 0) {
				_lastIndex = _firstIndex;
				_numSamples++;
			}
			else if (_numSamples == _data.Length) {
				_lastIndex++;
				if (_lastIndex == _data.Length) {
					_lastIndex = 0;
				}
			}
			else {
				_numSamples++;
			}
		}
		/// <summary>
		/// Returns last inserted value
		/// </summary>
		/// <returns></returns>
		public double GetLastValue() {
			if (_numSamples > 0) {
				return _data[_lastIndex];
			}
			return 0.0;
		}
		/// <summary>
		/// Gets value at index i
		/// </summary>
		/// <param name="i">index of value to return</param>
		/// <returns></returns>
		public double GetValue(int i) {
			if (i <= 0) {
				return _data[_firstIndex];
			}
			else if (i > _numSamples) {
				return _data[_lastIndex];
			}
			else {
				var i2 = _firstIndex - i;
				if (i2 < 0) {
					i2 = _data.Length - i;
				}
				return _data[i2];
			}
		}
		/// <summary>
		/// Initializes the array
		/// </summary>
		/// <param name="i">Maximum number of values to store</param>
		/// <param name="d">Values lower than limit will be ignored while calculating average</param>
		public void Initialize(int i, double d) {
			_data = new double[i];

			_numSamples = 0;
			_firstIndex = -1;
			_lastIndex = -1;
			_limit = d;
		}
		/// <summary>
		/// Reruns number of stored values
		/// </summary>
		/// <returns></returns>
		public int Length() {
			return _numSamples;
		}
		/// <summary>
		/// Inserts new value to array and returns an average of last n inserted values
		/// </summary>
		/// <param name="d">Value to add</param>
		/// <returns></returns>
		public double UpdateValue(double d) {
			var i = 0;
			var d2 = 0.0;
			AddValue(d);

			var i2 = _numSamples - 1;
			for (var i3 = 0; i3 <= i2; ++i3) {
				if (_data[i3] > _limit) {
					d2 += _data[i3];
					++i;
				}
			}
			if (i > 0) {
				return d2 / i;
			}
			return d2;
		}
	}
}

