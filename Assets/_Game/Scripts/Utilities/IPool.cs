/// <summary>
/// Interface for the object pool.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IPool<T>
{
    T Pull();
    void Push(T t);
}
