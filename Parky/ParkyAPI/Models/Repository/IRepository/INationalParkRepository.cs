﻿using ParkyAPI.Models;

namespace ParkyAPI.Models.Repository.IRepository
{
    public interface INationalParkRepository
    {
        ICollection<NationalPark> GetNationalParks();

        NationalPark GetNationalPark(int nationalParkId);

        bool NationalParkExists(string name);

        bool NationalParkExists(int nationalParkId);

        bool CreateNationalPark(NationalPark nationalPark);
        bool UpdateNationalPark(NationalPark nationalPark);
        bool DeleteNationalPark(NationalPark nationalPark);

        bool Save();

    }
}
