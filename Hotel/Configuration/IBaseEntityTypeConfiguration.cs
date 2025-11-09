using Microsoft.EntityFrameworkCore;

public interface IBaseEntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : class
{

}