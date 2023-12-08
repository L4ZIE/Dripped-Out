using infrastructure.Entities;
using infrastructure.Repositories;
using service.Helpers;

namespace service.Services
{
    public class LoginService
    {
        private readonly LoginRepository _loginRepository;
        private readonly UserRepository _userRepository;
        private readonly AuthenticationHelper _authentication;

        public LoginService(LoginRepository loginRepository, AuthenticationHelper authenticationHelper, UserRepository userRepository)
        {
            _loginRepository = loginRepository;
            _userRepository = userRepository;
            _authentication = authenticationHelper;
        }

        public bool Register(string email, string password)
        {
            var user = new Users
            {
                UserId = 0,
                Email = email,
                IsAdmin = false
            };

            byte[] passwordHash;
            byte[] passwordSalt;
            
            _userRepository.Create(user);
            _authentication.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            var createdUser = _loginRepository.GetUsersByEmail(email);
            
            if (createdUser == null)
                return false;
            
            var passwordObject = new PasswordHash
            {
                UserId = createdUser.UserId,
                Hash = passwordHash, 
                Salt = passwordSalt, 
                Algorithm = "HMACSHA512"
            };
            _loginRepository.Create(passwordObject); 
            return true;
            
        }

        public bool Login(string email, string password)
        {
            var user = _loginRepository.GetUsersByEmail(email);

            if (user == null)
                return false;

            PasswordHash passwordHash = _loginRepository.Read(user.UserId);

            if (!_authentication.VerifyPasswordHash(password, passwordHash.Hash, passwordHash.Salt))
                return false;

            return true;
        }
        
        
    }
}