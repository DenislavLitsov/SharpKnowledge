using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
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

        public List<BrainModel> GetAll()
        {
            return _context.Set<BrainModel>().ToList();
        }

        public IEnumerable<BrainModel> GetAllByName(string name)
        {
            return _context.Set<BrainModel>().Where(b => b.Name == name).ToList();
        }

        // public BrainModel? GetBestFromName(string name)
        // {
        //     var res = _context.Set<BrainModel>().Where(b => b.Name == name).OrderByDescending(x => x.BestScore).FirstOrDefault();
        //     return res;
        // }

        public BrainModel? GetById(Guid id)
        {
            var res = _context.Set<BrainModel>().FirstOrDefault(x => x.Id == id);
            if (res == null)
                return null;

            var biases = _context.Set<Bias>().Where(b => b.BrainModelId == id)
                .OrderBy(x => x.Id)
                .ToList();

            var weights = _context.Set<Weight>().Where(w => w.BrainModelId == id)
                .OrderBy(x => x.Id)
                .ToList();

            foreach (var weight in weights)
            {
                var col = _context.Set<WeightCol>()
                    .Where(wc => wc.WeightId == weight.Id).ToList()
                    .OrderBy(x => x.Id)
                    .ToList();

                weight.WeightData = col;
            }

            res.BiasesData = biases;
            res.WeightsData = weights;

            return res;
        }

        public Guid GetBestScore(string name)
        {
            var res = _context.Set<BrainModel>().Where(b => b.Name == name).OrderByDescending(x => x.BestScore).FirstOrDefault();
            if (res == null) return Guid.Empty;

            return res.Id;
        }

        public BrainModel Create(BrainModel model)
        {
            _context.Set<BrainModel>().Add(model);
            _context.SaveChanges();
            return model;
        }

        public bool Update(BrainModel model)
        {
            _context.Set<BrainModel>().Update(model);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(Guid id)
        {
            var entity = this.GetById(id);

            if (entity == null) return false;
            _context.RemoveRange(entity.WeightsData);
            _context.RemoveRange(entity.BiasesData);
            _context.SaveChanges();
            _context.Remove(entity);
            return _context.SaveChanges() > 0;
        }
    }
}