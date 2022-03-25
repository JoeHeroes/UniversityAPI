using System.Collections.Generic;
using UniAPI.Models;

namespace UniAPI.Services
{
    public interface IUniversityService
    {
        int Create(CreateUniversityDto dto);
        void Delete(int id);
        IEnumerable<UniversityDto> GetAll();
        UniversityDto GetById(int id);
        void Update(int id, UpdateUniversityDto dto);
    }
}