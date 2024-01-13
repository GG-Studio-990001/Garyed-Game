namespace Runtime.Interface
{
    public interface IProvider<T>
    {
        public T Get();
        public void Set(T value);
    }
}