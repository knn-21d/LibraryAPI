using LibraryAPI.Data;
using LibraryAPI.Data.Repositories;
using System.Web.Http;

namespace LibraryAPI.Services
{
    public class StorageManagementService
    {
        private readonly CopiesRepository _copiesRepository;

        public StorageManagementService(CopiesRepository copiesRepository)
        {
            _copiesRepository = copiesRepository;
        }

        public async Task<Copy?> DeleteCopy(int id)
        {
            Copy copy = await _copiesRepository.GetCopy(id) ?? throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);

            await _copiesRepository.DeleteCopy(copy);

            return null;
        }

        public async Task<Copy> AddCopy(string isbn)
        {
            return await _copiesRepository.AddCopy(new Copy { Isbn = isbn });
        }

        public async Task<IEnumerable<Copy>> AddNCopies(string isbn, int n)
        {
            List<Copy> result = new();

            for (int i = 0; i < n; i++)
            {
                result.Add(await AddCopy(isbn));
            }

            return result;
        }
    }
}
