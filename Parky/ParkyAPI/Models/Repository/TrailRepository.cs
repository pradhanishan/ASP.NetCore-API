using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Models.Repository.IRepository;

namespace ParkyAPI.Models.Repository
{
    public class TrailRepository : ITrailRepository
    {

        private readonly ApplicationDbContext _db;

        public TrailRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool CreateTrail(Trail Trail)
        {
            _db.Trails.Add(Trail);
            return Save();
        }

        public bool DeleteTrail(Trail Trail)
        {
            _db.Trails.Remove(Trail);
            return Save();
        }

        public Trail GetTrail(int nationalParkId)
        {
            return _db.Trails.Include(c => c.NationalPark).FirstOrDefault(x => x.Id == nationalParkId);
        }

        public ICollection<Trail> GetTrails()
        {
            return _db.Trails.Include(c => c.NationalPark).OrderBy(a => a.Name).ToList();
        }

        public bool TrailExists(string name)
        {
            bool value = _db.Trails.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool TrailExists(int nationalParkId)
        {
            return _db.Trails.Any(a => a.Id == nationalParkId);
        }

        public bool Save()
        {
            return _db.SaveChanges() > 0 ? true : false;
        }

        public bool UpdateTrail(Trail Trail)
        {
            _db.Trails.Update(Trail);
            return Save();
        }

        public ICollection<Trail> GetTrailsInNationalPark(int npId)
        {
            return _db.Trails.Include(c => c.NationalPark).Where(c => c.NationalParkId == npId).ToList();
        }
    }
}
