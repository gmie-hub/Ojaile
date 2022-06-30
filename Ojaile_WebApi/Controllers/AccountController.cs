using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Ojaile_WebApi.Model;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ojaile_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountController(IConfiguration configuration, UserManager<ApplicationUser> userManager, ILogger<AccountController> logger)
        {
            _configuration = configuration;
            _userManager = userManager;
            _logger = logger;
        }

        List<RegisterViewModel> registers = new List<RegisterViewModel>()
        {
            new RegisterViewModel()
            {
                FirstName = "Obafemi",
                LastName = "Awolowo",
                Email = "obaf123@gmail.com",
                PhoneNumber = "09087347382",
                Password = "obaf485",
                UserName = "obafa",
            },
            new RegisterViewModel()
            {
                FirstName = "Obiora",
                LastName = "iglowo",
                Email = "obiora12@gmail.com",
                PhoneNumber = "08034573289",
                Password = "obi3453475",
                UserName = "obicubana",
            },
            new RegisterViewModel()
            {
                FirstName = "Jimi",
                LastName = "Olaosebikan",
                Email = "folagmie16@gmail.com",
                PhoneNumber = "09039158414",
                Password = "jimosky01",
                UserName = "folagmie16",
                MobilePhone = "08104946963"
            }
        };
        
        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            Log.Information("Call login action");
            _logger.LogInformation("Call login action");
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = AuthenticateUser(model);
            if(user != null)
            {
                var token = GenerateAuthenticatedUserToken(user);
                return Ok(token);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = new ApplicationUser();
            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.UserName;
            user.Created = DateTime.Now;
            user.Institution = 1;

            var result = await _userManager.CreateAsync(user, model.Password);

            if(result.Succeeded)
                return Ok(result);
            return BadRequest(ModelState);
        }

        private string GenerateAuthenticatedUserToken(RegisterViewModel register)
        {
            var signKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]));

            var credential = new SigningCredentials(signKey, SecurityAlgorithms.HmacSha256);
            var claim = new[]
            {
                new Claim(ClaimTypes.Name, register.FirstName +' '+ register.LastName),
                new Claim(ClaimTypes.Email, register.Email),
                new Claim(ClaimTypes.MobilePhone, register.PhoneNumber),
            };
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"], claim, notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(30), credential).ToString();
            return token;
        }

        private RegisterViewModel AuthenticateUser(LoginViewModel model)
        {
            //throw new NotImplementedException();
            var user = registers.Where(c => c.UserName == model.UserName && 
            c.Password == model.Password).FirstOrDefault();
            if (user != null)
                return user;
            return null;
        }
    }
}
