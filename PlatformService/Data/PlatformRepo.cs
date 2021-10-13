using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlatformService.Models;

namespace PlatformService.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly AppDbContext _appDbContext;
        public PlatformRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;

        }
        public void CreatePlatform(Platform platform)
        {
            if (platform is null)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            _appDbContext.Platforms.Add(platform);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _appDbContext.Platforms.ToList();
        }

        public Platform GetPlatformById(int id)
        {
            return _appDbContext.Platforms.FirstOrDefault(p => p.Id == id);
        }

        public bool SaveChanges()
        {
            return _appDbContext.SaveChanges() >= 0;
        }
    }
}