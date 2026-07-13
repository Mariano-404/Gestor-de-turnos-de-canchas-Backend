using AutoMapper;
using AutoMapper.QueryableExtensions;
using FutbolAPI.DTOs;
using FutbolAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;

namespace FutbolAPI.Controllers
{

    [Route("api/anotarse_Equipo")]
    [ApiController]
    public class AnotarseEquipoController: ControllerBase
    {
        private readonly IOutputCacheStore outputCacheStore;
        private readonly AplicationDBContext context;
        private readonly IMapper mapper;
        private const string cacheTag = "equipo";

        public AnotarseEquipoController(IOutputCacheStore outputCacheStore, AplicationDBContext context, IMapper mapper)
        {
            this.outputCacheStore = outputCacheStore;
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet(Name = "ObteberTodosEquipo")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<List<EquipoDTO>> Get() {

            return await context.Equipos.ProjectTo<EquipoDTO>(mapper.ConfigurationProvider).ToListAsync();


        }

        [HttpGet("{id}", Name = "ObtenerPorEquipoId")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<EquipoDTO>> Get(int id)
        {
             var equipo = await context.Equipos
                .ProjectTo<EquipoDTO>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.id == id);

            if (equipo is null)
            {
                return NotFound();
            }

            return equipo;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EquipoCreacionDTO equipoCreacionDTO) {

            var equipo = mapper.Map<Equipo>(equipoCreacionDTO); 
            context.Add(equipo);
            await context.SaveChangesAsync();
            return CreatedAtRoute("ObtenerPorEquipoId", new { id = equipo.id }, equipo);
        
        }

        [HttpDelete]
        public void Delete() {

            throw new NotImplementedException();

        }

    }
}
