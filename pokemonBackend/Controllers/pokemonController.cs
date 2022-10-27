using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pokemonBackend.Data;
using pokemonBackend.Models;
using pokemonBackend.Models.reponseModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;

namespace pokemonBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class pokemonController : ControllerBase
    {
        private readonly dataContext _dataContext;

        public pokemonController(dataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [HttpGet("getPokemons")]
        public async Task<JsonResult> getPokemons()
        {
            try
            {
                var pokemons = await _dataContext.pokemons.ToListAsync();

                responseModel resp = new responseModel { estado = 1, mensaje = "List of All Pokemons!", detalle = pokemons };

                return new JsonResult(resp);
            }
            catch (Exception ex)
            {

                return new JsonResult(ex);
            }
        }
        [HttpPost("savePokemon")]
        public async Task<JsonResult> savePokemon([FromBody] pokemonModel pokemon)
        {
            try
            {
                responseModel respValidateData = validateData(pokemon);

                if (respValidateData.estado == 1)
                {
                    List<pokemonModel> reqPokemon = new List<pokemonModel>();


                    reqPokemon.Add(pokemon);

                    await _dataContext.pokemons.AddAsync(reqPokemon[0]);
                    await _dataContext.SaveChangesAsync();

                    responseModel resp = new responseModel { estado = 1, mensaje = "Pokémon saved successfully!", detalle = reqPokemon };

                    return new JsonResult(resp);
                }
                else
                {
                    return new JsonResult(respValidateData);
                }
            }
            catch (Exception ex)
            {
                responseModel resp = new responseModel { estado = 3, mensaje = "Error in Data!", detalle = null };
                return new JsonResult(resp);
            }
        }
        [HttpGet("getPokemonById")]
        public async Task<JsonResult> getPokemonById(int pokemonId)
        {
            try
            {
                var pokemonData = await _dataContext.pokemons.FirstOrDefaultAsync(x => x.Id == pokemonId);

                List<pokemonModel> listPok = new List<pokemonModel>();

                if (pokemonData == null)
                {
                    responseModel resp = new responseModel { estado = 2, mensaje = "Pokemon not Found!!", detalle = pokemonId };
                    return new JsonResult(resp);
                }
                else
                {
                    listPok.Add(pokemonData);
                    responseModel resp = new responseModel { estado = 1, mensaje = "Data of Pokemon", detalle = listPok };
                    return new JsonResult(resp);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPut("updatePokemonData")]
        public async Task<JsonResult> updatePokemonData(string namePokemon, pokemonUpdateModel pokemon)
        {
            try
            {
                var pokemonData = await _dataContext.pokemons.FirstOrDefaultAsync(x => x.name == namePokemon);

                List<pokemonModel> respPok = new List<pokemonModel>();
                if (pokemonData == null)
                {
                    responseModel resp = new responseModel { estado = 2, mensaje = "Pokemon Name not Found!!", detalle = namePokemon };
                    return new JsonResult(resp);
                }
                else
                {
                    pokemonData.name = pokemon.namePokemon;
                    pokemonData.season = pokemon.season;
                    pokemonData.partner = pokemon.partner;
                    pokemonData.imageUrl = pokemon.imageUrl;

                    await _dataContext.SaveChangesAsync();

                    respPok.Add(pokemonData);
                    responseModel resp = new responseModel { estado = 1, mensaje = "Data of Pokemon Updated!", detalle = respPok };
                    return new JsonResult(resp);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpPost("validateData")]
        public responseModel validateData(pokemonModel pokemonData)
        {
            List<responseModel> respValidated = new List<responseModel>();
            responseModel resp;


            int validate = 0;
            if (pokemonData.name != null || pokemonData.name != "")
            {
                if (pokemonData.name?.Length > 30)
                {
                    validate++;
                    respValidated.Add(new responseModel { estado = 2, mensaje = "Name of Pokemon should have a max of 30 characteres", detalle = pokemonData.name });
                }
            }
            if (pokemonData.season != null || pokemonData.season != "")
            {
                if (pokemonData.season?.Length > 30)
                {
                    validate++;
                    respValidated.Add(new responseModel { estado = 2, mensaje = "Season of Pokemon should have a max of 30 characteres", detalle = pokemonData.season });
                }
            }
            if (pokemonData.partner != null || pokemonData.partner != "")
            {
                if (pokemonData.partner?.Length > 100)
                {
                    validate++;
                    respValidated.Add(new responseModel { estado = 2, mensaje = "Partner of Pokemon should have a max of 30 characteres", detalle = pokemonData.partner });
                }
            }
            if (pokemonData.imageUrl != null || pokemonData.imageUrl != "")
            {
                if (pokemonData.imageUrl?.Length > 500)
                {
                    validate++;
                    respValidated.Add(new responseModel { estado = 2, mensaje = "imageUrl of Pokemon should have a max of 30 characteres", detalle = pokemonData.imageUrl });
                }
            }
            if (validate > 0)
            {
                resp = new responseModel { estado = 2, mensaje = "Incorrect Data to Save Pokemon!", detalle = respValidated };
            }
            else
            {
                resp = new responseModel { estado = 1, mensaje = "Datos Correctos!", detalle = pokemonData };

            }
            return resp;

        }
    }
  
}
