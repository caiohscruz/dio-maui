using SQLite;

namespace diomaui.Services
{
    public class DatabaseService<T> where T : new()
    {
        private SQLiteAsyncConnection _connection;

        public DatabaseService(string dbPath)
        {
            _connection = new SQLiteAsyncConnection(dbPath);
            _connection.CreateTableAsync<T>().Wait();
        }        

        public Task<int> InsertAsync(T entity)
        {
            return _connection.InsertAsync(entity);
        }

        public Task<int> UpdateAsync(T entity)
        {
            return _connection.UpdateAsync(entity);
        }

        public Task<int> DeleteAsync(T entity)
        {
            return _connection.DeleteAsync(entity);
        }

        public Task<List<T>> GetAllAsync() 
        {
            return _connection.Table<T>().ToListAsync();
        }

         public Task<int> CountAllAsync() 
        {
            return _connection.Table<T>().CountAsync();
        }

        public Task<T> GetByIdAsync(int id) 
        {
            return _connection.GetAsync<T>(id);
        }
    }  
}