using Microsoft.AspNetCore.Mvc;
using service.Services;

namespace api.Controllers;

[ApiController]
public class LoginController : ControllerBase
{
    private readonly LoginService _loginService;

    public LoginController(LoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpPost("/registeruser")]
    public bool Register([FromBody] LoginDto loginDto)
    {
       return _loginService.Register(loginDto.Email, loginDto.Password);
    }

    [HttpPost("/loginuser")]
    public bool Login([FromBody] LoginDto loginDto)
    {
        return _loginService.Login(loginDto.Email, loginDto.Password);
    }

    [HttpPost("/updateuser")]
    public void Update([FromBody] LoginDto loginDto)
    {
        _loginService.Update(loginDto.Email, loginDto.Password);
    }

    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}