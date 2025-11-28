namespace Garage;


internal interface IGarage<T> {
	IEnumerator<T> GetEnumerator();
	public int FindIndex(Func<T, bool> func);
	public bool Add(T item);
	public bool Remove(int i);
	public int Length { get; }
	public int Capacity { get; }
	public bool IsFull { get; }
	// public bool Resizable {get;set;}
}
