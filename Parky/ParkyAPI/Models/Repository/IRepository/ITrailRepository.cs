using ParkyAPI.Models;

namespace ParkyAPI.Models.Repository.IRepository
{
    public interface ITrailRepository
    {
        ICollection<Trail> GetTrails();
        ICollection<Trail> GetTrailsInNationalPark(int npId);

        Trail GetTrail(int TrailId);

        bool TrailExists(string name);

        bool TrailExists(int TrailId);

        bool CreateTrail(Trail Trail);
        bool UpdateTrail(Trail Trail);
        bool DeleteTrail(Trail Trail);

        bool Save();

    }
}
