namespace DAL.Context
{
  public class Include<TEntity> : IInclude<TEntity> where TEntity : class
  {
    private readonly Func<IQueryable<TEntity>, IQueryable<TEntity>> include;
    public Include(Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
    {
      this.include = include;
    }
    public T Execute<T>(T query) where T : IQueryable<TEntity>
    {
      return (T)include.Invoke(query);
    }
  }
}