using MiniMercadoSaas.Application.DTO.Request;
using MiniMercadoSaas.Application.ServiceInterfaces;
using MiniMercadoSaas.Domain.Entities;
using MiniMercadoSaas.Domain.Interfaces;

namespace MiniMercadoSaas.Application.Services;

public class UserService : IUserService
{
    private readonly IUsuarioRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUsuarioRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<User> Create(UserCreateRequest request)
    {
        var existingUser = await _repository.GetByEmailAsync(request.Email);
        if (existingUser != null) throw new Exception("Esse email ja foi cadastrado");

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var newUser = new User
        {
            Name = request.Name,
            Email = request.Email,
            SenhaHash = passwordHash,
            Role = request.Role,
            Active = true
        };

        await _repository.AddAsync(newUser);
        await _unitOfWork.CommitAsync();

        return newUser;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user == null) throw new Exception("Usuário não encontrado.");

        await _repository.DeleteAsync(user);
        await _unitOfWork.CommitAsync();
    }
}