using System.IO;
using docs_project.Application.Dto.Read;

namespace docs_project.Application.Interfaces.Repositories
{
    public interface ICsvReader
    {
        Task<IEnumerable<CsvRecord>> ReadAllAsync(Stream stream);
    }
}
