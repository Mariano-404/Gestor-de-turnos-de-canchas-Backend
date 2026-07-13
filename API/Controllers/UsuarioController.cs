using FutbolAPI.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FutbolAPI.Controllers
{
    [Route("api/usuario")]
    [ApiController]
    
    public class UsuarioController: ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;

        public UsuarioController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> Registrar([FromBody] CredencialesUsuariosDTO credencialesUsuariosDTO)
        {
            var usuario = new IdentityUser
            {
                Email = credencialesUsuariosDTO.Email,
                UserName = credencialesUsuariosDTO.Email,
            };
            
            var resultado = await userManager.CreateAsync(usuario, credencialesUsuariosDTO.Password);

            if (resultado.Succeeded)
            {
                return await Token(usuario);
            }
            else
            {
                return BadRequest(resultado.Errors);    
            }
        }

        [HttpPost("HacerAdmin")]
        public async Task<IActionResult> HacerAdmin(EditarClaimDTO editarClaimDTO)
        {
            var usuario = await userManager.FindByEmailAsync(editarClaimDTO.Email);

            if(usuario is null)
            {
                return NotFound();
            }

            await userManager.AddClaimAsync(usuario, new Claim(ClaimTypes.Role, "admin"));
            return NoContent();
        }

        [HttpPost("login")]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> login([FromBody] CredencialesUsuariosDTO credencialesUsuariosDTO)
        {
            var usuario = await userManager.FindByEmailAsync(credencialesUsuariosDTO.Email);

            if (usuario is null)
            {
                var errores =  ConstruirLoginIncorrecto();
                return BadRequest(errores); 
                
            }

            var resultado = await signInManager.CheckPasswordSignInAsync(usuario, credencialesUsuariosDTO.Password, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                return await Token(usuario);
            }
            else
            {
                var errores = ConstruirLoginIncorrecto();
                return BadRequest(errores);
            }
        }

        private IEnumerable<IdentityError> ConstruirLoginIncorrecto()
        {
            var identityError = new IdentityError() { Description = "Login Incorrecto" };
            var errores = new List<IdentityError>();

            errores.Add(identityError);
            return errores;
        }

        private async Task<RespuestaAutenticacionDTO> Token(IdentityUser identityUser)
        {
            var claims = new List<Claim>
            {
                new Claim("Email", identityUser.Email!)
            };

            var claimsDb = await userManager.GetClaimsAsync(identityUser);

            claims.AddRange(claimsDb);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]!));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddYears(1);

            var tokenSeguridad = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiracion, signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenSeguridad);

            return new RespuestaAutenticacionDTO
            {
                Token = token,
                Expiracion = expiracion

            };
        }
    }
}
