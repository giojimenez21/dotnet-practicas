using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiAutores.Dtos;
using WebApiAutores.Services;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/cuentas")]
    public class CuentasController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly HashService hashService;
        private readonly IDataProtector dataProtector;

        public CuentasController(UserManager<IdentityUser> userManager, IConfiguration configuration, SignInManager<IdentityUser> signInManager, IDataProtectionProvider dataProtectionProvider, HashService hashService)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.hashService = hashService;
            dataProtector = dataProtectionProvider.CreateProtector("valordeejemplo");
        }

        [HttpGet("encriptar")]
        public ActionResult Encriptar()
        {
            var textoPlano = "hola";
            var textoCifrado = dataProtector.Protect(textoPlano);
            var textoDescencriptado = dataProtector.Unprotect(textoCifrado);
            return Ok(new
            {
                textoPlano,
                textoCifrado,
                textoDescencriptado
            });
        }

        [HttpGet("encriptarPorTiempo")]
        public ActionResult EncriptarPorTiempo()
        {
            var protectorTiempo = dataProtector.ToTimeLimitedDataProtector();
            var textoPlano = "hola";
            var textoCifrado = protectorTiempo.Protect(textoPlano, lifetime: TimeSpan.FromSeconds(5));
            Thread.Sleep(6000);
            var textoDescencriptado = protectorTiempo.Unprotect(textoCifrado);
            return Ok(new
            {
                textoPlano,
                textoCifrado,
                textoDescencriptado
            });
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<RespuestaAutenticacion>> Registrar(CredencialesUsuario credenciales)
        {
            var usuario = new IdentityUser
            {
                UserName = credenciales.Email,
                Email = credenciales.Email
            };
            var resultado = await userManager.CreateAsync(usuario, credenciales.Password);
            if (resultado.Succeeded)
            {
                return await ConstruirToken(credenciales);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<RespuestaAutenticacion>> Login(CredencialesUsuario credencialesUsuario)
        {
            var resultado = await signInManager.PasswordSignInAsync(credencialesUsuario.Email, credencialesUsuario.Password, false, false);
            if (!resultado.Succeeded)
            {
                return BadRequest("Login incorrecto");
            }
            return await ConstruirToken(credencialesUsuario);
        }

        [HttpGet("RenovarToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<RespuestaAutenticacion>> Renovar()
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault().Value;
            var credenciales = new CredencialesUsuario
            {
                Email = emailClaim
            };
            return await ConstruirToken(credenciales);
        }


        private async Task<RespuestaAutenticacion> ConstruirToken(CredencialesUsuario credenciales)
        {
            var claims = new List<Claim>()
            {
                new ("email", credenciales.Email)
            };
            var usuario = await userManager.FindByEmailAsync(credenciales.Email);
            var claimsDB = await userManager.GetClaimsAsync(usuario);
            claims.AddRange(claimsDB);
            //Genera la llave
            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["KeyJwt"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
            var expiracion = DateTime.UtcNow.AddYears(1);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiracion,
                signingCredentials: creds
            );
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return new RespuestaAutenticacion
            {
                Token = token,
                Expiracion = expiracion
            };
        }

        [HttpPost("hacerAdmin")]
        public async Task<ActionResult> HacerAdmin(AdminDTO adminDTO)
        {
            var usuario = await userManager.FindByEmailAsync(adminDTO.Email);
            await userManager.AddClaimAsync(usuario, new Claim("esAdmin", "1"));
            return NoContent();
        }

        [HttpPost("removerAdmin")]
        public async Task<ActionResult> RemoverAdmin(AdminDTO adminDTO)
        {
            var usuario = await userManager.FindByEmailAsync(adminDTO.Email);
            await userManager.RemoveClaimAsync(usuario, new Claim("esAdmin", "1"));
            return NoContent();
        }

        [HttpGet("{textoPlano}")]
        public ActionResult RealizarHash(string textoPlano)
        {
            var resultado1 = hashService.Hash(textoPlano);
            var resultado2 = hashService.Hash(textoPlano);
            return Ok(new
            {
                resultado1,
                resultado2,
                textoPlano
            });
        }
    }
}
