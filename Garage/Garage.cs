using System.Collections;

namespace Garage;


internal class Garage<T> : IEnumerable<T>, IGarage<T> where T : Vehicle {
	protected T[] _list;
	public T this[int index] => _list[index];
	public bool IsFull => Capacity <= Length;
	public int Capacity => _list.Length;
	public int Length {
		get {
			int length = 0;
			foreach (T item in this.Where(item => item != null)) length++;
			return length;
		}
	}

	public Garage(int capacity) {
		_list = new T[Math.Max(capacity,2)];
		if(capacity==0) Resizable = true;
	}

	public int FindIndex(Func<T, bool> func) {
		int index = Array.FindIndex(_list, (item) => {
			if (item == null) return false;
			return func(item);
		});
		return index;
	}

	private void Resize(int capacity) {
		T[] newArr = new T[capacity];
		Array.Copy(_list, 0, newArr, 0, _list.Length);
		_list = newArr;
	}

	private bool? _resizable;
	public bool Resizable { get { return _resizable ?? false; } set { _resizable = value; } }
	public virtual bool Add(T item) {
		int index = Array.FindIndex(_list, (v) => v == null);
		if (index == -1) {
			// expands if overflowing and instance was created with capacity 0
			if (Resizable) {
				Resize(Math.Max(Capacity, 2) * 2);
				index = Length;
			} else {
				return false;
			}
		}
		_list[index] = item;
		return true;
	}

	// ensure that the items is at correct index without null values inbetween. Currently only needed if doing normal for loop or retrieving item from index
	private void FixArrayGaps(int i_ = 0) {
		// for (int i = i_; i < _list.Length; i++) {
		// 	if (_list[i] == default(T)) {
		// 		for (int j = i + 1; j < _list.Length; j++) {
		// 			if (_list[j] != default(T)) {
		// 				(_list[i], _list[j]) = (_list[j], default(T)!);
		// 				break;
		// 			}
		// 		}
		// 	}
		// }
	}

	public virtual bool Remove(int i) {
		if (i < 0 || i >= _list.Length) return false;
		_list[i] = default(T)!;
		FixArrayGaps(i);
		return true;
	}


	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	public IEnumerator<T> GetEnumerator() {
		foreach (T item in _list) {
			if (item == null) continue;
			yield return item;
		}
	}

}

