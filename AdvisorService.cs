using AdvisorManagement.Cache;
using AdvisorManagement.Domain;
using AdvisorManagement.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AdvisorManagement
{
    public class AdvisorService
    {
        private readonly AdvisorContext _context;
        private readonly MRUCache<int, Advisor> _cache;

        public AdvisorService(AdvisorContext context)
        {
            _context = context;
            _cache = new MRUCache<int, Advisor>();
        }

        public async Task<Advisor> CreateAdvisorAsync(Advisor advisor)
        {
            advisor.HealthStatus = GenerateHealthStatus();
            _context.Advisors.Add(advisor);
            await _context.SaveChangesAsync();
            _cache.Put(advisor.Id, advisor);
            return advisor;
        }

        public async Task<Advisor?> GetAdvisorAsync(int id)
        {
            if (_cache.Contains(id))
            {
                return _cache.Get(id);
            }

            var advisor = await _context.Advisors.FindAsync(id);
            if (advisor != null)
            {
                _cache.Put(advisor.Id, advisor);
            }
            return advisor;
        }

        public async Task UpdateAdvisorAsync(Advisor advisor)
        {
            _context.Advisors.Update(advisor);
            await _context.SaveChangesAsync();
            _cache.Put(advisor.Id, advisor);
        }

        public async Task DeleteAdvisorAsync(int id)
        {
            var advisor = await _context.Advisors.FindAsync(id);
            if (advisor != null)
            {
                _context.Advisors.Remove(advisor);
                await _context.SaveChangesAsync();
                _cache.Delete(id);
            }
        }

        public async Task<List<Advisor>> ListAdvisorsAsync()
        {
            return await _context.Advisors.ToListAsync();
        }

        private string GenerateHealthStatus()
        {
            var random = new Random();
            var value = random.Next(100);

            if (value < 60) return "Green";
            if (value < 80) return "Yellow";
            return "Red";
        }
    }

}
