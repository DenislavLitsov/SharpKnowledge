using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SharpKnowledge.Data.Models;

namespace SharpKnowledge.Data.Services
{
    public class BrainModelsService
    {
        private readonly DbContext _context;

        public BrainModelsService()
        {
            _context = PostgreContext.GetContext();
        }

        public async Task<List<BrainModel>> GetAllAsync()
        {
            return await _context.Set<BrainModel>().ToListAsync();
        }

        public async Task<BrainModel?> GetByIdAsync(int id)
        {
            return await _context.Set<BrainModel>().FindAsync(id);
        }

        public async Task<BrainModel> CreateAsync(BrainModel model)
        {
            _context.Set<BrainModel>().Add(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<bool> UpdateAsync(BrainModel model)
        {
            _context.Set<BrainModel>().Update(model);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Set<BrainModel>().FindAsync(id);
            if (entity == null) return false;
            _context.Set<BrainModel>().Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}